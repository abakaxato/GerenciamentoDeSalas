using GerenciamentoSalas.API.Common.Service.Interface;
using GerenciamentoSalas.API.Models.DTOs;
using GerenciamentoSalas.API.Models.Requests;
using GerenciamentoSalas.API.Repositories.Interfaces;
using GerenciamentoSalas.API.Services.Interfaces;

namespace GerenciamentoSalas.API.Services
{
    public class ProfissionalService(IProfissionalRepository profissionalRepository, ICommonService commonService, ILogger<ProfissionalService> logger) : IProfissionalService
    {
        private readonly ICommonService _commonService = commonService;
        private readonly IProfissionalRepository _profissionalRepository = profissionalRepository;
        private readonly ILogger<ProfissionalService> _logger = logger;

        public async Task<ProfissionalDto?> ObterProfissionalPorIdAsync(int id, CancellationToken cancellationToken = default)
        {
            if (id <= 0) { throw new ArgumentException("ID Menor ou igual a zero"); }

            ProfissionalDto? profissionalDto = await _profissionalRepository.ObterProfissionalPorIdAsync(id, cancellationToken);

            return profissionalDto;
        }
        public async Task<List<ProfissionalDto>> ObterTodosProfissionalAsync(CancellationToken cancellationToken = default)
        {
            List<ProfissionalDto> profissionalList = await _profissionalRepository.ObterTodosProfissionalAsync(cancellationToken);

            return profissionalList;
        }
        public async Task<int> InserirProfissionalAsync(CriarProfissionalRequest criarProfissionalRequest, CancellationToken cancellationToken = default)
        {
            if (String.IsNullOrWhiteSpace(criarProfissionalRequest.Nome) ||
                String.IsNullOrWhiteSpace(criarProfissionalRequest.Especialidade) || String.IsNullOrWhiteSpace(criarProfissionalRequest.Telefone))
            {
                _logger.LogError("Para inserir um novo profissional é necessario informar o Nome, Especialidade e Telefone");
                throw new ArgumentException("Para inserir um novo profissional é necessario informar o Nome, Especialidade e Telefone");
            }

            ProfissionalDto profissional = new()
            {
                Nome = criarProfissionalRequest.Nome,
                Especialidade = criarProfissionalRequest.Especialidade,
                Telefone = criarProfissionalRequest.Telefone,
                Ativo = true
            };

            int idGerado = await _profissionalRepository.InserirProfissionalAsync(profissional, cancellationToken);

            return idGerado;
        }
        public async Task<ProfissionalDto?> AtualizarProfissionalAsync(int id, AtualizarProfissionalRequest request, CancellationToken cancellationToken = default)
        {
            if (request == null)
            {
                _logger.LogError("Erro ao atualizar profissional, requisição vazia");
                throw new ArgumentNullException(nameof(request));
            }

            if (id <= 0) { throw new ArgumentException("ID Menor ou igual a zero"); }

            AtualizarProfissionalDto atualizarProfissionalDto = new()
            {
                Id = id,
                Nome = request.Nome,
                Especialidade = request.Especialidade,
                Telefone = request.Telefone,
            };

            ProfissionalDto? profissionalAtualizado = await _profissionalRepository.AtualizarProfissionalAsync(atualizarProfissionalDto, cancellationToken);

            if (profissionalAtualizado is null)
            {
                _logger.LogError("Nao foi possivel atualizar o profissional {requestId}, ID nao encontrado no banco", id);
                throw new KeyNotFoundException($"Nao foi possivel atualizar o profissional {id}, ID nao encontrado no banco");
            }

            return profissionalAtualizado;
        }

        public async Task<int> DeletarProfissionalAsync(int id, CancellationToken cancellationToken = default)
        {
            if (id <= 0) { throw new ArgumentException("ID Menor ou igual a zero"); }

            ProfissionalDto profissional = new() { Id = id };

            bool existe = await _commonService.VerificarExistenciaAsync(profissional, cancellationToken);

            if (!existe)
            {
                _logger.LogError("Nao existe profissional com ID : {profissionalid}, delecao cancelada", profissional.Id);
                throw new KeyNotFoundException($"Nao existe profissional com ID : {profissional.Id}, delecao cancelada");
            }

            var (agendamentosDeletados, profissionalId) = await _profissionalRepository.DeletarProfissionalAsync(profissional, cancellationToken);

            _logger.LogInformation("Foram deletados {agendamentosDeletados} agendamentos junto do profissional {id}", agendamentosDeletados,profissionalId);

            return profissionalId;
        }
    }
}
