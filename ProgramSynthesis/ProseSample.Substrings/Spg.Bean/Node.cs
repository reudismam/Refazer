using Microsoft.CodeAnalysis;
using TreeElement.Spg.Node;

namespace ProseSample.Substrings
{
    public class Node
    {
        public ITreeNode<SyntaxNodeOrToken> Value { get; set; }
        public ITreeNode<SyntaxNodeOrToken> SyntaxTree { get; set; }

        public Node(ITreeNode<SyntaxNodeOrToken> value, ITreeNode<SyntaxNodeOrToken> syntaxTree = null)
        {
            Value = value;
            SyntaxTree = syntaxTree;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
