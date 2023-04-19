using FunnyTrees.Application.Contracts.Dto;

namespace FunnyTrees.Application.Contracts.Queries.TreeNode;

public record GetByNodeIdQuery(int NodeId) : IQuery<TreeNodeDto?>;
