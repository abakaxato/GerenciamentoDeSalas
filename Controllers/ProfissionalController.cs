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
        private readonly ProfissionalRepository _ProfissionalRepository;

        public ProfissionalController(IConfiguration configuration)
        {
            _ProfissionalRepository = new ProfissionalRepository(configuration);
        }

        [HttpGet]
        [Route("api/[controller]/ObterTodos")]
        public IActionResult ObterTodos()
        {
            try
            {
                var profissionais = _ProfissionalRepository.ObterTodosProfissional();
                return Ok(profissionais);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno no servidor ao buscar profissionais: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("api/[controller]/Inserir")]
        public IActionResult Inserir([FromBody] CriarProfissionalRequest input)
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

                int idGerado = _ProfissionalRepository.InserirProfissional(profissional);
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