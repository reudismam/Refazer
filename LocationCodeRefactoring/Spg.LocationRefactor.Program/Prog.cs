using System;
using System.Collections.Generic;
using Spg.ExampleRefactoring.Synthesis;
using LocationCodeRefactoring.Spg.LocationRefactor.Operator;
using Microsoft.CodeAnalysis;
using Spg.ExampleRefactoring.Synthesis;
using Spg.LocationRefactor.TextRegion;

namespace LocationCodeRefactoring.Spg.LocationRefactor.Program
{
    /// <summary>
    /// Location program
    /// </summary>
    public class Prog
    {
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

        public ListNode Execute(SyntaxNode input)
        {
            return Ioperator.Execute(input);
        }

        ///// <summary>
        ///// Retrieve region
        ///// </summary>
        ///// <param name="input">Source code</param>
        ///// <returns>Region result</returns>
        //public List<TRegion> RetrieveString(string input)
        //{
        //    return Ioperator.RetrieveRegion(input);
        //}

        /// <summary>
        /// Retrieve region
        /// </summary>
        /// <param name="input">Source code</param>
        /// <returns>Region result</returns>
        public List<TRegion> RetrieveString()
        {
            return Ioperator.RetrieveRegion();
        }

        internal List<TRegion> RetrieveString(SyntaxNode input, string sourceCode)
        {
            return Ioperator.RetrieveRegion(input, sourceCode);
        }

        public override string ToString()
        {
            return Ioperator.ToString();
        }
    }
}


