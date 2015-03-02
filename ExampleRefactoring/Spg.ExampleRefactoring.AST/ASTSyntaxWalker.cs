using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace ExampleRefactoring.Spg.ExampleRefactoring.AST
{
    /// <summary>
    /// Syntax Walker
    /// </summary>
    public class ASTSyntaxWalker : CSharpSyntaxWalker
    {
        /// <summary>
        /// Token list
        /// </summary>
        /// <returns>Token list</returns>
        public List<SyntaxNodeOrToken> tokenList { get; set; }

        /// <summary>
        /// Construct a new ASTSyntaxWalker
        /// </summary>
        public ASTSyntaxWalker()
        {
            tokenList = new List<SyntaxNodeOrToken>();
        }

        /// <summary>
        /// Visit tokens
        /// </summary>
        /// <param name="token">Token</param>
        public override void VisitToken(SyntaxToken token)
        {
            tokenList.Add(token);
            base.VisitToken(token);
        }
    }
}
