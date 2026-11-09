using GerenciamentoSalas.API.DTOs;
using GerenciamentoSalas.API.Models;
using GerenciamentoSalas.API.Repositories.Interfaces;
using GerenciamentoSalas.API.Requests;
using Microsoft.AspNetCore.Http.HttpResults;
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

        public async Task<List<Agendamento>> ObterTodosAgendamentoAsync(CancellationToken cancellationToken = default)
        {
            List<Agendamento> agendamentos = await _dbContext.Agendamentos.ToListAsync(cancellationToken);
            return agendamentos;
        }

        public async Task<int> InserirAgendamentoAsync(CriarAgendamentoRequest request, CancellationToken cancellationToken = default)
        {

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
                await _dbContext.Agendamentos.AddAsync(agendamento, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
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

            using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                Agendamento? agendamento = await _dbContext.Agendamentos
                .SingleOrDefaultAsync(
                x => x.Id == request.AgendamentoId,
                cancellationToken);

                if (agendamento is null)
                {
                    _logger.LogError("A pesquisa pelo agendamento no banco não retornou nenhuma linha, deleção incompleta");
                    throw new KeyNotFoundException("Agendamento não encontrado.");
                }
                
                _logger.LogInformation($"O registro no banco foi encontrado e vai ser deletado");

                _dbContext.Agendamentos.Remove(agendamento);

                int linhas = await _dbContext.SaveChangesAsync(cancellationToken);

                if (linhas == 1)
                {
                    _logger.LogInformation($"Agendamento com ID {agendamento.Id} removido com sucesso");
                    _logger.LogInformation($"O Agendamento de {agendamento.DataHoraInicio} ate {agendamento.DataHoraFim} agora esta disponivel Para sala {agendamento.SalaId}");

                    await transaction.CommitAsync(cancellationToken);

                    return agendamento.Id;
                }
                else if (linhas == 0)
                {
                    throw new Exception("Não foi possivel deletar o agendamento no banco, deleção cancelada");
                }
                else
                {
                    throw new InvalidOperationException($"essa deleção iria impactar {linhas} linhas no banco, deleção cancelada");
                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                _logger.LogError(ex, "Erro ao deletar agendamento");
                throw;
            }
        }
    }
}