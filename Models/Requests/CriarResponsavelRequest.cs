namespace GerenciamentoSalas.API.Models.Requests
{
    public class CriarResponsavelRequest
    {
        public required string Nome { get; set; } = string.Empty;
        public required string Telefone { get; set; } = string.Empty;
        public string? Email { get; set; }
    }
}