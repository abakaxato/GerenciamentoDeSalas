namespace GerenciamentoSalas.API.Requests
{
    public class DeletarAgendamentoRequest
    {
        public required int SalaId { get; set; }
        public required DateTime DataHoraInicio { get; set; }
        public required DateTime DataHoraFim { get; set; }
    }
}
