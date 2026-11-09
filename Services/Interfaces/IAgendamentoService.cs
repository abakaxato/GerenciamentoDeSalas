using GerenciamentoSalas.API.DTOs;
using GerenciamentoSalas.API.Models;
using GerenciamentoSalas.API.Requests;

namespace GerenciamentoSalas.API.Services.Interfaces
{
    public interface IAgendamentoService
    {
        public Task<Agendamento?> ObterAgendamentoPorIDAsync(int id, CancellationToken cancellationToken = default);
        public Task<List<Agendamento>> ObterTodosAgendamentoAsync(CancellationToken cancellationToken = default);

        public Task<int> InserirAgendamentoAsync(CriarAgendamentoRequest agendamentoRequest, CancellationToken cancellationToken = default);

        public Task<int> DeletarAgendamentoAsync(DeletarAgendamentoRequest request, CancellationToken cancellationToken = default);
    }
}
