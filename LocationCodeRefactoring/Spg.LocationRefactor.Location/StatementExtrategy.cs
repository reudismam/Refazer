using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Spg.ExampleRefactoring.AST;
using System.Collections.Generic;

namespace LocationCodeRefactoring.Br.Spg.Location
{
    public class StatementStrategy : Strategy
    {

        private static StatementStrategy instance;
        private SyntaxKind syntaxKind;

        private Dictionary<string, List<SyntaxNode>> computed;

        private StatementStrategy(SyntaxKind syntaxKind)
        {
            this.syntaxKind = syntaxKind;
            computed = new Dictionary<string, List<SyntaxNode>>();
        }

        /// <summary>
        /// Singleton instance
        /// </summary>
        /// <param name="syntaxKind">Syntax kind</param>
        /// <returns>Statement strategy instance</returns>
        public static StatementStrategy GetInstance(SyntaxKind syntaxKind)
        {
            if(instance == null)
            {
                instance = new StatementStrategy(syntaxKind);
            }

            return instance;
        }

        /// <summary>
        /// Syntax nodes 
        /// </summary>
        /// <param name="sourceCode">Source code</param>
        /// <returns>Syntax nodes</returns>
        public override List<SyntaxNode> SyntaxNodes(string sourceCode)
        {
            List<SyntaxNode> nodes = null;
            if (!computed.TryGetValue(sourceCode, out nodes))
            {
                nodes = ASTManager.SyntaxElements(sourceCode, syntaxKind);
                computed.Add(sourceCode, nodes);
            }

            return computed[sourceCode];
        }
    }
}
