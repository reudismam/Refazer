using System.Collections.Generic;
using Spg.LocationRefactor.Operator.Filter;
using Spg.LocationRefactor.Learn;
using Spg.LocationRefactor.Operator;
using Spg.LocationRefactor.TextRegion;

namespace Spg.LocationRefactor.Learn.Filter
{

    /// <summary>
    /// Filter statements
    /// </summary>
    public class StatementFilterLearner : FilterLearnerBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="list">List of selected regions</param>
        public StatementFilterLearner(List<TRegion> list) :base(list)
        {
        }

        /// <summary>
        /// Return a statement filter
        /// </summary>
        /// <param name="list">List of selected regions</param>
        /// <returns>List of selected regions</returns>
        protected override FilterBase GetFilter(List<TRegion> list)
        {
            return new StatementFilter(list);
        }
    }
}

