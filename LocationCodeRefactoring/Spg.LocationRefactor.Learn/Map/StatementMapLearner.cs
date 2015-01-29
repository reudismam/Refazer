using System;
using System.Collections.Generic;
using LocationCodeRefactoring.Spg.LocationRefactor.Learn.Filter;
using LocationCodeRefactoring.Spg.LocationRefactor.Location;
using LocationCodeRefactoring.Spg.LocationRefactor.Operator.Map;
using Microsoft.CodeAnalysis;
using Spg.ExampleRefactoring.Synthesis;
using Spg.LocationRefactor.Learn;
using Spg.LocationRefactor.Operator;
using Spg.LocationRefactor.Predicate;
using Spg.LocationRefactor.TextRegion;

namespace LocationCodeRefactoring.Spg.LocationRefactor.Learn.Map
{
    /// <summary>
    /// Statement map learner
    /// </summary>
    public class StatementMapLearner : MapLearnerBase
    {
        /// <summary>
        /// Filter
        /// </summary>
        /// <returns>Filter</returns>
        protected override FilterLearnerBase GetFilter(List<TRegion> list)
        {
            return new StatementFilterLearner(list);
        }

        /// <summary>
        /// Map
        /// </summary>
        /// <returns>Map</returns>
        protected override MapBase GetMap(List<TRegion> list)
        {
            return new StatementMap(list);
        }

        /// <summary>
        /// Predicate
        /// </summary>
        /// <returns>Predicate</returns>
        protected override IPredicate GetPredicate()
        {
            return new Contains();
        }

        /// <summary>
        /// Decompose
        /// </summary>
        /// <param name="list">Examples</param>
        /// <returns>Examples</returns>
        public override List<Tuple<ListNode, ListNode>> Decompose(List<TRegion> list)
        {
            Strategy strategy = StatementStrategy.GetInstance();
            return strategy.Extract(list);
        }

        /// <summary>
        /// Syntax nodes
        /// </summary>
        /// <param name="sourceCode">Source code</param>
        /// <returns>Syntax nodes</returns>
        public override List<SyntaxNode> SyntaxNodes(string sourceCode, List<TRegion> list)
        {
            Strategy strategy = StatementStrategy.GetInstance();
            return strategy.SyntaxNodes(sourceCode, list);
        }
    }
}
