using GerenciamentoSalas.API.DTOs;
using GerenciamentoSalas.API.Models;
using GerenciamentoSalas.API.Repositories;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost]
        public IActionResult Criar([FromBody] CriarProfissionalRequest input)
        {
            try
            {
                if (input == null) return BadRequest("Dados inválidos.");

                var profissional = new Profissional
                {
                    Nome = input.Nome,
                    Especialidade = input.Especialidade,
                    Ativo = input.Ativo
                };

                int idGerado = _repository.Inserir(profissional);
                profissional.Id = idGerado;

                return StatusCode(201, profissional);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao salvar profissional: {ex.Message}");
            }
        }
    }
}