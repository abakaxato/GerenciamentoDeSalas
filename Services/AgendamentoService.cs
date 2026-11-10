using GerenciamentoSalas.API.Common.Service.Interface;
using GerenciamentoSalas.API.DTOs;
using GerenciamentoSalas.API.Enums;
using GerenciamentoSalas.API.Exceptions;
using GerenciamentoSalas.API.Models;
using GerenciamentoSalas.API.Repositories.Interfaces;
using GerenciamentoSalas.API.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace GerenciamentoSalas.API.Services
{
    public class AgendamentoService : IAgendamentoService
    {
        private readonly IAgendamentoRepository _agendamentoRepository;
        private readonly ICommonService _commonService;
        private readonly ILogger<IAgendamentoService> _logger;
        public AgendamentoService(IAgendamentoRepository agendamentoRepository, ILogger<AgendamentoService> logger, ICommonService commonService)
        {
            _agendamentoRepository = agendamentoRepository;
            _commonService = commonService;
            _logger = logger;
        }

        public async Task<Agendamento?> ObterAgendamentoPorIDAsync(int id, CancellationToken cancellationToken = default)
        {
            Agendamento? agendamento = await _agendamentoRepository.ObterAgendamentoPorIDAsync(id, cancellationToken);
            return agendamento;
        }

        public async Task<List<Agendamento>> ObterTodosAgendamentoAsync(CancellationToken cancellationToken = default)
        {
            List<Agendamento> agendamentos = await _agendamentoRepository.ObterTodosAgendamentoAsync(cancellationToken);
            return agendamentos;
        }

        public async Task<int> InserirAgendamentoAsync(CriarAgendamentoRequest request, CancellationToken cancellationToken = default)
        {
            if (request == null)
            {
                _logger.LogError("Erro ao inserir agendamento, requsição vazia");
                throw new ArgumentNullException($"{request}");
            }

            if (request.DataHoraFim <= request.DataHoraInicio)
            {
                _logger.LogError("Erro ao inserir agendamento, a data de término deve ser maior que a data de início.");
                throw new InvalidOperationException("Erro ao inserir agendamento, a data de término deve ser maior que a data de início.");
            }

            Sala sala = new() { Id = request.SalaId};
            bool disponibilidadeSala = await _commonService.VerificarDisponibilidade(sala, request.DataHoraInicio,request.DataHoraFim);
            if (!disponibilidadeSala) { throw new ConflitoHorarioException("Já existe um agendamento para este horário nesta sala");}

            Paciente paciente = new() { Id = request.PacienteId };
            bool disponibilidadePaciente = await _commonService.VerificarDisponibilidade(paciente, request.DataHoraInicio, request.DataHoraFim);
            if (!disponibilidadePaciente) { throw new ConflitoHorarioException("Já existe um agendamento para este horário com esse paciente"); }
            
            Profissional profissional = new() { Id = request.ProfissionalId };
            bool disponibilidadeProfissional = await _commonService.VerificarDisponibilidade(profissional, request.DataHoraInicio, request.DataHoraFim);
            if (!disponibilidadeProfissional) { throw new ConflitoHorarioException("Já existe um agendamento para este horário com esse profisional");}

            Agendamento agendamento = new()
            {
                PacienteId = request.PacienteId,
                ProfissionalId = request.ProfissionalId,
                SalaId = request.SalaId,
                DataHoraInicio = request.DataHoraInicio,
                DataHoraFim = request.DataHoraFim,
                Observacoes = request.Observacoes,
                Status = request.Status.ToString().IsNullOrEmpty() ? StatusAgendamentoEnum.Pendente.ToString() : request.Status.ToString()
            };

            try
            {
                await _agendamentoRepository.InserirAgendamentoAsync(agendamento,cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao inserir agendamento");
                throw;
            }
            return agendamento.Id;
        }

        public async Task<int> DeletarAgendamentoAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                Agendamento? agendamento = await _agendamentoRepository.ObterAgendamentoPorIDAsync(id, cancellationToken);
                if (agendamento is null)
                {
                    _logger.LogError("A pesquisa pelo agendamento no banco não retornou nenhuma linha, deleção incompleta");
                    throw new KeyNotFoundException("Agendamento não encontrado.");
                }

                _logger.LogInformation($"O registro no banco foi encontrado e vai ser deletado");

                await _agendamentoRepository.DeletarAgendamentoAsync(agendamento,cancellationToken);

                _logger.LogInformation($"Agendamento com ID {agendamento.Id} removido com sucesso");
                return agendamento.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar agendamento");
                throw;
            }
}
    }
}
