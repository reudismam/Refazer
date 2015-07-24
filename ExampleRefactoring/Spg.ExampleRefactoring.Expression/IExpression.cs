using System;
using Spg.ExampleRefactoring.Synthesis;

namespace Spg.ExampleRefactoring.Expression
{
    /// <summary>
    /// IExpression
    /// </summary>
    public interface IExpression
    {
        /// <summary>
        /// Expression is present on the input s
        /// </summary>
        /// <param name="example">Examples</param>
        /// <returns>true if expression is present on the nodes</returns>
        bool IsPresentOn(Tuple<ListNode, ListNode> example);

        /// <summary>
        /// Sub nodes of expression on input s
        /// </summary>
        /// <returns>Sub nodes that match expression input s</returns>
        ListNode RetrieveSubNodes(ListNode input);
    }
}



