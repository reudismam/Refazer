using System.Linq;
using ProseSample.Substrings;

namespace ProseSample.Substrings.Spg.Witness
{
    public class RightChild : Context
    {
        //public override TreeNode<Token> Target(TreeNode<Token> sot)
        //{
        //    var rchild = RChild(sot);
        //    return rchild != null ? rchild : null;
        //}

        public static int NodePosition(TreeNode<Token> sot)
        {
            return sot.Parent.Children.TakeWhile(node => !node.Equals(sot)).Count();
        }

        public static TreeNode<Token> RChild(TreeNode<Token> sot)
        {
            int position = NodePosition(sot);

            if (position + 1 < sot.Parent.Children.Count)
            {
                return sot.Parent.Children[position + 1];
            }

            return null;
        }

    }
}
