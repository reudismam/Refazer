using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace ProseFunctions.Substrings
{
    public class Bindings
    {
        public List<SyntaxNodeOrToken> bindings { get; set; }

        public Bindings(List<SyntaxNodeOrToken> bindings)
        {
            this.bindings = bindings;
        }
    }
}