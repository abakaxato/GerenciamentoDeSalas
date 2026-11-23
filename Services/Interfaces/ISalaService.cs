using GerenciamentoSalas.API.Models.DTOs;
using GerenciamentoSalas.API.Models.Requests;

namespace GerenciamentoSalas.API.Services.Interfaces
{
    public interface ISalaService
    {
        public Task<SalaDto?> ObterSalaPorIdAsync(int id, CancellationToken cancellationToken = default);
        public Task<List<SalaDto>> ObterTodosSalaAsync(CancellationToken cancellationToken = default);
        public Task<int> InserirSalaAsync(CriarSalaRequest criarSalaRequest, CancellationToken cancellationToken = default);
        public Task<SalaDto?> AtualizarSalaAsync(int id, AtualizarSalaRequest request, CancellationToken cancellationToken = default);
        public Task<int> DeletarSalaAsync(int id, CancellationToken cancellationToken = default);
    }
}
