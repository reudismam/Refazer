using System.Collections.Generic;

namespace ProseFunctions.Spg.Bean
{
    public class Patch
    {
        public List<IEnumerable<Node>> Edits { get; set; }

        public Patch(List<IEnumerable<Node>> edits)
        {
            Edits = edits;
        }

        public Patch()
        {
            Edits = new List<IEnumerable<Node>>();
        }
    }
}
