namespace GerenciamentoSalas.API.DTOs
{
    public class CriarResponsavelRequest
    {
        public string Nome { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string? Email { get; set; }
    }
}