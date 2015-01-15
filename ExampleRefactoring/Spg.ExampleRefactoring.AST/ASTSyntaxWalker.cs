using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace ExampleRefactoring.Spg.ExampleRefactoring.AST
{
    public class ASTSyntaxWalker : SyntaxWalker
    {
        public List<SyntaxNodeOrToken> tokenList { get; set; }

        public ASTSyntaxWalker()
        {
            tokenList = new List<SyntaxNodeOrToken>();
        }

        protected override void VisitToken(SyntaxToken token)
        {
            tokenList.Add(token);
        }
    }
}
