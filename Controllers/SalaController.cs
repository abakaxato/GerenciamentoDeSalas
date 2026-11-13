using GerenciamentoSalas.API.Models.DTOs;
using GerenciamentoSalas.API.Models.Requests;
using GerenciamentoSalas.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GerenciamentoSalas.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalaController : ControllerBase
    {
        private readonly SalaRepository _SalaRepository;

        public SalaController(IConfiguration configuration)
        {
            _SalaRepository = new SalaRepository(configuration);
        }

        [HttpGet]
        [Route("api/[controller]")]
        public IActionResult ObterTodos()
        {
            try
            {
                var salas = _SalaRepository.ObterTodosSala();
                return Ok(salas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno no servidor ao buscar salas: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("api/[controller]")]
        public IActionResult Inserir([FromBody] CriarSalaRequest input)
        {
            try
            {
                if (input == null) return BadRequest("Dados inválidos.");

                var sala = new SalaDto
                {
                    Nome = input.Nome,
                    Capacidade = input.Capacidade,
                };

                int idGerado = _SalaRepository.InserirSala(sala);
                sala.Id = idGerado;

                return StatusCode(201, sala);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao salvar sala: {ex.Message}");
            }
        }
        [HttpDelete]
        [Route("api/[controller]")]
        public async Task<IActionResult> Deletar([FromBody] int salaId)
        {
            try
            {
                var request = new DeletarSalaRequest { id = salaId };
                var (salaDeletada, agendamentosDeletados) =
                    await _SalaRepository.DeletarSalaAsync(request);

                return Ok(new
                {
                    AgendamentosDeletados = agendamentosDeletados,
                    SalaDeletada = salaDeletada
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao deletar Sala: {ex.Message}");
            }
        }
    }
}