using GerenciamentoSalas.API.Common.Exceptions;
using GerenciamentoSalas.API.Common.Repositorie.interfaces;
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
            if (!existe) { _logger.LogInformation($"{entidade} nao existe"); throw new ExistenciaNaTabelaException(entidade); }
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
    }
}
