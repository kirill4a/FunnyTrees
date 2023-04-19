using FunnyTrees.Application.Contracts.Queries;
using System.Threading.Tasks;
using System.Threading;

namespace FunnyTrees.Application.Contracts.Dispatching;

/// <summary>
/// CQRS-styled query dispatcher
/// </summary>
public interface IQueryDispatcher
{
    /// <summary>
    /// Dispatches the query and executes the appropriate handler for the query
    /// </summary>
    /// <typeparam name="TQuery">The type of query</typeparam>
    /// <typeparam name="TResult">The type of query execution result</typeparam>
    /// <param name="query">The query</param>
    /// <param name="cancellation">Cancellation token</param>
    /// <returns>The result of query execution</returns>
    Task<TResult> DispatchAsync<TQuery, TResult>(TQuery query, CancellationToken cancellation)
        where TQuery : IQuery<TResult>;
}