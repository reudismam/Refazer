using ProseFunctions.Substrings;
using TreeElement.Spg.Node;

namespace ProseFunctions.Spg.Bean
{
    public class Pattern
    {
        public TreeNode<Token> Tree;

        public string K { get; set; }

        public Pattern(TreeNode<Token> tree, string k)
        {
            Tree = tree;
            K = k;
        }

        public Pattern(TreeNode<Token> tree)
        {
            Tree = tree;
            K = ".";
        }

        public override string ToString()
        {
            return $"Pattern({Tree}, {K})";
        }
    }
}
