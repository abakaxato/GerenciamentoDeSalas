namespace GerenciamentoSalas.API.Common.Service.Interface
{
    public interface ICommonService
    {
        public Task<bool> VerificarDisponibilidadeAsync<T>(T entidade, DateTime DataInicio, DateTime DataFim, int? agendamentoIdIgnorar = null, CancellationToken cancellationToken = default);
        public Task<bool> VerificarExistenciaAsync<T>(T entidade, CancellationToken cancellationToken = default);
        public Task<bool> VerificarExistenciaDisponibilidadeAsync<T>(T entidade, DateTime DataInicio, DateTime DataFim, int? agendamentoIdIgnorar = null, CancellationToken cancellationToken = default);
    }
}
