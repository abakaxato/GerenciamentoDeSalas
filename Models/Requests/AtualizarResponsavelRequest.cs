namespace GerenciamentoSalas.API.Models.Requests
{
    public class AtualizarResponsavelRequest
    {
        public string? Nome { get; set; } = string.Empty;
        public string? Telefone { get; set; } = string.Empty;
        public string? Email { get; set; }
    }
}