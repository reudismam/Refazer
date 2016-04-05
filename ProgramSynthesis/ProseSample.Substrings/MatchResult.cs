using Microsoft.CodeAnalysis;
using System;

namespace ProseSample.Substrings
{
    public class MatchResult
    {
        public Tuple<SyntaxNodeOrToken, Bindings> match { get; set; }

        public MatchResult(Tuple<SyntaxNodeOrToken, Bindings> match)
        {
            this.match = match;
        }
    }
}
