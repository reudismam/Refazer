using System.Linq;
using TreeElement.Spg.Node;

namespace ProseSample.Substrings.Spg.Witness
{
    public class RightChild : Context
    {
        //public override ITreeNode<Token> Target(ITreeNode<Token> sot)
        //{
        //    var rchild = RChild(sot);
        //    return rchild != null ? rchild : null;
        //}

        public static int NodePosition(ITreeNode<Token> sot)
        {
            return sot.Parent.Children.TakeWhile(node => !node.Equals(sot)).Count();
        }

        public static ITreeNode<Token> RChild(ITreeNode<Token> sot)
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
