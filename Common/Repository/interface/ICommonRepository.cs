using GerenciamentoSalas.API.Models;

namespace GerenciamentoSalas.API.Common.Repositorie.interfaces
{
    public interface ICommonRepository
    {
        public Task<bool> VerificarDisponibilidade<T>(T entidade, DateTime DataInicio, DateTime DataFim, CancellationToken cancellationToken = default);
    }
}
