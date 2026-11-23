using GerenciamentoSalas.API.Models.DTOs;
using GerenciamentoSalas.API.Models.Requests;
using GerenciamentoSalas.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GerenciamentoSalas.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResponsavelController(IResponsavelService responsavelService) : ControllerBase
    {
        private readonly IResponsavelService _responsavelService = responsavelService;
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> ObterPorID([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            ResponsavelDto? responsavel = await _responsavelService.ObterResponsavelPorIdAsync(id, cancellationToken);
            if (responsavel == null) { return NotFound($"Responsavel com ID {id} não encontrado"); }
            else { return Ok(responsavel); }
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos(CancellationToken cancellationToken = default)
        {
            List<ResponsavelDto> responsavels = await _responsavelService.ObterTodosResponsavelAsync(cancellationToken);
            return Ok(responsavels);
        }

        [HttpPost]
        public async Task<IActionResult> Inserir([FromBody] CriarResponsavelRequest criarResponsavelRequest, CancellationToken cancellationToken = default)
        {
            int idGerado = await _responsavelService.InserirResponsavelAsync(criarResponsavelRequest, cancellationToken);

            return CreatedAtAction
                (
                nameof(ObterPorID),
                new { id = idGerado },
                new { Id = idGerado }
                );
        }

        [HttpPatch]
        [Route("{id:int}")]
        public async Task<IActionResult> Atualizar([FromRoute] int id, [FromBody] AtualizarResponsavelRequest atualizarResponsavelRequest, CancellationToken cancellationToken = default)
        {
            ResponsavelDto? responsavelAtualizado = await _responsavelService.AtualizarResponsavelAsync(id, atualizarResponsavelRequest, cancellationToken);

            return Ok(responsavelAtualizado);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Deletar([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            int idDeletado = await _responsavelService.DeletarResponsavelAsync(id, cancellationToken);
            return Ok(idDeletado);
        }
    }
}
