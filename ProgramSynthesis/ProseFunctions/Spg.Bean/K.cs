using Microsoft.CodeAnalysis;
using ProseSample.Substrings;
using ProseSample.Substrings.Spg.Witness;
using TreeEdit.Spg.Match;
using TreeElement.Spg.Node;

namespace ProseSample.Substrings
{
    public class K
    {
        private readonly TreeNode<SyntaxNodeOrToken> _input;
        private readonly TreeNode<SyntaxNodeOrToken> _node;

        public K(TreeNode<SyntaxNodeOrToken> input, TreeNode<SyntaxNodeOrToken> node)
        {
            _input = input;
            _node = node;
        }

        public int GetK(Pattern patternExample)
        {
            var pattern = patternExample.Tree;

            var currentTree = _input;
            var matches = MatchManager.Matches(currentTree, pattern);

            for (int i = 0; i < matches.Count; i++)
            {
                var match = matches[i];
                var compare = Semantics.FindChild(match, patternExample.K);
                if (compare != null && Match.IsEqual(compare.Value, _node.Value))
                {
                    return i + 1;
                }
            }
            return -1;
        }

        public override string ToString()
        {
            return "K";
        }

        public override bool Equals(object obj)
        {
            if (obj is K) return true;
            return false;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}
