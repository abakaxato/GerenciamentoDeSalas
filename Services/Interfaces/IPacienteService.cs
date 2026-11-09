namespace GerenciamentoSalas.API.Services.Interfaces
{
    public interface IPacienteService
    {
        public Task<bool> VerificarDisponibilidadeAsync(int id, DateTime DataInicio, DateTime DataFim, CancellationToken cancellationToken = default);
    }
}
