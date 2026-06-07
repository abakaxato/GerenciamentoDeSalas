using GerenciamentoSalas.API.Repositories;
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
    }
}
