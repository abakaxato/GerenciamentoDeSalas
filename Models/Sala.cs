namespace GerenciamentoSalas.API.Models
{
    public class Sala
    {
        public int Id { get; set;}
        public string NomeSala { get; set; } = string.Empty;
        public int capacidade { get; set; }
    }
}
