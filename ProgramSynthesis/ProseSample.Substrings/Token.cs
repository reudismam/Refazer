using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using TreeElement.Spg.Node;

namespace ProseSample.Substrings
{
    public class Token
    {
        public SyntaxKind Kind { get; set; }

        public Token(SyntaxKind kind)
        {
            Kind = kind;
        }

        public virtual bool IsMatch(ITreeNode<SyntaxNodeOrToken> node)
        {
            return node.Value.IsKind(Kind);
        }

        public override string ToString()
        {
            return $"Token({Kind})";
        }
    }
}
