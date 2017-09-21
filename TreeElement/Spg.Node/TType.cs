using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeElement.Spg.Node
{
    public class TType
    {
        public object NodeType { get; set; }

        public TType(object type)
        {
            NodeType = type;
        }

        public override string ToString()
        {
            return NodeType.ToString();
        }
    }
}
