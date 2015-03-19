using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ExampleRefactoring.Spg.ExampleRefactoring.AST;
using ExampleRefactoring.Spg.ExampleRefactoring.Synthesis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Spg.ExampleRefactoring.Synthesis;

namespace Spg.ExampleRefactoring.AST
{
    /// <summary>
    /// Update AST
    /// </summary>
    public static class UpdateASTManager
    {
        /// <summary>
        /// Transform the AST
        /// </summary>
        /// <param name="oldTree">Old syntax tree</param>
        /// <param name="synthesizedProg">Synthesized program</param>
        /// <returns>Syntax tree transformation</returns>
        public static ASTTransformation UpdateASTTree(SyntaxTree oldTree, SynthesizedProgram synthesizedProg)
        {
            if (oldTree == null || synthesizedProg == null)
            {
                throw new Exception("Old tree or synthesized program cannot be null");
            }
            List<SyntaxNodeOrToken> nodes = new List<SyntaxNodeOrToken>();
            nodes = ASTManager.EnumerateSyntaxNodesAndTokens(oldTree.GetRoot(), nodes);

            ListNode listNode = new ListNode(nodes);
            ListNode composition = new ListNode();
            for (int i = 0; i < synthesizedProg.Solutions.Count; i++)
            {
                ListNode subNodes = synthesizedProg.Solutions[i].RetrieveSubNodes(listNode);
                composition.List.AddRange(subNodes.List);
            }

            ASTTransformation combTree = GetSyntaxTree(composition);

            return combTree;
        }

        ///// <summary>
        ///// Transform the AST
        ///// </summary>
        ///// <param name="oldNode">Old syntax tree</param>
        ///// <param name="synthesizedProg">Synthesized program</param>
        ///// <returns>Syntax tree transformation</returns>
        //public static ASTTransformation UpdateASTTree(SyntaxNode oldNode, SynthesizedProgram synthesizedProg)
        //{
        //    if (oldNode == null || synthesizedProg == null)
        //    {
        //        throw new Exception("Old node or synthesized program cannot be null");
        //    }
        //    List<SyntaxNodeOrToken> nodes = new List<SyntaxNodeOrToken>();
        //    nodes = ASTManager.EnumerateSyntaxNodesAndTokens(oldNode, nodes);

        //    ListNode listNode = new ListNode(nodes);
        //    ListNode composition = new ListNode();
        //    for (int i = 0; i < synthesizedProg.Solutions.Count; i++)
        //    {
        //        ListNode subNodes = synthesizedProg.Solutions[i].RetrieveSubNodes(listNode);
        //        composition.List.AddRange(subNodes.List);
        //    }
        //    ASTTransformation combTree = GetSyntaxTree(composition);
        //    return combTree;
        //}

        /// <summary>
        /// Transform the AST
        /// </summary>
        /// <param name="listNode">ListNodes</param>
        /// <param name="synthesizedProg">Synthesized program</param>
        /// <returns>Syntax tree transformation</returns>
        public static ASTTransformation UpdateASTTree(ListNode listNode, SynthesizedProgram synthesizedProg)
        {
            if (listNode == null || synthesizedProg == null)
            {
                throw new Exception("Old node or synthesized program cannot be null");
            }
            //List<SyntaxNodeOrToken> nodes = new List<SyntaxNodeOrToken>();
            //nodes = ASTManager.EnumerateSyntaxNodesAndTokens(oldNode, nodes);

            //ListNode listNode = new ListNode(nodes);
            ListNode composition = new ListNode();
            for (int i = 0; i < synthesizedProg.Solutions.Count; i++)
            {
                ListNode subNodes = synthesizedProg.Solutions[i].RetrieveSubNodes(listNode);
                composition.List.AddRange(subNodes.List);
            }
            ASTTransformation combTree = GetSyntaxTree(composition);
            return combTree;
        }
        /// <summary>
        /// Create a tree
        /// </summary>
        /// <param name="nodes">List of nodes</param>
        /// <returns>Syntax tree</returns>
        public static ASTTransformation GetSyntaxTree(ListNode nodes)
        {
            if(nodes == null)
            {
                throw new Exception("Nodes cannot be true");
            }
            String astText = Parse(nodes);
            SyntaxTree tree = CSharpSyntaxTree.ParseText(astText);
            SyntaxNode root = tree.GetRoot();
            tree = tree.WithChangedText(root.GetText());

            ASTTransformation transformation = new ASTTransformation(astText, tree);
            return transformation;
        }

        /// <summary>
        /// Parse a syntax node list
        /// </summary>
        /// <param name="nodes">Syntax nodes list</param>
        /// <returns>Syntax tree text</returns>
        public static string Parse(ListNode nodes)
        {
            if(nodes == null)
            {
                throw new Exception("Nodes cannot be null");
            }
            String method = "";
            for (int i = 0; i < nodes.List.Count; i++)
            {
                SyntaxNodeOrToken n = nodes.List[i];
                String node = n.ToString();
                if (n.HasLeadingTrivia && i != 0)
                {
                    String leadingTrivial = "";

                    foreach (SyntaxTrivia trivia in n.GetLeadingTrivia())
                    {
                        leadingTrivial += trivia.ToFullString();
                    }

                    method += leadingTrivial;
                }

                //especial case for in keyword
                if (n.IsKind(SyntaxKind.InKeyword))
                {
                    method += " ";
                }
                method += node;

                if (n.HasTrailingTrivia && i != nodes.List.Count - 1)
                {
                    String trailingTrivia = "";

                    foreach (SyntaxTrivia trivia in n.GetTrailingTrivia())
                    {
                        trailingTrivia += trivia.ToFullString();
                    }

                    method += trailingTrivia;
                }
            }
            return method;
        }
    }
}
