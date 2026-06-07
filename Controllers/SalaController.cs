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

        [HttpPost]
        public IActionResult Criar([FromBody] CriarSalaRequest input)
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

                int idGerado = _repository.Inserir(sala);
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