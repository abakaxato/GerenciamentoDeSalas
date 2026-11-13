using GerenciamentoSalas.API.Enums;

namespace GerenciamentoSalas.API.Models.DTOs
{
    public class AtualizarAgendamentoDto
    {
        public required int Id { get; set; }
        public DateTime? DataHoraInicio { get; set; }
        public DateTime? DataHoraFim { get; set; }
        public string? Observacoes { get; set; }
        public StatusAgendamentoEnum? Status { get; set; } = StatusAgendamentoEnum.Agendado;
    }
}