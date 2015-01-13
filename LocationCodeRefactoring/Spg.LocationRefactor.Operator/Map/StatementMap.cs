using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;
using Spg.LocationRefactor.TextRegion;

namespace Spg.LocationRefactor.Operator
{
    public class StatementMap : MapBase
    {
        private SyntaxKind syntaxKind;
        private List<TRegion> list;

        public StatementMap(SyntaxKind syntaxKind, List<TRegion> list)
        {
            this.list = list;
            this.syntaxKind = syntaxKind;
        }
        public override string ToString()
        {
            return "StatementMap(λ" + syntaxKind.ToString() + ": Pair(Pos(" + syntaxKind.ToString().Substring(0, 1) + ", p1), Pos(" + syntaxKind.ToString().Substring(0, 1) + ", p2)), " + syntaxKind.ToString().Substring(0, 1) + "S) "
                + "\n\tp1 = " + ((Pair)scalarExpression.ioperator).expression.p1.ToString()
                + "\n\tp2 = " + ((Pair)scalarExpression.ioperator).expression.p2.ToString()
                + "\n\t" + syntaxKind.ToString().Substring(0, 1) + "S=" + sequenceExpression.ToString();
        }
    }
}
