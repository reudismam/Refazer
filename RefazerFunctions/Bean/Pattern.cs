using TreeElement.Spg.Node;
using TreeElement.Token;

namespace RefazerFunctions.Bean
{
    public class Pattern
    {
        /// <summary>
        /// Defines the pattern tree
        /// </summary>
        public TreeNode<Token> Tree;

        /// <summary>
        /// Defines the path to the target node
        /// </summary>
        public string XPath { get; set; }

        /// <summary>
        /// Constructs a new pattern
        /// </summary>
        /// <param name="tree">Pattern</param>
        /// <param name="xPath">XPath</param>
        public Pattern(TreeNode<Token> tree, string xPath)
        {
            Tree = tree;
            XPath = xPath;
        }

        /// <summary>
        /// Constructs a new pattern for the non-root path
        /// </summary>
        /// <param name="tree">Pattern</param>
        public Pattern(TreeNode<Token> tree)
        {
            Tree = tree;
            XPath = ".";
        }

        /// <summary>
        /// String representation of the pattern
        /// </summary>
        public override string ToString()
        {
            return $"Pattern({Tree}, {XPath})";
        }
    }
}
