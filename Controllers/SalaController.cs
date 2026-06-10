using GerenciamentoSalas.API.DTOs;
using GerenciamentoSalas.API.Models;
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
        [Route("api/[controller]/ObterTodos")]
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
        [Route("api/[controller]/Inserir")]
        public IActionResult Inserir([FromBody] CriarSalaRequest input)
        {
            try
            {
                if (input == null) return BadRequest("Dados inválidos.");

                var sala = new Sala
                {
                    Nome = input.Nome,
                    Capacidade = input.Capacidade,
                    Ativa = input.Ativa
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
    }
}