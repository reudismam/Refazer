using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using TreeElement.Spg.Node;

namespace ProseSample.Substrings
{
    public class LeafToken : Token 
    {
        public LeafToken(SyntaxKind kind) : base(kind)
        {
            Kind = kind;
        }

        public override bool IsMatch(ITreeNode<SyntaxNodeOrToken> node)
        {
            return base.IsMatch(node) && !node.Children.Any();
        }

        public override string ToString()
        {
            return $"LeafToken({Kind})";
        }
    }
}
