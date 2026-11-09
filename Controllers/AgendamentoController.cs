using GerenciamentoSalas.API.DTOs;
using GerenciamentoSalas.API.Models;
using GerenciamentoSalas.API.Requests;
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
        [Route("api/[controller]/{id}")]
        public async Task<IActionResult> ObterAgendamentoPorID([FromRoute]int id, CancellationToken cancellationToken = default)
        {
            try
            {
                Agendamento? agendamento = await _agendamentoService.ObterAgendamentoPorIDAsync(id,cancellationToken);
                return Ok(agendamento);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno no servidor ao buscar agendamento com ID {id}: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObterAgendamentos(CancellationToken cancellationToken = default)
        {
            try
            {
                List<Agendamento> agendamentos = await _agendamentoService.ObterTodosAgendamentoAsync(cancellationToken);
                return Ok(agendamentos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno no servidor ao buscar agendamentos: {ex.Message}");
            }
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

        [HttpDelete]
        public async Task<IActionResult> Deletar([FromBody]DeletarAgendamentoRequest request, CancellationToken cancellationToken = default) {

            try
            {
                int idDeletado = await _agendamentoService.DeletarAgendamentoAsync(request, cancellationToken);

                return StatusCode(200, $"O Agendamento com o ID {idDeletado} foi deletado com sucesso");
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