using GerenciamentoSalas.API.Models.DTOs;
using GerenciamentoSalas.API.Models.Requests;

namespace GerenciamentoSalas.API.Services.Interfaces
{
    public interface IProfissionalService
    {
        public Task<ProfissionalDto?> ObterProfissionalPorIdAsync(int id, CancellationToken cancellationToken = default);
        public Task<List<ProfissionalDto>> ObterTodosProfissionalAsync(CancellationToken cancellationToken = default);
        public Task<int> InserirProfissionalAsync(CriarProfissionalRequest criarProfissionalRequest, CancellationToken cancellationToken = default);
        public Task<ProfissionalDto?> AtualizarProfissionalAsync(int id, AtualizarProfissionalRequest request, CancellationToken cancellationToken = default);
        public Task<int> DeletarProfissionalAsync(int id, CancellationToken cancellationToken = default);
    }
}
