using System.Collections.Generic;
using Spg.LocationRefactor.TextRegion;

namespace Spg.LocationRefactor.Operator.Map
{
    /// <summary>
    /// Statement map
    /// </summary>
    public class StatementMap : MapBase
    {
        /// <summary>
        /// List of regions
        /// </summary>
        private List<TRegion> _list;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="list">List of regions</param>
        public StatementMap(List<TRegion> list)
        {
            this._list = list;
        }
        public override string ToString()
        {
            return "NodeMap(λx: Pair(Pos(x, p1), Pos(x, p2)), SSeq)"
                + "\n\tp1 = " + ((Pair)ScalarExpression.Ioperator).Expression
                + "\n\tp2 = " + ((Pair)ScalarExpression.Ioperator).Expression
                + "\n\tS=" + SequenceExpression;
        }
    }
}

