using Microsoft.CodeAnalysis;
using TreeElement.Spg.Node;

namespace TreeElement.Token
{
    public class EmptyToken : Token 
    {
        public EmptyToken() : base(Token.Expression, null)
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
