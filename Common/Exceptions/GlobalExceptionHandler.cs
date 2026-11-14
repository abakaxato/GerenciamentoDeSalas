using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace GerenciamentoSalas.API.Common.Exceptions
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {

            var (statusCode, title) = exception switch
            {
                KeyNotFoundException => (StatusCodes.Status404NotFound, "Não encontrado"),
                InvalidOperationException => (StatusCodes.Status409Conflict, "Conflito de regra de negócio"),
                ArgumentException => (StatusCodes.Status400BadRequest, "Requisição inválida"),
                DisponibilidadeException => (StatusCodes.Status409Conflict, "Conflito de disponibilidade"),
                ExistenciaNaTabelaException => (StatusCodes.Status404NotFound, "Nao Existe na tabela"),
                _ => (StatusCodes.Status500InternalServerError, "Erro interno no servidor")
            };

            if (statusCode >= 500)
            {
                _logger.LogError(exception, "Erro crítico não tratado: {Message}", exception.Message);
            }
            else
            {
                _logger.LogWarning("Validação de negócio acionada [{StatusCode} - {Title}]: {Message}", statusCode, title, exception.Message);
            }

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = exception.Message
            };

            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            return true;
        }
    }
}
