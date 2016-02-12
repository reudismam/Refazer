using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
