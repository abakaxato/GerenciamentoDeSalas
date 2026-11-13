using GerenciamentoSalas.API.Models.DTOs;
using GerenciamentoSalas.API.Models.Requests;
using GerenciamentoSalas.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GerenciamentoSalas.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AgendamentoController : ControllerBase
    {
        private readonly IAgendamentoService _agendamentoService;
        public AgendamentoController (IAgendamentoService agendamentoService)
        {
            _agendamentoService = agendamentoService;
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> ObterPorID([FromRoute]int id, CancellationToken cancellationToken = default)
        {
                AgendamentoDto? agendamento = await _agendamentoService.ObterAgendamentoPorIDAsync(id,cancellationToken);
                return Ok(agendamento);
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos(CancellationToken cancellationToken = default)
        {
                List<AgendamentoDto> agendamentos = await _agendamentoService.ObterTodosAgendamentoAsync(cancellationToken);
                return Ok(agendamentos);
        }

        [HttpPost]
        public async Task<IActionResult> Inserir([FromBody] CriarAgendamentoRequest criarAgendamentoRequest, CancellationToken cancellationToken = default)
        {
            try
            {

                int idGerado = await  _agendamentoService.InserirAgendamentoAsync(criarAgendamentoRequest, cancellationToken);

                return StatusCode(201, new
                {
                    Id = idGerado,
                    Mensagem = "Agendamento inserido com sucesso."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao salvar agendamento: {ex.Message}");
            }
        }
        [HttpPatch]
        [Route("{id:int}")]
        public async Task<IActionResult> Atualizar([FromRoute] int id,[FromBody]AtualizarAgendamentoRequest atualizarAgendamentoRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                AgendamentoDto agendamentoAtualizado = await _agendamentoService.AtualizarAgendamentoAsync(id,atualizarAgendamentoRequest, cancellationToken);

                return Ok(agendamentoAtualizado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao salvar agendamento: {ex.Message}");
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Deletar([FromRoute]int id, CancellationToken cancellationToken = default) {

            try
            {
                int idDeletado = await _agendamentoService.DeletarAgendamentoAsync(id, cancellationToken);

                return StatusCode(200, $"O Agendamento com o ID {idDeletado} foi deletado com sucesso");
            }
            catch (InvalidOperationException)
            {
                return NotFound($"O Agendamento com ID {id} nao existe");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    $"Erro ao deletar agendamento: {ex.Message}");
            }

        }
    }
}