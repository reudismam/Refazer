using System;
using System.Collections.Generic;
using LocationCodeRefactoring.Br.Spg.Location;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Spg.LocationRefactor.Learn;
using Spg.LocationRefactor.TextRegion;

namespace Spg.LocationRefactor.Operator
{
    public class StatementFilter : FilterBase
    {
        private SyntaxKind syntaxKind { get; set; }

        public StatementFilter(SyntaxKind syntaxKind, List<TRegion> list): base(list)
        {
            if (list == null)
            {
                throw new Exception("List cannot be null");
            }
            this.syntaxKind = syntaxKind;
        }



        /// <summary>
        /// Filter learner
        /// </summary>
        /// <returns>Filter learner</returns>
        protected override FilterLearnerBase GetFilterLearner(List<TRegion> list)
        {
            return new StatementFilterLearner(syntaxKind, list);
        }

        /// <summary>
        /// Syntax nodes
        /// </summary>
        /// <param name="sourceCode">Source code</param>
        /// <returns>Syntax nodes</returns>
        protected override IEnumerable<SyntaxNode> SyntaxNodes(string sourceCode)
        {
            //return Strategy.SyntaxElements(sourceCode, syntaxKind);
            return Strategy.SyntaxElements(sourceCode, list);
        }
    }
}
