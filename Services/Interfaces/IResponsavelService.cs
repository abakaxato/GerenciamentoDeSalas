using GerenciamentoSalas.API.Models.DTOs;
using GerenciamentoSalas.API.Models.Requests;

namespace GerenciamentoSalas.API.Services.Interfaces
{
    public interface IResponsavelService
    {
        public Task<ResponsavelDto?> ObterResponsavelPorIdAsync(int id, CancellationToken cancellationToken = default);
        public Task<List<ResponsavelDto>> ObterTodosResponsavelAsync(CancellationToken cancellationToken = default);
        public Task<int> InserirResponsavelAsync(CriarResponsavelRequest criarResponsavelRequest, CancellationToken cancellationToken = default);
        public Task<ResponsavelDto?> AtualizarResponsavelAsync(int id, AtualizarResponsavelRequest request, CancellationToken cancellationToken = default);
        public Task<int> DeletarResponsavelAsync(int id, CancellationToken cancellationToken = default);
    }
}
