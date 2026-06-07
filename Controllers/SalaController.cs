using Microsoft.AspNetCore.Mvc;
using GerenciamentoSalas.API.Repositories;

namespace GerenciamentoSalas.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalaController : ControllerBase
    {
        private readonly SalaRepository _repository;

        public SalaController(IConfiguration configuration)
        {
            _repository = new SalaRepository(configuration);
        }

        [HttpGet]
        public IActionResult ObterTodos()
        {
            try
            {
                var salas = _repository.ObterTodos();
                return Ok(salas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno no servidor ao buscar salas: {ex.Message}");
            }
        }
    }
}