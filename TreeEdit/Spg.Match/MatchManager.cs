using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using ProseSample.Substrings;
using TreeElement;
using TreeElement.Spg.Node;
using TreeElement.Spg.Walker;

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

        /// <summary>
        /// Return all matches of the pattern on node.
        /// </summary>
        /// <param name="node">Node</param>
        /// <param name="pattern">Pattern</param>
        public static List<ITreeNode<SyntaxNodeOrToken>> Matches(ITreeNode<SyntaxNodeOrToken> node, ITreeNode<Token> pattern)
        {
            //TreeTraversal<SyntaxNodeOrToken> tree = new TreeTraversal<SyntaxNodeOrToken>();
            //var nodes = tree.PostOrderTraversal(node);
            var nodes =  BFSWalker<SyntaxNodeOrToken>.BreadFirstSearch(node);
            nodes.Insert(0, node);
            var matchNodes = nodes.Where(v => IsValue(v, pattern)).ToList();

            //var edited = new List<Tuple<ITreeNode<SyntaxNodeOrToken>, List<ITreeNode<SyntaxNodeOrToken>>>>();
            //foreach (var match in matchNodes)
            //{
            //    var children = match.Children;
            //    var list = pattern.Children.Select(p => children.Where(v => IsValue(v, p)).ToList()).ToList();

            //    if (list.Count <= 1) continue;

            //    var added = new List<ITreeNode<SyntaxNodeOrToken>>();
            //    for (int i = 0; i < list.First().Count; i++)
            //    {
            //        var parent = ConverterHelper.MakeACopy(match);
            //        parent.Children = new List<ITreeNode<SyntaxNodeOrToken>>();
            //        foreach (var c in list)
            //        {
            //            parent.AddChild(c[i], parent.Children.Count());
            //        }
            //        //matchNodes.Add(parent);
            //        added.Add(parent);
            //    }
            //    edited.Add(Tuple.Create(match, added));
            //}

            //foreach (var e in edited)
            //{
            //    int position = matchNodes.FindIndex(o => o.Equals(e.Item1));
            //    matchNodes.InsertRange(position, e.Item2); 
            //    matchNodes.RemoveAll(o => o.Equals(e.Item1));
            //}

            return matchNodes;
        }


        /// <summary>
        /// Verify if the node match the pattern
        /// </summary>
        /// <param name="node">Node</param>
        /// <param name="pattern">Pattern</param>
        public static bool IsValue(ITreeNode<SyntaxNodeOrToken> node, ITreeNode<Token> pattern)
        {
            if (!pattern.Value.IsMatch(node))
            {
                return false;
            }

            //if (node.Children.Count <= pattern.Children.Count) return false;

            foreach (var child in pattern.Children)
            {
                var valid = node.Children.Any(tchild => IsValue(tchild, child));
                if (!valid)
                {
                    return false;
                }
            }
            //var child = pattern.Children[index];
            //var pchild = node.Children[index];
            //var valid = IsValue(pchild, child);

            return true;
        }

        /// <summary>
        /// Verify if the node match the pattern
        /// </summary>
        /// <param name="node">Node</param>
        /// <param name="pattern">Pattern</param>
        public static bool IsValueEachChild(ITreeNode<SyntaxNodeOrToken> node, ITreeNode<Token> pattern)
        {
            if (!pattern.Value.IsMatch(node))
            {
                return false;
            }

            if (node.Children.Count != pattern.Children.Count) return false;

            for (int i = 0; i < pattern.Children.Count; i++)
            {
                var nodechild = node.Children[i];
                var child = pattern.Children[i];
                var valid = IsValueEachChild(nodechild, child);
                if (!valid)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
