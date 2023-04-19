using FunnyTrees.Application.Contracts.Dto;
using System;

namespace FunnyTrees.Application.Contracts.Queries.TreeNode;

public record GetByRootNameQuery : IQuery<TreeNodeDto?>
{
    public GetByRootNameQuery(string rootNodeName)
    {
        if (string.IsNullOrWhiteSpace(rootNodeName))
            throw new ArgumentNullException(nameof(rootNodeName));

        RootNodeName = rootNodeName;
    }

    public string RootNodeName { get; }
}
