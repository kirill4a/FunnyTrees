using FunnyTrees.Application.Contracts.Dto;
using FunnyTrees.Application.Contracts.Queries;
using FunnyTrees.Application.Contracts.Queries.TreeNode;
using FunnyTrees.Application.Extensions.Mapping;
using FunnyTrees.Domain.Repositories;

namespace FunnyTrees.Application.QueryHandlers.TreeNode;
internal class GetByRootNameQueryHandler : IQueryHandler<GetByRootNameQuery, TreeNodeDto?>
{
    private readonly ITreeNodeRepository _treeNodeRepository;

    public GetByRootNameQueryHandler(ITreeNodeRepository treeNodeRepository)
        =>
        _treeNodeRepository = treeNodeRepository ?? throw new ArgumentNullException(nameof(treeNodeRepository));

    public async Task<TreeNodeDto?> HandleAsync(GetByRootNameQuery query, CancellationToken cancellation)
    {
        if (query == null)
            throw new ArgumentNullException(nameof(query));

        var domainEntity = await _treeNodeRepository.GetRootAsync(query.RootNodeName, cancellation)
                                                    .ConfigureAwait(false);
        return domainEntity?.MapToDto();
    }
}