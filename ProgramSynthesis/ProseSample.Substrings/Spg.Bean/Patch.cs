using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using TreeEdit.Spg.Script;

namespace ProseSample.Substrings
{
    public class Patch
    {
        public List<List<Edit<SyntaxNodeOrToken>>> Edits { get; set; }

        public Patch(List<List<Edit<SyntaxNodeOrToken>>> edits)
        {
            Edits = edits;
        }

        public Patch()
        {
            Edits = new List<List<Edit<SyntaxNodeOrToken>>>();
        }
    }
}
