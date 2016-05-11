using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace TreeEdit.Spg.TreeEdit.Update
{
    public class AddAnnotationRewriter : CSharpSyntaxRewriter
    {
        private readonly SyntaxNode _snode;
        private readonly List<SyntaxAnnotation> _annotations;

        public AddAnnotationRewriter(SyntaxNode snode, List<SyntaxAnnotation> annotations)
        {
            _annotations = annotations;
            _snode = snode;
        }

        public override SyntaxNode Visit(SyntaxNode node)
        {
            if (node != null)
            {
                if (IsEqual(node, _snode))
                {
                    foreach (var ann in _annotations)
                    {
                        node = node.WithAdditionalAnnotations(ann);
                    }
                }
            }
            return base.Visit(node);
        }

        private bool IsEqual(SyntaxNode n1, SyntaxNode n2)
        {
            return n1.IsKind(n2.Kind()) && n1.SpanStart == n2.SpanStart && n1.Span.Length == n2.Span.Length;
        }
    }
}
