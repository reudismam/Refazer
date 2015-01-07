using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiGraph
{
    public class Vertex
    {
        string id;
        public string Id
        {
            get { return id; }
        }

        double weight;
        public double Weight
        {
            get { return weight; }
        }
        public Vertex(string id, double weight)
        {
            this.id = id;
            this.weight = weight;
        }

        public override string ToString()
        {
            return id.ToString();
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
