namespace GerenciamentoSalas.API.Models
{
    public class Agendamento
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public int ProfissionalId { get; set; }
        public required int SalaId { get; set; }
        public required DateTime DataHoraInicio { get; set; }
        public required DateTime DataHoraFim { get; set; }
        public string? Status { get; set; } = string.Empty;
        public string? Observacoes { get; set; } = string.Empty;
    }
}
