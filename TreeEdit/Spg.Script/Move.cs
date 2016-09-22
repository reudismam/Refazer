using ProseSample.Substrings;

namespace TreeEdit.Spg.Script
{
    public class Move<T> : EditOperation<T>
    {
        private ITreeNode<T> _previousParent;

        public ITreeNode<T> PreviousParent 
        {
            get{ return _previousParent; }
            set { _previousParent = value; }
        }

        /// <summary>
        /// Create a move operation
        /// </summary>
        /// <param name="movedNode">Moved node</param>
        /// <param name="parent">Parent where the node will go.</param>
        /// <param name="k">Position of this node in the parent</param>
        public Move(ITreeNode<T> movedNode, ITreeNode<T> parent, int k) : base(movedNode, parent, k){}

        /// <summary>
        /// String represent on this object
        /// </summary>
        /// <returns>Strring representation</returns>
        public override string ToString()
        {
            return "Move(" + T1Node.Label + " to " + Parent.Label + ", " + K + ")";
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Move<T>)) return false;

            return Equals(obj as Move<T>);
        }

        public bool Equals(Move<T> other)
        {
            bool isParentLabel = false;

            if (Parent != null && other.Parent != null)
            {
                isParentLabel = other.Parent.IsLabel(Parent.Label);
            }
            else if (Parent == null && other.Parent == null)
            {
                isParentLabel = true;
            }

            return K == other.K && other.T1Node.IsLabel(T1Node.Label) && isParentLabel;
        }
    }

}
