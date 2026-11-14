namespace GerenciamentoSalas.API.Common.Exceptions
{
    public class DisponibilidadeException : Exception
    {
        public DisponibilidadeException() : base() { }
        public DisponibilidadeException(object? obj, DateTime horaInicio, DateTime horaFim, string tipoEntidade = "") : base($"{tipoEntidade} {obj} nao esta disponivel entre {horaInicio} e {horaFim}") { }
    }
}
