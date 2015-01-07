using Spg.LocationRefactor.Operator;

namespace Spg.LocationRefactor.Learn
{
    public class MethodFilterLearner : FilterLearnerBase
    {
        protected override FilterBase GetFilter()
        {
            return new MethodFilter();
        }
    }
}
