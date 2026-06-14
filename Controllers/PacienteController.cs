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
        private readonly PacienteRepository _PacienteRepository;

        public PacienteController(IConfiguration configuration) {
            _PacienteRepository = new PacienteRepository(configuration);
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
                
                var novoPaciente = new Paciente {
                    ResponsavelId = request.ResponsavelId,
                    Nome = request.Nome,
                    DataNascimento = request.DataNascimento,
                };
                int idGerado = _PacienteRepository.InserirPaciente(novoPaciente);

                novoPaciente.Id = idGerado;
                novoPaciente.DataCadastro = DateTime.Now;

                return StatusCode(201, novoPaciente);

            }catch (Exception ex) {
                return StatusCode(500, $"Erro ao salvar: {ex.Message}");
            }

        }
    }
}
