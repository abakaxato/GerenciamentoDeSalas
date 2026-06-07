using GerenciamentoSalas.API.Models;
using GerenciamentoSalas.API.Repositories;
using GerenciamentoSalas.API.Requests;
using Microsoft.AspNetCore.Mvc;

namespace GerenciamentoSalas.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PacienteController : ControllerBase
    {
        private readonly PacienteRepository _pacienteRepository;

        public PacienteController(IConfiguration configuration) {
            _pacienteRepository = new PacienteRepository(configuration);
        }

        [HttpGet]
        public IActionResult ObterTodos()
        {
            try
            {
                var pacientes = _pacienteRepository.ObterTodos();
                return Ok(pacientes);
            }
            catch(Exception ex) {
                return StatusCode(500, $"Erro Interno no servidor{ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult InserirPaciente([FromBody] CriarPacienteRequest request) {
            try {

                if (request == null) return BadRequest("Dados inválidos.");
                
                var novoPaciente = new Paciente {
                    ResponsavelId = request.ResponsavelId,
                    Nome = request.Nome,
                    DataNascimento = request.DataNascimento,
                };
                int idGerado = _pacienteRepository.InserirPaciente(novoPaciente);

                novoPaciente.Id = idGerado;
                novoPaciente.DataCadastro = DateTime.Now;

                return StatusCode(201, novoPaciente);

            }catch (Exception ex) {
                return StatusCode(500, $"Erro ao salvar: {ex.Message}");
            }

        }
    }
}
