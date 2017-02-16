using Microsoft.CodeAnalysis;
using TreeElement.Spg.Node;

namespace TreeElement.Token
{
    public class EmptyToken : Token
    {
        public EmptyToken() : base(new Label(Expression), null)
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
