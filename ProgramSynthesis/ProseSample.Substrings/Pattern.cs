using Microsoft.CodeAnalysis;
using TreeElement.Spg.Node;

namespace ProseSample.Substrings
{
    public class Pattern
    {
        public ITreeNode<Token> Tree;

        public Pattern(ITreeNode<Token> tree)
        {
            Tree = tree;
        }
    }
}
