using Spg.ExampleRefactoring.Expression;
using System.Linq;
using System;

namespace Spg.LocationRefactor.Operator.Map
{
    /// <summary>
    /// Statement map
    /// </summary>
    public class NodeMap : MapBase
    {

        public override string ToString()
        {
            Pair pair = (Pair) ScalarExpression.Ioperator;

            if(pair.Expression.Solutions.Count > 1) { throw new Exception("Map can contains only an expression."); }

            SubStr expression = (SubStr) pair.Expression.Solutions.First();

            return "NodeMap(λx: Pair(Pos(x, p1), Pos(x, p2)), NSeq)"
                + "\n\tp1 = " + expression.P1
                + "\n\tp2 = " + expression.P2
                + "\n\tNSeq=" + SequenceExpression;
        }
    }
}

