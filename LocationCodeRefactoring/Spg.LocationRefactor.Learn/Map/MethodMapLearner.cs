using LocationCodeRefactoring.Br.Spg.Location;
using Microsoft.CodeAnalysis;
using Spg.ExampleRefactoring.Synthesis;
using Spg.LocationRefactor.Operator;
using Spg.LocationRefactor.Predicate;
using Spg.LocationRefactor.TextRegion;
using System;
using System.Collections.Generic;

namespace Spg.LocationRefactor.Learn
{
    /// <summary>
    /// Learn methods
    /// </summary>
    public class MethodMapLearner : MapLearnerBase
    {
        /// <summary>
        /// Contains predicate
        /// </summary>
        /// <returns>Contains predicate</returns>
        protected override IPredicate GetPredicate()
        {
            return new Contains();
        }

        /// <summary>
        /// Method map
        /// </summary>
        /// <returns>Method map</returns>
        protected override MapBase GetMap()
        {
            return new MethodMap();
        }

        /// <summary>
        /// Filter
        /// </summary>
        /// <returns>Filter</returns>
        protected override FilterLearnerBase GetFilter()
        {
            return new MethodFilterLearner();
        }

        /// <summary>
        /// Decompose
        /// </summary>
        /// <param name="list">Examples</param>
        /// <returns>Examples</returns>
        public override List<Tuple<ListNode, ListNode>> Decompose(List<TRegion> list)
        {
            Strategy strategy = new MethodExtrategy();
            return strategy.Extract(list);
        }

        /// <summary>
        /// Syntax nodes
        /// </summary>
        /// <param name="sourceCode">Source code</param>
        /// <returns>Syntax nodes</returns>
        public override List<SyntaxNode> SyntaxNodes(string sourceCode)
        {
            Strategy strategy = new MethodExtrategy();
            return strategy.SyntaxNodes(sourceCode);
        }
    }
}
