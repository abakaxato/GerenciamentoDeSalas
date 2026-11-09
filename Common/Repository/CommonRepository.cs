using GerenciamentoSalas.API.Common.Repositorie.interfaces;
using GerenciamentoSalas.API.Models;
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
        public async Task<bool> VerificarDisponibilidade<T>(T entidade, DateTime DataInicio, DateTime DataFim, CancellationToken cancellationToken = default)
        {
            
            try
            {
                bool disponivel = false;
                switch (entidade)
                {
                    case (Paciente paciente):
                        List<Agendamento> registrosPaciente = await _dbContext.Agendamentos.
                            Where(x => x.PacienteId == paciente.Id)
                            .Where(y => y.DataHoraInicio <= DataInicio)
                            .Where(z => z.DataHoraFim >= DataFim)
                            .ToListAsync(cancellationToken);
                        if (registrosPaciente.IsNullOrEmpty()) { disponivel = true; }
                        break;
                    case (Sala Sala):
                        List<Agendamento> registrosSala = await _dbContext.Agendamentos.
                            Where(x => x.PacienteId == Sala.Id)
                            .Where(y => y.DataHoraInicio <= DataInicio)
                            .Where(z => z.DataHoraFim >= DataFim)
                            .ToListAsync(cancellationToken);
                        if (registrosSala.IsNullOrEmpty()) { disponivel = true; }
                        break;
                    case (Profissional profissional):
                        List<Agendamento> registrosProfissional = await _dbContext.Agendamentos.
                            Where(x => x.PacienteId == profissional.Id)
                            .Where(y => y.DataHoraInicio <= DataInicio)
                            .Where(z => z.DataHoraFim >= DataFim)
                            .ToListAsync(cancellationToken);
                        if (registrosProfissional.IsNullOrEmpty()) { disponivel = true; }
                        break;
                    default:
                        _logger.LogError("Entidade não reconhecida para verificar disponibilidade");
                        disponivel = false;
                        break;
                }
                return disponivel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ocorreu um erro ao tentar fazer a busca por disponibilidade de {entidade}. {ex.Message}");
                throw;
            }
        }
    }
}
