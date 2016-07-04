using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using TreeEdit.Spg.Script;

namespace ProseSample.Substrings.Spg.Bean
{
    public class Script
    {
        /// <summary>
        /// Edit list
        /// </summary>
        public List<Edit<SyntaxNodeOrToken>> Edits { get; set; }

        public Script(List<Edit<SyntaxNodeOrToken>> edits)
        {
            Edits = edits;
        }
    }
}
