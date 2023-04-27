using FunnyTrees.Common.Exceptions;
using System.Collections.Concurrent;

namespace FunnyTrees.Domain.Aggregates;

public class TreeNode : IAggregateRoot
{
    private readonly ConcurrentDictionary<int, TreeNode> _childrenDictionary;

    public TreeNode(int id, int? parentId, string name)
    {
        if (id > 0 && parentId >= id)
            throw new ArgumentException($"Parent node [{parentId}] should be younger than this node [{id}]");

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(nameof(name));

        Id = id;
        ParentId = parentId;
        Name = name;
        _childrenDictionary = new ConcurrentDictionary<int, TreeNode>();
    }

    public int Id { get; }
    public int? ParentId { get; }
    public string Name { get; }

    public bool IsRoot => !ParentId.HasValue;

    public IEnumerable<TreeNode> Children => _childrenDictionary.Values;

    public bool TryAddChild(TreeNode child)
    {
        if (child == null)
            throw new ArgumentNullException(nameof(child));

        if (child.ParentId != Id)
            throw new SecureException($"Node with id '{child.Id}' is not the child of node '{Id}'");

        if (_childrenDictionary.TryGetValue(child.Id, out var _))
            return false;

        return _childrenDictionary.TryAdd(child.Id, child);
    }
}