using Microsoft.CodeAnalysis;
using Spg.ExampleRefactoring.Synthesis;
using Spg.LocationRefactor.Operator;
using Spg.LocationRefactor.Predicate;
using Spg.LocationRefactor.TextRegion;
using System;
using System.Collections.Generic;

namespace Spg.LocationRefactor.Learn
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
        protected override MapBase GetMap()
        {
            return new EndPosMap();
        }

        /// <summary>
        /// Returns a new EndPosFilter
        /// </summary>
        /// <returns></returns>
        protected override FilterLearnerBase GetFilter()
        {
            return new EndPosFilterLearner(); 
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
        public override List<SyntaxNode> SyntaxNodes(string sourceCode)
        {
            throw new NotImplementedException();
        }
    }
}
