using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using TreeEdit.Spg.Script;

namespace ProseSample.Substrings
{
    public class Script
    {
        public List<Edit<SyntaxNodeOrToken>> Edits { get; set; }

        public Script(List<Edit<SyntaxNodeOrToken>> edits)
        {
            Edits = edits;
        }
    }
}
