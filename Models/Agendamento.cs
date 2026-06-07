namespace GerenciamentoSalas.API.Models
{
    public class Agendamento
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public int ProfissionalId { get; set; }
        public int SalaId { get; set; }
        public DateTime DataHoraInicio { get; set; }
        public DateTime DataHoraFim { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Observacoes { get; set; } = string.Empty;
    }
}
