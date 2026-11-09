using GerenciamentoSalas.API.Models;
using GerenciamentoSalas.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Agendamento?> ObterAgendamentoPorIDAsync(int id,CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation($"Busca por agendamento {id} iniciada");
                
                Agendamento? agendamento = await _dbContext.Agendamentos.
                    Where(x => x.Id == id).FirstAsync(cancellationToken);

                _logger.LogInformation($"Busca por agendamento {id} finalizada");
                return agendamento;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,$"Ocorreu um erro ao tentar fazer a busca por agendamento com ID {id}. {ex.Message}");
                throw;
            }
        }
        public async Task<List<Agendamento>> ObterTodosAgendamentoAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation($"Busca por todos os agendamentos iniciada");
                List<Agendamento> agendamentos = await _dbContext.Agendamentos.ToListAsync(cancellationToken);
                _logger.LogInformation($"Busca por todos os agendamentos finalizada");
                return agendamentos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,$"Ocorreu um erro ao tentar fazer a busca por todos os agendamentos. {ex.Message}");
                throw;
            }

        }

        public async Task<int> InserirAgendamentoAsync(Agendamento agendamento, CancellationToken cancellationToken = default)
        {
            try
            {
                var idGerado = await _dbContext.Agendamentos.AddAsync(agendamento, cancellationToken);
                _logger.LogInformation($"Agendamento inserido com sucesso. {agendamento}");
                await _dbContext.SaveChangesAsync(cancellationToken);
                return agendamento.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,$"Ocorreu um erro ao tentar inserir o agendamento. {ex.Message}, {agendamento.Id}");
                throw;
            }
        }
        public async Task<int> DeletarAgendamentoAsync(Agendamento agendamento, CancellationToken cancellationToken = default)
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