using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiGraph;
using Spg.ExampleRefactoring.Digraph;
using Spg.ExampleRefactoring.Expression;

namespace Spg.ExampleRefactoring.Intersect
{
    internal abstract  class ConstIntersectStrategyBase
    {
        /// <summary>
        /// Get Expressions for ConstruStr
        /// </summary>
        /// <param name="dag1">First dag</param>
        /// <param name="dag2">Second dag</param>
        /// <param name="tuple1">Firt tuple</param>
        /// <param name="tuple2">Second tuple</param>
        /// <returns>Expressions for ConstruStr</returns>
        public List<IExpression> GetExpressions(Dag dag1, Dag dag2, Tuple<Vertex, Vertex> tuple1, Tuple<Vertex, Vertex> tuple2)
        {
            List<IExpression> expressions = new List<IExpression>();
            Dictionary<ExpressionKind, List<IExpression>> expressions1 = dag1.Mapping[tuple1];
            Dictionary<ExpressionKind, List<IExpression>> expressions2 = dag2.Mapping[tuple2];
            if (expressions1.ContainsKey(GetExpressionKind()) && expressions2.ContainsKey(GetExpressionKind()))
            {
                if (expressions1[GetExpressionKind()].Count > 0 &&
                    expressions2[GetExpressionKind()].Count > 0 &&
                    expressions1[GetExpressionKind()][0].Equals(expressions2[GetExpressionKind()][0]))
                {
                    expressions.AddRange(expressions1[GetExpressionKind()]);
                }
            }

            return expressions;
        }

        /// <summary>
        /// Get the ExpressionKind for ConstIntersectStrategy
        /// </summary>
        /// <returns>the ExpressionKind for ConstStrs</returns>
        public abstract ExpressionKind GetExpressionKind();

    }
}







