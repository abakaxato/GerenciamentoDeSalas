using GerenciamentoSalas.API.DTOs;
using GerenciamentoSalas.API.Models;
using GerenciamentoSalas.API.Requests;
using Microsoft.EntityFrameworkCore;

namespace GerenciamentoSalas.API.Repositories.Interfaces
{
    public interface IAgendamentoRepository
    {
        public Task<List<Agendamento>> ObterTodosAgendamentoAsync(CancellationToken cancellationToken = default);

        public Task<int> InserirAgendamentoAsync(CriarAgendamentoRequest agendamentoRequest, CancellationToken cancellationToken = default);

        public Task<int> DeletarAgendamentoAsync(DeletarAgendamentoRequest request, CancellationToken cancellationToken = default);
    }
}
