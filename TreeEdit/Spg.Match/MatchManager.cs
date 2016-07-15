using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using ProseSample.Substrings;
using TreeElement;
using TreeElement.Spg.Node;

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
        public static List<ITreeNode<SyntaxNodeOrToken>> AbstractMatches(ITreeNode<SyntaxNodeOrToken> inpTree, SyntaxKind kind)
        {
            return (from item in inpTree.DescendantNodes() where item.Value.IsKind(kind) select item).ToList();
        }

        /// <summary>
        /// Abstract match
        /// </summary>
        /// <param name="inpTree">Input tree</param>
        /// <param name="kind">Syntax node or token to be matched.</param>
        /// <returns>Abstract match</returns>
        public static List<ITreeNode<SyntaxNodeOrToken>> LeafAbstractMatches(ITreeNode<SyntaxNodeOrToken> inpTree, SyntaxKind kind)
        {
            var nodes = from item in inpTree.DescendantNodes() where item.Value.IsKind(kind) select item;
            var result = nodes.Where(v => !v.Children.Any()).ToList();
            return result;
        }

        public static List<ITreeNode<SyntaxNodeOrToken>> Matches(ITreeNode<SyntaxNodeOrToken> node, ITreeNode<Token> pattern)
        {
            TreeTraversal<SyntaxNodeOrToken> tree = new TreeTraversal<SyntaxNodeOrToken>();
            var nodes = tree.PostOrderTraversal(node);
            return nodes.Where(v => IsValue(v, pattern)).ToList();
        }

        public static bool IsValue(ITreeNode<SyntaxNodeOrToken> snode, ITreeNode<Token> pattern)
        {
            //if (!snode.Value.IsKind(pattern.Value.Kind)) return false; //root pattern
            if (!pattern.Value.IsMatch(snode))
            {
                return false;
            }
            foreach (var child in pattern.Children)
            {
                var valid = snode.Children.Any(tchild => IsValue(tchild, child));
                if (!valid)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
