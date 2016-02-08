using System;

namespace DiGraph
{
    public class Edge
    {
        readonly string _fromVertex;
        public string FromVertex
        {
            get { return _fromVertex; }
        }

        readonly string _toVertex;
        public string ToVertex
        {
            get { return _toVertex; }
        }

        readonly double _weight;
        public double Weight
        {
            get { return _weight; }
        }

        public Edge(Edge edge)
        {
            _fromVertex = edge.FromVertex;
            _toVertex = edge.ToVertex;
            this._weight = edge.Weight;
        }

        public Edge(string fromVertex, string toVertex)
        {
            _fromVertex = fromVertex;
            _toVertex = toVertex;
            this._weight = 0;
        }

        public Edge(string fromVertex, string toVertex, double weight)
        {
            _fromVertex = fromVertex;
            _toVertex = toVertex;
            this._weight = weight;
        }

        public override string ToString()
        {
            return _fromVertex+"=>"+_toVertex+":"+_weight.ToString();
        }

        public override int GetHashCode()
        {
            return _fromVertex.GetHashCode() ^ _toVertex.GetHashCode() ^ _weight.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Edge)
            {
                var temp = (Edge)obj;
                return _fromVertex.Equals(temp._fromVertex) && _toVertex.Equals(temp._toVertex) 
                        && _weight.Equals(temp._weight);
            }

            throw new ArgumentException("object is not of this type!");
        }
    }
}
