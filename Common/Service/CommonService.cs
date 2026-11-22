using GerenciamentoSalas.API.Common.Repository.interfaces;
using GerenciamentoSalas.API.Common.Service.Interface;

namespace GerenciamentoSalas.API.Common.Service
{
    public class CommonService : ICommonService
    {
        private readonly ICommonRepository _commonRepository;
        private readonly ILogger<ICommonService> _logger;
        public CommonService(ICommonRepository commonRepository, ILogger<ICommonService> logger)
        {
            _commonRepository = commonRepository;
            _logger = logger;
        }
        public async Task<bool> VerificarDisponibilidadeAsync<T>(T entidade, DateTime dataInicio, DateTime dataFim, int? agendamentoIdIgnorar = null, CancellationToken cancellationToken = default)
        {
            bool disponivel = await _commonRepository.VerificarDisponibilidadeAsync(entidade, dataInicio, dataFim, agendamentoIdIgnorar, cancellationToken);
            if (!disponivel) { _logger.LogInformation($"{entidade} nao esta dinponivel"); }
            return disponivel;
        }
        public async Task<bool> VerificarExistenciaAsync<T>(T entidade, CancellationToken cancellationToken = default)
        {
            bool existe = await _commonRepository.VerificarExistenciaAsync(entidade, cancellationToken);
            if (!existe) { _logger.LogInformation("{entidade} nao existe", entidade); throw new KeyNotFoundException("Entidade não encontrada na base de dados"); }
            return existe;
        }
        public async Task<bool> VerificarExistenciaDisponibilidadeAsync<T>(T entidade, DateTime DataInicio, DateTime DataFim, int? agendamentoIdIgnorar = null, CancellationToken cancellationToken = default)
        {
            bool existe = await VerificarExistenciaAsync(entidade, cancellationToken);
            bool disponivel = await VerificarDisponibilidadeAsync(entidade, DataInicio, DataFim, agendamentoIdIgnorar, cancellationToken);

            if (existe && disponivel)
            {
                return true;
            }
            else return false;
        }
        public async Task<bool> VerficarAgendamentosAtivos<T>(T entidade, CancellationToken cancellationToken = default)
        {
            bool existe = await _commonRepository.VerficarAgendamentosAtivos(entidade, cancellationToken);
            return existe;
        }
    }
}
