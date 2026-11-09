namespace GerenciamentoSalas.API.DTOs
{
    public class CriarAgendamentoRequest
    {
        public int PacienteId { get; set; }
        public int ProfissionalId { get; set; }
        public int SalaId { get; set; }
        public DateTime DataHoraInicio { get; set; }
        public DateTime DataHoraFim { get; set; }
        public string? Observacoes { get; set; }
        public string? Status { get; set; } = "Confirmado";
    }
}