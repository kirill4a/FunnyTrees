using FunnyTrees.Application.Contracts.Commands;
using FunnyTrees.Application.Contracts.Commands.TreeNode;
using FunnyTrees.Domain.Repositories;
using DomainTreeNode = FunnyTrees.Domain.Aggregates.TreeNode;

namespace FunnyTrees.Application.CommandHandlers.TreeNode;

internal class CreateTreeNodeCommandHandler : ICommandHandler<CreateTreeNodeCommand, int>
{
    private readonly ITreeNodeRepository _treeNodeRepository;

    public CreateTreeNodeCommandHandler(ITreeNodeRepository treeNodeRepository)
    {
        _treeNodeRepository = treeNodeRepository ?? throw new ArgumentNullException(nameof(treeNodeRepository));
    }

    public async Task<int> HandleAsync(CreateTreeNodeCommand command, CancellationToken cancellation)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        var domainEntity = new DomainTreeNode(0, command.ParentId, command.Name);
        var result = await _treeNodeRepository.CreateAsync(domainEntity, cancellation).ConfigureAwait(false);
        return result;
    }
}