using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Spg.ExampleRefactoring.Synthesis;
using Spg.LocationRefactor.Operator;
using Spg.LocationRefactor.TextRegion;

namespace Spg.LocationRefactor.Program
{
    /// <summary>
    /// Location program
    /// </summary>
    public class Prog
    {
        /// <summary>
        /// IOperator
        /// </summary>
        public IOperator Ioperator {get; set;}

        /// <summary>
        /// Execute the operator
        /// </summary>
        /// <param name="input">Source code</param>
        /// <returns>Transformation</returns>
        public ListNode Execute(string input)
        {
            return Ioperator.Execute(input);
        }

        /// <summary>
        /// Execute prog
        /// </summary>
        /// <param name="input">Input data</param>
        /// <returns>ListNode result of program execution</returns>
        public ListNode Execute(SyntaxNode input)
        {
            return Ioperator.Execute(input);
        }

        /// <summary>
        /// Retrieve region
        /// </summary>
        /// <returns>Region result</returns>
        public List<TRegion> RetrieveString()
        {
            return Ioperator.RetrieveRegion();
        }

        /// <summary>
        /// Retrieve result for data
        /// </summary>
        /// <param name="input">Syntax node input</param>
        /// <param name="sourceCode">Source code</param>
        /// <returns>Retrieve regions from data</returns>
        internal List<TRegion> RetrieveString(SyntaxNode input, string sourceCode)
        {
            return Ioperator.RetrieveRegion(input, sourceCode);
        }

        /// <summary>
        /// String representation
        /// </summary>
        /// <returns>String representation of this object</returns>
        public override string ToString()
        {
            return Ioperator.ToString();
        }
    }
}




