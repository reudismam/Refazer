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
    internal class ConstIntersectStrategy: IIntersectStrategy
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
            if (expressions1.ContainsKey(ExpressionKind.Consttrustr) && expressions2.ContainsKey(ExpressionKind.Consttrustr))
            {
                if (expressions1[ExpressionKind.Consttrustr].Count > 0 &&
                    expressions2[ExpressionKind.Consttrustr].Count > 0 &&
                    expressions1[ExpressionKind.Consttrustr][0].Equals(expressions2[ExpressionKind.Consttrustr][0]))
                {
                    expressions.AddRange(expressions1[ExpressionKind.Consttrustr]);
                }
            }

            return expressions;
        }

        /// <summary>
        /// Get the ExpressionKind for ConstIntersectStrategy
        /// </summary>
        /// <returns>the ExpressionKind for ConstIntersectStrategy</returns>
        public ExpressionKind GetExpressionKind()
        {
            return ExpressionKind.Consttrustr;
        }
    }
}






