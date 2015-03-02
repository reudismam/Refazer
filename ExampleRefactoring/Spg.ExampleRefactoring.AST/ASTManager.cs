﻿using System;
using System.Collections.Generic;
using System.Linq;
using ExampleRefactoring.Spg.ExampleRefactoring.Synthesis;
using ExampleRefactoring.Spg.LocationRefactoring.Tok;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Spg.ExampleRefactoring.Comparator;

namespace ExampleRefactoring.Spg.ExampleRefactoring.AST
{
    /// <summary>
    /// Abstract syntax tree (AST) operations
    /// </summary>
    public static class ASTManager
    {
        /// <summary>
        /// Convert syntax node to a list
        /// </summary>
        /// <param name="root">Syntax node root</param>
        /// <param name="nodes">Syntax node or token list</param>
        /// <returns>Syntax node list</returns>
        public static List<SyntaxNodeOrToken> EnumerateSyntaxNodesAndTokens(SyntaxNodeOrToken root, List<SyntaxNodeOrToken> nodes)
        {
            if (root == null || nodes == null)
            {
                throw new Exception("Root node and list cannot be null and list must not be empty.");
            }

            if (!root.ChildNodesAndTokens().Any())
            {
                nodes.Add(root);
                return nodes;
            }

            foreach (SyntaxNodeOrToken n in root.ChildNodesAndTokens())
            {
                if (!n.IsKind(SyntaxKind.EndOfFileToken))
                {
                    EnumerateSyntaxNodesAndTokens(n, nodes);
                }
            }
            return nodes;
        }

        /// <summary>
        /// Convert syntax node to a list
        /// </summary>
        /// <param name="root">Syntax node root</param>
        /// <param name="nodes">Nodes</param>
        /// <returns>Syntax node list</returns>
        public static List<SyntaxNodeOrToken> EnumerateSyntaxNodes(SyntaxNodeOrToken root, List<SyntaxNodeOrToken> nodes)
        {
            if (root == null ||  nodes == null)
            {
                throw new Exception("Root node cannot be null and list cannot be null.");
            }

            if (root.AsNode() != null)
            {
                nodes.Add(root);
            }

            foreach (SyntaxNodeOrToken n in root.ChildNodesAndTokens())
            {
                if (!n.IsKind(SyntaxKind.EndOfFileToken))
                {
                    EnumerateSyntaxNodes(n, nodes);
                }
            }
            return nodes;
        }

        /// <summary>
        /// Syntax or tokens nodes between i and length j
        /// </summary>
        /// <param name="nodes">List of nodes</param>
        /// <param name="startPosition">Start selection index</param>
        /// <param name="selectionLength">Selection length</param>
        /// <returns></returns>
        public static ListNode SubNotes(ListNode nodes, int startPosition, int selectionLength)
        {
            if (nodes == null)
            {
                throw new Exception("Nodes cannot be null");
            }
            return new ListNode(nodes.List.GetRange(startPosition, selectionLength));
        }

        /// <summary>
        /// Evaluate if a match exists
        /// </summary>
        /// <param name="input">Input nodes</param>
        /// <param name="regex">Regular expression</param>
        /// <returns>True if a match exists</returns>
        public static bool IsMatch(ListNode input, TokenSeq regex)
        {
            if (input == null || regex == null)
            {
                throw new Exception("Input or regular expression cannot be null");
            }

            RegexComparer comparer = new RegexComparer();
            return (comparer.Matches(input, regex).Count > 0);
        }

        /// <summary>
        /// How many times the subNodes appears on input
        /// </summary>
        /// <param name="input">Input nodes</param>
        /// <param name="subNodes">SubNodes</param>
        /// <param name="comparer">comparer</param>
        /// <returns>Matches of start matches</returns>
        public static List<int> Matches(ListNode input, ListNode subNodes, ComparerBase comparer)
        {
            if (input == null || subNodes == null || comparer == null)
            {
                throw new Exception("Input or SubNodes or Comparer cannot be null");
            }
            List<int> matches = new List<int>();
            matches = comparer.Matches(input, subNodes);

            return matches;
        }

        /// <summary>
        /// How many times regular expression appears on the input
        /// </summary>
        /// <param name="input">Input nodes</param>
        /// <param name="regex">Regular expression</param>
        /// <param name="comparer">Comparer</param>
        /// <returns>Matches start index</returns>
        public static List<Tuple<int, ListNode>> Matches(ListNode input, TokenSeq regex, RegexComparer comparer)
        {
            List<Tuple<int, ListNode>> matches = new List<Tuple<int, ListNode>>();
            matches = comparer.Matches(input, regex);

            return matches;
        }

        /// <summary>
        /// First parent token
        /// </summary>
        /// <param name="token">Token</param>
        /// <returns>Fist parent of a token</returns>
        public static SyntaxNodeOrToken Parent(SyntaxNodeOrToken token)
        {
            if(token == null)
            {
                throw new Exception("Token cannot be null");
            }
            SyntaxNodeOrToken parent = token;
            while (parent.Parent.ChildNodesAndTokens().Count() <= 1)
            {
                parent = parent.Parent;
            }
            return parent.Parent;
        }

        public static List<SyntaxNodeOrToken> NodesBetweenStartAndEndPosition(SyntaxTree tree, int startPosition, int end)
        {
            List<SyntaxNodeOrToken> nodesSelection = new List<SyntaxNodeOrToken>();
            var descedentsBegin = from node in tree.GetRoot().DescendantNodesAndTokens()
                                  where startPosition <= node.SpanStart && node.Span.End <= end
                                  select node;
            nodesSelection.AddRange(descedentsBegin);
            return nodesSelection;
        }

        /// <summary>
        /// Descendant nodes with the start position, end position and syntax kind specified
        /// </summary>
        /// <param name="tree">Syntax tree</param>
        /// <param name="start">Start position</param>
        /// <param name="end">End position</param>
        /// <param name="syntaxKind">Syntax kind</param>
        /// <returns>Descendant node with the start position, end position and syntax kind specified</returns>
        private static IEnumerable<SyntaxNode> NodesWithSameStartEndAndKind(SyntaxNode tree, int start, int end,
            SyntaxKind syntaxKind)
        {
            var decedents = from snode in tree.DescendantNodes()
                            where snode.Span.Start == start && snode.Span.End == end && snode.CSharpKind() == syntaxKind
                            select snode;
            return decedents;
        }

        /// <summary>
        /// Descendant nodes with the syntax kind specified
        /// </summary>
        /// <param name="tree">Node representation of the syntax tree</param>
        /// <param name="syntaxKind">Syntax node to be considered</param>
        /// <returns>Descendant nodes with the syntax kind specified</returns>
        public static IEnumerable<SyntaxNode> NodesWithTheSameSyntaxKind(SyntaxNode tree, SyntaxKind syntaxKind)
        {
            var treeDescendents = from snode in tree.DescendantNodes()
                                  where snode.CSharpKind() == syntaxKind
                                  select snode;
            return treeDescendents;
        }

        /// <summary>
        /// Descendant node with the start position specified
        /// </summary>
        /// <param name="tree">Syntax tree</param>
        /// <param name="startPosition">Start position</param>
        /// <returns>Descendant node with the start position specified</returns>
        public static IEnumerable<SyntaxNodeOrToken> NodesWithTheSameStartPosition(SyntaxTree tree, int startPosition)
        {
            List<SyntaxNodeOrToken> nodesSelection = new List<SyntaxNodeOrToken>();
            var descedentsBegin = from node in tree.GetRoot().DescendantNodesAndTokens()
                                  where node.SpanStart == startPosition
                                  select node;
            nodesSelection.AddRange(descedentsBegin);
            return nodesSelection;
        }

        /// <summary>
        /// Descendant node with the end position specified
        /// </summary>
        /// <param name="tree">Syntax tree</param>
        /// <param name="end">End position</param>
        /// <returns>Descendant node with the end position specified</returns>
        public static IEnumerable<SyntaxNodeOrToken> NodesWithTheSameEndPosition(SyntaxTree tree, int end)
        {
            var descedentsEnd = from node in tree.GetRoot().DescendantNodesAndTokens()
                                where node.Span.End == end
                                select node;
            return descedentsEnd;
        }
    }
}

///// <summary>
///// Convert syntax node to a list
///// </summary>
///// <param name="root">Syntax node root</param>
///// <param name="nodes">Syntax node or token list</param>
///// <returns>Syntax node list</returns>
//public static List<SyntaxNodeOrToken> EnumerateSyntaxNodesAndTokens2(SyntaxNodeOrToken root, List<SyntaxNodeOrToken> nodes)
//{
//    if (root == null || nodes == null)
//    {
//        throw new Exception("Root node and list cannot be null and list must not be empty.");
//    }

//    ASTSyntaxWalker walker = new ASTSyntaxWalker();
//    walker.Visit(root.AsNode());
//    List<SyntaxNodeOrToken> walkerNodes = walker.tokenList;

//    return nodes;
//}

















/*/// <summary>
        /// Convert string to nodes
        /// </summary>
        /// <param name="examples">Examples</param>
        /// <returns>Syntax nodes or tokens list</returns>
        public static List<Tuple<SyntaxNodeOrToken, SyntaxNodeOrToken>> ConvertToNodes(List<Tuple<String, String>> examples)
        {
            List<Tuple<SyntaxNodeOrToken, SyntaxNodeOrToken>> transformed = new List<Tuple<SyntaxNodeOrToken, SyntaxNodeOrToken>>();

            foreach (Tuple<String, String> example in examples)
            {
                SyntaxTree tree1 = CSharpSyntaxTree.ParseText(example.Item1);
                SyntaxTree tree2 = CSharpSyntaxTree.ParseText(example.Item2);

                Tuple<SyntaxNodeOrToken, SyntaxNodeOrToken> node = ChangeManager.MethodsChanged(tree1, tree2);

                transformed.Add(node);
            }

            return transformed;
        }*/
/*//private static List<SyntaxNode> Nodes(SyntaxNode syntaxNode, List<SyntaxNode> nodes, SyntaxKind kind)
       //{
       //    /*if (nodes.Contains(syntaxNode))
       //    {
       //        return nodes;
       //    }
       //    if (syntaxNode.IsKind(kind))
       //    {*/
//        nodes.Add(syntaxNode);
//    //    return nodes;
//    //}

//    foreach (SyntaxNode node in syntaxNode.DescendantNodes())
//    {
//        nodes = Nodes(node, nodes, kind);
//    }

//    return nodes;
//}        */
