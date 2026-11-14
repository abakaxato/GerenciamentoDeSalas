using GerenciamentoSalas.API.Models;
using GerenciamentoSalas.API.Models.DTOs;
using GerenciamentoSalas.API.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace GerenciamentoSalas.API.Repositories
{
    public class PacienteRepository(GerenciamentoContext gerenciamentoContext) : IPacienteRepository
    {
        private readonly GerenciamentoContext _dbContext = gerenciamentoContext;

        public async Task<PacienteDto?> ObterPacientePorIdAsync(int id, CancellationToken cancellationToken = default)
        {
            PacienteDto? pacienteDto = await _dbContext.Pacientes.FirstOrDefaultAsync(x=> x.Id == id);

            return pacienteDto;
        }

        public async Task<List<PacienteDto>> ObterTodosPacienteAsync(CancellationToken cancellationToken)
        {
            List<PacienteDto> pacienteDtos = await _dbContext.Pacientes.ToListAsync(cancellationToken);

            return pacienteDtos;
        }

        public async Task<int> InserirPacienteAsync(PacienteDto paciente, CancellationToken cancellationToken = default)
        {
            await _dbContext.Pacientes.AddAsync(paciente,cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return paciente.Id;
        }
        
        public async Task<int> DeletarPacienteAsync(PacienteDto paciente, CancellationToken cancellationToken = default)
        {
            _dbContext.Pacientes.Remove(paciente);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return paciente.Id;
        }
    }
}
