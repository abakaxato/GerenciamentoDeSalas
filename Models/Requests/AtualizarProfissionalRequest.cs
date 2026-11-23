namespace GerenciamentoSalas.API.Models.Requests
{
    public class AtualizarProfissionalRequest
    {
        public string? Nome { get; set; } = string.Empty;
        public string? Especialidade { get; set; } = string.Empty;
        public string? Telefone { get; set; }
    }
}