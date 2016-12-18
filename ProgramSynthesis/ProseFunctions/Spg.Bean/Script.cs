﻿using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using TreeEdit.Spg.Script;

namespace ProseFunctions.Substrings.Spg.Bean
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
