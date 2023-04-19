namespace FunnyTrees.Domain.Repositories;
public interface IUnitOfWork
{
    Task CommitAsync(CancellationToken cancellation);
}
