using System;
using System.Collections.Generic;
using System.Linq;
using DiGraph;
using Spg.ExampleRefactoring.Digraph;
using Spg.ExampleRefactoring.Expression;

namespace Spg.ExampleRefactoring.Intersect
{
    /// <summary>
    /// Intersect manager
    /// </summary>
    public class IntersectManager
    {
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
            if (dags.Count == 1) return dags.First();

            var composition = dags[0];
            for (int i = 1; i < dags.Count; i++)
            {
                Dag dag = dags[i];
                try
                {
                    composition = Intersect(composition, dag);
                }
                catch (Exception)
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

            Dictionary<string, Vertex> vertexes = new Dictionary<string, Vertex>();
            DirectedGraph graph = new DirectedGraph();
            Dag composition;
            Dictionary<Tuple<Vertex, Vertex>, Dictionary<ExpressionKind, List<IExpression>>> W = new Dictionary<Tuple<Vertex, Vertex>, Dictionary<ExpressionKind, List<IExpression>>>();
            
            //if (dag1.Init.Equals(dag1.End) && dag2.Init.Equals(dag2.End)) //Empty Dags.
            //{
            //    Vertex vertex1 = new Vertex(dag1.Init + " : " + dag1.End, 0.0);
            //    vertexes.Add(vertex1.Id, vertex1);
            //    graph.AddVertex(vertex1);
            //    Tuple<Vertex, Vertex> vertex = Tuple.Create(vertex1, vertex1);
            //    W[vertex] = dag1.Mapping[ExpressionKind.Consttrustr]; 
            //    return new Dag(graph, vertex1, vertex1, dag1.Mapping, vertexes);
            //}

            foreach (Tuple<Vertex, Vertex> edge1 in dag1.Mapping.Keys)
            {
                foreach (Tuple<Vertex, Vertex> edge2 in dag2.Mapping.Keys)
                {
                    Dictionary<ExpressionKind, List<IExpression>> intersection = Intersect(dag1, dag2, edge1, edge2);
                    Vertex vertex1 = new Vertex(edge1.Item1.Id + " : " + edge2.Item1.Id, 0.0);
                    Vertex vertex2 = new Vertex(edge1.Item2.Id + " : " + edge2.Item2.Id, 0.0);

                    bool containElement = false;
                    foreach (KeyValuePair<ExpressionKind, List<IExpression>> item in intersection)
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
        private Dictionary<ExpressionKind, List<IExpression>> Intersect(Dag dag1, Dag dag2, Tuple<Vertex, Vertex> tuple1, Tuple<Vertex, Vertex> tuple2)
        {
            Dictionary<ExpressionKind, List<IExpression>> expressions = new Dictionary<ExpressionKind, List<IExpression>>();

            List<IIntersectStrategy> strategies = GetStrategies();
            foreach (IIntersectStrategy strategy in strategies)
            {
                List<IExpression> exp = strategy.GetExpressions(dag1, dag2, tuple1, tuple2);
                if (exp.Any())
                {
                    expressions.Add(strategy.GetExpressionKind(), exp);
                }
            }
            return expressions;
        }

        private List<IIntersectStrategy> GetStrategies()
        {
            List<IIntersectStrategy> strategies = new List<IIntersectStrategy>();
            strategies.Add(new ConstIntersectStrategy());
            strategies.Add(new FakeConstIntersectStrategy());
            strategies.Add(new SubStrIntersectStrategy());
            strategies.Add(new IdenToStrIntersectStrategy());

            return strategies;
        }
    }
}



