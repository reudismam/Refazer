using System;
using System.Collections.Generic;
using System.Linq;
using DiGraph;
using Spg.ExampleRefactoring.Digraph;
using Spg.ExampleRefactoring.Expression;
using Spg.ExampleRefactoring.Position;
using Spg.ExampleRefactoring.Synthesis;

namespace Spg.ExampleRefactoring.Intersect
{
    internal abstract class SubStrIntersectStrategyBase : IIntersectStrategy
    {
        /// <summary>
        /// Fist element represents the example, second element represents the position 
        /// in the example
        /// </summary>
        public Dictionary<Tuple<Dag, Tuple<Vertex, Vertex>>, Tuple<HashSet<IPosition>, HashSet<IPosition>>> PositionMap = new Dictionary<Tuple<Dag, Tuple<Vertex, Vertex>>, Tuple<HashSet<IPosition>, HashSet<IPosition>>>();

        /// <summary>
        /// Get substr expressions
        /// </summary>
        /// <param name="dag1">Fisrt dag</param>
        /// <param name="dag2">Second dag</param>
        /// <param name="tuple1">First tuple</param>
        /// <param name="tuple2">Second tuple</param>
        /// <returns>Substr expressions</returns>
        public List<IExpression> GetExpressions(Dag dag1, Dag dag2, Tuple<Vertex, Vertex> tuple1, Tuple<Vertex, Vertex> tuple2)
        {
            List<IExpression> expressions = new List<IExpression>();
            Dictionary<ExpressionKind, List<IExpression>> expressions1 = dag1.Mapping[tuple1];
            Dictionary<ExpressionKind, List<IExpression>> expressions2 = dag2.Mapping[tuple2];

            if (expressions1.ContainsKey(GetExpressionKind()) && expressions2.ContainsKey(GetExpressionKind()))
            {
                Tuple<HashSet<IPosition>, HashSet<IPosition>> hashes1 = Positions(dag1, tuple1, GetExpressionKind());
                Tuple<HashSet<IPosition>, HashSet<IPosition>> hashes2 = Positions(dag2, tuple2, GetExpressionKind());

                expressions.AddRange(SubStrIntersect(hashes1, hashes2));
            }

            return expressions;
        }

        private Tuple<HashSet<IPosition>, HashSet<IPosition>> Positions(Dag dag1, Tuple<Vertex, Vertex> tuple, ExpressionKind kind)
        {

            Tuple<Dag, Tuple<Vertex, Vertex>> tupleposition = Tuple.Create(dag1, tuple);
            Dictionary<ExpressionKind, List<IExpression>> expressions = dag1.Mapping[tuple];

            Tuple<HashSet<IPosition>, HashSet<IPosition>> positions = null;
            if (!PositionMap.TryGetValue(tupleposition, out positions))
            {
                HashSet<IPosition> positions1 = PositionsHash(expressions[kind], 1);
                HashSet<IPosition> positions2 = PositionsHash(expressions[kind], 2);
                positions = Tuple.Create(positions1, positions2);
                PositionMap.Add(tupleposition, positions);
            }

            return PositionMap[tupleposition];
        }

        /// <summary>
        /// Create a hashset of positions
        /// </summary>
        /// <param name="expressions">SubStrs expressions</param>
        /// <param name="position">Indicate if we are considering first position or second position</param>
        /// <returns>A hashset of positions</returns>
        private HashSet<IPosition> PositionsHash(List<IExpression> expressions, int position)
        {
            HashSet<IPosition> positions = new HashSet<IPosition>();
            foreach (IExpression expression in expressions)
            {
                SubStr sbstr = expression as SubStr;
                if (position == 1)
                {
                    if (sbstr != null && !positions.Contains(sbstr.p1))
                    {
                        positions.Add(sbstr.p1);
                    }
                }
                else
                {
                    if (sbstr != null && !positions.Contains(sbstr.p2))
                    {
                        positions.Add(sbstr.p2);
                    }
                }

            }
            return positions;
        }

        /// <summary>
        /// Sub nodes intersection
        /// </summary>
        /// <param name="hashes1">First hashset</param>
        /// <param name="hashes2">Second hashset</param>
        /// <returns>Intersection</returns>
        private List<IExpression> SubStrIntersect(Tuple<HashSet<IPosition>, HashSet<IPosition>> hashes1, Tuple<HashSet<IPosition>, HashSet<IPosition>> hashes2)
        {
            List<IExpression> intersection = new List<IExpression>();
            IEnumerable<IPosition> hs1 = hashes1.Item1.Intersect(hashes2.Item1);
            IEnumerable<IPosition> hs2 = hashes1.Item2.Intersect(hashes2.Item2);

            List<Tuple<IPosition, IPosition>> combinations = ASTProgram.ConstructCombinations(hs1, hs2);
            foreach (Tuple<IPosition, IPosition> positions in combinations)
            {
                IExpression expression = GetExpression(positions.Item1, positions.Item2);
                intersection.Add(expression);
            }

            return intersection;
        }

        public abstract ExpressionKind GetExpressionKind();

        public abstract IExpression GetExpression(IPosition p1, IPosition p2);

    }
}






