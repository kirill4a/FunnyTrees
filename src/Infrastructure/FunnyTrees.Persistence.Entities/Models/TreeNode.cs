using System.Collections.Generic;

namespace FunnyTrees.Persistence.Entities.Models
{
    public class TreeNode
    {
        public int Id { get; init; }
        public int? ParentId { get; init; }
        public string Name { get; init; }

        public TreeNode? Parent { get; init; }
        public ICollection<TreeNode> Children { get; init; }
    }
}
