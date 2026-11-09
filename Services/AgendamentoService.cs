using GerenciamentoSalas.API.DTOs;
using GerenciamentoSalas.API.Models;
using GerenciamentoSalas.API.Repositories;
using GerenciamentoSalas.API.Repositories.Interfaces;
using GerenciamentoSalas.API.Requests;
using GerenciamentoSalas.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GerenciamentoSalas.API.Services
{
    public class AgendamentoService : IAgendamentoService
    {
        private readonly IAgendamentoRepository _agendamentoRepository;
        private readonly ILogger<AgendamentoService> _logger;

        public AgendamentoService(IAgendamentoRepository agendamentoRepository, ILogger<AgendamentoService> logger)
        {
            _agendamentoRepository = agendamentoRepository;
            _logger = logger;
        }

        public async Task<Agendamento> ObterAgendamentoPorIDAsync(int id, CancellationToken cancellationToken = default)
        {
            Agendamento agendamento = await _agendamentoRepository.ObterAgendamentoPorIDAsync(id);
            return agendamento;
        }

        public async Task<List<Agendamento>> ObterTodosAgendamentoAsync(CancellationToken cancellationToken = default)
        {
            List<Agendamento> agendamentos = await _agendamentoRepository.ObterTodosAgendamentoAsync();
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
            //Posteriormente vou criar validações para verificar se alguma das entidades estão sendo utilizados naquele horario
            Agendamento agendamento = new()
            {
                PacienteId = request.PacienteId,
                ProfissionalId = request.ProfissionalId,
                SalaId = request.SalaId,
                DataHoraInicio = request.DataHoraInicio,
                DataHoraFim = request.DataHoraFim,
                Observacoes = request.Observacoes,
                Status = request.Status
            };

            try
            {
                await _agendamentoRepository.InserirAgendamentoAsync(agendamento);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao inserir agendamento");
                throw;
            }
            return agendamento.Id;
        }

        public async Task<int> DeletarAgendamentoAsync(DeletarAgendamentoRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                Agendamento agendamento = await _agendamentoRepository.ObterAgendamentoPorIDAsync(request.AgendamentoId, cancellationToken);
                if (agendamento is null)
                {
                    _logger.LogError("A pesquisa pelo agendamento no banco não retornou nenhuma linha, deleção incompleta");
                    throw new KeyNotFoundException("Agendamento não encontrado.");
                }

                _logger.LogInformation($"O registro no banco foi encontrado e vai ser deletado");

                await _agendamentoRepository.DeletarAgendamentoAsync(agendamento);

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
