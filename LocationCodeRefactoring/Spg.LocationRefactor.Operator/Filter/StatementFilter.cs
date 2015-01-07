using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Spg.ExampleRefactoring.AST;
using Spg.LocationRefactor.Learn;
using System.Collections.Generic;

namespace Spg.LocationRefactor.Operator
{
    public class StatementFilter : FilterBase
    {
        private SyntaxKind syntaxKind { get; set; }

        public StatementFilter(SyntaxKind syntaxKind)
        {
            this.syntaxKind = syntaxKind;
        }

        /// <summary>
        /// Filter learner
        /// </summary>
        /// <returns>Filter learner</returns>
        protected override FilterLearnerBase GetFilterLearner()
        {
            return new StatementFilterLearner(syntaxKind);
        }

        /// <summary>
        /// Syntax nodes
        /// </summary>
        /// <param name="sourceCode">Source code</param>
        /// <returns>Syntax nodes</returns>
        protected override IEnumerable<SyntaxNode> SyntaxNodes(string sourceCode)
        {
            return ASTManager.SyntaxElements(sourceCode, syntaxKind);
        }
    }
}
