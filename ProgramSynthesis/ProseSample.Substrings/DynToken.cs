using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Tutor.Spg.Node;

namespace ProseSample.Substrings
{
    public class DynToken : Token
    {
        public ITreeNode<SyntaxNodeOrToken> Value;

        public DynToken(SyntaxKind kind, ITreeNode<SyntaxNodeOrToken> value = null) : base(kind)
        {
            Value = value;
        }
    }
}
