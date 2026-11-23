using GerenciamentoSalas.API.Models;
using GerenciamentoSalas.API.Models.DTOs;
using GerenciamentoSalas.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GerenciamentoSalas.API.Repositories
{
    public class ResponsavelRepository(GerenciamentoContext gerenciamentoContext) : IResponsavelRepository
    {
        private readonly GerenciamentoContext _dbContext = gerenciamentoContext;

        public async Task<ResponsavelDto?> ObterResponsavelPorIdAsync(int id, CancellationToken cancellationToken = default)
        {
            ResponsavelDto? responsavelDto = await _dbContext.Responsaveis
                .FirstOrDefaultAsync(x => x.Id == id && x.Ativo == true, cancellationToken);

            if (responsavelDto is null)
                return null;

            responsavelDto.Pacientes = await _dbContext.Pacientes
                .Where(x => x.ResponsavelId == responsavelDto.Id && x.Ativo == true)
                .ToListAsync(cancellationToken);

            return responsavelDto;
        }

        public async Task<List<ResponsavelDto>> ObterTodosResponsavelAsync(CancellationToken cancellationToken)
        {
            List<ResponsavelDto> responsavelDtos = await _dbContext.Responsaveis
            .Where(x => x.Ativo == true)
            .ToListAsync(cancellationToken);

            List<int> responsavelIds = responsavelDtos.Select(p => p.Id).ToList();

            var PacientePorResponsavel = await _dbContext.Pacientes
                .Where(x => x.Ativo == true && x.ResponsavelId != null && responsavelIds.Contains(x.ResponsavelId.Value))
                .GroupBy(x => x.ResponsavelId!.Value)
                .ToDictionaryAsync(g => g.Key, g => g.ToList(), cancellationToken);

            foreach (ResponsavelDto responsavel in responsavelDtos)
            {
                responsavel.Pacientes = PacientePorResponsavel.TryGetValue(responsavel.Id, out var lista)
                    ? lista
                    : [];
            }

            return responsavelDtos;
        }

        public async Task<int> InserirResponsavelAsync(ResponsavelDto responsavel, CancellationToken cancellationToken = default)
        {
            await _dbContext.Responsaveis.AddAsync(responsavel, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return responsavel.Id;
        }

        public async Task<ResponsavelDto?> AtualizarResponsavelAsync(AtualizarResponsavelDto atualizarResponsavelDto, CancellationToken cancellationToken = default)
        {
            ResponsavelDto? responsavelBanco = await _dbContext.Responsaveis.FirstOrDefaultAsync(x => x.Id == atualizarResponsavelDto.Id, cancellationToken);

            if (responsavelBanco is not null)
            {
                if (!String.IsNullOrWhiteSpace(atualizarResponsavelDto.Nome)) { responsavelBanco.Nome = atualizarResponsavelDto.Nome!; }
                if (!String.IsNullOrWhiteSpace(atualizarResponsavelDto.Telefone)) { responsavelBanco.Telefone = atualizarResponsavelDto.Telefone!; }

                responsavelBanco.Email = atualizarResponsavelDto.Email;

                await _dbContext.SaveChangesAsync(cancellationToken);

                responsavelBanco.Pacientes = await _dbContext.Pacientes.Where(x => x.ResponsavelId == responsavelBanco.Id && x.Ativo == true).ToListAsync(cancellationToken);

                return responsavelBanco;
            }
            else
            {
                return null;
            }
        }

        public async Task<(int pacientesDeletados, int agendamentosDeletados, int responsavelId)> DeletarResponsavelAsync(ResponsavelDto responsavel, CancellationToken cancellationToken = default)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            ResponsavelDto? responsavelBanco = await _dbContext.Responsaveis
                .FirstOrDefaultAsync(x => x.Id == responsavel.Id, cancellationToken)
                ?? throw new KeyNotFoundException("Responsável não encontrado no banco");

            List<int> pacienteIds = await _dbContext.Pacientes
                .Where(x => x.Ativo == true && x.ResponsavelId == responsavel.Id)
                .Select(x => x.Id)
                .ToListAsync(cancellationToken);

            int agendamentosDeletados = 0;
            if (pacienteIds.Count > 0)
            {
                agendamentosDeletados = await _dbContext.Agendamentos
                    .Where(x => x.Ativo == true && pacienteIds.Contains(x.PacienteId))
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(a => a.Ativo, false),
                        cancellationToken);
            }

            int pacientesDeletados = await _dbContext.Pacientes
                .Where(x => x.Ativo == true && x.ResponsavelId == responsavel.Id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(p => p.Ativo, false),
                    cancellationToken);

            responsavelBanco.Ativo = false;

            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return (pacientesDeletados, agendamentosDeletados, responsavel.Id);
        }
    }
}
