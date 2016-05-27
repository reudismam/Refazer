using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.CodeAnalysis.Text;
using TreeEdit.Spg.Isomorphic;
using TreeElement.Spg.Node;

namespace TreeEdit.Spg.TreeEdit.Script
{
    public class CSharpTreeComparer: TreeComparer<SyntaxNode>
    {
        public CSharpTreeComparer()
        {
            LabelCount = 10000;
        }

        public override double GetDistance(SyntaxNode oldNode, SyntaxNode newNode)
        {
            if (ValuesEqual(oldNode, newNode)) return 1;

            return 0;
        }

        public override bool ValuesEqual(SyntaxNode oldNode, SyntaxNode newNode)
        {
            return oldNode.ToString().Equals(newNode.ToString());
        }

        protected override int GetLabel(SyntaxNode node)
        {
            return node.RawKind;
        }

        protected override int TiedToAncestor(int label)
        {
            return 0;
        }

        protected override IEnumerable<SyntaxNode> GetChildren(SyntaxNode node)
        {
            return node.ChildNodes();
        }

        protected override IEnumerable<SyntaxNode> GetDescendants(SyntaxNode node)
        {
            return node.DescendantNodes();
        }

        protected override bool TryGetParent(SyntaxNode node, out SyntaxNode parent)
        {
            parent = node.Parent;
            if (parent == null || parent.IsKind(Microsoft.CodeAnalysis.CSharp.SyntaxKind.None)) return false;

            return true;

        }

        protected override bool TreesEqual(SyntaxNode oldNode, SyntaxNode newNode)
        {
            var oldTree = new TreeNode<SyntaxNodeOrToken>(oldNode, new TLabel(oldNode.Kind()));
            var newTree = new TreeNode<SyntaxNodeOrToken>(newNode, new TLabel(newNode.Kind()));
            return IsomorphicManager<SyntaxNodeOrToken>.AhuTreeIsomorphism(oldTree, newTree);
        }

        protected override TextSpan GetSpan(SyntaxNode node)
        {
            return node.Span;
        }

        protected override int LabelCount { get; }
    }
}
