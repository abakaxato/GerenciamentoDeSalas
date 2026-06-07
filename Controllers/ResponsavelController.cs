using Microsoft.AspNetCore.Mvc;
using GerenciamentoSalas.API.Repositories;

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
    }
}