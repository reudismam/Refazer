using System;
using System.Collections.Generic;
using System.Linq;
using DiGraph;
using Spg.ExampleRefactoring.Digraph;
using Spg.ExampleRefactoring.Expression;
using Spg.ExampleRefactoring.Position;
using Spg.ExampleRefactoring.Synthesis;

namespace ExampleRefactoring.Spg.ExampleRefactoring.Synthesis
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
            Dag composition;

            if (dags.Count == 0)
            {
                throw new Exception("Dag list cannot be empty");
            }
            composition = dags[0];
            for (int i = 1; i < dags.Count; i++)
            {
                Dag dag = dags[i];
                composition = Intersect(composition, dag);
            }
            return composition;
        }

        private Dag Intersect(Dag dag1, Dag dag2)
        {
            Dag composition = null;
            DirectedGraph graph = new DirectedGraph();
            Dictionary<Tuple<Vertex, Vertex>, List<IExpression>> W = new Dictionary<Tuple<Vertex, Vertex>, List<IExpression>>();

            Dictionary<string, Vertex> vertexes = new Dictionary<string, Vertex>();
            foreach (Tuple<Vertex, Vertex> edge1 in dag1.mapping.Keys)
            {
                foreach (Tuple<Vertex, Vertex> edge2 in dag2.mapping.Keys)
                {
                    List<IExpression> intersection = Intersect(dag1, dag2, edge1, edge2);
                    Vertex vertex1 = new Vertex(edge1.Item1.Id + " : " + edge2.Item1.Id, 0.0);
                    Vertex vertex2 = new Vertex(edge1.Item2.Id + " : " + edge2.Item2.Id, 0.0);

                    if (intersection.Count > 0)
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
            composition = new Dag(graph, vertexes[(dag1.init.Id + " : " + dag2.init.Id)], vertexes[(dag1.end.Id + " : " + dag2.end.Id).ToString()], W, vertexes);
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
        private List<IExpression> Intersect(Dag dag1, Dag dag2, Tuple<Vertex, Vertex> tuple1, Tuple<Vertex, Vertex> tuple2)
        {
            Tuple<HashSet<IPosition>, HashSet<IPosition>> hashes1 = Positions(dag1, tuple1);
            Tuple<HashSet<IPosition>, HashSet<IPosition>> hashes2 = Positions(dag2, tuple2);

            List<IExpression> expressions = new List<IExpression>();

            List<IExpression> expressions1 = dag1.mapping[tuple1];
            List<IExpression> expressions2 = dag2.mapping[tuple2];
            if (expressions1.Count > 0 && expressions2.Count > 0 && expressions1[0] is ConstruStr && expressions1[0].Equals(expressions2[0]))
            {
                expressions.Add(expressions1[0]);
            }

            expressions.AddRange(SubStrIntersect(hashes1, hashes2));
            
            return expressions;
        }

        private List<IExpression> SubStrIntersect(Tuple<HashSet<IPosition>, HashSet<IPosition>> hashes1, Tuple<HashSet<IPosition>, HashSet<IPosition>> hashes2)
        {
            List<IExpression> intersection = new List<IExpression>();
            IEnumerable<IPosition> hs1 =  hashes1.Item1.Intersect(hashes2.Item1);
            IEnumerable<IPosition> hs2 =  hashes1.Item2.Intersect(hashes2.Item2);

            List <Tuple<IPosition, IPosition >> combinations = ASTProgram.ConstructCombinations(hs1, hs2);
            foreach (Tuple<IPosition, IPosition> positions in combinations)
            {
                IExpression expression = new SubStr(positions.Item1, positions.Item2);
                intersection.Add(expression);
            }

            return intersection;
        }

        private Tuple<HashSet<IPosition>, HashSet<IPosition>> Positions(Dag dag1, Tuple<Vertex, Vertex> tuple)
        {
            
            Tuple<Dag, Tuple<Vertex, Vertex>> tupleposition = Tuple.Create(dag1, tuple);
            List<IExpression> expressions = dag1.mapping[tuple];
            Tuple<HashSet<IPosition>, HashSet<IPosition>> positions = null;
            if (!positionMap.TryGetValue(tupleposition, out positions))
            {
                HashSet<IPosition>  positions1 = PositionsHash(expressions, 1);
                HashSet<IPosition> positions2 = PositionsHash(expressions, 2);
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
