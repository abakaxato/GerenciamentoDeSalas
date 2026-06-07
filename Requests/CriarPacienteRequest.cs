namespace GerenciamentoSalas.API.Requests
{
    public class CriarPacienteRequest
    {
        public int ResponsavelId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public DateTime? DataNascimento { get; set; }
    }
}
