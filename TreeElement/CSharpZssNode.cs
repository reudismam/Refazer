using System;
using System.Linq;
using ProseFunctions.Substrings;
using TreeElement.Spg.Node;

namespace TreeElement
{
    public class CSharpZssNode<T> : ZssNode<TreeNode<T>>
    {

        public CSharpZssNode(TreeNode<T> inode)
        {
            InternalNode = inode;
            Label = inode.ToString();
        }

        public override ZssNode<TreeNode<T>> GetLeftMostDescendant()
        {
            var traversal = new TreeTraversal<T>();
            var list = traversal.PostOrderTraversal(InternalNode);

            if (!list.Any()) throw new Exception("tree must have a left most descendant");

            return new CSharpZssNode<T>(list.First());
        }

        public override bool Similar(ZssNode<TreeNode<T>> other)
        {
            bool isEqual = InternalNode.IsLabel(other.InternalNode.Label) && InternalNode.ToString().Equals(other.InternalNode.ToString());
            return isEqual;
        }

        public override string ToString()
        {
            return Label + "- (" + InternalNode + ")";
        }

        protected bool Equals(CSharpZssNode<T> other)
        {
            return string.Equals(Label, other.Label) && Equals(InternalNode, other.InternalNode);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((CSharpZssNode<T>)obj);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}
