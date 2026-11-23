using GerenciamentoSalas.API.Models;
using GerenciamentoSalas.API.Models.DTOs;
using GerenciamentoSalas.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GerenciamentoSalas.API.Repositories
{
    public class ProfissionalRepository(GerenciamentoContext gerenciamentoContext) : IProfissionalRepository
    {
        private readonly GerenciamentoContext _dbContext = gerenciamentoContext;

        public async Task<ProfissionalDto?> ObterProfissionalPorIdAsync(int id, CancellationToken cancellationToken = default)
        {
            ProfissionalDto? profissionalDto = await _dbContext.Profissionais
                .FirstOrDefaultAsync(x => x.Id == id && x.Ativo == true, cancellationToken);

            if (profissionalDto is null)
                return null;

            profissionalDto.Agendamentos = await _dbContext.Agendamentos
                .Where(x => x.ProfissionalId == profissionalDto.Id && x.Ativo == true)
                .ToListAsync(cancellationToken);

            return profissionalDto;
        }

        public async Task<List<ProfissionalDto>> ObterTodosProfissionalAsync(CancellationToken cancellationToken)
        {
            List<ProfissionalDto> profissionalDtos = await _dbContext.Profissionais
            .Where(x => x.Ativo == true)
            .ToListAsync(cancellationToken);

            List<int> profissionalIds = profissionalDtos.Select(p => p.Id).ToList();

            var agendamentoPorProfissional = await _dbContext.Agendamentos
                .Where(x => x.Ativo == true && profissionalIds.Contains(x.ProfissionalId))
                .GroupBy(x => x.ProfissionalId)
                .ToDictionaryAsync(g => g.Key, g => g.ToList(), cancellationToken);

            foreach (ProfissionalDto profissional in profissionalDtos)
            {
                profissional.Agendamentos = agendamentoPorProfissional.TryGetValue(profissional.Id, out var lista)
                    ? lista
                    : [];
            }

            return profissionalDtos;
        }

        public async Task<int> InserirProfissionalAsync(ProfissionalDto profissional, CancellationToken cancellationToken = default)
        {
            await _dbContext.Profissionais.AddAsync(profissional, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return profissional.Id;
        }

        public async Task<ProfissionalDto?> AtualizarProfissionalAsync(AtualizarProfissionalDto atualizarProfissionalDto, CancellationToken cancellationToken = default)
        {
            ProfissionalDto? profissionalBanco = await _dbContext.Profissionais.FirstOrDefaultAsync(x => x.Id == atualizarProfissionalDto.Id, cancellationToken);

            if (profissionalBanco is not null)
            {
                if (!String.IsNullOrWhiteSpace(atualizarProfissionalDto.Nome)) { profissionalBanco.Nome = atualizarProfissionalDto.Nome!; }
                if (!String.IsNullOrWhiteSpace(atualizarProfissionalDto.Especialidade)) { profissionalBanco.Especialidade = atualizarProfissionalDto.Especialidade!; }
                if (!String.IsNullOrWhiteSpace(atualizarProfissionalDto.Telefone)) { profissionalBanco.Telefone = atualizarProfissionalDto.Telefone!; }

                //profissionalBanco.Email = atualizarProfissionalDto.Email; lembrar de inserir o e-mail depois

                await _dbContext.SaveChangesAsync(cancellationToken);

                profissionalBanco.Agendamentos = await _dbContext.Agendamentos.Where(x => x.ProfissionalId == profissionalBanco.Id && x.Ativo == true).ToListAsync(cancellationToken);

                return profissionalBanco;
            }
            else
            {
                return null;
            }
        }

        public async Task<(int agendamentosDeletados, int profissionalId)> DeletarProfissionalAsync(ProfissionalDto profissional, CancellationToken cancellationToken = default)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            ProfissionalDto? profissionalBanco = await _dbContext.Profissionais
                .FirstOrDefaultAsync(x => x.Id == profissional.Id, cancellationToken)
                ?? throw new KeyNotFoundException("Profissional não encontrado no banco");

            int agendamentosDeletados = await _dbContext.Agendamentos
            .Where(x => x.Ativo == true && x.ProfissionalId == profissional.Id)
            .ExecuteUpdateAsync(setters => setters
            .SetProperty(a => a.Ativo, false),
            cancellationToken);

            profissionalBanco.Ativo = false;

            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return (agendamentosDeletados, profissional.Id);
        }
    }
}
