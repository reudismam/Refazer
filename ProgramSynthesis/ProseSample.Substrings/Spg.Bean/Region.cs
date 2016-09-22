using Microsoft.CodeAnalysis;
using ProseSample.Substrings;

namespace ProseSample.Substrings
{
    public class Region
    {
        public TreeNode<SyntaxNodeOrToken> Tree { get; set; }

        public Region(TreeNode<SyntaxNodeOrToken> tree)
        {
            Tree = tree;
        }

        public Region() { }

        public override bool Equals(object obj)
        {
            return Tree.Equals(obj);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            return Tree.ToString();
        }
    }
}
