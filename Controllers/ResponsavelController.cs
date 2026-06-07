using GerenciamentoSalas.API.DTOs;
using GerenciamentoSalas.API.Models;
using GerenciamentoSalas.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GerenciamentoSalas.API.Controllers
{
    // 1. Define que esta classe é um Controlador de API e ativa comportamentos automáticos de validação
    [ApiController]
    // 2. Define a rota de acesso padrão baseada no nome do controller: "api/responsavel"
    [Route("api/[controller]")]
    public class ResponsavelController : ControllerBase
    {
        private readonly ResponsavelRepository _Responsavelrepository;

        // 3. O construtor recebe o IConfiguration para repassar ao repositório
        public ResponsavelController(IConfiguration configuration)
        {
            _Responsavelrepository = new ResponsavelRepository(configuration);
        }

        // 4. Mapeia as requisições HTTP GET para este método
        [HttpGet]
        public IActionResult ObterTodos()
        {
            try
            {
                // 5. Aciona o repositório para buscar a lista do banco
                var responsaveis = _Responsavelrepository.ObterTodos();

                // 6. Retorna o Status HTTP 200 (OK) e serializa a lista automaticamente em formato JSON
                return Ok(responsaveis);
            }
            catch (Exception ex)
            {
                // 7. Em caso de falha catastrófica (ex: banco fora do ar), retorna HTTP 500 (Internal Server Error)
                return StatusCode(500, $"Erro interno no servidor: {ex.Message}");
            }
        }
        [HttpPost]
        public IActionResult InserirResponsavel([FromBody] CriarResponsavelRequest input) // Aceita o DTO aqui
        {
            try
            {
                if (input == null) return BadRequest("Dados inválidos.");

                // 2. MAPEAMENTO: Você extrai as variáveis que quis e monta o Model real
                var novoResponsavel = new Responsavel
                {
                    Nome = input.Nome,
                    Telefone = input.Telefone,
                    Email = input.Email
                    // O Id começa como 0 e a DataCadastro como DateTime.MinValue automaticamente no C#
                };

                // 3. O Repository insere no SQL Server e te devolve o ID gerado pelo IDENTITY
                int idGerado = _Responsavelrepository.Inserir(novoResponsavel);

                // 4. Preenchemos o Model com os dados reais gerados pelo banco de dados
                novoResponsavel.Id = idGerado;
                novoResponsavel.DataCadastro = DateTime.Now; // Apenas para exibir na resposta o horário aproximado

                // 5. RESPOSTA COMPLETA: O usuário recebe o Model cheio, com o ID e a Data criados!
                return StatusCode(201, novoResponsavel);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao salvar: {ex.Message}");
            }
        }
    }
}