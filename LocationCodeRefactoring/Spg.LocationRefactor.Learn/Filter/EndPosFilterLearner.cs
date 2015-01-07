using Spg.LocationRefactor.Operator;

namespace Spg.LocationRefactor.Learn
{
    public class EndPosFilterLearner : FilterLearnerBase
    {
        protected override FilterBase GetFilter()
        {
            return new EndPosFilter();
        }
    }
}
