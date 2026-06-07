namespace GerenciamentoSalas.API.Models
{
    public class Agendamento
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int ProfissionalId { get; set; }
        public int SalaId { get; set; }
    }
}
