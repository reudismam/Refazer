using System;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Spg.ExampleRefactoring.Comparator
{
    /// <summary>
    /// Compare substring of a node
    /// </summary>
    public class SubStrNodeComparer: ComparerBase
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
            firstStr = new string(firstStr.Where(c => !char.IsPunctuation(c)).ToArray());
            string secondStr = second.ToString();
            secondStr = new string(secondStr.Where(c => !char.IsPunctuation(c)).ToArray());

            string pattern = System.Text.RegularExpressions.Regex.Escape(firstStr);
            bool isEqual = secondStr.Contains(firstStr);//System.Text.RegularExpressions.Regex.IsMatch(secondStr, pattern);

            return isEqual;
        }
    }
}
