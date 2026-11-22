using GerenciamentoSalas.API.Models;
using GerenciamentoSalas.API.Models.DTOs;
using GerenciamentoSalas.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GerenciamentoSalas.API.Repositories
{
    public class PacienteRepository(GerenciamentoContext gerenciamentoContext) : IPacienteRepository
    {
        private readonly GerenciamentoContext _dbContext = gerenciamentoContext;

        public async Task<PacienteDto?> ObterPacientePorIdAsync(int id, CancellationToken cancellationToken = default)
        {
            PacienteDto? pacienteDto = await _dbContext.Pacientes
                .FirstOrDefaultAsync(x => x.Id == id && x.Ativo == true, cancellationToken);

            if (pacienteDto is null)
                return null;

            pacienteDto.Agendamentos = await _dbContext.Agendamentos
                .Where(x => x.PacienteId == pacienteDto.Id && x.Ativo == true)
                .ToListAsync(cancellationToken);

            return pacienteDto;
        }

        public async Task<List<PacienteDto>> ObterTodosPacienteAsync(CancellationToken cancellationToken)
        {
            List<PacienteDto> pacienteDtos = await _dbContext.Pacientes
            .Where(x => x.Ativo == true)
            .ToListAsync(cancellationToken);

            List<int> pacienteIds = pacienteDtos.Select(p => p.Id).ToList();

            var agendamentosPorPaciente = await _dbContext.Agendamentos
                .Where(x => x.Ativo == true && pacienteIds.Contains(x.PacienteId))
                .GroupBy(x => x.PacienteId)
                .ToDictionaryAsync(g => g.Key, g => g.ToList(), cancellationToken);

            foreach (PacienteDto paciente in pacienteDtos)
            {
                paciente.Agendamentos = agendamentosPorPaciente.TryGetValue(paciente.Id, out var lista)
                    ? lista
                    : [];
            }

            return pacienteDtos;
        }

        public async Task<int> InserirPacienteAsync(PacienteDto paciente, CancellationToken cancellationToken = default)
        {
            await _dbContext.Pacientes.AddAsync(paciente, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return paciente.Id;
        }

        public async Task<PacienteDto?> AtualizarPacienteAsync(AtualizarPacienteDto atualizarPacienteDto, CancellationToken cancellationToken = default)
        {
            PacienteDto? pacienteBanco = await _dbContext.Pacientes.FirstOrDefaultAsync(x => x.Id == atualizarPacienteDto.Id, cancellationToken);

            if (pacienteBanco is not null)
            {
                if (!String.IsNullOrWhiteSpace(atualizarPacienteDto.Nome)) { pacienteBanco.Nome = atualizarPacienteDto.Nome!; }
                if (atualizarPacienteDto.DataNascimento.HasValue) { pacienteBanco.DataNascimento = atualizarPacienteDto.DataNascimento; }
                if (atualizarPacienteDto.ResponsavelId.HasValue) { pacienteBanco.ResponsavelId = atualizarPacienteDto.ResponsavelId; }

                await _dbContext.SaveChangesAsync(cancellationToken);

                pacienteBanco.Agendamentos = await _dbContext.Agendamentos.Where(x => x.PacienteId == pacienteBanco.Id).ToListAsync(cancellationToken);

                return pacienteBanco;
            }
            else
            {
                return null;
            }
        }

        public async Task<(int agendamentosDeletados, int PacienteId)> DeletarPacienteAsync(PacienteDto paciente, CancellationToken cancellationToken = default)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            int agendamentosDeletados = await _dbContext.Agendamentos
             .Where(x => x.Ativo == true && x.PacienteId == paciente.Id)
             .ExecuteUpdateAsync(setters => setters
             .SetProperty(a => a.Ativo, false), cancellationToken);

            PacienteDto? pacienteDeletado = await _dbContext.Pacientes.FirstOrDefaultAsync(x => x.Id == paciente.Id, cancellationToken)
                ?? throw new KeyNotFoundException("Paciente não encontrado no banco");

            pacienteDeletado.Ativo = false;

            await _dbContext.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return (agendamentosDeletados, paciente.Id);
        }
    }
}
