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
            foreach (var candicate in klist)
            {
                if (candicate.Children.Count != children.Count()) continue;

                bool isMatch = true;
                for (int i = 0; i < candicate.Children.Count; i++)
                {
                    var child = candicate.Children[i];
                    var childCandidate = children.ElementAt(i);
                    var isKind = child.Value.Kind().Equals(childCandidate.Match.Item1.Value.Kind());
                    var isValue = child.Value.ToString().Equals(childCandidate.Match.Item1.Value.ToString());
                    var isSame  = child.Value.Equals(childCandidate.Match.Item1.Value);
                    if (childCandidate.Type != MatchResult.Literal && !isKind) {
                        isMatch = false;
                        break;
                    }

                    if (childCandidate.Type == MatchResult.Literal && !(isKind && isValue))
                    {
                        isMatch = false;
                        break;
                    }

                    if (childCandidate.Type == MatchResult.C && !isSame)
                    {
                        isMatch = false;
                        break;
                    }
                }

                if (isMatch)
                {
                    var match = Tuple.Create<ITreeNode<SyntaxNodeOrToken>, Bindings>(candicate, null);
                    var matchResult = new MatchResult(match);
                    matchResult.Type = MatchResult.C;
                    return matchResult;
                }
            }
            return null;
        }
    }
}
