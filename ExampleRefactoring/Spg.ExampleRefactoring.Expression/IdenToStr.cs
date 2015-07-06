using System;
using Spg.ExampleRefactoring.AST;
using Spg.ExampleRefactoring.Expression;
using Spg.ExampleRefactoring.Position;
using Spg.ExampleRefactoring.Synthesis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Formatting;

namespace Spg.ExampleRefactoring.Expression
{
    /// <summary>
    /// SubNode atomic expression
    /// </summary>
    public class IdenToStr : SubStr
    {
        ///// <summary>
        ///// Position one on the expression.
        ///// </summary>
        //public IPosition p1 { get; set; }

        ///// <summary>
        ///// Position two on the expression.
        ///// </summary>
        //public IPosition p2 { get; set; }


        /// <summary>
        /// Construct a SubStr expression.
        /// </summary>
        /// <param name="p1">Position one on the string.</param>
        /// <param name="p2">Position two on the string.</param>
        public IdenToStr(IPosition p1, IPosition p2):base(p1, p2)
        {
            //this.p1 = p1;
            //this.p2 = p2;
        }

        ///// <summary>
        ///// Verify if this expression is present on the string s. See super class documentation.
        ///// </summary>
        ///// <param name="example">String to be verified.</param>
        ///// <returns>True is this expression is present on the string s. False otherwise.</returns>
        //public bool IsPresentOn(Tuple<ListNode, ListNode> example)
        //{
        //    int position1 = p1.GetPositionIndex(example.Item1);

        //    int position2 = p2.GetPositionIndex(example.Item1);

        //    if (AreValidPositions(position1, position2, example.Item1))
        //    {
        //        return true;
        //    }

        //    return false;
        //}

        ///// <summary>
        ///// Verify is positions are valid
        ///// </summary>
        ///// <param name="position1"></param>
        ///// <param name="position2"></param>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //public static bool AreValidPositions(int position1, int position2, ListNode input)
        //{
        //    if ((Math.Abs(position1) <= input.Length()) && (Math.Abs(position2) <= input.Length()) && (position2 - position1) >= 0 && position1 >= 0)
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        /// <summary>
        /// Retrieve a substring using this expression of string s.
        /// </summary>
        /// <param name="input">String in which this expression look at.</param>
        /// <returns>A substring of s that match this expression.</returns>
        public override ListNode RetrieveSubNodes(ListNode input)
        {
            int position1 = p1.GetPositionIndex(input);

            int position2 = p2.GetPositionIndex(input);

            ListNode nodes = ASTManager.SubNotes(input, position1, (position2 - position1));

            ListNode replace = new ListNode();
            if (nodes.Length() == 1 && nodes.List[0].IsKind(SyntaxKind.IdentifierToken))
            {
                SyntaxNodeOrToken identifier = nodes.List[0];
                var newLiteral = SyntaxFactory.ParseExpression("\"" + identifier + "\"")
                    .WithLeadingTrivia(identifier.GetLeadingTrivia())
                    .WithTrailingTrivia(identifier.GetTrailingTrivia())
                    .WithAdditionalAnnotations(Formatter.Annotation);
                replace.List.Add(newLiteral);
            }
            else
            {
                throw new IndexOutOfRangeException("Cannot convert nodes to literal stirng.");
            }

            return replace;
        }

        /// <summary>
        /// To String
        /// </summary>
        /// <returns>String representation of this expression</returns>
        public override string ToString()
        {
            return "IdenToStr(vi, "+ p1 +", "+ p2 +")";
        }

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (!(obj is IdenToStr))
            {
                return false;
            }
            SubStr another = obj as IdenToStr;
            bool result = another.p1.Equals(this.p1) && another.p2.Equals(this.p2);
            return result;
        }

        ///// <summary>
        ///// HashCode
        ///// </summary>
        ///// <returns></returns>
        //public override int GetHashCode()
        //{
        //    return ToString().GetHashCode();
        //}
    }
}



