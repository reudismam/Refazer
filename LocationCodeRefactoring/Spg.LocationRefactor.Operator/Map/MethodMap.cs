using System.Collections.Generic;
using Spg.LocationRefactor.TextRegion;

namespace Spg.LocationRefactor.Operator
{
    class MethodMap : MapBase
    {
        public MethodMap(List<TRegion> list)
        {

        }
        public override string ToString()
        {
            return "MethodMap(λM: Pair(Pos(M, p1), Pos(M, p2)), MS) "
                + "\n\tp1 = " + ((Pair)scalarExpression.ioperator).expression.p1.ToString()
                + "\n\tp2 = " + ((Pair)scalarExpression.ioperator).expression.p2.ToString()
                + "\n\tMS=" + sequenceExpression.ToString();
        }
    }
}