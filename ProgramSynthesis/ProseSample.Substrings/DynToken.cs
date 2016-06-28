using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using TreeElement.Spg.Node;

namespace ProseSample.Substrings
{
    public class DynToken : Token
    {
        public SyntaxNodeOrToken Value;

        public DynToken(SyntaxKind kind, SyntaxNodeOrToken value) : base(kind)
        {
            Value = value;
        }

        public override bool IsMatch(ITreeNode<SyntaxNodeOrToken> node)
        {
            return node.Value.IsKind(Kind) && node.ToString().Equals(Value.ToString());
        }

        public override string ToString()
        {
            return $"DynToken({Value})";
        }
    }
}
