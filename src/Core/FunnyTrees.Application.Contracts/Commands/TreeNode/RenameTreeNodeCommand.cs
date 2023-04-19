using FunnyTrees.Common.Types;

namespace FunnyTrees.Application.Contracts.Commands.TreeNode;

public record RenameTreeNodeCommand(int NodeId, string NewNodeName):ICommand<Unit>;
