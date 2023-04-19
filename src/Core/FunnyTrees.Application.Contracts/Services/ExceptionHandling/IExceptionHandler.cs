using FunnyTrees.Application.Contracts.Dto;
using System.Threading;
using System.Threading.Tasks;

namespace FunnyTrees.Application.Contracts.Services.ExceptionHandling;

public interface IExceptionHandler
{
    Task<ErrorDto> HandleAsync(ExceptionWrapper wrapper, CancellationToken cancellation);
}
