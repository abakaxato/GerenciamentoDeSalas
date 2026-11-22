using GerenciamentoSalas.API.Models.DTOs;
using GerenciamentoSalas.API.Models.Requests;
using GerenciamentoSalas.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GerenciamentoSalas.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PacienteController(IPacienteService pacienteService) : ControllerBase
    {
        private readonly IPacienteService _pacienteService = pacienteService;
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> ObterPorID([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            PacienteDto? paciente = await _pacienteService.ObterPacientePorIdAsync(id, cancellationToken);
            if (paciente == null) { return NotFound($"Paciente com ID {id} não encontrado"); }
            else { return Ok(paciente); }
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos(CancellationToken cancellationToken = default)
        {
            List<PacienteDto> pacientes = await _pacienteService.ObterTodosPacienteAsync(cancellationToken);
            return Ok(pacientes);
        }

        [HttpPost]
        public async Task<IActionResult> Inserir([FromBody] CriarPacienteRequest criarPacienteRequest, CancellationToken cancellationToken = default)
        {
            int idGerado = await _pacienteService.InserirPacienteAsync(criarPacienteRequest, cancellationToken);

            return CreatedAtAction
                (
                nameof(ObterPorID),
                new { id = idGerado },
                new { Id = idGerado }
                );
        }

        [HttpPatch]
        [Route("{id:int}")]
        public async Task<IActionResult> Atualizar([FromRoute] int id, [FromBody] AtualizarPacienteRequest atualizarPacienteRequest, CancellationToken cancellationToken = default)
        {
            PacienteDto? pacienteAtualizado = await _pacienteService.AtualizarPacienteAsync(id, atualizarPacienteRequest, cancellationToken);

            return Ok(pacienteAtualizado);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Deletar([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            int idDeletado = await _pacienteService.DeletarPacienteAsync(id, cancellationToken);
            return Ok(idDeletado);
        }
    }
}
