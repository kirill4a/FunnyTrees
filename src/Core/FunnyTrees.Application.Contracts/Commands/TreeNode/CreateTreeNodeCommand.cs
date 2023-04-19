using System;

namespace FunnyTrees.Application.Contracts.Commands.TreeNode;

public record CreateTreeNodeCommand : ICommand<int>
{
    public CreateTreeNodeCommand(string name, int? parentId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(nameof(name));

        Name = name;
        ParentId = parentId;
    }

    public string Name { get; }
    public int? ParentId { get; }
}