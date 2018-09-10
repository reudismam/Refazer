using Microsoft.CodeAnalysis;
using RefazerObject.Region;
using TreeElement.Spg.Node;

namespace RefazerFunctions.Bean
{
    public class Node
    {
        /// <summary>
        /// Region associated to a node.
        /// </summary>
        public Region Region { get; set; }

        /// <summary>
        /// Enum, which defines the example kind.
        /// </summary>
        public enum ExampleKind
        {
            Positive,
            Negative
        }

        /// <summary>
        /// Kind of the example, which can be positive or negative.
        /// </summary>
        public ExampleKind Kind;

        /// <summary>
        /// Value of the node
        /// </summary>
        public TreeNode<SyntaxNodeOrToken> Value { get; set; }

        public SyntaxNodeOrToken SyntaxTree { get; set; }

        /// <summary>
        /// Left Sibling
        /// </summary>
        public Node LeftNode { get; set; }

        /// <summary>
        /// /Right sibling
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

        public Node(Region region)
        {
            Region = region;
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
