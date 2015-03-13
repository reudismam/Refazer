using System;
using ExampleRefactoring.Spg.ExampleRefactoring.AST;
using ExampleRefactoring.Spg.ExampleRefactoring.Expression;
using ExampleRefactoring.Spg.ExampleRefactoring.Position;
using ExampleRefactoring.Spg.ExampleRefactoring.Synthesis;
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
        public IPosition p1 { get; set; }

        /// <summary>
        /// Position two on the expression.
        /// </summary>
        public IPosition p2 { get; set; }


        /// <summary>
        /// Construct a SubStr expression.
        /// </summary>
        /// <param name="p1">Position one on the string.</param>
        /// <param name="p2">Position two on the string.</param>
        public SubStr(IPosition p1, IPosition p2)
        {
            this.p1 = p1;
            this.p2 = p2;
        }

        /// <summary>
        /// Verify if this expression is present on the string s. See super class documentation.
        /// </summary>
        /// <param name="example">String to be verified.</param>
        /// <returns>True is this expression is present on the string s. False otherwise.</returns>
        public Boolean IsPresentOn(Tuple<ListNode, ListNode> example)
        {
            int position1 = p1.GetPositionIndex(example.Item1);

            int position2 = p2.GetPositionIndex(example.Item1);

            Boolean presence = false;
            if (AreValidPositions(position1, position2, example.Item1))
            {
                return true;
            }

            return presence;
        }

        /// <summary>
        /// Verify is positions are valid
        /// </summary>
        /// <param name="position1"></param>
        /// <param name="position2"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Boolean AreValidPositions(int position1, int position2, ListNode input)
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
        public ListNode RetrieveSubNodes(ListNode input)
        {
            int position1 = p1.GetPositionIndex(input);

            int position2 = p2.GetPositionIndex(input);

            ListNode nodes = ASTManager.SubNotes(input, position1, (position2 - position1));

            return nodes;
        }

        /// <summary>
        /// Size
        /// </summary>
        /// <returns>Size</returns>
        [Obsolete("No used anymore")]
        public int Size() {
            int size = p1.Size() * p2.Size();
            return size;
        }

        /// <summary>
        /// To String
        /// </summary>
        /// <returns>String representation of this expression</returns>
        public override string ToString()
        {
            return "SubStr(vi, "+ p1 +", "+ p2 +")";
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
            bool result = another.p1.Equals(this.p1) && another.p2.Equals(this.p2);
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
