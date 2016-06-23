using System.Linq;
using Microsoft.CodeAnalysis;
using TreeElement.Spg.Node;

namespace ProseSample.Substrings.Spg.Witness
{
    public class RightChild : Context
    {
        public override ITreeNode<SyntaxNodeOrToken> Target(ITreeNode<SyntaxNodeOrToken> sot)
        {
            var rchild = RChild(sot);
            return rchild != null ? rchild : null;
        }

        public static int NodePosition(ITreeNode<SyntaxNodeOrToken> sot)
        {
            return sot.Parent.Children.TakeWhile(node => !node.Equals(sot)).Count();
        }

        public static ITreeNode<SyntaxNodeOrToken> RChild(ITreeNode<SyntaxNodeOrToken> sot)
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
