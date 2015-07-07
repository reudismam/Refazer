using System.Collections.Generic;
using Spg.LocationRefactor.Operator.Map;
using Spg.LocationRefactor.TextRegion;

namespace Spg.LocationRefactor.Operator
{
    public class EndPosMap : MapBase
    {
        public EndPosMap(List<TRegion> list)
        {
        }

        public override string ToString()
        {
            return "EndSeqMap(" + ((Pair)ScalarExpression.Ioperator).Expression.p1.ToString() + "\nLS=" + SequenceExpression.ToString() + ")";
        }
    }
}


