using System.Collections.Generic;
using Spg.LocationRefactor.Operator.Filter;
using Spg.LocationRefactor.Operator;
using Spg.LocationRefactor.TextRegion;

namespace Spg.LocationRefactor.Learn
{
    public class EndPosFilterLearner : FilterLearnerBase
    {

        public EndPosFilterLearner(List<TRegion> list) : base(list)
        {
        }
        protected override FilterBase GetFilter(List<TRegion> list)
        {
            return new EndPosFilter(list);
        }
    }
}
