using FunnyTrees.Common.Types;

namespace FunnyTrees.Application.Contracts.Commands.TreeNode;

public record DeleteTreeNodeCommand(int NodeId):ICommand<Unit>;
