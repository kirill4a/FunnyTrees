using FunnyTrees.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FunnyTrees.Persistence.Repositories;

internal abstract class RepositoryBase<T> where T : class, new()
{
    protected readonly IUnitOfWork _unitOfWork;

    protected RepositoryBase(DbSet<T> set, IUnitOfWork unitOfWork)
    {
        Set = set ?? throw new ArgumentNullException(nameof(set));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    protected DbSet<T> Set { get; }
}
