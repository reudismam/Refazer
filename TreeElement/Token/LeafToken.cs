using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using ProseSample.Substrings;
using TreeElement.Spg.Node;

namespace ProseSample.Substrings
{
    public class LeafToken : Token 
    {
        public LeafToken(SyntaxKind kind, TreeNode<SyntaxNodeOrToken> value) : base(kind, value)
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
