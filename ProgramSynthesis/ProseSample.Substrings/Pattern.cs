using Microsoft.CodeAnalysis;
using Tutor.Spg.Node;

namespace ProseSample.Substrings
{
    public class Pattern
    {
        public ITreeNode<Token> Tokens;

        public ITreeNode<SyntaxNodeOrToken> Tree;

        public Pattern(ITreeNode<SyntaxNodeOrToken> tree)
        {
            Tree = tree;
        }
    }
}
