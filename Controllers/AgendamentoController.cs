using GerenciamentoSalas.API.DTOs;
using GerenciamentoSalas.API.Models;
using GerenciamentoSalas.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GerenciamentoSalas.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AgendamentoController : ControllerBase
    {
        private readonly AgendamentoRepository _AgendamentoRepository;

        public AgendamentoController(IConfiguration configuration)
        {
            _AgendamentoRepository = new AgendamentoRepository(configuration);
        }

        [HttpGet]
        [Route("api/[controller]/ObterTodos")]
        public IActionResult ObterTodos()
        {
            try
            {
                var agendamentos = _AgendamentoRepository.ObterTodosAgendamento();
                return Ok(agendamentos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno no servidor ao buscar agendamentos: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("api/[controller]/Inserir")]
        public IActionResult Inserir([FromBody] CriarAgendamentoRequest input)
        {
            try
            {
                if (input == null) return BadRequest("Dados inválidos.");

                // Validação técnica básica de negócio: a consulta não pode terminar antes de começar
                if (input.DataHoraFim <= input.DataHoraInicio)
                {
                    return BadRequest("A data de término deve ser maior que a data de início.");
                }

                var agendamento = new Agendamento
                {
                    PacienteId = input.PacienteId,
                    ProfissionalId = input.ProfissionalId,
                    SalaId = input.SalaId,
                    DataHoraInicio = input.DataHoraInicio,
                    DataHoraFim = input.DataHoraFim,
                    Observacoes = input.Observacoes,
                    Status = input.Status
                };

                int idGerado = _AgendamentoRepository.InserirAgendamento(agendamento);
                agendamento.Id = idGerado;

                return StatusCode(201, agendamento);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao salvar agendamento: {ex.Message}");
            }
        }
    }
}