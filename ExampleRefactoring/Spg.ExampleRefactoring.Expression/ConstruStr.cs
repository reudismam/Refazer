using System;
using Spg.ExampleRefactoring.Synthesis;
using Spg.ExampleRefactoring.Comparator;

namespace Spg.ExampleRefactoring.Expression
{
    /// <summary>
    /// ConstruStr expression
    /// </summary>
    public class ConstruStr: IExpression
    {
        /// <summary>
        /// Word constructed by this expression
        /// </summary>
        public ListNode Nodes {get; set;}


        /// <summary>
        /// Construct a string with the passed word.
        /// </summary>
        /// <param name="nodes">Nodes to represent the regular expression.</param>
        public ConstruStr(ListNode nodes) {
            this.Nodes = nodes;
        }

        /// <summary>
        /// Return always true
        /// </summary>
        /// <param name="example">Example</param>
        /// <returns>True</returns>
        public bool IsPresentOn(Tuple<ListNode, ListNode> example) {
            return true;
        }

        /// <summary>
        /// Return the internal List
        /// </summary>
        /// <param name="input">Input</param>
        /// <returns>Internal list</returns>
        public virtual ListNode RetrieveSubNodes(ListNode input) {
            return Nodes;
        }

        /// <summary>
        /// Size of nodes
        /// </summary>
        /// <returns>Size of nodes</returns>
        public int Size() {
            return 1;
        }

        /// <summary>
        /// To String
        /// </summary>
        /// <returns>String representation</returns>
        public override string ToString()
        {
            return "ConstructNodes(" + Nodes + ")";
        }

        /// <summary>
        /// Equals method
        /// </summary>
        /// <param name="obj">Object to compare</param>
        /// <returns>True is obj is equals to this object</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is ConstruStr))
            {
                return false;
            }
            ConstruStr another = obj as ConstruStr;

            NodeComparer compare = new NodeComparer();
            return compare.SequenceEqual(another.Nodes, this.Nodes);
        }

        /// <summary>
        /// Hash code method
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}



