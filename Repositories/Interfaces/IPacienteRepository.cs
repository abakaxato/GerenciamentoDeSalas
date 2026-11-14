using GerenciamentoSalas.API.Models.DTOs;

namespace GerenciamentoSalas.API.Repositories.Interfaces
{
    public interface IPacienteRepository
    {
        public Task<PacienteDto?> ObterPacientePorIdAsync(int id, CancellationToken cancellationToken = default);
        public Task<List<PacienteDto>> ObterTodosPacienteAsync(CancellationToken cancellationToken = default);
        public Task<int> InserirPacienteAsync(PacienteDto paciente, CancellationToken cancellationToken = default);
        public Task<int> DeletarPacienteAsync(PacienteDto paciente, CancellationToken cancellationToken = default);
    }
}
