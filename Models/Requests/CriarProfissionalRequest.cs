namespace GerenciamentoSalas.API.Models.Requests
{
    public class CriarProfissionalRequest
    {
        public string Nome { get; set; } = string.Empty;
        public string Especialidade { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true;
    }
}