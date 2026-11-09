using GerenciamentoSalas.API.Common.Repositorie;
using GerenciamentoSalas.API.Common.Service.Interface;

namespace GerenciamentoSalas.API.Common.Service
{
    public class CommonService : ICommonService
    {
        private readonly ILogger<CommonService> _logger;
        private readonly CommonRepository _commonRepository;
        public CommonService(ILogger<CommonService> logger, CommonRepository commonRepository)
        {
            _logger = logger;
            _commonRepository = commonRepository;
        }
        public async  Task<bool> VerificarDisponibilidade<T>(T entidade, DateTime DataInicio, DateTime DataFim, CancellationToken cancellationToken = default) {
            bool disponivel = await _commonRepository.VerificarDisponibilidade(entidade, DataInicio, DataFim, cancellationToken);
            return disponivel;
        }
    }
}
