using ProseSample.Substrings;
using TreeElement.Spg.Node;

namespace ProseSample.Substrings
{
    public class Pattern
    {
        public TreeNode<Token> Tree;

        public Pattern(TreeNode<Token> tree)
        {
            Tree = tree;
        }

        public override string ToString()
        {
            return $"Pattern({Tree})";
        }
    }
}
