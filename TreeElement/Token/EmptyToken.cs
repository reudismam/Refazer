using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using TreeElement.Spg.Node;

namespace ProseSample.Substrings
{
    public class EmptyToken : Token 
    {
        public EmptyToken() : base(SyntaxKind.None)
        {
        }

        public override bool IsMatch(ITreeNode<SyntaxNodeOrToken> node)
        {
            return true;
        }

        public override string ToString()
        {
            return "EmmptyToken";
        }
    }
}
