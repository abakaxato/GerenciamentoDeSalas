using GerenciamentoSalas.API.DTOs;
using GerenciamentoSalas.API.Models;
using GerenciamentoSalas.API.Repositories;
using GerenciamentoSalas.API.Repositories.Interfaces;
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
        public async Task<IActionResult> ObterAgendamentoPorID([FromBody]int id)
        {
            try
            {
                Agendamento agendamento = await _agendamentoService.ObterAgendamentoPorIDAsync(id);
                return Ok(agendamento);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno no servidor ao buscar agendamento com ID {id}: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObterAgendamentos()
        {
            try
            {
                List<Agendamento> agendamentos = await _agendamentoService.ObterTodosAgendamentoAsync();
                return Ok(agendamentos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno no servidor ao buscar agendamentos: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Inserir([FromBody] CriarAgendamentoRequest input)
        {
            try
            {
                var agendamento = new CriarAgendamentoRequest
                {
                    PacienteId = input.PacienteId,
                    ProfissionalId = input.ProfissionalId,
                    SalaId = input.SalaId,
                    DataHoraInicio = input.DataHoraInicio,
                    DataHoraFim = input.DataHoraFim,
                    Observacoes = input.Observacoes,
                    Status = input.Status
                };

                int idGerado = await  _agendamentoService.InserirAgendamentoAsync(agendamento);

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
        public async Task<IActionResult> Deletar([FromBody]DeletarAgendamentoRequest request) {

            try
            {
                int idDeletado = await _agendamentoService.DeletarAgendamentoAsync(request);

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