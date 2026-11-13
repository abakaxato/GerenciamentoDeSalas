using GerenciamentoSalas.API.Common.Service.Interface;
using GerenciamentoSalas.API.Models.DTOs;
using GerenciamentoSalas.API.Services.Interfaces;

namespace GerenciamentoSalas.API.Services
{
    public class PacienteService : IPacienteService
    {
        private readonly ICommonService _commonService;
        public PacienteService(ICommonService commonService)
        {
            _commonService = commonService;
        }
        public async Task<bool> VerificarDisponibilidadeAsync(int id, DateTime DataInicio, DateTime DataFim, CancellationToken cancellationToken = default)
        {
            //Aqui teoricamente o paciente foi validadado, método atualmente implementado como teste
            PacienteDto pacienteTeste = new() { Id = id };
            bool disponibilidade = await _commonService.VerificarDisponibilidade(pacienteTeste,DataInicio,DataFim);
            return disponibilidade;
        }
    }
}
