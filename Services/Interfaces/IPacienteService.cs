using GerenciamentoSalas.API.Models.DTOs;
using GerenciamentoSalas.API.Models.Requests;

namespace GerenciamentoSalas.API.Services.Interfaces
{
    public interface IPacienteService
    {
        public Task<PacienteDto?> ObterPacientePorIdAsync(int id, CancellationToken cancellationToken = default);
        public Task<List<PacienteDto>> ObterTodosPacienteAsync(CancellationToken cancellationToken = default);
        public Task<int> InserirPacienteAsync(CriarPacienteRequest criarPacienteRequest, CancellationToken cancellationToken = default);
        public Task<PacienteDto?> AtualizarPacienteAsync(int id, AtualizarPacienteRequest request, CancellationToken cancellationToken = default);
        public Task<int> DeletarPacienteAsync(int id, CancellationToken cancellationToken = default);
    }
}
