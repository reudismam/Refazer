using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace ProseSample.Substrings.ProseSample.Substrings.Witness
{
    class WitnessIdentifier: WitnessLiteralTemplate
    {
        public override bool Match(SyntaxNodeOrToken toCompare, List<SyntaxKind> kinds)
        {
            return IsKind(toCompare, kinds);
        }

        public override void InitializeKinds()
        {
            Kinds = new List<SyntaxKind> {SyntaxKind.IdentifierToken, SyntaxKind.IdentifierName};
        }
    }

    
}
