using GerenciamentoSalas.API.Common.Service.Interface;
using GerenciamentoSalas.API.Models.DTOs;
using GerenciamentoSalas.API.Models.Requests;
using GerenciamentoSalas.API.Repositories.Interfaces;
using GerenciamentoSalas.API.Services.Interfaces;

namespace GerenciamentoSalas.API.Services
{
    public class SalaService(ISalaRepository salaRepository, ICommonService commonService, ILogger<SalaService> logger) : ISalaService
    {
        private readonly ICommonService _commonService = commonService;
        private readonly ISalaRepository _salaRepository = salaRepository;
        private readonly ILogger<SalaService> _logger = logger;

        public async Task<SalaDto?> ObterSalaPorIdAsync(int id, CancellationToken cancellationToken = default)
        {
            if (id <= 0) { throw new ArgumentException("ID Menor ou igual a zero"); }

            SalaDto? salaDto = await _salaRepository.ObterSalaPorIdAsync(id, cancellationToken);

            return salaDto;
        }
        public async Task<List<SalaDto>> ObterTodosSalaAsync(CancellationToken cancellationToken = default)
        {
            List<SalaDto> salaList = await _salaRepository.ObterTodosSalaAsync(cancellationToken);

            return salaList;
        }
        public async Task<int> InserirSalaAsync(CriarSalaRequest criarSalaRequest, CancellationToken cancellationToken = default)
        {
            if (String.IsNullOrWhiteSpace(criarSalaRequest.Nome) || criarSalaRequest.Capacidade <= 0)
            {
                _logger.LogError("Para inserir uma nova sala é necessario informar o Nome e uma capacidade acima de 0");
                throw new ArgumentException("Para inserir uma nova sala é necessario informar o Nome e uma capacidade acima de 0");
            }

            SalaDto sala = new()
            {
                Nome = criarSalaRequest.Nome,
                Capacidade = criarSalaRequest.Capacidade,
                Ativo = true
            };

            int idGerado = await _salaRepository.InserirSalaAsync(sala, cancellationToken);

            return idGerado;
        }
        public async Task<SalaDto?> AtualizarSalaAsync(int id, AtualizarSalaRequest request, CancellationToken cancellationToken = default)
        {
            if (request == null)
            {
                _logger.LogError("Erro ao atualizar sala, requisição vazia");
                throw new ArgumentNullException(nameof(request));
            }

            if (id <= 0) { throw new ArgumentException("ID Menor ou igual a zero"); }

            if (request.Capacidade <= 0)
            {
                _logger.LogError("A capacidade da sala deve ser maior que 0");
                throw new ArgumentException("A capacidade da sala deve ser maior que 0");
            }
            AtualizarSalaDto atualizarSalaDto = new()
            {
                Id = id,
                Nome = request.Nome,
                Capacidade = request.Capacidade
            };

            SalaDto? salaAtualizada = await _salaRepository.AtualizarSalaAsync(atualizarSalaDto, cancellationToken);

            if (salaAtualizada is null)
            {
                _logger.LogError("Nao foi possivel atualizar a sala {requestId}, ID nao encontrado no banco", id);
                throw new KeyNotFoundException($"Nao foi possivel atualizar a sala {id}, ID nao encontrado no banco");
            }

            return salaAtualizada;
        }

        public async Task<int> DeletarSalaAsync(int id, CancellationToken cancellationToken = default)
        {
            if (id <= 0) { throw new ArgumentException("ID Menor ou igual a zero"); }

            SalaDto sala = new() { Id = id };

            bool existe = await _commonService.VerificarExistenciaAsync(sala, cancellationToken);

            if (!existe)
            {
                _logger.LogError("Nao existe sala com ID : {salaid}, delecao cancelada", sala.Id);
                throw new KeyNotFoundException($"Nao existe sala com ID : {sala.Id}, delecao cancelada");
            }

            var (agendamentosDeletados, salaId) = await _salaRepository.DeletarSalaAsync(sala, cancellationToken);

            _logger.LogInformation("Foram deletados {agendamentosDeletados} agendamentos junto da sala {id}", agendamentosDeletados, salaId);

            return salaId;
        }
    }
}
