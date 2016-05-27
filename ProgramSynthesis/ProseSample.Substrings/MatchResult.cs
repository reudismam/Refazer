using Microsoft.CodeAnalysis;
using System;
using TreeElement.Spg.Node;

namespace ProseSample.Substrings
{
    public class MatchResult
    {
        private Tuple<ITreeNode<SyntaxNodeOrToken>, Bindings> _match { get; set; }

        public Tuple<ITreeNode<SyntaxNodeOrToken>, Bindings> Match
        {
            get { return _match; }
            set { _match = value; }
        }

        public MatchResult(Tuple<ITreeNode<SyntaxNodeOrToken>, Bindings> match)
        {
            Match = match;
        }
    }
}
