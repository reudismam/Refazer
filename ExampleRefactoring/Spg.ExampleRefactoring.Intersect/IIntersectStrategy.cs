using System;
using System.Collections.Generic;
using DiGraph;
using Spg.ExampleRefactoring.Digraph;
using Spg.ExampleRefactoring.Expression;

namespace Spg.ExampleRefactoring.Intersect
{
    internal interface IIntersectStrategy
    {
        /// <summary>
        /// Get expressions
        /// </summary>
        /// <param name="dag1">First dag</param>
        /// <param name="dag2">Second dag</param>
        /// <param name="tuple1">First tuple</param>
        /// <param name="tuple2">Second tuple</param>
        /// <returns>Expressions</returns>
        List<IExpression> GetExpressions(Dag dag1, Dag dag2, Tuple<Vertex, Vertex> tuple1, Tuple<Vertex, Vertex> tuple2);

        /// <summary>
        /// Return expression kind for this specifig strategy
        /// </summary>
        /// <returns>expression kind for this specifig strategy</returns>
        ExpressionKind GetExpressionKind();
    }
}






