using Autofac;
using FunnyTrees.Application.Contracts.Dispatching;
using FunnyTrees.Application.Contracts.Queries;

namespace FunnyTrees.Application.Dispatching;
internal class QueryDispatcher : IQueryDispatcher
{
    private readonly IComponentContext _componentContext;

    public QueryDispatcher(IComponentContext componentContext)
        => _componentContext = componentContext ?? throw new ArgumentNullException(nameof(componentContext));

    public async Task<TResult> DispatchAsync<TQuery, TResult>(TQuery query, CancellationToken cancellation)
        where TQuery : IQuery<TResult>
    {
        if (query == null) throw new ArgumentNullException(nameof(query));
        if (!_componentContext.TryResolve<IQueryHandler<TQuery, TResult>>(out var handler))
            throw new InvalidOperationException("Query handler for the following query type is not registered." +
                                                $" Query type: '{typeof(TQuery).FullName}'");

        var result = await handler.HandleAsync(query, cancellation).ConfigureAwait(false);
        return result;
    }
}