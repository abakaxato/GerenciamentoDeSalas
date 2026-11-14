using GerenciamentoSalas.API.Models;
using GerenciamentoSalas.API.Models.DTOs;
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

        public async Task<AgendamentoDto?> ObterAgendamentoPorIdAsync(int id, CancellationToken cancellationToken = default)
        {
            AgendamentoDto? agendamento = await _dbContext.Agendamentos.FirstOrDefaultAsync(x => x.Id == id,cancellationToken);

            return agendamento;
        }
        public async Task<List<AgendamentoDto>> ObterTodosAgendamentoAsync(CancellationToken cancellationToken = default)
        {
            List<AgendamentoDto> agendamentos = await _dbContext.Agendamentos.ToListAsync(cancellationToken);

            return agendamentos;
        }

        public async Task<int> InserirAgendamentoAsync(AgendamentoDto agendamento, CancellationToken cancellationToken = default)
        {
            await _dbContext.Agendamentos.AddAsync(agendamento, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return agendamento.Id;
        }
        public async Task<AgendamentoDto?> AtualizarAgendamentoAsync(AtualizarAgendamentoDto atualizarAgendamentoDto, CancellationToken cancellationToken = default)
        {

            AgendamentoDto? agendamentoBanco = await _dbContext.Agendamentos.FirstOrDefaultAsync(x => x.Id == atualizarAgendamentoDto.Id,cancellationToken);

            if (agendamentoBanco is not null)
            {
                if (atualizarAgendamentoDto.DataHoraInicio.HasValue) { agendamentoBanco.DataHoraInicio = atualizarAgendamentoDto.DataHoraInicio.Value; }
                if (atualizarAgendamentoDto.DataHoraFim.HasValue) { agendamentoBanco.DataHoraFim = atualizarAgendamentoDto.DataHoraFim.Value; }
                if (atualizarAgendamentoDto.Observacoes != null) { agendamentoBanco.Observacoes = atualizarAgendamentoDto.Observacoes; }
                if (atualizarAgendamentoDto.Status.HasValue) { agendamentoBanco.Status = atualizarAgendamentoDto.Status.Value; }

                await _dbContext.SaveChangesAsync(cancellationToken);

                return agendamentoBanco;
            }
            else
            {
                _logger.LogError($"Durante a atualizacao nao foram encontrados agendamentos com o ID {atualizarAgendamentoDto.Id}");
                return null;
            }
            ;
        }
        public async Task<int> DeletarAgendamentoAsync(AgendamentoDto agendamento, CancellationToken cancellationToken = default)
        {
            _dbContext.Agendamentos.Remove(agendamento);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return agendamento.Id;
        }
    }
}