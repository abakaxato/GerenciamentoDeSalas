namespace GerenciamentoSalas.API.DTOs
{
    public class CriarSalaRequest
    {
        public string Nome { get; set; } = string.Empty;
        public int Capacidade { get; set; }
    }
}