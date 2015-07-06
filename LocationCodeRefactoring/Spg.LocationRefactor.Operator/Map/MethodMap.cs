using System.Collections.Generic;
using Spg.LocationRefactor.Operator;
using Spg.LocationRefactor.TextRegion;

namespace Spg.LocationRefactor.Operator.Map
{
    class MethodMap : MapBase
    {
        public MethodMap(List<TRegion> list)
        {

        }
        public override string ToString()
        {
            return "MethodMap(λM: Pair(Pos(M, p1), Pos(M, p2)), MS) "
                   + "\n\tp1 = " + ((Pair)ScalarExpression.Ioperator).expression.p1
                   + "\n\tp2 = " + ((Pair)ScalarExpression.Ioperator).expression.p2
                   + "\n\tMS=" + SequenceExpression;
        }
    }
}
