using System.Threading.Tasks;
using System.Threading;
using FunnyTrees.Application.Contracts.Commands;

namespace FunnyTrees.Application.Contracts.Dispatching;

/// <summary>
/// CQRS-styled command dispatcher
/// </summary>
public interface ICommandDispatcher
{
    /// <summary>
    /// Dispatches the command and executes the appropriate handler for the command
    /// </summary>
    /// <typeparam name="TCommand">The type of command</typeparam>
    /// <typeparam name="TResult">The type of command execution result</typeparam>
    /// <param name="command">The command</param>
    /// <param name="cancellation">Cancellation token</param>
    /// <returns>The result of command execution</returns>
    Task<TResult> DispatchAsync<TCommand, TResult>(TCommand command, CancellationToken cancellation)
        where TCommand : ICommand<TResult>;
}
