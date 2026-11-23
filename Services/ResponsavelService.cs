using GerenciamentoSalas.API.Common.Service.Interface;
using GerenciamentoSalas.API.Models.DTOs;
using GerenciamentoSalas.API.Models.Requests;
using GerenciamentoSalas.API.Repositories.Interfaces;
using GerenciamentoSalas.API.Services.Interfaces;

namespace GerenciamentoSalas.API.Services
{
    public class ResponsavelService(IResponsavelRepository responsavelRepository, ICommonService commonService, ILogger<ResponsavelService> logger) : IResponsavelService
    {
        private readonly ICommonService _commonService = commonService;
        private readonly IResponsavelRepository _responsavelRepository = responsavelRepository;
        private readonly ILogger<ResponsavelService> _logger = logger;

        public async Task<ResponsavelDto?> ObterResponsavelPorIdAsync(int id, CancellationToken cancellationToken = default)
        {
            if (id <= 0) { throw new ArgumentException("ID Menor ou igual a zero"); }

            ResponsavelDto? responsavelDto = await _responsavelRepository.ObterResponsavelPorIdAsync(id, cancellationToken);

            return responsavelDto;
        }
        public async Task<List<ResponsavelDto>> ObterTodosResponsavelAsync(CancellationToken cancellationToken = default)
        {
            List<ResponsavelDto> responsavelList = await _responsavelRepository.ObterTodosResponsavelAsync(cancellationToken);

            return responsavelList;
        }
        public async Task<int> InserirResponsavelAsync(CriarResponsavelRequest criarResponsavelRequest, CancellationToken cancellationToken = default)
        {
            if(String.IsNullOrWhiteSpace(criarResponsavelRequest.Nome) || String.IsNullOrWhiteSpace(criarResponsavelRequest.Telefone))
            {
                _logger.LogError("Não é possivel inserir um novo responsavel sem Nome ou Telefone");
                throw new ArgumentException("Não é possivel inserir um novo responsavel sem Nome e Telefone");
            }

            ResponsavelDto responsavel = new()
            {
                Nome = criarResponsavelRequest.Nome,
                Email = criarResponsavelRequest.Email,
                Telefone = criarResponsavelRequest.Telefone,
                Ativo = true
            };

            int idGerado = await _responsavelRepository.InserirResponsavelAsync(responsavel, cancellationToken);

            return idGerado;
        }
        public async Task<ResponsavelDto?> AtualizarResponsavelAsync(int id, AtualizarResponsavelRequest request, CancellationToken cancellationToken = default)
        {
            if (request == null)
            {
                _logger.LogError("Erro ao atualizar responsavel, requisição vazia");
                throw new ArgumentNullException(nameof(request));
            }

            if (id <= 0) { throw new ArgumentException("ID Menor ou igual a zero"); }

            AtualizarResponsavelDto atualizarResponsavelDto = new()
            {
                Id = id,
                Nome = request.Nome,
                Email = request.Email,
                Telefone = request.Telefone
            };

            ResponsavelDto? responsavelAtualizado = await _responsavelRepository.AtualizarResponsavelAsync(atualizarResponsavelDto, cancellationToken);

            if (responsavelAtualizado is null)
            {
                _logger.LogError("Nao foi possivel atualizar o responsavel {requestId}, ID nao encontrado no banco", id);
                throw new KeyNotFoundException($"Nao foi possivel atualizar o responsavel {id}, ID nao encontrado no banco");
            }

            return responsavelAtualizado;
        }

        public async Task<int> DeletarResponsavelAsync(int id, CancellationToken cancellationToken = default)
        {
            if (id <= 0) { throw new ArgumentException("ID Menor ou igual a zero"); }

            ResponsavelDto responsavel = new() { Id = id };

            bool existe = await _commonService.VerificarExistenciaAsync(responsavel, cancellationToken);

            if (!existe)
            {
                _logger.LogError("Nao existe responsavel com ID : {responsavelid}, delecao cancelada", responsavel.Id);
                throw new KeyNotFoundException($"Nao existe responsavel com ID : {responsavel.Id}, delecao cancelada");
            }

            var (pacientesDeletados,agendamentosDeletados, responsavelId) = await _responsavelRepository.DeletarResponsavelAsync(responsavel, cancellationToken);

            _logger.LogInformation("Foram deletados {PacientesDeletados} Pacientes e {agendamentosDeletados} agendamentos junto do responsavel {id}", pacientesDeletados, agendamentosDeletados,responsavelId);

            return responsavelId;
        }
    }
}
