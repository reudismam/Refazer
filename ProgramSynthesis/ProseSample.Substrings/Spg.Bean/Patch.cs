using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace ProseSample.Substrings.Spg.Bean
{
    public class Patch
    {
        public List<List<SyntaxNodeOrToken>> Edits { get; set; }


        public Patch(List<List<SyntaxNodeOrToken>> edits)
        {
            Edits = edits;
        }
    }
}
