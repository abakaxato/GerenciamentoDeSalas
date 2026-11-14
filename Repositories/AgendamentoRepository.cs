using GerenciamentoSalas.API.Common.Exceptions;
using GerenciamentoSalas.API.Models;
using GerenciamentoSalas.API.Models.DTOs;
using GerenciamentoSalas.API.Models.Requests;
using GerenciamentoSalas.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace GerenciamentoSalas.API.Repositories
{
    public class AgendamentoRepository : IAgendamentoRepository
    {
        private readonly GerenciamentoContext _dbContext;

        private readonly ILogger<AgendamentoRepository> _logger;

        public AgendamentoRepository(GerenciamentoContext context, ILogger<AgendamentoRepository> logger)
        {
            _dbContext = context;
            _logger = logger;
        }

        public async Task<AgendamentoDto?> ObterAgendamentoPorIdAsync(int id,CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation($"Busca por agendamento {id} iniciada");
                
                AgendamentoDto? agendamento = _dbContext.Agendamentos.
                    Where(x => x.Id == id).FirstOrDefault();
                _logger.LogInformation($"Busca por agendamento {id} finalizada");
                return agendamento;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,$"Ocorreu um erro ao tentar fazer a busca por agendamento com ID {id}. {ex.Message}");
                throw;
            }
        }
        public async Task<List<AgendamentoDto>> ObterTodosAgendamentoAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation($"Busca por todos os agendamentos iniciada");
                List<AgendamentoDto> agendamentos = await _dbContext.Agendamentos.ToListAsync(cancellationToken);
                _logger.LogInformation($"Busca por todos os agendamentos finalizada");
                return agendamentos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,$"Ocorreu um erro ao tentar fazer a busca por todos os agendamentos. {ex.Message}");
                throw;
            }

        }

        public async Task<int> InserirAgendamentoAsync(AgendamentoDto agendamento, CancellationToken cancellationToken = default)
        {
            try
            {
                await _dbContext.Agendamentos.AddAsync(agendamento, cancellationToken);
                _logger.LogInformation($"Agendamento inserido com sucesso. {agendamento.Id}");
                await _dbContext.SaveChangesAsync(cancellationToken);
                return agendamento.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,$"Ocorreu um erro ao tentar inserir o agendamento. {ex.Message}, {agendamento.Id}");
                throw;
            }
        }
        public async Task<AgendamentoDto> AtualizarAgendamentoAsync(AtualizarAgendamentoDto atualizarAgendamentoDto, CancellationToken cancellationToken = default)
        {
            try
            {
                AgendamentoDto? agendamentoBanco = _dbContext.Agendamentos
                    .Where(x => x.Id == atualizarAgendamentoDto.Id)
                    .FirstOrDefault();

                if (agendamentoBanco is not null)
                {
                    if (atualizarAgendamentoDto.DataHoraInicio.HasValue) { agendamentoBanco.DataHoraInicio = atualizarAgendamentoDto.DataHoraInicio.Value; }
                    if (atualizarAgendamentoDto.DataHoraFim.HasValue) { agendamentoBanco.DataHoraFim = atualizarAgendamentoDto.DataHoraFim.Value; }
                    if (!atualizarAgendamentoDto.Observacoes.IsNullOrEmpty()) { agendamentoBanco.Observacoes = atualizarAgendamentoDto.Observacoes; }
                    if (atualizarAgendamentoDto.Status.HasValue) { agendamentoBanco.Status = atualizarAgendamentoDto.Status.Value; }
                    await _dbContext.SaveChangesAsync(cancellationToken);
                    return agendamentoBanco;
                }
                else 
                {
                    _logger.LogError($"Durante a atualizacao nao foram encontrados agendamentos com o ID {atualizarAgendamentoDto.Id}");
                    throw new ExistenciaNaTabelaException($"Durante a atualizacao nao foram encontrados agendamentos com o ID {atualizarAgendamentoDto.Id}"); 
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,$"Ocorreu um erro ao tentar atualizar o agendamento. {ex.Message}, {atualizarAgendamentoDto.Id}");
                throw;
            }
        }
        public async Task<int> DeletarAgendamentoAsync(AgendamentoDto agendamento, CancellationToken cancellationToken = default)
        {
            try
            {
                var idDeletado = _dbContext.Agendamentos.Remove(agendamento);
                await _dbContext.SaveChangesAsync(cancellationToken);
                _logger.LogInformation($"Agendamento deletado com sucesso. {agendamento.Id}");
                return agendamento.Id;
            }catch(Exception ex)
            {
                _logger.LogError(ex,$"Ocorreu um erro ao tentar deletar o agendamento. {ex.Message}, {agendamento.Id}");
                throw;
            }
        }
    }
}