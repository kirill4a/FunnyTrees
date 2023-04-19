using FunnyTrees.Application.Contracts.Dto;
using FunnyTrees.Application.Contracts.Services.ExceptionHandling;
using FunnyTrees.Common.Exceptions;

namespace FunnyTrees.Application.Services.ExceptionHandling;

internal class ExceptionHandlerStrategy : IExceptionHandlerStrategy
{
    private readonly IExceptionHandler secureExceptionHandler;
    private readonly IExceptionHandler exceptionHandler;

    public ExceptionHandlerStrategy(
        IExceptionHandler secureExceptionHandler,
        IExceptionHandler exceptionHandler)
    {
        this.secureExceptionHandler = secureExceptionHandler
            ?? throw new ArgumentNullException(nameof(secureExceptionHandler));
        this.exceptionHandler = exceptionHandler ?? throw new ArgumentNullException(nameof(exceptionHandler));
    }

    public async Task<ErrorDto> ExecuteAsync(ExceptionWrapper wrapper, CancellationToken cancellation)
    {
        if (wrapper.Error is SecureException _)
            return await secureExceptionHandler.HandleAsync(wrapper, cancellation).ConfigureAwait(false);

        return await exceptionHandler.HandleAsync(wrapper, cancellation).ConfigureAwait(false);
    }
}
