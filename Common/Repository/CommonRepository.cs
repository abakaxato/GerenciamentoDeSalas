using GerenciamentoSalas.API.Common.Repositorie.interfaces;
using GerenciamentoSalas.API.Models;
using GerenciamentoSalas.API.Models.DTOs;
using Microsoft.EntityFrameworkCore;

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
                case (PacienteDto paciente):
                    temConflito = await _dbContext.Agendamentos
                        .AnyAsync(x => x.Ativo == true 
                                    && x.PacienteId == paciente.Id
                                    && (!agendamentoIdIgnorar.HasValue || x.Id != agendamentoIdIgnorar.Value)
                                    && x.DataHoraInicio < dataFim
                                    && x.DataHoraFim > dataInicio, cancellationToken);
                    break;
                case (SalaDto sala):
                    temConflito = await _dbContext.Agendamentos
                        .AnyAsync(x => x.Ativo == true
                                    && x.SalaId == sala.Id
                                    && (!agendamentoIdIgnorar.HasValue || x.Id != agendamentoIdIgnorar.Value)
                                    && x.DataHoraInicio < dataFim
                                    && x.DataHoraFim > dataInicio, cancellationToken);
                    break;
                case (ProfissionalDto profissional):
                    temConflito = await _dbContext.Agendamentos
                        .AnyAsync(x => x.Ativo == true
                                    && x.ProfissionalId == profissional.Id
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
            bool existe = true;
            switch (entidade)
            {
                case (PacienteDto paciente):
                    existe = await _dbContext.Pacientes.AnyAsync(x => x.Ativo == true && x.Id == paciente.Id, cancellationToken);
                    break;
                case (SalaDto sala):
                    existe = await _dbContext.Salas.AnyAsync(x => x.Ativo == true && x.Id == sala.Id, cancellationToken);
                    break;
                case (ProfissionalDto profissional):
                    existe = await _dbContext.Profissionais.AnyAsync(x => x.Ativo == true && x.Id == profissional.Id, cancellationToken);
                    break;
                case (ResponsavelDto responsavel):
                    existe = await _dbContext.Responsaveis.AnyAsync(x => x.Ativo == true && x.Id == responsavel.Id, cancellationToken);
                    break;
                default:
                    existe = false;
                    _logger.LogError("Entidade não registrada para verificar existencia");
                    throw new ArgumentException("Entidade não registrada para verificar existencia");
            }
            return existe;
        }

        public async Task<bool> VerficarAgendamentosAtivos<T>(T entidade, CancellationToken cancellationToken = default)
        {
            bool existe = false;
            switch (entidade)
            {
                case (PacienteDto paciente):
                    existe = await _dbContext.Agendamentos
                        .AnyAsync(x => x.Ativo == true && x.PacienteId == paciente.Id && x.Status != Enums.StatusAgendamentoEnum.Pendente, cancellationToken);
                    break;
                case (SalaDto sala):
                    existe = await _dbContext.Agendamentos
                        .AnyAsync(x => x.Ativo == true && x.SalaId == sala.Id && x.Status != Enums.StatusAgendamentoEnum.Pendente, cancellationToken);
                    break;
                case (ProfissionalDto profissional):
                    existe = await _dbContext.Agendamentos
                        .AnyAsync(x => x.Ativo == true && x.ProfissionalId == profissional.Id && x.Status != Enums.StatusAgendamentoEnum.Pendente, cancellationToken);
                    break;
                case ResponsavelDto responsavel:
                    existe = await _dbContext.Agendamentos
                        .AnyAsync(x => x.Ativo == true && x.Paciente != null && x.Paciente.ResponsavelId == responsavel.Id && x.Status != Enums.StatusAgendamentoEnum.Pendente, cancellationToken);
                    break;
                default:
                    _logger.LogError("Entidade não reconhecida para verificar agendamentos ativos");
                    throw new ArgumentException("Entidade não reconhecida para verificar agendamentos ativos");
            }
            return existe;
        }
    }
}
