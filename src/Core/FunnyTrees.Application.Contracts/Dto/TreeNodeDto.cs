using System.Collections.Generic;

namespace FunnyTrees.Application.Contracts.Dto;

/// <summary>
/// The node of the tree
/// </summary>
public class TreeNodeDto
{
    /// <summary>
    /// Node id
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Node name
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Children nodes
    /// </summary>
    public IEnumerable<TreeNodeDto> Children { get; init; }
}
