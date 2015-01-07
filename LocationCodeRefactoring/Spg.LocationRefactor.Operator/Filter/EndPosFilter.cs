using Microsoft.CodeAnalysis;
using Spg.LocationRefactor.Learn;
using System;
using System.Collections.Generic;

namespace Spg.LocationRefactor.Operator
{
    public class EndPosFilter: FilterBase
    {
        /// <summary>
        /// Filter learner
        /// </summary>
        /// <returns>Filter Learner</returns>
        protected override FilterLearnerBase GetFilterLearner()
        {
            return new EndPosFilterLearner();
        }

        /// <summary>
        /// Syntax nodes
        /// </summary>
        /// <param name="input">Source code</param>
        /// <returns>Syntax nodes</returns>
        protected override IEnumerable<SyntaxNode> SyntaxNodes(string input)
        {
            throw new NotImplementedException();
        }
    }
}
