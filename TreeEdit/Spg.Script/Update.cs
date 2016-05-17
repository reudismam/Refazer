using Spg.TreeEdit.Node;

namespace TreeEdit.Spg.TreeEdit.Script
{
    public class Update<T> : EditOperation<T>
    {
        public ITreeNode<T> To { get; internal set; }

        /// <summary>
        /// Construct a update object
        /// </summary>
        /// <param name="from">Node that will be moved</param>
        /// <param name="to">Update node</param>
        /// <param name="parent">Where the node will go</param>
        public Update(ITreeNode<T> from, ITreeNode<T> to, ITreeNode<T> parent) : base(from, parent, -1)
        {
            To = to;
        }

        /// <summary>
        /// String representation of this object
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Update(" + T1Node.Label + "- (" + T1Node + ")" + " to " + To.Label + "- (" + To + "))";
        }

        //public override bool Equals(object obj)
        //{
        //    if (!(obj is Update<T>)) return false;

        //    return Equals(obj as Update<T>);
        //}

        //public bool Equals(Update<T> other)
        //{
        //    bool isParentLabel = false;

        //    if (Parent != null && other.Parent != null)
        //    {
        //        isParentLabel = other.Parent.IsLabel(Parent.Label);
        //    }
        //    else if (Parent == null && other.Parent == null)
        //    {
        //        isParentLabel = true;
        //    }

        //    return other.T1Node.IsLabel(T1Node.Label) && other.To.IsLabel(To.Label) && isParentLabel;
        //}
    }
}
