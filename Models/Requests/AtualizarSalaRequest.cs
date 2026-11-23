namespace GerenciamentoSalas.API.Models.Requests
{
    public class AtualizarSalaRequest
    {
        public string? Nome { get; set; } = string.Empty;
        public int? Capacidade { get; set; }
    }
}