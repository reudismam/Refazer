using TreeElement.Spg.Node;

namespace TreeEdit.Spg.Script
{
    public class InsertBefore<T>: EditOperation<T>
    {
        /// <summary>
        /// Construct a new insert object
        /// </summary>
        /// <param name="insertedNode">Node that will be inserted</param>
        /// <param name="parent">Parent of the node that will be inserted</param>
        /// <param name="k">Position where the node will be inserted</param>
        public InsertBefore(TreeNode<T> insertedNode, TreeNode<T> parent) : base(insertedNode, parent, -1)
        {
        }

        /// <summary>
        /// String representation of this object
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Insert(" + T1Node.Label + ", " + Parent.Label + ", " + K + ")";
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Insert<T>)) return false;

            return Equals(obj as Insert<T>);
        }

        public bool Equals(Insert<T> other)
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
