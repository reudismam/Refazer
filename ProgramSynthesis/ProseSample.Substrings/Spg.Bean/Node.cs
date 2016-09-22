using Microsoft.CodeAnalysis;
using ProseSample.Substrings;

namespace ProseSample.Substrings
{
    public class Node
    {
        public ITreeNode<SyntaxNodeOrToken> Value { get; set; }

        public Node LeftNode { get; set; }

        public Node RightNode { get; set; }

        public Node(ITreeNode<SyntaxNodeOrToken> value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
