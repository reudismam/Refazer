using System.Linq;
using Microsoft.CodeAnalysis;
using TreeElement.Spg.Node;

namespace ProseFunctions.Substrings
{
    public class LeafToken : Token 
    {
        public LeafToken(string kind, TreeNode<SyntaxNodeOrToken> value) : base(kind, value)
        {
            Kind = kind;
        }

        public override bool IsMatch(TreeNode<SyntaxNodeOrToken> node)
        {
            return base.IsMatch(node) && !node.Children.Any();
        }

        public override string ToString()
        {
            return $"LeafToken({Kind})";
        }
    }
}
