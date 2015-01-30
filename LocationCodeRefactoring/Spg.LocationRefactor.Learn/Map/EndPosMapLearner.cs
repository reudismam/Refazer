using System;
using System.Collections.Generic;
using ExampleRefactoring.Spg.ExampleRefactoring.Synthesis;
using LocationCodeRefactoring.Spg.LocationRefactor.Operator.Map;
using Microsoft.CodeAnalysis;
using Spg.ExampleRefactoring.Synthesis;
using Spg.LocationRefactor.Learn;
using Spg.LocationRefactor.Operator;
using Spg.LocationRefactor.Predicate;
using Spg.LocationRefactor.TextRegion;

namespace LocationCodeRefactoring.Spg.LocationRefactor.Learn.Map
{
    public class EndPosMapLearner: MapLearnerBase
    {
        public EndPosMapLearner()
        {
        }

        /// <summary>
        /// Predicate
        /// </summary>
        /// <returns>Predicate for EndPosMap</returns>
        protected override IPredicate GetPredicate()
        {
            return new EndsWith();
        }

        /// <summary>
        /// Map for EndPosMap
        /// </summary>
        /// <returns>Map</returns>
        protected override MapBase GetMap(List<TRegion> list)
        {
            return new EndPosMap(list);
        }

        /// <summary>
        /// Returns a new EndPosFilter
        /// </summary>
        /// <returns></returns>
        protected override FilterLearnerBase GetFilter(List<TRegion> list)
        {
            return new EndPosFilterLearner(list); 
        }

        /// <summary>
        /// Decompose in examples
        /// </summary>
        /// <param name="list">Example</param>
        /// <returns>Example list</returns>
        public override List<Tuple<ListNode, ListNode>> Decompose(List<TRegion> list)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Syntax nodes for EndPosMap
        /// </summary>
        /// <param name="sourceCode">Source code</param>
        /// <returns>Syntax nodes</returns>
        public override List<SyntaxNode> SyntaxNodes(string sourceCode, List<TRegion> list)
        {
            throw new NotImplementedException();
        }
    }
}
