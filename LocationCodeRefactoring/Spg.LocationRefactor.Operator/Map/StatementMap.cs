using System.Collections.Generic;
using Spg.LocationRefactor.Operator;
using Spg.LocationRefactor.TextRegion;

namespace LocationCodeRefactoring.Spg.LocationRefactor.Operator.Map
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
            return "StatementMap(λSyntaxNode: Pair(Pos(S, p1), Pos(S, p2)), S)"
                + "\n\tp1 = " + ((Pair)ScalarExpression.Ioperator).expression.p1
                + "\n\tp2 = " + ((Pair)ScalarExpression.Ioperator).expression.p2
                + "\n\tS=" + SequenceExpression;
        }
    }
}
