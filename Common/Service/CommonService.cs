using GerenciamentoSalas.API.Common.Repositorie;
using GerenciamentoSalas.API.Common.Repositorie.interfaces;
using GerenciamentoSalas.API.Common.Service.Interface;

namespace GerenciamentoSalas.API.Common.Service
{
    public class CommonService : ICommonService
    {
        private readonly ICommonRepository _commonRepository;
        public CommonService(ICommonRepository commonRepository)
        {
            _commonRepository = commonRepository;
        }
        public async  Task<bool> VerificarDisponibilidade<T>(T entidade, DateTime DataInicio, DateTime DataFim, CancellationToken cancellationToken = default) {
            bool disponivel = await _commonRepository.VerificarDisponibilidadeAsync(entidade, DataInicio, DataFim, cancellationToken);
            return disponivel;
        }
    }
}
