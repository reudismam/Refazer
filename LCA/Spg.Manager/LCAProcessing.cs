using System.Collections.Generic;

namespace LCA.Spg.Manager
{
    public class LCAProcessing<T>
    {
        public object IndexLookup { get; set; }
        public object Nodes { get; set; }
        public List<int> Values { get; set; }

        public LCAProcessing(object indexLookup, object nodes, List<int> values)
        {
            IndexLookup = indexLookup;
            Nodes = nodes;
            Values = values;
        }
    }
}