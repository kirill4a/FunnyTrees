using FunnyTrees.Application.Contracts.Commands.TreeNode;
using FunnyTrees.Application.Contracts.Commands;
using FunnyTrees.Common.Types;
using FunnyTrees.Domain.Repositories;
using FunnyTrees.Common.Exceptions;

namespace FunnyTrees.Application.CommandHandlers.TreeNode;

internal class DeleteTreeNodeCommandHandler : ICommandHandler<DeleteTreeNodeCommand, Unit>
{
    private readonly ITreeNodeRepository _treeNodeRepository;

    public DeleteTreeNodeCommandHandler(ITreeNodeRepository treeNodeRepository)
    {
        _treeNodeRepository = treeNodeRepository ?? throw new ArgumentNullException(nameof(treeNodeRepository));
    }

    public async Task<Unit> HandleAsync(DeleteTreeNodeCommand command, CancellationToken cancellation)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        var domainEntity = await _treeNodeRepository.GetAsync(command.NodeId, cancellation).ConfigureAwait(false);
        if (domainEntity == null)
            return Unit.Value;

        if (domainEntity.Children.Any())
            throw new SecureException("You have to delete all children nodes first");

        await _treeNodeRepository.DeleteAsync(domainEntity, cancellation).ConfigureAwait(false);
        return Unit.Value;
    }
}
