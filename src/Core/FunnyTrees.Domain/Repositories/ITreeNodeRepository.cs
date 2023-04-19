using FunnyTrees.Domain.Aggregates;

namespace FunnyTrees.Domain.Repositories;

public interface ITreeNodeRepository : IRepository<TreeNode>
{
    Task<TreeNode?> GetRootAsync(string rootNodeName, CancellationToken cancellation);
}
