using GerenciamentoSalas.API.Common.Exceptions;
using GerenciamentoSalas.API.Common.Service.Interface;
using GerenciamentoSalas.API.Enums;
using GerenciamentoSalas.API.Models.DTOs;
using GerenciamentoSalas.API.Models.Requests;
using GerenciamentoSalas.API.Repositories.Interfaces;
using GerenciamentoSalas.API.Services.Interfaces;

namespace GerenciamentoSalas.API.Services
{
    public class AgendamentoService : IAgendamentoService
    {
        private readonly IAgendamentoRepository _agendamentoRepository;
        private readonly ICommonService _commonService;
        private readonly ILogger<AgendamentoService> _logger;
        public AgendamentoService(IAgendamentoRepository agendamentoRepository, ILogger<AgendamentoService> logger, ICommonService commonService)
        {
            _agendamentoRepository = agendamentoRepository;
            _commonService = commonService;
            _logger = logger;
        }

        public async Task<AgendamentoDto?> ObterAgendamentoPorIDAsync(int id, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"Busca por agendamento {id} iniciada");

            AgendamentoDto? agendamento = await _agendamentoRepository.ObterAgendamentoPorIdAsync(id, cancellationToken);

            _logger.LogInformation($"Busca por agendamento {id} finalizada");

            return agendamento;
        }

        public async Task<List<AgendamentoDto>> ObterTodosAgendamentoAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"Busca por todos os agendamentos iniciada");

            List<AgendamentoDto> agendamentos = await _agendamentoRepository.ObterTodosAgendamentoAsync(cancellationToken);

            _logger.LogInformation($"Busca por todos os agendamentos finalizada");

            return agendamentos;
        }

        public async Task<int> InserirAgendamentoAsync(CriarAgendamentoRequest request, CancellationToken cancellationToken = default)
        {
            if (request == null)
            {
                _logger.LogError("Erro ao inserir agendamento, requisição vazia");
                throw new ArgumentNullException($"{request}");
            }

            if (request.DataHoraFim <= request.DataHoraInicio)
            {
                _logger.LogError("Erro ao inserir agendamento, a data de término deve ser maior que a data de início.");
                throw new InvalidOperationException("Erro ao inserir agendamento, a data de término deve ser maior que a data de início.");
            }

            SalaDto sala = new() { Id = request.SalaId };
            bool salaDisponivel = await _commonService.VerificarExistenciaDisponibilidadeAsync(sala, request.DataHoraInicio, request.DataHoraFim, null, cancellationToken);
            if (!salaDisponivel) { throw new DisponibilidadeException(sala, request.DataHoraInicio, request.DataHoraFim, "sala"); }

            PacienteDto paciente = new() { Id = request.PacienteId };
            bool pacienteDisponivel = await _commonService.VerificarExistenciaDisponibilidadeAsync(paciente, request.DataHoraInicio, request.DataHoraFim, null, cancellationToken);
            if (!pacienteDisponivel) { throw new DisponibilidadeException(paciente, request.DataHoraInicio, request.DataHoraFim, "paciente"); }

            ProfissionalDto profissional = new() { Id = request.ProfissionalId };
            bool profissionalDisponivel = await _commonService.VerificarExistenciaDisponibilidadeAsync(profissional, request.DataHoraInicio, request.DataHoraFim, null, cancellationToken);
            if (!profissionalDisponivel) { throw new DisponibilidadeException(profissional, request.DataHoraInicio, request.DataHoraFim, "profissional"); }

            AgendamentoDto agendamento = new()
            {
                PacienteId = request.PacienteId,
                ProfissionalId = request.ProfissionalId,
                SalaId = request.SalaId,
                DataHoraInicio = request.DataHoraInicio,
                DataHoraFim = request.DataHoraFim,
                Observacoes = request.Observacoes,
                Status = request.Status ?? StatusAgendamentoEnum.Agendado
            };


            int idGerado = await _agendamentoRepository.InserirAgendamentoAsync(agendamento, cancellationToken);

            _logger.LogInformation($"Agendamento inserido com sucesso. {agendamento.Id}");

            return idGerado;
        }

        public async Task<AgendamentoDto> AtualizarAgendamentoAsync(int id, AtualizarAgendamentoRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                _logger.LogError("Erro ao atualizar agendamento, requisição vazia");
                throw new ArgumentNullException($"{request}");
            }

            if (request.DataHoraInicio >= request.DataHoraFim)
            {
                _logger.LogError("Erro ao atualizar agendamento, a data de término deve ser maior que a data de início.");
                throw new InvalidOperationException("Erro ao atualizar agendamento, a data de término deve ser maior que a data de início.");
            }
            AgendamentoDto? agendamentoBanco = await _agendamentoRepository.ObterAgendamentoPorIdAsync(id, cancellationToken);
            if (agendamentoBanco != null)
            {
                if (request.DataHoraInicio.HasValue || request.DataHoraFim.HasValue)
                {
                    DateTime inicio = request.DataHoraInicio ?? agendamentoBanco.DataHoraInicio;
                    DateTime fim = request.DataHoraFim ?? agendamentoBanco.DataHoraInicio;

                    if (request.DataHoraInicio >= request.DataHoraFim)
                    {
                        _logger.LogError("Erro ao atualizar agendamento, a data de término deve ser maior que a data de início.");
                        throw new InvalidOperationException("Erro ao atualizar agendamento, a data de término deve ser maior que a data de início.");
                    }

                    SalaDto sala = new() { Id = agendamentoBanco.SalaId };
                    bool salaDisponivel = await _commonService.VerificarDisponibilidadeAsync(sala, inicio, fim, id, cancellationToken);
                    if (!salaDisponivel) { throw new DisponibilidadeException(sala, inicio, fim, "sala"); }

                    PacienteDto paciente = new() { Id = agendamentoBanco.PacienteId };
                    bool pacienteDisponivel = await _commonService.VerificarDisponibilidadeAsync(paciente, inicio, fim, id, cancellationToken);
                    if (!pacienteDisponivel) { throw new DisponibilidadeException(paciente, inicio, fim, "paciente"); }

                    ProfissionalDto profissional = new() { Id = agendamentoBanco.ProfissionalId };
                    bool profissionalDisponivel = await _commonService.VerificarDisponibilidadeAsync(profissional, inicio, fim, id, cancellationToken);
                    if (!profissionalDisponivel) { throw new DisponibilidadeException(profissional, inicio, fim, "profissional"); }
                }
            }
            else { throw new KeyNotFoundException($"Nao foi possivel atualizar o agendamento {id}, ID nao encontrado no banco"); }

            AtualizarAgendamentoDto AtualizarAgendamentoDto = new()
            {
                Id = id,
                DataHoraInicio = request.DataHoraInicio,
                DataHoraFim = request.DataHoraFim,
                Observacoes = request.Observacoes,
                Status = request.Status
            };

            if (String.IsNullOrEmpty(AtualizarAgendamentoDto.Observacoes)) { AtualizarAgendamentoDto.Observacoes = agendamentoBanco.Observacoes; }
            if (!AtualizarAgendamentoDto.Status.HasValue) { AtualizarAgendamentoDto.Status = agendamentoBanco.Status; }

            AgendamentoDto agendamentoAtualizado = await _agendamentoRepository.AtualizarAgendamentoAsync(AtualizarAgendamentoDto, cancellationToken);
            return agendamentoAtualizado;
        }
        public async Task<int?> DeletarAgendamentoAsync(int id, CancellationToken cancellationToken = default)
        {
            AgendamentoDto? agendamento = await _agendamentoRepository.ObterAgendamentoPorIdAsync(id, cancellationToken);
            if (agendamento is null)
            {
                _logger.LogError("A pesquisa pelo agendamento no banco não retornou nenhuma linha, deleção incompleta");
                return null;
            }

            _logger.LogInformation($"O registro no banco foi encontrado e vai ser deletado");

            await _agendamentoRepository.DeletarAgendamentoAsync(agendamento, cancellationToken);

            _logger.LogInformation($"Agendamento com ID {agendamento.Id} removido com sucesso");
            return agendamento.Id;
        }
    }
}
