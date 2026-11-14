using GerenciamentoSalas.API.Common.Repositorie.interfaces;
using GerenciamentoSalas.API.Models;
using GerenciamentoSalas.API.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace GerenciamentoSalas.API.Common.Repositorie
{
    public class CommonRepository : ICommonRepository
    {
        private readonly ILogger<CommonRepository> _logger;
        private readonly GerenciamentoContext _dbContext;
        public CommonRepository(ILogger<CommonRepository> logger, GerenciamentoContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
        public async Task<bool> VerificarDisponibilidadeAsync<T>(T entidade, DateTime dataInicio, DateTime dataFim, int? agendamentoIdIgnorar = null, CancellationToken cancellationToken = default)
        {
            bool temConflito = false;

            switch (entidade)
            {
                case PacienteDto paciente:
                    temConflito = await _dbContext.Agendamentos
                        .AnyAsync(x => x.PacienteId == paciente.Id
                                    && (!agendamentoIdIgnorar.HasValue || x.Id != agendamentoIdIgnorar.Value)
                                    && x.DataHoraInicio < dataFim
                                    && x.DataHoraFim > dataInicio, cancellationToken);
                    break;

                case SalaDto sala:
                    temConflito = await _dbContext.Agendamentos
                        .AnyAsync(x => x.SalaId == sala.Id
                                    && (!agendamentoIdIgnorar.HasValue || x.Id != agendamentoIdIgnorar.Value)
                                    && x.DataHoraInicio < dataFim
                                    && x.DataHoraFim > dataInicio, cancellationToken);
                    break;

                case ProfissionalDto profissional:
                    temConflito = await _dbContext.Agendamentos
                        .AnyAsync(x => x.ProfissionalId == profissional.Id
                                    && (!agendamentoIdIgnorar.HasValue || x.Id != agendamentoIdIgnorar.Value)
                                    && x.DataHoraInicio < dataFim
                                    && x.DataHoraFim > dataInicio, cancellationToken);
                    break;

                default:
                    _logger.LogError("Entidade não reconhecida para verificar disponibilidade");
                    throw new ArgumentException("Entidade não reconhecida para verificar disponibilidade");
            }
            return !temConflito;
        }
        public async Task<bool> VerificarExistenciaAsync<T>(T entidade, CancellationToken cancellationToken = default)
        {
            bool disponivel = true;
            switch (entidade)
            {
                case (PacienteDto paciente):
                    List<PacienteDto> registrosPaciente = await _dbContext.Pacientes.
                        Where(x => x.Id == paciente.Id)
                        .ToListAsync(cancellationToken);
                    if (!registrosPaciente.IsNullOrEmpty()) { disponivel = false; }
                    break;
                case (SalaDto sala):
                    List<SalaDto> registrosSala = await _dbContext.Salas.
                        Where(x => x.Id == sala.Id)
                        .ToListAsync(cancellationToken);
                    if (!registrosSala.IsNullOrEmpty()) { disponivel = false; }
                    break;
                case (ProfissionalDto profissional):
                    List<ProfissionalDto> registrosProfissional = await _dbContext.Profissionais.
                        Where(x => x.Id == profissional.Id)
                        .ToListAsync(cancellationToken);
                    if (!registrosProfissional.IsNullOrEmpty()) { disponivel = false; }
                    break;
                default:
                    disponivel = false;
                    _logger.LogError("Entidade não registrada para verificar existencia");
                    throw new ArgumentException("Entidade não registrada para verificar existencia");
            }
            return disponivel;
        }
    }
}
