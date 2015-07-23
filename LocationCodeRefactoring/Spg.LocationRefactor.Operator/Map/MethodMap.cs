using System.Collections.Generic;
using Spg.LocationRefactor.TextRegion;

namespace Spg.LocationRefactor.Operator.Map
{
    /// <summary>
    /// Method map
    /// </summary>
    class MethodMap : MapBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="list">List of syntax kind.</param>
        public MethodMap(List<TRegion> list)
        {
        }

        /// <summary>
        /// String representation
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "MethodMap(λM: Pair(Pos(M, p1), Pos(M, p2)), MS) "
                   + "\n\tp1 = " + ((Pair)ScalarExpression.Ioperator).Expression.P1
                   + "\n\tp2 = " + ((Pair)ScalarExpression.Ioperator).Expression.P2
                   + "\n\tMS=" + SequenceExpression;
        }
    }
}
