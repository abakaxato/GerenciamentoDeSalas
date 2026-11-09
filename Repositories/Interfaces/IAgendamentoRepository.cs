using GerenciamentoSalas.API.Models;

namespace GerenciamentoSalas.API.Repositories.Interfaces
{
    public interface IAgendamentoRepository
    {
        public Task<Agendamento?> ObterAgendamentoPorIDAsync(int id,CancellationToken cancellationToken = default);
        public Task<List<Agendamento>> ObterTodosAgendamentoAsync(CancellationToken cancellationToken = default);
        public Task<int> InserirAgendamentoAsync(Agendamento agendamento, CancellationToken cancellationToken = default);

        public Task<int> DeletarAgendamentoAsync(Agendamento agendamento, CancellationToken cancellationToken = default);
    }
}
