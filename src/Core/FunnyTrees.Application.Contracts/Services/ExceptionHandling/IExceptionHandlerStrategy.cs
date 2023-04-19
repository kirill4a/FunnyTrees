using FunnyTrees.Application.Contracts.Dto;
using System.Threading;
using System.Threading.Tasks;

namespace FunnyTrees.Application.Contracts.Services.ExceptionHandling;

public interface IExceptionHandlerStrategy
{
    Task<ErrorDto> ExecuteAsync(ExceptionWrapper wrapper, CancellationToken cancellation);
}
