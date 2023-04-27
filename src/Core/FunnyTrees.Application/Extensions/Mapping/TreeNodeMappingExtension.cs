using FunnyTrees.Application.Contracts.Dto;
using DomainEntity = FunnyTrees.Domain.Aggregates.TreeNode;

namespace FunnyTrees.Application.Extensions.Mapping;

internal static class TreeNodeMappingExtension
{
    internal static TreeNodeDto? MapToDto(this DomainEntity? source)
    {
        return (source == null)
            ? null
            : new()
            {
                Id = source.Id,
                Name = source.Name,
                Children = source.Children.Select(MapNotNull)
            };
    }

    private static TreeNodeDto MapNotNull(DomainEntity source)
        =>
        source == null
        ? throw new ArgumentNullException(nameof(source))
        : MapToDto(source)!;
}