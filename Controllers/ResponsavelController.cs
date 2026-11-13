using GerenciamentoSalas.API.Models.DTOs;
using GerenciamentoSalas.API.Models.Requests;
using GerenciamentoSalas.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GerenciamentoSalas.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResponsavelController : ControllerBase
    {
        private readonly ResponsavelRepository _ResponsavelRepository;

        public ResponsavelController(IConfiguration configuration)
        {
            _ResponsavelRepository = new ResponsavelRepository(configuration);
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            try
            {
                var responsaveis = await _ResponsavelRepository.ObterTodosResponsavelAsync();
                return Ok(responsaveis);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno no servidor: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Inserir([FromBody] CriarResponsavelRequest input)
        {
            try
            {
                if (input == null) return BadRequest("Dados inválidos.");

                var novoResponsavel = new ResponsavelDto
                {
                    Nome = input.Nome,
                    Telefone = input.Telefone,
                    Email = input.Email
                };

                int idGerado = await _ResponsavelRepository.InserirAsync(novoResponsavel);

                novoResponsavel.Id = idGerado;
                novoResponsavel.DataCadastro = DateTime.Now;

                return StatusCode(201, novoResponsavel);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao salvar: {ex.Message}");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Deletar([FromBody]int id)
        {
            try
            {
                /* var (agendamentosDeletados, idsPacientesDeletados, responsavelDeletado) =
                     await _ResponsavelRepository.DeletarResponsavelAsync(request);

                 return Ok(new
                 {
                     AgendamentosDeletados = agendamentosDeletados,
                     PacientesDeletados = idsPacientesDeletados.Count,
                     IdsPacientesDeletados = idsPacientesDeletados,
                     ResponsavelDeletado = responsavelDeletado
                 });*/
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao deletar Responsavel: {ex.Message}");
            }
        }
    }
}