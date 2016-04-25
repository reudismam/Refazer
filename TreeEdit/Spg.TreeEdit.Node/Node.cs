using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Spg.TreeEdit.Node
{
    /// <summary>
    /// Class node
    /// </summary>
    public class Node
    {
        /// <summary>
        /// Syntax node or token reference
        /// </summary>
        public SyntaxNodeOrToken Snt;

        /// <summary>
        /// Start position
        /// </summary>
        public int Start { get; set; }


        /// <summary>
        /// End position
        /// </summary>
        public int End { get; set; }

        /// <summary>
        /// SyntaxKind
        /// </summary>
        public SyntaxKind SyntaxKind { get; set; }

        /// <summary>
        /// Create a new Node instance
        /// </summary>
        /// <param name="start">Start position</param>
        /// <param name="end">End position</param>
        /// <param name="snt">Syntax node of token reference</param>
        public Node(int start, int end, SyntaxNodeOrToken snt)
        {
            Start = start;
            End = end;
            SyntaxKind = snt.Kind();
            Snt = snt;
        }
        /// <summary>
        /// Determine if obj is equal to this.
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>True is obj is equal to this.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Node))
            {
                return false;
            }
            Node other = (Node)obj;
            return other.Start == Start && other.End == End && other.SyntaxKind == SyntaxKind;
        }

        /// <summary>
        /// String representation of this node.
        /// </summary>
        /// <returns>String representation of this node.</returns>
        public override string ToString()
        {
            return Snt.ToFullString();
        }

        /// <summary>
        /// Hash code for this node.
        /// </summary>
        /// <returns>Hash code for this node.</returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}