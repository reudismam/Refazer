using Spg.ExampleRefactoring.Synthesis;
using Spg.LocationRefactor.Operator;
using Spg.LocationRefactor.TextRegion;
using System;
using System.Collections.Generic;

namespace Spg.LocationRefactor.Program
{
    public class Prog
    {
        public IOperator ioperator {get; set;}

        /// <summary>
        /// Execute the operator
        /// </summary>
        /// <param name="input">Source code</param>
        /// <returns>Transformation</returns>
        public ListNode Execute(String input)
        {
            return ioperator.Execute(input);
        }

        /// <summary>
        /// Retrieve region
        /// </summary>
        /// <param name="input">Source code</param>
        /// <returns>Region result</returns>
        public List<TRegion> RetrieveString(String input) {
            return ioperator.RetrieveRegion(input);
        }

        public override string ToString()
        {
            return ioperator.ToString();
        }
    }
}
