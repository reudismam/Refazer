using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Spg.ExampleRefactoring.Comparator
{
    /// <summary>
    /// Compare nodes
    /// </summary>
    public class IdenToStrComparer : ComparerBase
    {
        /// <summary>
        /// First and second syntax node or token nodes content are equal
        /// </summary>
        /// <param name="first">First syntax node or token</param>
        /// <param name="second">Second syntax node or token</param>
        /// <returns>True if first and second syntax node or token nodes content are equal</returns>
        public override bool Match(SyntaxNodeOrToken first, SyntaxNodeOrToken second)
        {
            if(first == null) throw new ArgumentNullException("first");
            if (second == null) throw new ArgumentNullException("second");

            if (!first.IsKind(SyntaxKind.IdentifierToken)) return false;

            string firstStr = "\"" + first + "\"";
            string secondStr = second.ToString();
            bool isEqual = firstStr.Equals(secondStr);

            return isEqual;
        }
    }
}

