using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Spg.ExampleRefactoring.Synthesis;
using Spg.LocationRefactor.TextRegion;

namespace Spg.LocationRefactor.Operator
{
    public interface IOperator
    {
        /// <summary>
        /// Execute the operator
        /// </summary>
        /// <returns>Execution result</returns>
        ListNode Execute(string input);

        /// <summary>
        /// Execute the operator
        /// </summary>
        /// <param name="input">SyntaxNode syntaxNode</param>
        /// <returns>ListNode</returns>
        ListNode Execute(SyntaxNode input);

        /// <summary>
        /// Retrieve region on all files on program
        /// </summary>
        /// <returns>Regions</returns>
        List<TRegion> RetrieveRegion();

        /// <summary>
        /// Retrieve region of the source code
        /// </summary>
        /// <param name="input">Syntax node to be considered</param>
        /// <param name="sourceCode">Source code in witch the source code was</param>
        /// <returns></returns>
        List<TRegion> RetrieveRegion(SyntaxNode input, string sourceCode);


    }
}



