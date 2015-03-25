﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Spg.ExampleRefactoring.Comparator
{
    /// <summary>
    /// Compare substring of a node
    /// </summary>
    public class ArgumentComparer : ComparerBase
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

            SyntaxNodeOrToken parent = second;
            int nInners = 0;
            while (parent.CSharpKind() != SyntaxKind.Block)
            {
                if (parent.CSharpKind() == SyntaxKind.Argument)
                {
                    nInners ++;
                    //InvocationExpressionSyntax invocation = (InvocationExpressionSyntax) parent.Parent.Parent;
                    //IdentifierNameSyntax member = invocation.Expression as IdentifierNameSyntax;
                    //if (member.GetText().ToString().Equals(first))
                    //{
                    //    return false;
                    //}

                    return true;
                }
                parent = parent.Parent; // up on the tree.
            }
            //if (nInners == 1)
            //{
            //    return true; //the argument is the last argument or the tree hierarchy.
            //}
            return false;
        }
    }
}