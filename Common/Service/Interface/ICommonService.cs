namespace GerenciamentoSalas.API.Common.Service.Interface
{
    public interface ICommonService
    {
        public Task<bool> VerificarDisponibilidade<T>(T entidade, DateTime DataInicio, DateTime DataFim, CancellationToken cancellationToken = default);
        public Task<bool> VerificarExistencia<T>(T entidade, CancellationToken cancellationToken = default);
        public Task<bool> VerificarExistenciaDisponibilidade<T>(T entidade, DateTime DataInicio, DateTime DataFim, CancellationToken cancellationToken = default);
    }
}
