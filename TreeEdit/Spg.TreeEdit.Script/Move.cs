using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace TreeEdit.Spg.TreeEdit.Script
{
    public class Move : EditOperation
    {
        /// <summary>
        /// Create a move operation
        /// </summary>
        /// <param name="movedNode">Moved node</param>
        /// <param name="parent">Parent where the node will go.</param>
        /// <param name="k">Position of this node in the parent</param>
        public Move(SyntaxNodeOrToken movedNode, SyntaxNodeOrToken parent, int k) : base(movedNode, parent, k)
        {
        }

        /// <summary>
        /// String represent on this object
        /// </summary>
        /// <returns>Strring representation</returns>
        public override string ToString()
        {
            return "Move(" + T1Node.Kind() + " to " + Parent.Kind() + ", " + K + ")";
        }

    }

}
