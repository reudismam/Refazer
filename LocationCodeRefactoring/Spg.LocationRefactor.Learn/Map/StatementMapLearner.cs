using LocationCodeRefactoring.Br.Spg.Location;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Spg.ExampleRefactoring.Synthesis;
using Spg.LocationRefactor.Operator;
using Spg.LocationRefactor.Predicate;
using Spg.LocationRefactor.TextRegion;
using System;
using System.Collections.Generic;

namespace Spg.LocationRefactor.Learn
{
    public class StatementMapLearner : MapLearnerBase
    {
        private SyntaxKind syntaxKind;

        public StatementMapLearner(SyntaxKind syntaxKind)
        {
            this.syntaxKind = syntaxKind;
        }

        /// <summary>
        /// Filter
        /// </summary>
        /// <returns>Filter</returns>
        protected override FilterLearnerBase GetFilter()
        {
            return new StatementFilterLearner(syntaxKind);
        }

        /// <summary>
        /// Map
        /// </summary>
        /// <returns>Map</returns>
        protected override MapBase GetMap()
        {
            return new StatementMap(syntaxKind);
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
            Strategy strategy = StatementStrategy.GetInstance(syntaxKind);
            return strategy.Extract(list);
        }

        /// <summary>
        /// Syntax nodes
        /// </summary>
        /// <param name="sourceCode">Source code</param>
        /// <returns>Syntax nodes</returns>
        public override List<SyntaxNode> SyntaxNodes(string sourceCode)
        {
            Strategy strategy = StatementStrategy.GetInstance(syntaxKind);
            return strategy.SyntaxNodes(sourceCode);
        }
    }
}
