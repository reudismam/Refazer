using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiGraph
{
    public class DirectedGraph
    {
        Dictionary<string, double> vertices;

        public Dictionary<string, Dictionary<string, double>> adjacencyList { get; set; }
        public Dictionary<string, Dictionary<string, double>> inverseAdjacencyList { get; set; }

        public DirectedGraph()
        {
            vertices = new Dictionary<string, double>();
            adjacencyList = new Dictionary<string, Dictionary<string, double>>();
            inverseAdjacencyList = new Dictionary<string, Dictionary<string, double>>();
        }

        private void makeFrom(DirectedGraph dg)
        {
            vertices = new Dictionary<string, double>();
            adjacencyList = new Dictionary<string, Dictionary<string, double>>();
            inverseAdjacencyList = new Dictionary<string, Dictionary<string, double>>();

            var vs = dg.GetVertices();
            foreach (var vertex in vs)
                AddVertex(vertex);
            var edges = dg.GetEdges();
            foreach (var edge in edges)
                AddEdge(edge);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("[");
            foreach (var pair in vertices)
                builder.Append(pair.Key.ToString() + ",");
            if (vertices.Count > 0)
                builder.Remove(builder.Length - 1, 1);
            builder.Append("]");
            builder.AppendLine();
            builder.Append("[");
            bool count = false;
            foreach (var pair in adjacencyList)
                foreach (var x in pair.Value.Keys)
                {
                    builder.Append("(" + pair.Key.ToString() + "," + x.ToString() + "),");
                    count = true;
                }
            if (count)
                builder.Remove(builder.Length - 1, 1);
            builder.Append("]");
            return builder.ToString();
        }

        public string ToDotFormat()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("digraph graphname {");
            foreach (var pair in vertices)
                builder.AppendFormat("{0};\n", pair.Key);
            foreach (var pair in adjacencyList)
                foreach (var pair2 in pair.Value)
                    builder.AppendFormat("{0} -> {1}  [weight={2}, label={2}];\n", pair.Key, pair2.Key, pair2.Value);
            builder.AppendLine("}");
            return builder.ToString();
        }

        public DirectedGraph Clone()
        {
            DirectedGraph result = new DirectedGraph();
            var vs = GetVertices();
            foreach (var vertex in vs)
                result.AddVertex(vertex);
            var edges = GetEdges();
            foreach (var edge in edges)
                result.AddEdge(edge);

            return result;
        }

        public static DirectedGraph MakeRandomDirectedAcyclicGraph(int vertex_count, double density)
        {
            Random r = new Random();
            DirectedGraph result = new DirectedGraph();
            for (int v = 0; v < vertex_count; v++)
                result.AddVertex(v.ToString());
            for (int i = 0; i < vertex_count; i++)
                for (int j = i + 1; j < vertex_count; j++)
                    if (r.NextDouble() <= density)
                        result.AddEdge(i.ToString(), j.ToString());
            return result;

        }


        public void AddVertex(string vertex)
        {
            if (vertices.ContainsKey(vertex))
                throw new ArgumentException("A vertex alreadey exists with this id:(" + vertex.ToString() + ")!");
            vertices.Add(vertex, 0);
            adjacencyList[vertex] = new Dictionary<string, double>();
            inverseAdjacencyList[vertex] = new Dictionary<string, double>();
        }

        public void AddVertex(string vertex, double weight)
        {
            if (vertices.ContainsKey(vertex))
                throw new ArgumentException("A vertex alreadey exists with this id:(" + vertex.ToString() + ")!");
            //weight = Math.Round(weight, 3);
            vertices.Add(vertex, weight);
            adjacencyList[vertex] = new Dictionary<string, double>();
            inverseAdjacencyList[vertex] = new Dictionary<string, double>();
        }

        public void AddVertex(Vertex vertex)
        {
            if (vertices.ContainsKey(vertex.Id))
                throw new ArgumentException("A vertex alreadey exists with this id:(" + vertex.ToString() + ")!");
            //double weight = Math.Round(vertex.Weight, 3);
            vertices.Add(vertex.Id, vertex.Weight);
            adjacencyList[vertex.Id] = new Dictionary<string, double>();
            inverseAdjacencyList[vertex.Id] = new Dictionary<string, double>();
        }

        public void AddEdge(string from_vertex, string to_vertex)
        {
            if (!vertices.ContainsKey(from_vertex))
                throw new ArgumentException("A vertex does not exist with this id:(" + from_vertex.ToString() + ")!");
            if (!vertices.ContainsKey(to_vertex))
                throw new ArgumentException("A vertex does not exist with this id:(" + to_vertex.ToString() + ")!");

            if (adjacencyList[from_vertex].ContainsKey(to_vertex))
                throw new ArgumentException("This edge already exists:(" + from_vertex.ToString() + "," + to_vertex.ToString() + ")!");
            adjacencyList[from_vertex].Add(to_vertex, 0);
            inverseAdjacencyList[to_vertex].Add(from_vertex, 0);
        }

        public void AddEdge(string from_vertex, string to_vertex, double weight)
        {
            if (!vertices.ContainsKey(from_vertex))
                throw new ArgumentException("A vertex does not exist with this id:(" + from_vertex.ToString() + ")!");
            if (!vertices.ContainsKey(to_vertex))
                throw new ArgumentException("A vertex does not exist with this id:(" + to_vertex.ToString() + ")!");
            if (adjacencyList[from_vertex].ContainsKey(to_vertex))
                throw new ArgumentException("This edge already exists:(" + from_vertex.ToString() + "," + to_vertex.ToString() + ")!");
            weight = Math.Round(weight, 3);
            adjacencyList[from_vertex].Add(to_vertex, weight);
            inverseAdjacencyList[to_vertex].Add(from_vertex, weight);
        }

        public void AddEdge(Edge edge)
        {
            double weight = Math.Round(edge.Weight, 3);
            AddEdge(edge.FromVertex, edge.ToVertex, weight);
        }

        public void RemoveVertex(string vertex)
        {
            if (vertices.Remove(vertex) == false)
                throw new ArgumentException("A vertex does not exist with this id:(" + vertex.ToString() + ")!");
            foreach (var pair in adjacencyList[vertex])
                inverseAdjacencyList[pair.Key].Remove(vertex);
            foreach (var pair in inverseAdjacencyList[vertex])
                adjacencyList[pair.Key].Remove(vertex);

            adjacencyList.Remove(vertex);
            inverseAdjacencyList.Remove(vertex);
            vertices.Remove(vertex);
        }

        public void UpdateVertex(string vertex,double weight)
        {
            if (vertices.ContainsKey(vertex) == false)
                throw new ArgumentException("A vertex does not exist with this id:(" + vertex.ToString() + ")!");
            vertices[vertex] = weight;
        }


        public void RemoveEdge(string from_vertex, string to_vertex)
        {
            if (!vertices.ContainsKey(from_vertex))
                throw new ArgumentException("A vertex does not exist with this id:(" + from_vertex.ToString() + ")!");
            if (!vertices.ContainsKey(to_vertex))
                throw new ArgumentException("A vertex does not exist with this id:(" + to_vertex.ToString() + ")!");
            if (!adjacencyList[from_vertex].ContainsKey(to_vertex))
                throw new ArgumentException("This Edge:(" + to_vertex.ToString() + ") does not exist!");
            adjacencyList[from_vertex].Remove(to_vertex);
            inverseAdjacencyList[to_vertex].Remove(from_vertex);
        }

        public void ZeroEdge(string from_vertex, string to_vertex)
        {
            if (!vertices.ContainsKey(from_vertex))
                throw new ArgumentException("A vertex does not exist with this id:(" + from_vertex.ToString() + ")!");
            if (!vertices.ContainsKey(to_vertex))
                throw new ArgumentException("A vertex does not exist with this id:(" + to_vertex.ToString() + ")!");
            if (!adjacencyList[from_vertex].ContainsKey(to_vertex))
                throw new ArgumentException("This Edge:(" + to_vertex.ToString() + ") does not exist!");
            adjacencyList[from_vertex][to_vertex] = 0;
            inverseAdjacencyList[to_vertex][from_vertex] = 0;
        }

        public void UpdateEdge(string from_vertex, string to_vertex, double new_weight)
        {
            if (!vertices.ContainsKey(from_vertex))
                throw new ArgumentException("A vertex does not exist with this id:(" + from_vertex.ToString() + ")!");
            if (!vertices.ContainsKey(to_vertex))
                throw new ArgumentException("A vertex does not exist with this id:(" + to_vertex.ToString() + ")!");
            if (!adjacencyList[from_vertex].ContainsKey(to_vertex))
                throw new ArgumentException("This Edge:(" + to_vertex.ToString() + ") does not exist!");
            adjacencyList[from_vertex][to_vertex] = new_weight;
            inverseAdjacencyList[to_vertex][from_vertex] = new_weight;
        }

        public bool HasVertex(string vertex)
        {
            return vertices.ContainsKey(vertex);
        }

        public bool HasEdge(string from_vertex, string to_vertex)
        {
            if (adjacencyList.ContainsKey(from_vertex))
                if (adjacencyList[from_vertex].ContainsKey(to_vertex))
                    return true;
            return false;
        }

        public List<Edge> GetEdges()
        {
            List<Edge> result = new List<Edge>();
            foreach (var pair1 in adjacencyList)
                foreach (var pair2 in pair1.Value)
                    result.Add(new Edge(pair1.Key, pair2.Key, pair2.Value));
            result = (from r in result orderby r.FromVertex, r.ToVertex select r).ToList();
            return result;
        }

        public List<Vertex> GetVertices()
        {
            List<Vertex> result = new List<Vertex>();
            foreach (var pair in vertices)
                result.Add(new Vertex(pair.Key, pair.Value));
            result = (from r in result orderby r.Id select r).ToList();
            return result;
        }

        public int GetVertexCount()
        {
            return vertices.Count;
        }

        public int GetEdgeCount()
        {
            int result = 0;
            foreach (var pair in adjacencyList)
                result += pair.Value.Count;
            return result;
        }

        public double GetVertexWeight(string vertex)
        {
            if (!vertices.ContainsKey(vertex))
                throw new ArgumentException("A vertex does not exist with this id:(" + vertex.ToString() + ")!");
            return vertices[vertex];
        }

        public double GetEdgeWeight(string from_vertex, string to_vertex)
        {
            if (!vertices.ContainsKey(from_vertex))
                throw new ArgumentException("A vertex does not exist with this id:(" + from_vertex.ToString() + ")!");
            if (!vertices.ContainsKey(to_vertex))
                throw new ArgumentException("A vertex does not exist with this id:(" + to_vertex.ToString() + ")!");
            if (!adjacencyList[from_vertex].ContainsKey(to_vertex))
                throw new ArgumentException("This Edge:(" + to_vertex.ToString() + ") does not exist!");
            return adjacencyList[from_vertex][to_vertex];
        }


        public List<string> Children(string vertex)
        {
            Dictionary<string, double> result = null;
            if (adjacencyList.TryGetValue(vertex, out result))
                return result.Keys.ToList();
            else
                throw new ArgumentException("A vertex does not exist with this id:(" + vertex.ToString() + ")!");
        }

        public List<string> Parents(string vertex)
        {
            Dictionary<string, double> result = null;
            if (inverseAdjacencyList.TryGetValue(vertex, out result))
                return result.Keys.ToList();
            else
                throw new ArgumentException("A vertex does not exist with this id:(" + vertex.ToString() + ")!");
        }

        public List<string> GetRootVertices()
        {
            List<string> result = new List<string>();
            foreach (var pair in inverseAdjacencyList)
                if (pair.Value.Count == 0)
                    result.Add(pair.Key);
            return result;
        }

        public List<string> GetLeafVertices()
        {
            List<string> result = new List<string>();
            foreach (var pair in adjacencyList)
                if (pair.Value.Count == 0)
                    result.Add(pair.Key);
            return result;
        }

        public bool HasCycle()
        {
            var copy = this.Clone();
            bool result = false;
            var v_count = GetVertices().Count;
            for (int i = 0; i < v_count; i++)
            {
                string victim = null;
                foreach (var pair in inverseAdjacencyList)
                    if (pair.Value.Count == 0)
                    {
                        victim = pair.Key;
                        break;
                    }
                if (victim == null)
                {
                    result = true;
                    break;
                }
                else
                    RemoveVertex(victim);

            }
            makeFrom(copy);
            return result;
        }

        public List<string> TopologicalOrder()
        {
            List<string> result = new List<string>();
            if (HasCycle())
                throw new Exception("Directed Graph contains a cycle!");
            var copy = this.Clone();

            var v_count = GetVertices().Count;
            for (int i = 0; i < v_count; i++)
            {
                object victim = null;
                foreach (var pair in inverseAdjacencyList)
                    if (pair.Value.Count == 0)
                    {
                        victim = pair.Key;
                        break;
                    }
                RemoveVertex((string)victim);
                result.Add((string)victim);
            }

            makeFrom(copy);
            return result;
        }

        public List<string> DepthFirstSearch(string start_vertex)
        {
            List<string> result = new List<string>();
            var unvisted_nodes = new HashSet<string>(vertices.Keys);
            unvisted_nodes.Remove(start_vertex);
            Stack<string> S = new Stack<string>();
            S.Push(start_vertex);
            while (S.Count > 0)
            {
                var top = S.Pop();
                result.Add(top);
                foreach (var neighbor in Children(top))
                    if (unvisted_nodes.Remove(neighbor))
                        S.Push(neighbor);
            }
            return result;
        }

        public List<string> BreathFirstSearch(string start_vertex)
        {
            List<string> result = new List<string>();
            var unvisted_nodes = new HashSet<string>(vertices.Keys);
            unvisted_nodes.Remove(start_vertex);
            Queue<string> Q = new Queue<string>();
            Q.Enqueue(start_vertex);
            while (Q.Count > 0)
            {
                var head = Q.Dequeue();
                result.Add(head);
                foreach (var neighbor in Children(head))
                    if (unvisted_nodes.Remove(neighbor))
                        Q.Enqueue(neighbor);
            }
            return result;
        }

        public List<string> GetCriticalPath()
        {
            var result = new List<string>();
            Dictionary<string, double> maxdistance = new Dictionary<string, double>();

            var topological_order = TopologicalOrder();
            foreach (var task in topological_order)
            {
                maxdistance[task] = GetVertexWeight(task);
                foreach (var parent in Parents(task))
                    maxdistance[task] = Math.Max(maxdistance[task], GetVertexWeight(task) + maxdistance[parent] + GetEdgeWeight(parent, task));
            }

            var leaf_nodes = GetLeafVertices();
            string max_node = null;
            foreach (var leaf in leaf_nodes)
                if (max_node == null)
                    max_node = leaf;
                else
                    if (maxdistance[leaf] > maxdistance[max_node])
                        max_node = leaf;

            result.Add(max_node);

            while (Parents(max_node).Count > 0)
            {
                var parents = Parents(max_node);
                var current_node = max_node;
                max_node = null;
                foreach (var parent in parents)
                    if (max_node == null)
                        max_node = parent;
                    else
                        if (maxdistance[parent] + GetEdgeWeight(parent, current_node) > maxdistance[max_node] + GetEdgeWeight(max_node, current_node))
                            max_node = parent;
                result.Add(max_node);
            }

            result.Reverse();
            return result;
        }

        public double CriticalPathLength()
        {
            var result = new List<string>();
            Dictionary<string, double> maxdistance = new Dictionary<string, double>();

            var topological_order = TopologicalOrder();
            foreach (var task in topological_order)
            {
                maxdistance[task] = GetVertexWeight(task);
                foreach (var parent in Parents(task))
                    maxdistance[task] = Math.Max(maxdistance[task], GetVertexWeight(task) + maxdistance[parent] + GetEdgeWeight(parent, task));
            }

            var leaf_nodes = GetLeafVertices();
            string max_node = null;
            foreach (var leaf in leaf_nodes)
                if (max_node == null)
                    max_node = leaf;
                else
                    if (maxdistance[leaf] > maxdistance[max_node])
                        max_node = leaf;

            return maxdistance[max_node];
        }

    }
}
