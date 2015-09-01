using System;
using Spg.ExampleRefactoring.AST;
using Spg.ExampleRefactoring.Position;
using Spg.ExampleRefactoring.Synthesis;

namespace Spg.ExampleRefactoring.Expression
{
    /// <summary>
    /// SubNode atomic expression
    /// </summary>
    public class SubStr : IExpression
    {
        /// <summary>
        /// Position one on the expression.
        /// </summary>
        public IPosition P1 { get; set; }

        /// <summary>
        /// Position two on the expression.
        /// </summary>
        public IPosition P2 { get; set; }


        /// <summary>
        /// Construct a SubStr expression.
        /// </summary>
        /// <param name="p1">Position one on the string.</param>
        /// <param name="p2">Position two on the string.</param>
        public SubStr(IPosition p1, IPosition p2)
        {
            this.P1 = p1;
            this.P2 = p2;
        }

        /// <summary>
        /// Verify if this expression is present on the string s. See super class documentation.
        /// </summary>
        /// <param name="example">String to be verified.</param>
        /// <returns>True is this expression is present on the string s. False otherwise.</returns>
        public bool IsPresentOn(Tuple<ListNode, ListNode> example)
        {
            int position1 = P1.GetPositionIndex(example.Item1);

            int position2 = P2.GetPositionIndex(example.Item1);

            if (AreValidPositions(position1, position2, example.Item1))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Verify is positions are valid
        /// </summary>
        /// <param name="position1"></param>
        /// <param name="position2"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool AreValidPositions(int position1, int position2, ListNode input)
        {
            if ((Math.Abs(position1) <= input.Length()) && (Math.Abs(position2) <= input.Length()) && (position2 - position1) >= 0 && position1 >= 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Retrieve a substring using this expression of string s.
        /// </summary>
        /// <param name="input">String in which this expression look at.</param>
        /// <returns>A substring of s that match this expression.</returns>
        public virtual ListNode RetrieveSubNodes(ListNode input)
        {
            int position1 = P1.GetPositionIndex(input);

            int position2 = P2.GetPositionIndex(input);

            ListNode nodes = ASTManager.SubNotes(input, position1, (position2 - position1));

            return nodes;
        }

        /// <summary>
        /// Size
        /// </summary>
        /// <returns>Size</returns>
        [Obsolete("No used anymore")]
        public int Size() {
            int size = P1.Size() * P2.Size();
            return size;
        }

        /// <summary>
        /// To String
        /// </summary>
        /// <returns>String representation of this expression</returns>
        public override string ToString()
        {
            return "SubStr(vi, "+ P1 +", "+ P2 +")";
        }

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (!(obj is SubStr))
            {
                return false;
            }
            SubStr another = obj as SubStr;
            bool result = another.P1.Equals(this.P1) && another.P2.Equals(this.P2);
            return result;
        }

        /// <summary>
        /// HashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}



