namespace GerenciamentoSalas.API.Common.Exceptions
{
    public class ExistenciaException : Exception
    {
        public ExistenciaException() : base() { }
        public ExistenciaException(object? obj)
            : base($"O ID {obj} não existe na tabela selecionada."){ }
    }
}
