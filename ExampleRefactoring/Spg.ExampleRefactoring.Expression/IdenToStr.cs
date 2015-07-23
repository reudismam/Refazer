using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Formatting;
using Spg.ExampleRefactoring.AST;
using Spg.ExampleRefactoring.Position;
using Spg.ExampleRefactoring.Synthesis;

namespace Spg.ExampleRefactoring.Expression
{
    /// <summary>
    /// SubNode atomic expression
    /// </summary>
    public class IdenToStr : SubStr
    {
        /// <summary>
        /// Construct a SubStr expression.
        /// </summary>
        /// <param name="p1">Position one on the string.</param>
        /// <param name="p2">Position two on the string.</param>
        public IdenToStr(IPosition p1, IPosition p2):base(p1, p2)
        {
        }   

        /// <summary>
        /// Retrieve a substring using this expression of string s.
        /// </summary>
        /// <param name="input">String in which this expression look at.</param>
        /// <returns>A substring of s that match this expression.</returns>
        public override ListNode RetrieveSubNodes(ListNode input)
        {
            int position1 = P1.GetPositionIndex(input);

            int position2 = P2.GetPositionIndex(input);

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
            return "IdenToStr(vi, "+ P1 +", "+ P2 +")";
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
            bool result = another.P1.Equals(this.P1) && another.P2.Equals(this.P2);
            return result;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}



