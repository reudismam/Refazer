using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using ProseFunctions.Substrings;
using TreeEdit.Spg.Isomorphic;
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
        public static List<TreeNode<SyntaxNodeOrToken>> ConcreteMatches(TreeNode<SyntaxNodeOrToken> inpTree, SyntaxNodeOrToken sot)
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
        /// <param name="kind">Syntax tree or token to be matched.</param>
        /// <returns>Abstract match</returns>
        public static List<TreeNode<SyntaxNodeOrToken>> AbstractMatches(TreeNode<SyntaxNodeOrToken> inpTree, SyntaxKind kind)
        {
            return (from item in inpTree.DescendantNodes() where item.Value.IsKind(kind) select item).ToList();
        }

        /// <summary>
        /// Abstract match
        /// </summary>
        /// <param name="inpTree">Input tree</param>
        /// <param name="kind">Syntax tree or token to be matched.</param>
        /// <returns>Abstract match</returns>
        public static List<TreeNode<SyntaxNodeOrToken>> LeafAbstractMatches(TreeNode<SyntaxNodeOrToken> inpTree, SyntaxKind kind)
        {
            var nodes = from item in inpTree.DescendantNodes() where item.Value.IsKind(kind) select item;
            var result = nodes.Where(v => !v.Children.Any()).ToList();
            return result;
        }

        /// <summary>
        /// Return all matches of the pattern on tree.
        /// </summary>
        /// <param name="node">Node</param>
        /// <param name="pattern">Pattern</param>
        public static List<TreeNode<SyntaxNodeOrToken>> Matches(TreeNode<SyntaxNodeOrToken> node, TreeNode<Token> pattern)
        {
            //TreeTraversal<SyntaxNodeOrToken> tree = new TreeTraversal<SyntaxNodeOrToken>();
            //var nodes = tree.PostOrderTraversal(tree);
            var nodes =  BFSWalker<SyntaxNodeOrToken>.BreadFirstSearch(node);
            nodes.Insert(0, node);
            var matchNodes = nodes.Where(v => IsValueEachChild(v, pattern)).ToList();

            //var edited = new List<Tuple<TreeNode<SyntaxNodeOrToken>, List<TreeNode<SyntaxNodeOrToken>>>>();
            //foreach (var match in matchNodes)
            //{
            //    var children = match.Children;
            //    var list = pattern.Children.Select(p => children.Where(v => IsValue(v, p)).ToList()).ToList();

            //    if (list.Count <= 1) continue;

            //    var added = new List<TreeNode<SyntaxNodeOrToken>>();
            //    for (int i = 0; i < list.First().Count; i++)
            //    {
            //        var parent = ConverterHelper.MakeACopy(match);
            //        parent.Children = new List<TreeNode<SyntaxNodeOrToken>>();
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
        /// Verify if the tree match the pattern
        /// </summary>
        /// <param name="node">Node</param>
        /// <param name="pattern">Pattern</param>
        public static bool IsValue(TreeNode<SyntaxNodeOrToken> node, TreeNode<Token> pattern)
        {
            if (!pattern.Value.IsMatch(node))
            {
                return false;
            }

            //if (tree.Children.Count <= pattern.Children.Count) return false;

            foreach (var child in pattern.Children)
            {
                var valid = node.Children.Any(tchild => IsValue(tchild, child));
                if (!valid)
                {
                    return false;
                }
            }
            //var child = pattern.Children[index];
            //var pchild = tree.Children[index];
            //var valid = IsValue(pchild, child);

            return true;
        }

        /// <summary>
        /// Verify if the tree match the pattern
        /// </summary>
        /// <param name="node">Node</param>
        /// <param name="pattern">Pattern</param>
        public static bool IsValueEachChild(TreeNode<SyntaxNodeOrToken> node, TreeNode<Token> pattern)
        {
            if (!pattern.Value.IsMatch(node))
            {
                return false;
            }

            if (node.Children.Count != pattern.Children.Count)
            {
                if (pattern.Children.Any()) return false;
                return pattern.Value.IsMatch(node);
            }

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

        /// <summary>
        /// Verify if the tree match the pattern
        /// </summary>
        /// <param name="tree">Node</param>
        /// <param name="node">Pattern</param>
        public static bool IsValueEachChild(TreeNode<SyntaxNodeOrToken> tree, TreeNode<SyntaxNodeOrToken> node)
        {
            if (!IsomorphicManager<SyntaxNodeOrToken>.IsIsomorphic(tree, node))
            {
                return false;
            }

            if (tree.Children.Count != node.Children.Count) return false;

            for (int i = 0; i < node.Children.Count; i++)
            {
                var nodechild = tree.Children[i];
                var child = node.Children[i];
                var valid = IsValueEachChild(nodechild, child);
                if (!valid)
                {
                    return false;
                }
            }
            return true;
        }

        ///// <summary>
        ///// Verify if the tree match the pattern
        ///// </summary>
        ///// <param name="tree">Node</param>
        ///// <param name="node">Pattern</param>
        //public static bool IsValueEachChild(TreeNode<SyntaxNodeOrToken> tree, TreeNode<SyntaxNodeOrToken> node)
        //{
        //    if (!node.Value.Equals(tree.Value))
        //    {
        //        return false;
        //    }

        //    if (tree.Children.Count != node.Children.Count) return false;

        //    for (int i = 0; i < node.Children.Count; i++)
        //    {
        //        var nodechild = tree.Children[i];
        //        var child = node.Children[i];
        //        var valid = IsValueEachChild(nodechild, child);
        //        if (!valid)
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}
    }
}
