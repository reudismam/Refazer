using System;
using System.Collections.Generic;
using DiGraph;

namespace Spg.ExampleRefactoring.Digraph
{
    /// <summary>
    /// Use a breadth first method to find a path from source vertex s to target vertex v in a directed
    /// graph(Digraph).  See a previous post for code for Digraph.We also add a new array, _distTo(compared
    /// to DepthFirstDirectedPaths, to keep track of the length of the shortest paths.
    /// </summary>
    public class BreadthFirstDirectedPaths
    {
        private static readonly double INFINITY = Double.MaxValue;
        private Dictionary<string, bool> Marked;  
        private Dictionary<string, string> EdgeTo;     
        private Dictionary<string, double> DistanceTo;    

        /// <summary>
        /// Single source breadth first search
        /// </summary>
        /// <param name="G">Directed graph</param>
        /// <param name="s">First node</param>
        public BreadthFirstDirectedPaths(DirectedGraph G, string s)
        {
            Marked = new Dictionary<string, bool>(); 
            DistanceTo = new Dictionary<string, double>(); 
            EdgeTo = new Dictionary<string, string>();                                      

            foreach (Vertex v in G.GetVertices())
            {
                Marked.Add(v.Id, false);
                DistanceTo.Add(v.Id, INFINITY);
                EdgeTo.Add(v.Id, null);
            }
            BFS(G, s);
        }

        /// <summary>
        /// A function to do breadth first search.  In this case we use a for loop rather than a
        ///recursive function to find the shortest path from s to v.
        ///We start with the source vertex s. Rather than "fanning out" from each vertex recursively
        /// we travel along a single path (in turn) adding connected vertices to the q Queue until
        ///all vertices have been reached.
        /// We avoid "going backwards" or needlessly looking at all paths by keeping track of 
        ///which vertices we've already visited using the Marked[] array.
        ///We keep track of how we're moving through the graph (from s to v) using EdgeTo[].
        ///We keep track of how far we've traveled using DistanceTo[w].
        /// </summary>
        /// <param name="G">Directed graph</param>
        /// <param name="s">First node</param>
        private void BFS(DirectedGraph G, string s)
        {
            Queue<string> q = new Queue<string>();
            Marked[s] = true;
            DistanceTo[s] = 0.0;
            q.Enqueue(s);
            while (q.Count > 0)
            {
                string v = q.Dequeue();
                foreach (KeyValuePair<string, double> w in G.adjacencyList[v])
                {
                    if (!Marked[w.Key])
                    {
                        EdgeTo[w.Key] = v;
                        DistanceTo[w.Key] = DistanceTo[v] + 1;
                        Marked[w.Key] = true;
                        q.Enqueue(w.Key);
                    }
                }
            }
        }


        /// <summary>
        /// In the BFS method we've kept track of the shortest path from s to all connected vertices
        /// using the DistanceTo[] array.
        /// </summary>
        /// <param name="v">Final vertex</param>
        /// <returns>Distance to from node s to node v</returns>
        public double DistTo(string v)
        {
            return DistanceTo[v];
        }

        /// <summary>
        /// In the BFS method we've kept track of vertices connected to the source s
        /// using the Marked[] array.
        /// </summary>
        /// <param name="v">Final vertex</param>
        /// <returns>True is there is a path from vertex s to vertex v</returns>
        public bool HasPathTo(string v)
        {
            return Marked[v];
        }

        /// <summary>
        /// We can find the path from s to v working backwards using the _edgeTo array.
        /// For example, if we want to find the path from 3 to 0.  We look at _edgeTo[0] which gives us
        /// a vertex, say 2.  We then look at _edgeTo[2] and so on until _edgeTo[x] equals 3 (our
        /// source vertex)
        /// </summary>
        /// <param name="v">Vertex v</param>
        /// <returns>Path from vertex s to vertex v</returns>
        public IEnumerable<String> PathTo(string v)
        {
            if (!HasPathTo(v)) return null;
            Stack<String> path = new Stack<String>();
            string x;
            for (x = v; DistanceTo[x] != 0; x = EdgeTo[x])
                path.Push(x);
            path.Push(x);
            return path;
        }
    }
}

