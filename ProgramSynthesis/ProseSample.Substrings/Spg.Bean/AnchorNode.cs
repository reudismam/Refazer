using Microsoft.CodeAnalysis;
using TreeElement.Spg.Node;

namespace ProseSample.Substrings
{
    public class AnchorNode
    {
        /// <summary>
        /// Value node
        /// </summary>
        public ITreeNode<SyntaxNodeOrToken> Value { get; set; }

        /// <summary>
        /// Parent node
        /// </summary>
        public ITreeNode<SyntaxNodeOrToken> Parent { get; set; }
        
        /// <summary>
        /// Left node
        /// </summary>
        public ITreeNode<SyntaxNodeOrToken> LeftNode { get; set; }

        /// <summary>
        /// Right node
        /// </summary>
        public ITreeNode<SyntaxNodeOrToken> RightNode { get; set; }

        /// <summary>
        /// Create a new anchor node
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="leftNode">Left node</param>
        /// <param name="rightNode">Right Node</param>
        public AnchorNode(ITreeNode<SyntaxNodeOrToken> value, ITreeNode<SyntaxNodeOrToken> leftNode = null, ITreeNode<SyntaxNodeOrToken> rightNode = null)
        {
            Value = value;
            LeftNode = leftNode;
            RightNode = rightNode;
            Parent = rightNode?.Parent;
        }

        public override string ToString()
        {
            return $"[{Value}, {Parent}, {LeftNode}, {RightNode}]";
        }
    }
}
