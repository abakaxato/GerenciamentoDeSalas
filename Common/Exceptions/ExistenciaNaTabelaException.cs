namespace GerenciamentoSalas.API.Common.Exceptions
{
    public class ExistenciaNaTabelaException : Exception
    {
        public ExistenciaNaTabelaException() : base() { }
        public ExistenciaNaTabelaException(object? obj)
            : base($"O ID {obj} não existe na tabela selecionada.") { }
    }
}
