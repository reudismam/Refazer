using System.Collections.Generic;
using Spg.LocationRefactoring.Tok;
using Spg.LocationRefactor.Predicate;

namespace LocationCodeRefactoring.Spg.LocationRefactor.Learn.Filter.BooleanLearner
{
    /// <summary>
    /// Positive boolean learner
    /// </summary>
    public class PositiveBooleanLearner :BooleanLearnerBase
    {
        public PositiveBooleanLearner(Dictionary<TokenSeq, bool> calculated): base(calculated)
        {

        }

        /// <summary>
        /// Get predicate for this filter
        /// </summary>
        /// <returns>IPredicate for this filter</returns>
        protected override IPredicate GetPredicate()
        {
            return PredicateFactory.Create(new Contains());
        }

        /// <summary>
        /// Get token seq for this filter
        /// </summary>
        /// <param name="r1">Regular expression</param>
        /// <returns>Token seq</returns>
        protected override TokenSeq GetTokenSeq(TokenSeq r1)
        {
            return r1;
        }
    }
}

