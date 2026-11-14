using GerenciamentoSalas.API.Models;

namespace GerenciamentoSalas.API.Common.Repositorie.interfaces
{
    public interface ICommonRepository
    {
        public Task<bool> VerificarDisponibilidadeAsync<T>(T entidade, DateTime dataInicio, DateTime dataFim, int? agendamentoIdIgnorar = null, CancellationToken cancellationToken = default);
        public Task<bool> VerificarExistenciaAsync<T>(T entidade, CancellationToken cancellationToken = default);
    }
}
