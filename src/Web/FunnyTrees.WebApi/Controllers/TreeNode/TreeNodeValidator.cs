using FluentValidation;
using FunnyTrees.Application.Contracts.Dto;

namespace FunnyTrees.WebApi.Controllers.TreeNode;

public class TreeNodeValidator : AbstractValidator<TreeNodeDto>
{
    public TreeNodeValidator()
    {
        RuleFor(n => n.Name).NotEmpty();
    }
}
