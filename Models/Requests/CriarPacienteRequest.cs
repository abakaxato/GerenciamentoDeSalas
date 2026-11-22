namespace GerenciamentoSalas.API.Models.Requests
{
    public class CriarPacienteRequest
    {
        public int? ResponsavelId { get; set; }
        public required string Nome { get; set; } = string.Empty;
        public required DateTime DataNascimento { get; set; }
    }
}
