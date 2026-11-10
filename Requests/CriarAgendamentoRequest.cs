using GerenciamentoSalas.API.Enums;

namespace GerenciamentoSalas.API.DTOs
{
    public class CriarAgendamentoRequest
    {
        public required int PacienteId { get; set; }
        public required int ProfissionalId { get; set; }
        public required int SalaId { get; set; }
        public required DateTime DataHoraInicio { get; set; }
        public required DateTime DataHoraFim { get; set; }
        public string? Observacoes { get; set; }
        public StatusAgendamentoEnum? Status { get; set; } = StatusAgendamentoEnum.Agendado;
    }
}