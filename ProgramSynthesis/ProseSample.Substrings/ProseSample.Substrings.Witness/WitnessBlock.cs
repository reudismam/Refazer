using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ProseSample.Substrings.ProseSample.Substrings.Witness
{
    class WitnessBlock : WitnessLiteralTemplate
    {
        public override bool Match(SyntaxNodeOrToken toCompare, List<SyntaxKind> kinds)
        {
            return IsKind(toCompare, kinds) && !((BlockSyntax)toCompare).ChildNodes().Any();
        }

        public override void InitializeKinds()
        {
            Kinds = new List<SyntaxKind> { SyntaxKind.Block };
        }
    }   
}
