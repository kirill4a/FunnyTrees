using FunnyTrees.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Npgsql;
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
        var rawSql = @"
with recursive cte as (
	SELECT ""Id"", ""ParentId"", ""Name""
	FROM public.""TreeNode"" t
	WHERE t.""Id"" = @nodeId
	
	UNION ALL 
	
	SELECT t1.""Id"", t1.""ParentId"", t1.""Name""
	FROM cte
	JOIN public.""TreeNode"" t1 ON t1.""ParentId"" = cte.""Id"" 
)
select * from cte
";
        var nodeIdParameter = new NpgsqlParameter("@nodeId", entityId);
        var recursiveFamily = await Set
            .FromSqlRaw(rawSql, nodeIdParameter)
            .ToListAsync(cancellation)
            .ConfigureAwait(false);

        var dbEntity = recursiveFamily.FirstOrDefault(n => n.Id == entityId);

        return Map(dbEntity);
    }

    public async Task<DomainTreeNode?> GetRootAsync(string rootNodeName, CancellationToken cancellation)
    {
        if (string.IsNullOrWhiteSpace(rootNodeName))
            throw new ArgumentNullException(nameof(rootNodeName));

        var rawSql = @"
with recursive cte as (
	SELECT ""Id"", ""ParentId"", ""Name""
	FROM public.""TreeNode"" t
	WHERE t.""ParentId"" is null and t.""Name"" = @nodeName
	
	UNION ALL 
	
	SELECT t1.""Id"", t1.""ParentId"", t1.""Name""
	FROM cte
	JOIN public.""TreeNode"" t1 ON t1.""ParentId"" = cte.""Id"" 
)
select * from cte
";
        var nodeNameParameter = new NpgsqlParameter("@nodeName", rootNodeName.Trim());
        var recursiveFamily = await Set
            .FromSqlRaw(rawSql, nodeNameParameter)
            .ToListAsync()
            .ConfigureAwait(false);

        var dbEntity = recursiveFamily.FirstOrDefault(n => !n.ParentId.HasValue);

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
    {
        if (dbEntity == null)
            return null;

        var result = new DomainTreeNode(dbEntity.Id, dbEntity.ParentId, dbEntity.Name);
        if (!(dbEntity.Children?.Any() ?? false))
            return result;

        void AddChild(DomainTreeNode parent, DomainTreeNode child)
        {
            parent.TryAddChild(child);
            foreach (var nextChild in child.Children)
                AddChild(child, nextChild);
        }

        var children = dbEntity.Children.Select(Map);
        foreach (var child in children)
            AddChild(result, child!);

        return result;
    }
}
