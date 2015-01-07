using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Spg.ExampleRefactoring.AST;
using Spg.LocationRefactor.Learn;
using System.Collections.Generic;

namespace Spg.LocationRefactor.Operator
{
    public class MethodFilter: FilterBase
    {
        public override string ToString()
        {
            return "MethodFilter(\n\t" + predicate.ToString() + ")";
        }

        /// <summary>
        /// Method filter learner
        /// </summary>
        /// <returns>Method filter learner</returns>
        protected override FilterLearnerBase GetFilterLearner()
        {
            return new MethodFilterLearner();
        }

        /// <summary>
        /// Syntax nodes
        /// </summary>
        /// <param name="sourceCode">Source code</param>
        /// <returns>Syntax nodes</returns>
        protected override IEnumerable<SyntaxNode> SyntaxNodes(string sourceCode)
        {
            return ASTManager.SyntaxElements(sourceCode, SyntaxKind.MethodDeclaration);
        }
    }
}
