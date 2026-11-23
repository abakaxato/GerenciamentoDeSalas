using GerenciamentoSalas.API.Models;
using GerenciamentoSalas.API.Models.DTOs;
using GerenciamentoSalas.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GerenciamentoSalas.API.Repositories
{
    public class SalaRepository(GerenciamentoContext gerenciamentoContext) : ISalaRepository
    {
        private readonly GerenciamentoContext _dbContext = gerenciamentoContext;

        public async Task<SalaDto?> ObterSalaPorIdAsync(int id, CancellationToken cancellationToken = default)
        {
            SalaDto? salaDto = await _dbContext.Salas
                .FirstOrDefaultAsync(x => x.Id == id && x.Ativo == true, cancellationToken);

            if (salaDto is null)
                return null;

            salaDto.Agendamentos = await _dbContext.Agendamentos
                .Where(x => x.SalaId == salaDto.Id && x.Ativo == true)
                .ToListAsync(cancellationToken);

            return salaDto;
        }

        public async Task<List<SalaDto>> ObterTodosSalaAsync(CancellationToken cancellationToken)
        {
            List<SalaDto> salaDtos = await _dbContext.Salas
            .Where(x => x.Ativo == true)
            .ToListAsync(cancellationToken);

            List<int> salaIds = salaDtos.Select(p => p.Id).ToList();

            var agendamentoPorSala = await _dbContext.Agendamentos
                .Where(x => x.Ativo == true && salaIds.Contains(x.SalaId))
                .GroupBy(x => x.SalaId)
                .ToDictionaryAsync(g => g.Key, g => g.ToList(), cancellationToken);

            foreach (SalaDto sala in salaDtos)
            {
                sala.Agendamentos = agendamentoPorSala.TryGetValue(sala.Id, out var lista)
                    ? lista
                    : [];
            }

            return salaDtos;
        }

        public async Task<int> InserirSalaAsync(SalaDto sala, CancellationToken cancellationToken = default)
        {
            await _dbContext.Salas.AddAsync(sala, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return sala.Id;
        }

        public async Task<SalaDto?> AtualizarSalaAsync(AtualizarSalaDto atualizarSalaDto, CancellationToken cancellationToken = default)
        {
            SalaDto? salaBanco = await _dbContext.Salas.FirstOrDefaultAsync(x => x.Id == atualizarSalaDto.Id, cancellationToken);

            if (salaBanco is not null)
            {
                if (!String.IsNullOrWhiteSpace(atualizarSalaDto.Nome)) { salaBanco.Nome = atualizarSalaDto.Nome!; }
                if (atualizarSalaDto.Capacidade.HasValue) { salaBanco.Capacidade = atualizarSalaDto.Capacidade; }

                await _dbContext.SaveChangesAsync(cancellationToken);

                salaBanco.Agendamentos = await _dbContext.Agendamentos.Where(x => x.SalaId == salaBanco.Id && x.Ativo == true).ToListAsync(cancellationToken);

                return salaBanco;
            }
            else
            {
                return null;
            }
        }

        public async Task<(int agendamentosDeletados, int salaId)> DeletarSalaAsync(SalaDto sala, CancellationToken cancellationToken = default)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            SalaDto? salaBanco = await _dbContext.Salas
                .FirstOrDefaultAsync(x => x.Id == sala.Id, cancellationToken)
                ?? throw new KeyNotFoundException("Sala não encontrada no banco");

            int agendamentosDeletados = await _dbContext.Agendamentos
            .Where(x => x.Ativo == true && x.SalaId == sala.Id)
            .ExecuteUpdateAsync(setters => setters
            .SetProperty(a => a.Ativo, false),
            cancellationToken);

            salaBanco.Ativo = false;

            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return (agendamentosDeletados, sala.Id);
        }
    }
}
