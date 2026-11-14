using GerenciamentoSalas.API.Models.DTOs;
using GerenciamentoSalas.API.Models.Requests;

namespace GerenciamentoSalas.API.Services.Interfaces
{
    public interface IAgendamentoService
    {
        public Task<AgendamentoDto?> ObterAgendamentoPorIDAsync(int id, CancellationToken cancellationToken = default);
        public Task<List<AgendamentoDto>> ObterTodosAgendamentoAsync(CancellationToken cancellationToken = default);
        public Task<int> InserirAgendamentoAsync(CriarAgendamentoRequest criarAgendamentoRequest, CancellationToken cancellationToken = default);
        public Task<AgendamentoDto> AtualizarAgendamentoAsync(int id, AtualizarAgendamentoRequest atualizarAgendamentoRequest, CancellationToken cancellationToken = default);
        public Task<int> DeletarAgendamentoAsync(int id, CancellationToken cancellationToken = default);
    }
}
