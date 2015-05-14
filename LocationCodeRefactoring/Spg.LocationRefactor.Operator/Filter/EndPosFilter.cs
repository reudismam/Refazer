using Microsoft.CodeAnalysis;
using Spg.LocationRefactor.Learn;
using System;
using System.Collections.Generic;
using Spg.LocationRefactor.Operator.Filter;
using Spg.LocationRefactor.TextRegion;

namespace Spg.LocationRefactor.Operator
{
    public class EndPosFilter: FilterBase
    {

        public EndPosFilter(List<TRegion> list) :base(list)
        {

        }
        /// <summary>
        /// Filter learner
        /// </summary>
        /// <returns>Filter Learner</returns>
        protected override FilterLearnerBase GetFilterLearner(List<TRegion> list)
        {
            return new EndPosFilterLearner(list);
        }

        ///// <summary>
        ///// Syntax nodes
        ///// </summary>
        ///// <param name="input">Source code</param>
        ///// <returns>Syntax nodes</returns>
        //protected override IEnumerable<SyntaxNode> SyntaxNodesWithSemanticModel(string input)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
