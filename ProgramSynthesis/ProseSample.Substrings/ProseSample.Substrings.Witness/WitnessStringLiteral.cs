﻿using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace ProseSample.Substrings.ProseSample.Substrings.Witness
{
    class WitnessStringLiteral : WitnessLiteralTemplate
    {
        public override bool Match(SyntaxNodeOrToken toCompare, List<SyntaxKind> kinds)
        {
            return IsKind(toCompare, kinds);
        }

        public override void InitializeKinds()
        {
            Kinds = new List<SyntaxKind> { SyntaxKind.StringLiteralToken, SyntaxKind.StringLiteralExpression };
        }
    }   
}