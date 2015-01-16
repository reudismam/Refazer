using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;
using Spg.LocationRefactor.TextRegion;

namespace Spg.LocationRefactor.Operator
{
    public class StatementMap : MapBase
    {
        private List<TRegion> list;

        public StatementMap(List<TRegion> list)
        {
            this.list = list;
        }
        public override string ToString()
        {
            return "StatementMap(λSyntaxNode: Pair(Pos(S, p1), Pos(S, p2)), S)"
                + "\n\tp1 = " + ((Pair)scalarExpression.ioperator).expression.p1.ToString()
                + "\n\tp2 = " + ((Pair)scalarExpression.ioperator).expression.p2.ToString()
                + "\n\tS=" + sequenceExpression.ToString();
        }
    }
}
