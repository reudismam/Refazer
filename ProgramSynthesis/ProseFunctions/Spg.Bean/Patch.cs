using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using TreeEdit.Spg.Script;

namespace ProseSample.Substrings
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
