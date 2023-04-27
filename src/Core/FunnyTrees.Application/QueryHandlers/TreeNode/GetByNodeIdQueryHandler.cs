using FunnyTrees.Application.Contracts.Dto;
using FunnyTrees.Application.Contracts.Queries;
using FunnyTrees.Application.Contracts.Queries.TreeNode;
using FunnyTrees.Application.Extensions.Mapping;
using FunnyTrees.Domain.Repositories;

namespace FunnyTrees.Application.QueryHandlers.TreeNode;

internal class GetByNodeIdQueryHandler : IQueryHandler<GetByNodeIdQuery, TreeNodeDto?>
{
    private readonly ITreeNodeRepository _treeNodeRepository;

    public GetByNodeIdQueryHandler(ITreeNodeRepository treeNodeRepository)
        =>
        _treeNodeRepository = treeNodeRepository ?? throw new ArgumentNullException(nameof(treeNodeRepository));

    public async Task<TreeNodeDto?> HandleAsync(GetByNodeIdQuery query, CancellationToken cancellation)
    {
        if (query == null)
            throw new ArgumentNullException(nameof(query));

        var domainEntity = await _treeNodeRepository.GetAsync(query.NodeId, cancellation)
                                                    .ConfigureAwait(false);
        return domainEntity?.MapToDto();
    }
}