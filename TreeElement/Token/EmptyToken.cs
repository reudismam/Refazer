using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using ProseSample.Substrings;

namespace ProseSample.Substrings
{
    public class EmptyToken : Token 
    {
        public EmptyToken() : base(SyntaxKind.EmptyStatement, null)
        {
        }

        public override bool IsMatch(TreeNode<SyntaxNodeOrToken> node)
        {
            return true;
        }

        public override string ToString()
        {
            return "EmptyToken";
        }
    }
}
