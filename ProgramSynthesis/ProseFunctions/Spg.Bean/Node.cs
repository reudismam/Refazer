using Microsoft.CodeAnalysis;
using TreeElement.Spg.Node;

namespace ProseFunctions.Substrings
{
    public class Node
    {
        /// <summary>
        /// Value of the node
        /// </summary>
        public TreeNode<SyntaxNodeOrToken> Value { get; set; }

        /// <summary>
        /// Left Sinbling
        /// </summary>
        public Node LeftNode { get; set; }

        /// <summary>
        /// /Right sinbling
        /// </summary>
        public Node RightNode { get; set; }

        /// <summary>
        /// Create a new Node
        /// </summary>
        /// <param name="value"></param>
        public Node(TreeNode<SyntaxNodeOrToken> value)
        {
            Value = value;
        }

        /// <summary>
        /// To string
        /// </summary>
        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
