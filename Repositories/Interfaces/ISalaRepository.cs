using GerenciamentoSalas.API.Models.DTOs;

namespace GerenciamentoSalas.API.Repositories.Interfaces
{
    public interface ISalaRepository
    {
        public Task<SalaDto?> ObterSalaPorIdAsync(int id, CancellationToken cancellationToken = default);
        public Task<List<SalaDto>> ObterTodosSalaAsync(CancellationToken cancellationToken = default);
        public Task<int> InserirSalaAsync(SalaDto sala, CancellationToken cancellationToken = default);
        public Task<SalaDto?> AtualizarSalaAsync(AtualizarSalaDto atualizarSalaDto, CancellationToken cancellationToken = default);
        public Task<(int agendamentosDeletados, int salaId)> DeletarSalaAsync(SalaDto sala, CancellationToken cancellationToken = default);
    }
}
