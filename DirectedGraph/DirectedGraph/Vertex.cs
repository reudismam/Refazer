using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiGraph
{
    public class Vertex
    {
        readonly string _id;
        public string Id
        {
            get { return _id; }
        }

        readonly double weight;
        public double Weight
        {
            get { return weight; }
        }
        public Vertex(string id, double weight)
        {
            this._id = id;
            this.weight = weight;
        }

        public override string ToString()
        {
            return _id.ToString();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Vertex))
            {
                return false;
            }

            Vertex another = obj as Vertex;
            return another.Id == Id;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}
