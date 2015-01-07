using Spg.ExampleRefactoring.Synthesis;
using Spg.LocationRefactor.TextRegion;
using System;
using System.Collections.Generic;

namespace Spg.LocationRefactor.Operator
{
    public interface IOperator
    {
        /// <summary>
        /// Execute the operator
        /// </summary>
        /// <returns>Execution result</returns>
        ListNode Execute(String input);

        /// <summary>
        /// Retrieve region
        /// </summary>
        /// <param name="input">Source code</param>
        /// <returns>Regions</returns>
        List<TRegion> RetrieveRegion(String input);
    }
}
