using GerenciamentoSalas.API.Common.Exceptions;
using GerenciamentoSalas.API.Common.Repositorie;
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
        public async  Task<bool> VerificarDisponibilidade<T>(T entidade, DateTime DataInicio, DateTime DataFim, CancellationToken cancellationToken = default) {
            bool disponivel = await _commonRepository.VerificarDisponibilidadeAsync(entidade, DataInicio, DataFim, cancellationToken);
            if (!disponivel) { _logger.LogInformation($"{entidade} nao esta dinponivel"); }
            return disponivel;
        }
        public async  Task<bool> VerificarExistencia<T>(T entidade, CancellationToken cancellationToken = default) {
            bool disponivel = await _commonRepository.VerificarExistencia(entidade, cancellationToken);
            if (!disponivel) { _logger.LogInformation($"{entidade} nao existe"); throw new ExistenciaException(entidade);}
            return disponivel;
        }
        public async  Task<bool> VerificarExistenciaDisponibilidade<T>(T entidade, DateTime DataInicio, DateTime DataFim, CancellationToken cancellationToken = default) {
            
            bool existe = await VerificarExistencia(entidade,cancellationToken);
            bool disponivel = await VerificarDisponibilidade(entidade, DataInicio, DataFim, cancellationToken);
            
            if (existe & disponivel) 
            {
                return true; 
            }
            else return false;

        }
    }
}
