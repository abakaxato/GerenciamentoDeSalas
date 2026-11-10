namespace GerenciamentoSalas.API.Exceptions
{
    public class ConflitoHorarioException : Exception
    {
        public ConflitoHorarioException() : base() { }
        public ConflitoHorarioException(string message) : base(message) { }
        public ConflitoHorarioException(string message, Exception innerException)
        : base(message, innerException) { }
    }
}
