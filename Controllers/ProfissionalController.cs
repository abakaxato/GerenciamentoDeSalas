using Microsoft.AspNetCore.Mvc;
using GerenciamentoSalas.API.Repositories;

namespace GerenciamentoSalas.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfissionalController : ControllerBase
    {
        private readonly ProfissionalRepository _repository;

        public ProfissionalController(IConfiguration configuration)
        {
            _repository = new ProfissionalRepository(configuration);
        }

        [HttpGet]
        public IActionResult ObterTodos()
        {
            try
            {
                var profissionais = _repository.ObterTodos();
                return Ok(profissionais);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno no servidor ao buscar profissionais: {ex.Message}");
            }
        }
    }
}