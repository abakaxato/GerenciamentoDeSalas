using GerenciamentoSalas.API.Models;
using GerenciamentoSalas.API.Models.DTOs;
using GerenciamentoSalas.API.Models.Requests;
using GerenciamentoSalas.API.Repositories;
using GerenciamentoSalas.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GerenciamentoSalas.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PacienteController : ControllerBase
    {
        private readonly PacienteRepository _PacienteRepository;
        private readonly IPacienteService _PacienteService;

        public PacienteController(IConfiguration configuration, IPacienteService pacienteService) {
            _PacienteRepository = new PacienteRepository(configuration);
            _PacienteService = pacienteService;
        }

        [HttpGet]
        [Route("VerificarDisponibilidade")]
        public async Task<IActionResult> VerificarDisponibilidade([FromQuery]int id, [FromQuery] DateTime dataInicio,DateTime dataFim)
        {
            try
            {
                bool disponibilidade = await _PacienteService.VerificarDisponibilidadeAsync(id, dataInicio, dataFim);
                return Ok(disponibilidade);
            }
            catch(Exception ex) {
                return StatusCode(500, $"Erro Interno no servidor{ex.Message}");
            }
        }

        [HttpGet]
        [Route("api/[controller]")]
        public IActionResult ObterTodos()
        {
            try
            {
                var pacientes = _PacienteRepository.ObterTodosPaciente();
                return Ok(pacientes);
            }
            catch(Exception ex) {
                return StatusCode(500, $"Erro Interno no servidor{ex.Message}");
            }
        }

        [HttpPost]
        [Route("api/[controller]")]
        public IActionResult Inserir([FromBody] CriarPacienteRequest request) {
            try {

                if (request == null) return BadRequest("Dados inválidos.");
                
                var novoPaciente = new PacienteDto {
                    ResponsavelId = request.ResponsavelId,
                    Nome = request.Nome,
                    DataNascimento = request.DataNascimento,
                };
                int idGerado = _PacienteRepository.InserirPaciente(novoPaciente);

                novoPaciente.Id = idGerado;
                novoPaciente.DataCadastro = DateTime.Now;

                return StatusCode(201, $"O novo paciente foi criado com o ID :{novoPaciente}");

            }catch (Exception ex) {
                return StatusCode(500, $"Erro ao salvar: {ex.Message}");
            }

        }

        [HttpDelete]
        [Route("api/[controller]")]
        public async Task <IActionResult>DeletarPacienteAsync(int id) {
            try
            {
                var (linhasDeletadas,idDeletado) = await _PacienteRepository.DeletarPacienteAsync(id);
                return StatusCode(200, $"Agendamentos deletados : {linhasDeletadas} \nID deletado : {idDeletado}");
            }
            catch (Exception ex) {
                return StatusCode(500, $"Erro ao Deletar Paciente: {ex.Message}");
            }
        }
    }
}
