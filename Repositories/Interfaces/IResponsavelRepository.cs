using GerenciamentoSalas.API.Models.DTOs;

namespace GerenciamentoSalas.API.Repositories.Interfaces
{
    public interface IResponsavelRepository
    {
        public Task<ResponsavelDto?> ObterResponsavelPorIdAsync(int id, CancellationToken cancellationToken = default);
        public Task<List<ResponsavelDto>> ObterTodosResponsavelAsync(CancellationToken cancellationToken = default);
        public Task<int> InserirResponsavelAsync(ResponsavelDto responsavel, CancellationToken cancellationToken = default);
        public Task<ResponsavelDto?> AtualizarResponsavelAsync(AtualizarResponsavelDto atualizarResponsavelDto, CancellationToken cancellationToken = default);
        public Task<(int pacientesDeletados, int agendamentosDeletados, int responsavelId)> DeletarResponsavelAsync(ResponsavelDto responsavel, CancellationToken cancellationToken = default);
    }
}
