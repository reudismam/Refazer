using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Linq;

namespace Tutor
{
    public class CSharpZssNode : ZssNode<SyntaxNodeOrToken>
    {

        public CSharpZssNode(SyntaxNodeOrToken inode)
        {
            InternalNode = inode;
            Label = inode.Kind().ToString();
        }

        public override ZssNode<SyntaxNodeOrToken> GetLeftMostDescendant()
        {
            TreeTraversal traversal = new TreeTraversal();
            var list = traversal.PostOrderTraversal(InternalNode);

            if (!list.Any()) throw new Exception("tree must have a left most descendant");

            return new CSharpZssNode(list.First());
        }

        public override bool Similar(ZssNode<SyntaxNodeOrToken> other)
        {
            bool isEqual = InternalNode.IsKind(other.InternalNode.Kind()) && InternalNode.ToString().Equals(other.InternalNode.ToString());
            return isEqual;
        }

        public override string ToString()
        {
            return Label + "- (" + InternalNode + ")";
        }

        protected bool Equals(CSharpZssNode other)
        {
            return string.Equals(Label, other.Label) && Equals(InternalNode, other.InternalNode);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((CSharpZssNode)obj);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}
