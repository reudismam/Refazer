using TreeElement.Spg.Node;

namespace TreeEdit.Spg.Script
{
    public class Delete<T>: EditOperation<T>
    {
        public Delete(ITreeNode<T> deletedNode) : base(deletedNode, deletedNode.Parent, -1)
        {
        }

        public override string ToString()
        {
            return "Delete(" + T1Node + ")";
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Delete<T>)) return false;

            return Equals(obj as Delete<T>);
        }

        public bool Equals(Delete<T> other)
        {
            return other.T1Node.IsLabel(T1Node.Label) && other.Parent.IsLabel(Parent.Label);
        }
    }
}
