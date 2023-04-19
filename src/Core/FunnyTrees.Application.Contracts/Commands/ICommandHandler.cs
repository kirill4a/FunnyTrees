using System.Threading.Tasks;
using System.Threading;

namespace FunnyTrees.Application.Contracts.Commands;

public interface ICommandHandler<in TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    Task<TResult> HandleAsync(TCommand command, CancellationToken cancellation);
}
