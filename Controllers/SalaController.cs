using GerenciamentoSalas.API.Models.DTOs;
using GerenciamentoSalas.API.Models.Requests;
using GerenciamentoSalas.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GerenciamentoSalas.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalaController(ISalaService salaService) : ControllerBase
    {
        private readonly ISalaService _salaService = salaService;
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> ObterPorID([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            SalaDto? sala = await _salaService.ObterSalaPorIdAsync(id, cancellationToken);
            if (sala == null) { return NotFound($"Sala com ID {id} não encontrado"); }
            else { return Ok(sala); }
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos(CancellationToken cancellationToken = default)
        {
            List<SalaDto> salas = await _salaService.ObterTodosSalaAsync(cancellationToken);
            return Ok(salas);
        }

        [HttpPost]
        public async Task<IActionResult> Inserir([FromBody] CriarSalaRequest criarSalaRequest, CancellationToken cancellationToken = default)
        {
            int idGerado = await _salaService.InserirSalaAsync(criarSalaRequest, cancellationToken);

            return CreatedAtAction
                (
                nameof(ObterPorID),
                new { id = idGerado },
                new { Id = idGerado }
                );
        }

        [HttpPatch]
        [Route("{id:int}")]
        public async Task<IActionResult> Atualizar([FromRoute] int id, [FromBody] AtualizarSalaRequest atualizarSalaRequest, CancellationToken cancellationToken = default)
        {
            SalaDto? salaAtualizado = await _salaService.AtualizarSalaAsync(id, atualizarSalaRequest, cancellationToken);

            return Ok(salaAtualizado);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Deletar([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            int idDeletado = await _salaService.DeletarSalaAsync(id, cancellationToken);
            return Ok(idDeletado);
        }
    }
}
