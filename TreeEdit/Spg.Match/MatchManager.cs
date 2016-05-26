using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Tutor.Spg.Node;

namespace TreeEdit.Spg.Match
{
    public class MatchManager
    {
        /// <summary>
        /// Matches concrete
        /// </summary>
        /// <param name="inpTree">Input tree</param>
        /// <param name="sot">Look for</param>
        /// <returns>Matched nodes</returns>
        public static List<ITreeNode<SyntaxNodeOrToken>> ConcreteMatches(ITreeNode<SyntaxNodeOrToken> inpTree, SyntaxNodeOrToken sot)
        {
            var descendants = inpTree.DescendantNodes();
            var matches = from item in descendants
                          where item.Value.IsKind(sot.Kind()) && item.ToString().Equals(sot.ToString())
                          select item;
            return matches.ToList();
        }

        /// <summary>
        /// Abstract match
        /// </summary>
        /// <param name="inpTree">Input tree</param>
        /// <param name="kind">Syntax node or token to be matched.</param>
        /// <returns>Abstract match</returns>
        public static List<SyntaxNodeOrToken> AbstractMatches(ITreeNode<SyntaxNodeOrToken> inpTree, SyntaxKind kind)
        {
            return (from item in inpTree.DescendantNodes() where item.Value.IsKind(kind) select item.Value).ToList();
        }
    }
}
