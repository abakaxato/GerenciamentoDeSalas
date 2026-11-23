using GerenciamentoSalas.API.Models.DTOs;
using GerenciamentoSalas.API.Models.Requests;
using GerenciamentoSalas.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GerenciamentoSalas.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfissionalController(IProfissionalService profissionalService) : ControllerBase
    {
        private readonly IProfissionalService _profissionalService = profissionalService;
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> ObterPorID([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            ProfissionalDto? profissional = await _profissionalService.ObterProfissionalPorIdAsync(id, cancellationToken);
            if (profissional == null) { return NotFound($"Profissional com ID {id} não encontrado"); }
            else { return Ok(profissional); }
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos(CancellationToken cancellationToken = default)
        {
            List<ProfissionalDto> profissionals = await _profissionalService.ObterTodosProfissionalAsync(cancellationToken);
            return Ok(profissionals);
        }

        [HttpPost]
        public async Task<IActionResult> Inserir([FromBody] CriarProfissionalRequest criarProfissionalRequest, CancellationToken cancellationToken = default)
        {
            int idGerado = await _profissionalService.InserirProfissionalAsync(criarProfissionalRequest, cancellationToken);

            return CreatedAtAction
                (
                nameof(ObterPorID),
                new { id = idGerado },
                new { Id = idGerado }
                );
        }

        [HttpPatch]
        [Route("{id:int}")]
        public async Task<IActionResult> Atualizar([FromRoute] int id, [FromBody] AtualizarProfissionalRequest atualizarProfissionalRequest, CancellationToken cancellationToken = default)
        {
            ProfissionalDto? profissionalAtualizado = await _profissionalService.AtualizarProfissionalAsync(id, atualizarProfissionalRequest, cancellationToken);

            return Ok(profissionalAtualizado);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Deletar([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            int idDeletado = await _profissionalService.DeletarProfissionalAsync(id, cancellationToken);
            return Ok(idDeletado);
        }
    }
}
