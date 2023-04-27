using FunnyTrees.Persistence.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FunnyTrees.Persistence.Entities.Configurations;

internal class TreeNodeConfiguration : IEntityTypeConfiguration<TreeNode>
{
    public void Configure(EntityTypeBuilder<TreeNode> builder)
    {
        var tableName = nameof(TreeNode);
        var columnId = nameof(TreeNode.Id);
        var columnParentId = nameof(TreeNode.ParentId);

        builder.ToTable(tableName, t => t.HasCheckConstraint($"CK_{tableName}_ParentId",
                                                             $"[{columnId}] > [{columnParentId}]"));

        builder.HasKey(n => n.Id);
        builder.Property(n => n.ParentId).IsRequired(false);
        builder.Property(n => n.Name).IsRequired();

        builder.HasOne(n => n.Parent)
              .WithMany(b => b.Children)
              .HasForeignKey(a => a.ParentId)
              .IsRequired()
              .OnDelete(DeleteBehavior.Restrict);
    }
}
