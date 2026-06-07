using Microsoft.AspNetCore.Mvc;
using GerenciamentoSalas.API.Repositories;

namespace GerenciamentoSalas.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AgendamentoController : ControllerBase
    {
        private readonly AgendamentoRepository _repository;

        public AgendamentoController(IConfiguration configuration)
        {
            _repository = new AgendamentoRepository(configuration);
        }

        [HttpGet]
        public IActionResult ObterTodos()
        {
            try
            {
                var agendamentos = _repository.ObterTodos();
                return Ok(agendamentos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno no servidor ao buscar agendamentos: {ex.Message}");
            }
        }
    }
}