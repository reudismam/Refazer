using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiGraph
{
    public class Edge
    {
        string fromVertex;
        public string FromVertex
        {
            get { return fromVertex; }
        }

        string toVertex;
        public string ToVertex
        {
            get { return toVertex; }
        }

        double weight;
        public double Weight
        {
            get { return weight; }
        }

        public Edge(Edge edge)
        {
            fromVertex = edge.FromVertex;
            toVertex = edge.ToVertex;
            this.weight = edge.Weight;
        }

        public Edge(string from_vertex, string to_vertex)
        {
            fromVertex = from_vertex;
            toVertex = to_vertex;
            this.weight = 0;
        }

        public Edge(string from_vertex, string to_vertex, double weight)
        {
            fromVertex = from_vertex;
            toVertex = to_vertex;
            this.weight = weight;
        }

        public override string ToString()
        {
            return fromVertex+"=>"+toVertex+":"+weight.ToString();
        }

        public override int GetHashCode()
        {
            return fromVertex.GetHashCode() ^ toVertex.GetHashCode() ^ weight.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Edge)
            {
                var temp = (Edge)obj;
                return fromVertex.Equals(temp.fromVertex) && toVertex.Equals(temp.toVertex) 
                        && weight.Equals(temp.weight);
            }
            else
                throw new ArgumentException("object is not of this type!");
        }
    }
}
