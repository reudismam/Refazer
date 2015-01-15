using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Spg.ExampleRefactoring.AST
{
    public class ASTRewriter : CSharpSyntaxRewriter
    {
        private SyntaxNode node;
        private SyntaxNode rewriter;

        public ASTRewriter(SyntaxNode node, SyntaxNode rewriter)
        {
            this.node = node;
            this.rewriter = rewriter;
        }

        public override SyntaxNode Visit(SyntaxNode root)
        {
            //base.Visit(root);
            foreach (var childNode in root.ChildNodes())
            {
                if (childNode.SpanStart == node.SpanStart && childNode.Span.Length == node.Span.Length)
                {
                    root = root.ReplaceNode(childNode, rewriter);
                    Visit(node);
                }
                else
                {
                    Visit(childNode);
                }
            }
            return root;
        }

    }
}
