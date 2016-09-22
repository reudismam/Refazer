using Microsoft.CodeAnalysis;

namespace ProseSample.Substrings
{
    public class Node
    {
        public TreeNode<SyntaxNodeOrToken> Value { get; set; }

        public Node Parent { get; set; }

        public Node Children { get; set; }

        public Node LeftNode { get; set; }

        public Node RightNode { get; set; }

        public Node(TreeNode<SyntaxNodeOrToken> value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
