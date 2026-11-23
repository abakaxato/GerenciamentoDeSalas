using GerenciamentoSalas.API.Models.DTOs;

namespace GerenciamentoSalas.API.Repositories.Interfaces
{
    public interface IProfissionalRepository
    {
        public Task<ProfissionalDto?> ObterProfissionalPorIdAsync(int id, CancellationToken cancellationToken = default);
        public Task<List<ProfissionalDto>> ObterTodosProfissionalAsync(CancellationToken cancellationToken = default);
        public Task<int> InserirProfissionalAsync(ProfissionalDto profissional, CancellationToken cancellationToken = default);
        public Task<ProfissionalDto?> AtualizarProfissionalAsync(AtualizarProfissionalDto atualizarProfissionalDto, CancellationToken cancellationToken = default);
        public Task<(int agendamentosDeletados, int profissionalId)> DeletarProfissionalAsync(ProfissionalDto profissional, CancellationToken cancellationToken = default);
    }
}
