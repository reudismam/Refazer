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
    }
}
