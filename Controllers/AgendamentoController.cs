using GerenciamentoSalas.API.DTOs;
using GerenciamentoSalas.API.Models;
using GerenciamentoSalas.API.Repositories;
using GerenciamentoSalas.API.Repositories.Interfaces;
using GerenciamentoSalas.API.Requests;
using Microsoft.AspNetCore.Mvc;

namespace GerenciamentoSalas.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AgendamentoController : ControllerBase
    {
        private readonly IAgendamentoRepository _agendamentoRepository;
        public AgendamentoController (IAgendamentoRepository agendamentoRepository)
        {
            _agendamentoRepository = agendamentoRepository;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            try
            {
                List<Agendamento> agendamentos = await _agendamentoRepository.ObterTodosAgendamentoAsync();
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
                if (input == null) return BadRequest("Dados inválidos.");

                if (input.DataHoraFim <= input.DataHoraInicio)
                {
                    return BadRequest(
                        "A data de término deve ser maior que a data de início.");
                }

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

                int idGerado = await  _agendamentoRepository.InserirAgendamentoAsync(agendamento);

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
                if (request is null || request.AgendamentoId <= 0) 
                {
                    return BadRequest("AgendamentoId inválido.");
                }
                int idDeletado = await _agendamentoRepository.DeletarAgendamentoAsync(request);

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