using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using TreeElement.Spg.Node;

namespace ProseSample.Substrings.Spg.Semantic
{
    public class Match
    {
        /// <summary>
        /// Match function. This function matches the first element on the tree that has the specified kind and child nodes.
        /// </summary>
        /// <param name="node">Node</param>
        /// <param name="kind">Syntax kind</param>
        /// <param name="children">Children nodes</param>
        /// <returns> Returns the first element on the tree that has the specified kind and child nodes.</returns>
        public static MatchResult C(SyntaxNodeOrToken node, SyntaxKind kind, IEnumerable<MatchResult> children)
        {
            var currentTree = Semantics.GetCurrentTree(node);
            var klist = Semantics.SplitToNodes(currentTree, kind);
            foreach (var item in klist)
            {
                for (int i = 0; i < children.Count(); i++)
                {
                    var expression = children.ElementAt(i);
                    if (MatchChildren(item.Value, expression.Match.Item1.Value))
                    {
                        var match = Tuple.Create<ITreeNode<SyntaxNodeOrToken>, Bindings>(item, null);
                        var matchResult = new MatchResult(match);
                        return matchResult;
                    }
                }
            }
            return null;
        }


        /// <summary>
        /// Verify if the parent contains the parameter child
        /// </summary>
        /// <param name="parent">Parent node</param>
        /// <param name="child">Child node</param>
        /// <returns>True, if parent contains the child, false otherwise.</returns>
        private static bool MatchChildren(SyntaxNodeOrToken parent, SyntaxNodeOrToken child)
        {
            foreach (var item in parent.ChildNodesAndTokens())
            {
                if (item.IsKind(child.Kind()))
                {
                    return true;
                }

                if (child.IsKind(SyntaxKind.IdentifierToken) || child.IsKind(SyntaxKind.IdentifierName) ||
                    (child.IsKind(SyntaxKind.NumericLiteralToken) || child.IsKind(SyntaxKind.NumericLiteralExpression))
                    || (child.IsKind(SyntaxKind.StringLiteralToken) || child.IsKind(SyntaxKind.StringLiteralExpression)))
                {
                    string itemString = item.ToString();
                    string childString = child.ToString();
                    if (itemString.Equals(childString))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
