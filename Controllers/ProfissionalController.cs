using Microsoft.AspNetCore.Mvc;

namespace GerenciamentoSalas.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfissionalController : ControllerBase
    {
        /*private readonly ProfissionaisRepository _ProfissionaisRepository;

        public ProfissionalController(IConfiguration configuration)
        {
            _ProfissionaisRepository = new ProfissionaisRepository(configuration);
        }

        [HttpGet] TempDataAttribute tem que colocar as coisas no singular
        [Route("api/[controller]")]
        public IActionResult ObterTodos()
        {
            try
            {
                var profissionais = _ProfissionaisRepository.ObterTodosProfissional();
                return Ok(profissionais);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno no servidor ao buscar profissionais: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("api/[controller]")]
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

                int idGerado = _ProfissionaisRepository.InserirProfissional(profissional);
                profissional.Id = idGerado;

                return StatusCode(201, profissional);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao salvar profissional: {ex.Message}");
            }
        }

        [HttpDelete]
        [Route("api/[controller]")]
        public async Task<IActionResult> Deletar([FromBody] int ProfissionaisId)
        {
            try
            {
                var request = new DeletarProfissionalRequest { Id = ProfissionaisId };
                var (agendamentosDeletados, profissionaisDeletados) =
                    await _ProfissionaisRepository.DeletarProfissionaisAsync(request);

                return Ok(new
                {
                    AgendamentosDeletados = agendamentosDeletados,
                    ProfissionaisDeletados = profissionaisDeletados
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao deletar Profissionais: {ex.Message}");
            }
        }
    */
    }
}