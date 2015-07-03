using System;
using Microsoft.CodeAnalysis;

namespace Spg.ExampleRefactoring.Comparator
{
    /// <summary>
    /// Compare nodes
    /// </summary>
    public class NodeComparer: ComparerBase
    {
        /// <summary>
        /// First and second syntax node or token nodes content are equal
        /// </summary>
        /// <param name="first">First syntax node or token</param>
        /// <param name="second">Second syntax node or token</param>
        /// <returns>True if first and second syntax node or token nodes content are equal</returns>
        public override bool Match(SyntaxNodeOrToken first, SyntaxNodeOrToken second)
        {
            if(first == null || second == null)
            {
                throw new Exception("Syntax nodes or token cannot be null");
            }
            string firstStr = first.ToString();
            string secondStr = second.ToString();
            bool isEqual = firstStr.Equals(secondStr);

            return isEqual;
        }
    }
}
