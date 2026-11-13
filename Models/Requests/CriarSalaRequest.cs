namespace GerenciamentoSalas.API.Models.Requests
{
    public class CriarSalaRequest
    {
        public string Nome { get; set; } = string.Empty;
        public int Capacidade { get; set; }
    }
}