using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace ProseSample.Substrings
{
    public class Patch
    {
        public IEnumerable<IEnumerable<SyntaxNodeOrToken>> Edits { get; set; }

        public Patch(IEnumerable<IEnumerable<SyntaxNodeOrToken>> edits)
        {
            Edits = edits;
        }
    }
}
