using FunnyTrees.Domain.Aggregates;

namespace FunnyTrees.Domain.Repositories;

public interface IRepository<T> where T : IAggregateRoot
{
    Task<int> CreateAsync(T entity, CancellationToken cancellation);
    Task<T?> GetAsync(int entityId, CancellationToken cancellation);
    Task DeleteAsync(T entity, CancellationToken cancellation);
}
