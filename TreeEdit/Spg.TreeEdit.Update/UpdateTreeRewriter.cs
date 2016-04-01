using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace TreeEdit.Spg.TreeEdit.Update
{
    public class UpdateTreeRewriter : CSharpSyntaxRewriter
    {
        private readonly SyntaxNode _snode;
        private readonly SyntaxNode _replacement;
        public UpdateTreeRewriter(SyntaxNode snode, SyntaxNode replacement)
        {
            _snode = snode;
            _replacement = replacement;
        }

        public override SyntaxNode Visit(SyntaxNode node)
        {
            if (_snode.Equals(node))
            {
                return _replacement;
            }
            return base.Visit(node);
        }
    }
}
