using GerenciamentoSalas.API.Enums;

namespace GerenciamentoSalas.API.Models.Requests
{
    public class AtualizarAgendamentoRequest
    {
        public DateTime? DataHoraInicio { get; set; }
        public DateTime? DataHoraFim { get; set; }
        public string? Observacoes { get; set; }
        public StatusAgendamentoEnum? Status { get; set; } = null;
    }
}