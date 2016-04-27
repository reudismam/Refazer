using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Tutor.Spg.TreeEdit.Node;

namespace Spg.TreeEdit.Node
{
    public class CSharpTreeNode : TreeNode<SyntaxNodeOrToken>
    {
        public CSharpTreeNode(SyntaxNodeOrToken value, List<ITreeNode<SyntaxNodeOrToken>> children) : base(value, new TLabel(value.Kind()), children)
        {
        }

        public CSharpTreeNode(SyntaxNodeOrToken value) : base(value, new TLabel(value.Kind()))
        {
        }

        public override bool Equals(object obj)
        {
            if (!(obj is SyntaxNodeOrToken)) return false;

            var other = (SyntaxNodeOrToken) obj;

            return Value.IsKind(other.Kind()) && Value.Span.CompareTo(other.Span) == 0;
        }
    }
}
