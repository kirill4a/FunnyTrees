using FunnyTrees.Application.Contracts.Dispatching;
using FunnyTrees.Application.Contracts.Dto;
using FunnyTrees.Application.Contracts.Services.ExceptionHandling;

namespace FunnyTrees.Application.Services.ExceptionHandling;
internal class RegularExceptionHandler : BaseExceptionHandler, IExceptionHandler
{
    public RegularExceptionHandler(ICommandDispatcher dispatcher) : base(dispatcher)
    {
    }

    public async Task<ErrorDto> HandleAsync(ExceptionWrapper wrapper, CancellationToken cancellation)
    {
        await DispathcException(wrapper, cancellation).ConfigureAwait(false);
        return new ErrorDto
        {
            Id = wrapper.EventId,
            Type = ErrorTypeDto.Common,
            Data = new ErrorDataDto { Message = $"Internal server error ID = {wrapper.EventId}" }
        };
    }
}