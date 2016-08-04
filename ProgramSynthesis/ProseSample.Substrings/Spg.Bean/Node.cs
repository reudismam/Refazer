using Microsoft.CodeAnalysis;
using TreeElement.Spg.Node;

namespace ProseSample.Substrings
{
    public class Node
    {
        public ITreeNode<SyntaxNodeOrToken> Value { get; set; }
        
        public Node LeftNode { get; set; }

        public Node RightNode { get; set; }

        public Node(ITreeNode<SyntaxNodeOrToken> value/*, ITreeNode<SyntaxNodeOrToken> leftNode = null, ITreeNode<SyntaxNodeOrToken> rightNode = null*/)
        {
            Value = value;
            //LeftNode = leftNode;
            //RightNode = rightNode;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
