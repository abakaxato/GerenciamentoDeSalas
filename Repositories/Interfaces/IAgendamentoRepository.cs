using GerenciamentoSalas.API.Models.DTOs;

namespace GerenciamentoSalas.API.Repositories.Interfaces
{
    public interface IAgendamentoRepository
    {
        public Task<AgendamentoDto?> ObterAgendamentoPorIdAsync(int id,CancellationToken cancellationToken = default);
        public Task<List<AgendamentoDto>> ObterTodosAgendamentoAsync(CancellationToken cancellationToken = default);
        public Task<int> InserirAgendamentoAsync(AgendamentoDto agendamento, CancellationToken cancellationToken = default);
        public Task<AgendamentoDto> AtualizarAgendamentoAsync(AtualizarAgendamentoDto atualizarAgendamento, CancellationToken cancellationToken = default);
        public Task<int> DeletarAgendamentoAsync(AgendamentoDto agendamento, CancellationToken cancellationToken = default);
    }
}
