using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Spg.TreeEdit.Node;

namespace TreeEdit.Spg.TreeEdit.Script
{
    public class Move<T> : EditOperation<T>
    {
        /// <summary>
        /// Create a move operation
        /// </summary>
        /// <param name="movedNode">Moved node</param>
        /// <param name="parent">Parent where the node will go.</param>
        /// <param name="k">Position of this node in the parent</param>
        public Move(ITreeNode<T> movedNode, ITreeNode<T> parent, int k) : base(movedNode, parent, k)
        {
        }

        /// <summary>
        /// String represent on this object
        /// </summary>
        /// <returns>Strring representation</returns>
        public override string ToString()
        {
            return "Move(" + T1Node.Label + " to " + Parent.Label + ", " + K + ")";
        }

    }

}
