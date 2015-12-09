using System;
using System.Collections.Generic;
using Spg.LocationRefactor.Learn.Filter;
using Spg.LocationRefactor.Operator.Filter;
using Spg.LocationRefactor.Learn;
using Spg.LocationRefactor.TextRegion;

namespace Spg.LocationRefactor.Operator
{
    /// <summary>
    /// Statement filter
    /// </summary>
    public class FilterBool : FilterBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="list">Region list</param>
        public FilterBool(List<TRegion> list): base(list)
        {
            if (list == null)throw new ArgumentNullException("list");
        }

        /// <summary>
        /// Filter learner
        /// </summary>
        /// <returns>Filter learner</returns>
        protected override FilterLearnerBase GetFilterLearner(List<TRegion> list)
        {
            return new NodeFilterLearner(list);
        }
    }
}


