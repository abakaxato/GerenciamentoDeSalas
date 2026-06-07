namespace GerenciamentoSalas.API.Models
{
    public class Paciente
    {
        public int Id { get; set; }
        public int ResponsavelId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public DateTime? DataNascimento { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}
