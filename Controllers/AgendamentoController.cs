using GerenciamentoSalas.API.Models.DTOs;
using GerenciamentoSalas.API.Models.Requests;
using GerenciamentoSalas.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace GerenciamentoSalas.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AgendamentoController : ControllerBase
    {
        private readonly IAgendamentoService _agendamentoService;
        public AgendamentoController(IAgendamentoService agendamentoService)
        {
            _agendamentoService = agendamentoService;
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> ObterPorID([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            AgendamentoDto? agendamento = await _agendamentoService.ObterAgendamentoPorIDAsync(id, cancellationToken);
            if (agendamento == null ){ return NotFound($"Agendamento com ID {id} nao encontrado"); }
            else { return Ok(agendamento); }
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos(CancellationToken cancellationToken = default)
        {
            List<AgendamentoDto> agendamentos = await _agendamentoService.ObterTodosAgendamentoAsync(cancellationToken);
            if (agendamentos.IsNullOrEmpty()) { return NotFound("Nenhum agendamento foi retornado"); }
            else { return Ok(agendamentos); }
        }

        [HttpPost]
        public async Task<IActionResult> Inserir([FromBody] CriarAgendamentoRequest criarAgendamentoRequest, CancellationToken cancellationToken = default)
        {
            int idGerado = await _agendamentoService.InserirAgendamentoAsync(criarAgendamentoRequest, cancellationToken);

            return Ok(idGerado);
        }
        [HttpPatch]
        [Route("{id:int}")]
        public async Task<IActionResult> Atualizar([FromRoute] int id, [FromBody] AtualizarAgendamentoRequest atualizarAgendamentoRequest, CancellationToken cancellationToken = default)
        {
            AgendamentoDto agendamentoAtualizado = await _agendamentoService.AtualizarAgendamentoAsync(id, atualizarAgendamentoRequest, cancellationToken);

            return Ok(agendamentoAtualizado);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Deletar([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            int idDeletado = await _agendamentoService.DeletarAgendamentoAsync(id, cancellationToken);
            return Ok(idDeletado);
        }
    }
}