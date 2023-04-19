using FunnyTrees.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using DbTreeNode = FunnyTrees.Persistence.Entities.Models.TreeNode;
using DomainTreeNode = FunnyTrees.Domain.Aggregates.TreeNode;

namespace FunnyTrees.Persistence.Repositories;

internal class TreeNodeRepository : RepositoryBase<DbTreeNode>, ITreeNodeRepository
{
    public TreeNodeRepository(DbSet<DbTreeNode> set, IUnitOfWork unitOfWork) : base(set, unitOfWork)
    {
    }

    public async Task<int> CreateAsync(DomainTreeNode entity, CancellationToken cancellation)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        var dbEntity = new DbTreeNode
        {
            ParentId = entity.ParentId,
            Name = entity.Name
        };
        Set.Add(dbEntity);

        await _unitOfWork.CommitAsync(cancellation).ConfigureAwait(false);
        return dbEntity.Id;
    }

    public async Task<DomainTreeNode?> GetAsync(int entityId, CancellationToken cancellation)
    {
        var dbEntity = await Set.FindAsync(entityId, cancellation).ConfigureAwait(false);
        return Map(dbEntity);
    }

    public async Task<DomainTreeNode?> GetRootAsync(string rootNodeName, CancellationToken cancellation)
    {
        var dbEntity = await Set.FirstOrDefaultAsync(
                                            n => n.Name.ToLower() == rootNodeName.ToLower(),
                                            cancellation)
                                .ConfigureAwait(false);
        return Map(dbEntity);
    }

    public async Task DeleteAsync(DomainTreeNode entity, CancellationToken cancellation)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        var dbEntity = await Set.FindAsync(entity.Id, cancellation).ConfigureAwait(false)
            ?? throw new Exception($"TreeNode with id '{entity.Id}' was not found in the database");

        Set.Remove(dbEntity);
        await _unitOfWork.CommitAsync(cancellation).ConfigureAwait(false);
    }

    private DomainTreeNode? Map(DbTreeNode? dbEntity)
        =>
        (dbEntity == null)
            ? null
            : new DomainTreeNode(dbEntity.Id, dbEntity.ParentId, dbEntity.Name);
}
