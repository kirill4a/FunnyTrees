using Autofac;
using FunnyTrees.Application.Contracts.Commands;
using FunnyTrees.Application.Contracts.Dispatching;

namespace FunnyTrees.Application.Dispatching;
internal class CommandDispatcher : ICommandDispatcher
{
    private readonly IComponentContext _componentContext;

    public CommandDispatcher(IComponentContext componentContext)
        => _componentContext = componentContext ?? throw new ArgumentNullException(nameof(componentContext));

    public async Task<TResult> DispatchAsync<TCommand, TResult>(TCommand command, CancellationToken cancellation)
        where TCommand : ICommand<TResult>
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));
        if (!_componentContext.TryResolve<ICommandHandler<TCommand, TResult>>(out var handler))
            throw new InvalidOperationException("Command handler for the following command type is not registered." +
                                                $" Command type: '{typeof(TCommand).FullName}'");

        var result = await handler.HandleAsync(command, cancellation).ConfigureAwait(false);
        return result;
    }
}