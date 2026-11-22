using GerenciamentoSalas.API.Common.Service.Interface;
using GerenciamentoSalas.API.Models.DTOs;
using GerenciamentoSalas.API.Models.Requests;
using GerenciamentoSalas.API.Repositories.Interfaces;
using GerenciamentoSalas.API.Services.Interfaces;

namespace GerenciamentoSalas.API.Services
{
    public class PacienteService(IPacienteRepository pacienteRepository, ICommonService commonService, ILogger<PacienteService> logger) : IPacienteService
    {
        private readonly ICommonService _commonService = commonService;
        private readonly IPacienteRepository _pacienteRepository = pacienteRepository;
        private readonly ILogger<PacienteService> _logger = logger;

        public async Task<PacienteDto?> ObterPacientePorIdAsync(int id, CancellationToken cancellationToken = default)
        {
            if (id <= 0) { throw new ArgumentException("ID Menor ou igual a zero"); }

            PacienteDto? pacienteDto = await _pacienteRepository.ObterPacientePorIdAsync(id, cancellationToken);

            return pacienteDto;
        }
        public async Task<List<PacienteDto>> ObterTodosPacienteAsync(CancellationToken cancellationToken = default)
        {
            List<PacienteDto> pacienteList = await _pacienteRepository.ObterTodosPacienteAsync(cancellationToken);

            return pacienteList;
        }
        public async Task<int> InserirPacienteAsync(CriarPacienteRequest criarPacienteRequest, CancellationToken cancellationToken = default)
        {
            bool existeResponsavel;

            if (criarPacienteRequest.ResponsavelId != null)
            {
                ResponsavelDto responsavelDto = new() { Id = criarPacienteRequest.ResponsavelId.Value };
                existeResponsavel = await _commonService.VerificarExistenciaAsync(responsavelDto, cancellationToken);
                if (existeResponsavel == false)
                {
                    _logger.LogError("Responsavel {Id} nao encontrado", criarPacienteRequest.ResponsavelId);
                    throw new InvalidOperationException($"Responsavel {criarPacienteRequest.ResponsavelId} nao encontrado");
                }
            }

            if (criarPacienteRequest.ResponsavelId == null && criarPacienteRequest.DataNascimento > DateTime.Today.AddYears(-18))
            {
                _logger.LogError("Paciente {Nome} com menos de 18 anos e sem responsável", criarPacienteRequest.Nome);
                throw new InvalidOperationException("Paciente com menos de 18 anos e sem responsável");
            }

            PacienteDto paciente = new()
            {
                ResponsavelId = criarPacienteRequest.ResponsavelId,
                Nome = criarPacienteRequest.Nome,
                DataNascimento = criarPacienteRequest.DataNascimento
            };

            int idGerado = await _pacienteRepository.InserirPacienteAsync(paciente, cancellationToken);

            return idGerado;
        }
        public async Task<PacienteDto?> AtualizarPacienteAsync(int id, AtualizarPacienteRequest request, CancellationToken cancellationToken = default)
        {
            if (request == null)
            {
                _logger.LogError("Erro ao atualizar paciente, requisição vazia");
                throw new ArgumentNullException(nameof(request));
            }

            if (id <= 0) { throw new ArgumentException("ID Menor ou igual a zero"); }

            if (request.DataNascimento.HasValue && request.DataNascimento > DateTime.Today.AddYears(-18) && request.ResponsavelId == null)
            {
                _logger.LogError("Erro ao atualizar paciente, é necessario inserir um responsavel para essa idade");
                throw new InvalidOperationException("É necessário informar um responsável para paciente menor de 18 anos");
            }

            AtualizarPacienteDto atualizarPacienteDto = new()
            {
                Id = id,
                ResponsavelId = request.ResponsavelId,
                Nome = request.Nome,
                DataNascimento = request.DataNascimento,
            };

            PacienteDto? pacienteAtualizado = await _pacienteRepository.AtualizarPacienteAsync(atualizarPacienteDto, cancellationToken);

            if (pacienteAtualizado is null)
            {
                _logger.LogError("Nao foi possivel atualizar o paciente {requestId}, ID nao encontrado no banco", id);
                throw new KeyNotFoundException($"Nao foi possivel atualizar o paciente {id}, ID nao encontrado no banco");
            }

            return pacienteAtualizado;
        }

        public async Task<int> DeletarPacienteAsync(int id, CancellationToken cancellationToken = default)
        {
            if (id <= 0) { throw new ArgumentException("ID Menor ou igual a zero"); }

            PacienteDto paciente = new() { Id = id };

            bool existe = await _commonService.VerificarExistenciaAsync(paciente, cancellationToken);

            if (!existe)
            {
                _logger.LogError("Nao existe paciente com ID : {pacienteid}, delecao cancelada", paciente.Id);
                throw new KeyNotFoundException($"Nao existe paciente com ID : {paciente.Id}, delecao cancelada");
            }

            var (agendamentosDeletados, pacienteId) = await _pacienteRepository.DeletarPacienteAsync(paciente, cancellationToken);

            _logger.LogInformation("Foram deletados {agendamentosDeletados} junto do paciente {id}", agendamentosDeletados, pacienteId);

            return pacienteId;
        }
    }
}
