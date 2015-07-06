using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Spg.ExampleRefactoring.AST;

namespace Spg.ExampleRefactoring.Comparator
{
    /// <summary>
    /// Compare substring of a node
    /// </summary>
    public class ArrayInitializerElementTokenComparer : ComparerBase
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

            bool isEqual = ASTManager.Parent(second).IsKind(SyntaxKind.ArrayInitializerExpression) &&
                  !(second.IsKind(SyntaxKind.CommaToken) || second.IsKind(SyntaxKind.OpenBraceToken) ||
                   second.IsKind(SyntaxKind.CloseBraceToken));
            return isEqual;
        }
    }
}

