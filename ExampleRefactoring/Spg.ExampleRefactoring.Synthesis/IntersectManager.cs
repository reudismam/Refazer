using System;
using System.Collections.Generic;
using System.Linq;
using DiGraph;
using Spg.ExampleRefactoring.Digraph;
using Spg.ExampleRefactoring.Expression;
using Spg.ExampleRefactoring.Position;
using Spg.ExampleRefactoring.Expression;

namespace Spg.ExampleRefactoring.Synthesis
{
    /// <summary>
    /// Intersect manager
    /// </summary>
    public class IntersectManager
    {
        /// <summary>
        /// Fist element represents the example, second element represents the position 
        /// in the example
        /// </summary>
        public Dictionary<Tuple<Dag, Tuple<Vertex, Vertex>>, Tuple<HashSet<IPosition>, HashSet<IPosition>>> positionMap  = new Dictionary<Tuple<Dag, Tuple<Vertex, Vertex>>, Tuple<HashSet<IPosition>, HashSet<IPosition>>>();
        /// <summary>
        /// Intersection among direct acyclic graphs
        /// </summary>
        /// <param name="dags">Dag list</param>
        /// <returns>Intersection among dags</returns>
        public Dag Intersect(List<Dag> dags) {
            if (dags.Count == 0)
            {
                throw new Exception("Dag list cannot be empty");
            }
            var composition = dags[0];
            for (int i = 1; i < dags.Count; i++)
            {
                Dag dag = dags[i];
                try
                {
                    composition = Intersect(composition, dag);
                }
                catch (Exception e)
                {
                    return null;
                }
            }
            return composition;
        }

        /// <summary>
        /// Intersect dags
        /// </summary>
        /// <param name="dag1">Fist dag</param>
        /// <param name="dag2">Second dag</param>
        /// <returns>Dag intersection</returns>
        private Dag Intersect(Dag dag1, Dag dag2)
        {
            if (dag1 == null) throw new ArgumentNullException("dag1");
            if (dag2 == null) throw new ArgumentNullException("dag2");

            Dag composition;
            DirectedGraph graph = new DirectedGraph();
            Dictionary<Tuple<Vertex, Vertex>, Dictionary<string, List<IExpression>>> W = new Dictionary<Tuple<Vertex, Vertex>, Dictionary<string, List<IExpression>>>();

            Dictionary<string, Vertex> vertexes = new Dictionary<string, Vertex>();
            foreach (Tuple<Vertex, Vertex> edge1 in dag1.Mapping.Keys)
            {
                foreach (Tuple<Vertex, Vertex> edge2 in dag2.Mapping.Keys)
                {
                    Dictionary<string, List<IExpression>> intersection = Intersect(dag1, dag2, edge1, edge2);
                    Vertex vertex1 = new Vertex(edge1.Item1.Id + " : " + edge2.Item1.Id, 0.0);
                    Vertex vertex2 = new Vertex(edge1.Item2.Id + " : " + edge2.Item2.Id, 0.0);

                    bool containElement = false;
                    foreach (KeyValuePair<string, List<IExpression>> item in intersection)
                    {
                        if (item.Value.Any())
                        {
                            containElement = true;
                            break;
                        }
                    }

                    if (containElement)
                    {
                        if (!graph.HasVertex(vertex1.Id))
                        {
                            vertexes.Add(vertex1.Id, vertex1);
                            graph.AddVertex(vertex1);
                        }

                        if (!graph.HasVertex(vertex2.Id))
                        {
                            vertexes.Add(vertex2.Id, vertex2);
                            graph.AddVertex(vertex2);
                        }

                        Tuple<Vertex, Vertex> vertex = Tuple.Create(vertex1, vertex2);
                        graph.AddEdge(vertex1.Id, vertex2.Id);
                        W[vertex] = intersection;
                    }
               }
            }

            try
            {
                composition = new Dag(graph, vertexes[(dag1.Init.Id + " : " + dag2.Init.Id)], vertexes[(dag1.End.Id + " : " + dag2.End.Id).ToString()], W, vertexes);
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
            return composition;
        }

        /// <summary>
        /// Intersection between two expression Dags
        /// </summary>
        /// <param name="dag1">First directed graph</param>
        /// <param name="dag2">Second directed graph</param>
        /// <param name="tuple1">First edge</param>
        /// <param name="tuple2">Second edge</param>
        /// <returns>Intersection</returns>
        private Dictionary<string, List<IExpression>> Intersect(Dag dag1, Dag dag2, Tuple<Vertex, Vertex> tuple1, Tuple<Vertex, Vertex> tuple2)
        {
            Dictionary<string, List<IExpression>> expressions = new Dictionary<string, List<IExpression>>();
            Dictionary<string, List<IExpression>> expressions1 = dag1.Mapping[tuple1];
            Dictionary<string, List<IExpression>> expressions2 = dag2.Mapping[tuple2];
            if (expressions1.ContainsKey(ExpressionKind.Consttrustr) && expressions2.ContainsKey(ExpressionKind.Consttrustr))
            {
                if (expressions1[ExpressionKind.Consttrustr].Count > 0 &&
                    expressions2[ExpressionKind.Consttrustr].Count > 0 &&
                    expressions1[ExpressionKind.Consttrustr][0].Equals(expressions2[ExpressionKind.Consttrustr][0]))
                {
                    expressions.Add(ExpressionKind.Consttrustr, expressions1[ExpressionKind.Consttrustr]);
                }
            }

            if (expressions1.ContainsKey(ExpressionKind.SubStr) && expressions2.ContainsKey(ExpressionKind.SubStr))
            {
                Tuple<HashSet<IPosition>, HashSet<IPosition>> hashes1 = Positions(dag1, tuple1, ExpressionKind.SubStr);
                Tuple<HashSet<IPosition>, HashSet<IPosition>> hashes2 = Positions(dag2, tuple2, ExpressionKind.SubStr);   

                expressions.Add(ExpressionKind.SubStr, SubStrIntersect(hashes1, hashes2, false));
            }

            if (expressions1.ContainsKey(ExpressionKind.Identostr) && expressions2.ContainsKey(ExpressionKind.Identostr))
            {
                Tuple<HashSet<IPosition>, HashSet<IPosition>> hashes1 = Positions(dag1, tuple1, ExpressionKind.Identostr);
                Tuple<HashSet<IPosition>, HashSet<IPosition>> hashes2 = Positions(dag2, tuple2, ExpressionKind.Identostr);
                expressions.Add(ExpressionKind.Identostr, SubStrIntersect(hashes1, hashes2, true));
            }

            return expressions;
        } 

        //private bool ContainsIdenToToken(List<IExpression> expressions)
        //{
        //    foreach (var expression in expressions)
        //    {
        //        if (expression is IdenToStr) return true;
        //    }
        //    return false;
        //}

        private List<IExpression> SubStrIntersect(Tuple<HashSet<IPosition>, HashSet<IPosition>> hashes1, Tuple<HashSet<IPosition>, HashSet<IPosition>> hashes2, bool addIdenToToken)
        {
            List<IExpression> intersection = new List<IExpression>();
            IEnumerable<IPosition> hs1 =  hashes1.Item1.Intersect(hashes2.Item1);
            IEnumerable<IPosition> hs2 =  hashes1.Item2.Intersect(hashes2.Item2);

            List <Tuple<IPosition, IPosition >> combinations = ASTProgram.ConstructCombinations(hs1, hs2);
            foreach (Tuple<IPosition, IPosition> positions in combinations)
            {
                if (addIdenToToken)
                {
                    IExpression expression1 = new IdenToStr(positions.Item1, positions.Item2);
                    intersection.Add(expression1);
                }
                else
                {
                    IExpression expression = new SubStr(positions.Item1, positions.Item2);
                    intersection.Add(expression);
                }
            }

            return intersection;
        }

        private Tuple<HashSet<IPosition>, HashSet<IPosition>> Positions(Dag dag1, Tuple<Vertex, Vertex> tuple, string kind)
        {
            
            Tuple<Dag, Tuple<Vertex, Vertex>> tupleposition = Tuple.Create(dag1, tuple);
            Dictionary<string, List<IExpression>> expressions = dag1.Mapping[tuple];

            Tuple<HashSet<IPosition>, HashSet<IPosition>> positions = null;
            if (!positionMap.TryGetValue(tupleposition, out positions))
            {
                HashSet<IPosition>  positions1 = PositionsHash(expressions[kind], 1);
                HashSet<IPosition> positions2 = PositionsHash(expressions[kind], 2);
                positions = Tuple.Create(positions1, positions2);
                positionMap.Add(tupleposition, positions);
            }

            return positionMap[tupleposition];
        }

        private HashSet<IPosition> PositionsHash(List<IExpression> expressions, int position)
        {
            HashSet<IPosition> positions = new HashSet<IPosition>();
            foreach (IExpression expression in expressions)
            {
                if (expression is SubStr)
                {
                    SubStr sbstr = expression as SubStr;
                    if (position == 1)
                    {
                        if (!positions.Contains(sbstr.p1))
                        {
                            positions.Add(sbstr.p1);
                        }
                    }else
                    {
                        if (!positions.Contains(sbstr.p2))
                        {
                            positions.Add(sbstr.p2);
                        }
                    }
                }
            }
            return positions;
        }

    }
}


