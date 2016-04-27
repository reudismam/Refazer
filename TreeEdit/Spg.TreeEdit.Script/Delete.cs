using Spg.TreeEdit.Node;

namespace TreeEdit.Spg.TreeEdit.Script
{
    public class Delete<T>: EditOperation<T>
    {
        public Delete(ITreeNode<T> deletedNode) : base(deletedNode, null, -1)
        {
        }

        public override string ToString()
        {
            return "Delete(" + T1Node + ")";
        }
    }
}
