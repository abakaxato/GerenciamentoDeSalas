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

        public async Task<int> InserirAgendamentoAsync(CriarAgendamentoRequest request,CancellationToken cancellationToken = default)
        {
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
            await _dbContext.Agendamentos.AddAsync(agendamento,cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);
            
            return agendamento.Id;
        }

        public async Task<int> DeletarAgendamentoAsync(DeletarAgendamentoRequest request, CancellationToken cancellationToken = default) {
            
            using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
            int IdDeletado = 0;
            try
            {
                var ent = await _dbContext.Agendamentos
                    .Where(ent =>
                    ent.DataHoraInicio == request.DataHoraInicio &&
                    ent.DataHoraFim == request.DataHoraFim &&
                    ent.SalaId == request.SalaId
                    ).ToListAsync(cancellationToken);

                if (ent.Count == 0)
                {
                    _logger.LogError("A pesquisa pelo agendamento no banco não retornou nenhuma linha, deleção incompleta");
                    throw new KeyNotFoundException("A pesquisa pelo agendamento no banco não retornou nenhuma linha, deleção incompleta");
                }
                else if (ent.Count == 1)
                {
                    _logger.LogInformation($"O registro no banco foi encontrado e vai ser deletado");
                    
                    IdDeletado = ent[0].Id;
                    _dbContext.Agendamentos.Remove(ent[0]);
                }

                 int linhas = await _dbContext.SaveChangesAsync(cancellationToken);
                
                if(linhas == 1)
                {
                    _logger.LogInformation($"Agendamento com ID : {IdDeletado} removido com sucesso",IdDeletado);
                    _logger.LogInformation($"O Agendamento de {request.DataHoraInicio} ate {request.DataHoraFim} na sala {request.SalaId} agora esta disponivel");

                    await transaction.CommitAsync(cancellationToken);
                    
                    return IdDeletado;
                }
                else if (linhas == 0)
                {
                    throw new Exception("Não foi possivel deletar o agendamento no banco, deleção cancelada");
                }
                else 
                {
                    throw new InvalidOperationException($"essa deleção iria impactar {linhas} linhas no banco, deleção cancelada");
                }
            }catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                _logger.LogError(ex,$"Erro ao deletar agendamento ID{IdDeletado}");
                throw;
            }
        }
    }
}