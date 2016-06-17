using Microsoft.CodeAnalysis;
using TreeElement.Spg.Node;

namespace ProseSample.Substrings
{
    public class Node
    {
        public ITreeNode<SyntaxNodeOrToken> Value;
        public Node(ITreeNode<SyntaxNodeOrToken> value)
        {
            Value = value;
        }
    }
}
