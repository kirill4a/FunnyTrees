using FunnyTrees.Application.Contracts.Dispatching;
using FunnyTrees.Application.Contracts.Dto;
using FunnyTrees.Application.Contracts.Services.ExceptionHandling;

namespace FunnyTrees.Application.Services.ExceptionHandling;
internal class SecureExceptionHandler : BaseExceptionHandler, IExceptionHandler
{
    public SecureExceptionHandler(ICommandDispatcher dispatcher) : base(dispatcher)
    {
    }

    public async Task<ErrorDto> HandleAsync(ExceptionWrapper wrapper, CancellationToken cancellation)
    {
        await DispathcException(wrapper, cancellation).ConfigureAwait(false);
        return new ErrorDto
        {
            Id = wrapper.EventId,
            Type = ErrorTypeDto.Secure,
            Data = new ErrorDataDto { Message = wrapper.Error.Message }
        };
    }
}