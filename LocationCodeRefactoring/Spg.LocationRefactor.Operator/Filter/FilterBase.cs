﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ExampleRefactoring.Spg.ExampleRefactoring.AST;
using ExampleRefactoring.Spg.ExampleRefactoring.Synthesis;
using ExampleRefactoring.Spg.ExampleRefactoring.Workspace;
using ExampleRefactoring.Spg.LocationRefactoring.Tok;
using Spg.LocationCodeRefactoring.Controller;
using LocationCodeRefactoring.Spg.LocationRefactor.Location;
using LocationCodeRefactoring.Spg.LocationRefactor.Node;
using LocationCodeRefactoring.Spg.LocationRefactor.Operator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Spg.LocationRefactor.Learn;
using Spg.LocationRefactor.Predicate;
using Spg.LocationRefactor.TextRegion;
using Spg.LocationRefactoring.Tok;

namespace Spg.LocationRefactor.Operator.Filter
{
    /// <summary>
    /// Filter base
    /// </summary>
    public abstract class FilterBase : IOperator
    {
        /// <summary>
        /// Predicate
        /// </summary>
        /// <returns>Predicate</returns>
        public IPredicate Predicate { get; set; }

        /// <summary>
        /// Region list
        /// </summary>
        public List<TRegion> List;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="list"></param>
        protected FilterBase(List<TRegion> list)
        {
            this.List = list;
        }

        /// <summary>
        /// Execute filter base
        /// </summary>
        /// <param name="input">Source code</param>
        /// <returns>Statements that follows the predicate</returns>
        public ListNode Execute(String input)
        {
            SyntaxTree tree1 = CSharpSyntaxTree.ParseText(input);
            return Execute(tree1.GetRoot());
        }

        /// <summary>
        /// Run the filter on input. Verify if input nodes match with the filter
        /// </summary>
        /// <param name="syntaxNode"></param>
        /// <returns>True if this filter match with the input</returns>
        public ListNode Execute(SyntaxNode syntaxNode)
        {
            FilterLearnerBase learn = GetFilterLearner(List);
            TokenSeq regex = Predicate.r1;

            Tuple<SyntaxNode, SyntaxNode> tuple = Tuple.Create(syntaxNode, syntaxNode);
            ListNode listNode = ASTProgram.Example(tuple).Item1;

            bool indicator = learn.Indicator(Predicate, listNode, regex);
            if (indicator) return listNode;

            return null;
        }

        public bool IsMatch(SyntaxNode syntaxNode)
        {
            FilterLearnerBase learn = GetFilterLearner(List);
            TokenSeq regex = new TokenSeq(Predicate.Regex());

            Tuple<SyntaxNode, SyntaxNode> tuple = Tuple.Create(syntaxNode, syntaxNode);
            ListNode listNode = ASTProgram.Example(tuple).Item1;

            bool indicator = learn.Indicator(Predicate, listNode, regex);

            return indicator;
        }

        /// <summary>
        /// Retrieve regions
        /// </summary>
        /// <param name="sourceClass">Source code</param>
        /// <returns>Retrieved regions</returns>
        public List<TRegion> RetrieveRegion(string sourceClass)
        {
            Dictionary<string, List<TRegion>> result = RegionManager.GetInstance().GroupRegionBySourceFile(List);
            if (result.Count == 1) throw new Exception("RetrieveRegion with only source code parameter does not accept single file transformation.");

            SyntaxTree tree = CSharpSyntaxTree.ParseText(sourceClass);
            string name = GetIdentifierName();
            IEnumerable<SyntaxNode> descedants = SyntacticalDecomposer.SyntaxNodes(tree.GetRoot(), name);
            List<TRegion> tregions = RetrieveRegionsBase(descedants);
            return tregions;
        }

        /// <summary>
        /// Retrieve regions
        /// </summary>
        /// <returns>Retrieved regions</returns>
        public List<TRegion> RetrieveRegion()
        {
            Dictionary<string, List<TRegion>> result = RegionManager.GetInstance().GroupRegionBySourceFile(List);
            if (result.Count == 1) throw new Exception("RetrieveRegion with only source code parameter does not accept single file transformation.");

            string name = GetIdentifierName();
            //SyntaxTree tree = CSharpSyntaxTree.ParseText(sourceClass);
            IEnumerable<SyntaxNode> descedants = SyntacticalDecomposer.SyntaxNodes(name);
            List<TRegion> tregions = RetrieveRegionsBase(descedants);
            return tregions;
        }

        /// <summary>
        /// Retrieve region given a syntax node
        /// </summary>
        /// <param name="syntaxNode">Syntax node</param>
        /// <param name="sourceCode">Source code</param>
        /// <returns>List of regions</returns>
        public List<TRegion> RetrieveRegion(SyntaxNode syntaxNode, string sourceCode)
        {
            string name = GetIdentifierName();
            IEnumerable<SyntaxNode> nodesForFiltering = SyntacticalDecomposer.SyntaxNodes(syntaxNode.Parent, name);
            //IEnumerable<SyntaxNode> nodesForFiltering =
            //    SyntacticalDecomposer.SyntaxNodesWithoutSemanticModel(syntaxNode.Parent);
            nodesForFiltering = NodesForSingleFile(nodesForFiltering, syntaxNode);
            return RetrieveRegionsBase(nodesForFiltering);
        }

        private IEnumerable<SyntaxNode> NodesForSingleFile(IEnumerable<SyntaxNode> nodes, SyntaxNode root)
        {
            List<SyntaxNode> resultList = new List<SyntaxNode>();
            foreach (SyntaxNode node in nodes)
            {
                if (node.SyntaxTree.FilePath.Equals(EditorController.GetInstance().CurrentViewCodePath.ToUpperInvariant()))
                {
                    SyntaxNode childNode = root.FindNode(node.Span);
                    if (childNode != null)
                    {
                        resultList.Add(childNode);
                    }
                }
                //MessageBox.Show("Node: " + childNode.CSharpKind());
                //Console.WriteLine(node);
            }
            return resultList;
        } 
        ///// <summary>
        ///// Syntax nodes to be used on filtering
        ///// </summary>
        ///// <param name="tree">Source code tree</param>
        ///// <returns>Syntax nodes to be used on filtering</returns>
        //private IEnumerable<SyntaxNode> SyntaxNodes(SyntaxNode tree)
        //{
        //    var nodes = from node in tree.DescendantNodesAndSelf()
        //                          where WithinLcas(node)
        //                          select node;
        //    return nodes;
        //}

        /// <summary>
        /// Base processing for RetrieveRegion
        /// </summary>
        /// <param name="regions">regions</param>
        /// <returns>List of regions</returns>
        private List<TRegion> RetrieveRegionsBase(IEnumerable<SyntaxNode> regions)
        {
            List<TRegion> tRegions = new List<TRegion>();
            //IEnumerable<SyntaxNode> regions = SyntaxNodes(syntaxNode, List);

            foreach (SyntaxNode node in regions)
            {
                List<SyntaxNodeOrToken> tokens = new List<SyntaxNodeOrToken>();
                tokens = ASTManager.EnumerateSyntaxNodesAndTokens(node, tokens);
                ListNode lNode = new ListNode(tokens);

                if (Predicate.Evaluate(lNode))
                {
                    TRegion parent = new TRegion();
                    parent.Text = node.SyntaxTree.GetText().ToString();
                    TRegion tRegion = new TRegion();

                    TextSpan span = node.Span;
                    tRegion.Text = node.GetText().ToString();
                    tRegion.Start = span.Start;
                    tRegion.Length = span.Length;
                    tRegion.Node = node;
                    tRegion.Path = node.SyntaxTree.FilePath;
                    tRegion.Parent = parent;

                    tRegions.Add(tRegion);
                }
            }
            return tRegions;
        }

        /// <summary>
        /// Get the name used to search for references
        /// </summary>
        /// <returns>Name used to search for references</returns>
        private string GetIdentifierName2()
        {
            string name = null;
            foreach (var token in Predicate.r1.Regex())
            {
                if (token is DymToken /*|| token.token.IsKind(SyntaxKind.IdentifierToken)*/)
                {
                    name = token.token.ToString();
                    break;
                }
            }

            if (name == null)
            {
                foreach (var token in Predicate.r2.Regex())
                {
                    if (token is DymToken /*|| token.token.IsKind(SyntaxKind.IdentifierToken)*/)
                    {
                        name = token.token.ToString();
                        break;
                    }
                }
            }
            return name;
            //return "Cmdlet";
        }

        /// <summary>
        /// Get the name used to search for references
        /// </summary>
        /// <returns>Name used to search for references</returns>
        private string GetIdentifierName()
        {
            string name = null;
            EditorController controller = EditorController.GetInstance();
            bool isCommonName = true;
            foreach (SyntaxNode lca in controller.Lcas)
            {
                switch (lca.CSharpKind())
                {
                    case SyntaxKind.InvocationExpression:
                        name = GetName(lca as InvocationExpressionSyntax, name);
                        if (name == null)
                        {
                            isCommonName = false;
                        }
                        break;
                    case SyntaxKind.ObjectCreationExpression:
                        ObjectCreationExpressionSyntax objectCreation = (ObjectCreationExpressionSyntax) lca;
                        SyntaxToken identifierToken = objectCreation.NewKeyword.GetNextToken();

                        string nameToken = identifierToken.ToFullString();

                        if (name == null)
                        {
                            name = nameToken;
                        }
                        else if (!name.Equals(nameToken))
                        {
                            isCommonName = false;
                        }
                        //objectCreation.
                        break;
                    case SyntaxKind.QualifiedName:
                        QualifiedNameSyntax qualifiedName = (QualifiedNameSyntax) lca;
                        if (name == null)
                        {
                            name = qualifiedName.Right.ToFullString();
                        }
                        else if (!name.Equals(qualifiedName.Right.ToFullString()))
                        {
                            isCommonName = false;
                        }
                        break;
                    case SyntaxKind.Argument:
                        ArgumentSyntax argumentSyntax = (ArgumentSyntax) lca;
                        ExpressionSyntax expressionSyntax = argumentSyntax.Expression;
                        if (expressionSyntax is InvocationExpressionSyntax)
                        {
                            name = GetName(expressionSyntax as InvocationExpressionSyntax, name);
                            if (name == null)
                            {
                                isCommonName = false;
                            }
                        }else if(expressionSyntax is ObjectCreationExpressionSyntax)
                        {
                            ObjectCreationExpressionSyntax objectArgument = (ObjectCreationExpressionSyntax) expressionSyntax;
                            SyntaxToken identifierArgument = objectArgument.NewKeyword.GetNextToken();

                            string nameArgument = identifierArgument.ToFullString();

                            if (name == null)
                            {
                                name = nameArgument;
                            }
                            else if (!name.Equals(nameArgument))
                            {
                                isCommonName = false;
                            }
                        }
                        break;
                    case SyntaxKind.ExpressionStatement:
                        ExpressionStatementSyntax expressionStatement = (ExpressionStatementSyntax) lca;
                        ExpressionSyntax expressionStatementSyntax = expressionStatement.Expression;
                        MessageBox.Show(expressionStatementSyntax.CSharpKind() + "");
                        if (expressionStatementSyntax is MemberAccessExpressionSyntax)
                        {
                            MemberAccessExpressionSyntax memberAccess = (MemberAccessExpressionSyntax)lca;

                            if (name == null)
                            {
                                name = memberAccess.Name.ToFullString();
                            }
                            else if (!name.Equals(memberAccess.Name.ToFullString()))
                            {
                                isCommonName = false;
                            }
                        }
                        else if (expressionStatementSyntax is InvocationExpressionSyntax)
                        {
                            name = GetName(expressionStatementSyntax as InvocationExpressionSyntax, name);
                            if (name == null)
                            {
                                isCommonName = false;
                            }
                        }
                        break;
                    default:
                        if (lca is MemberAccessExpressionSyntax)
                        {
                            MemberAccessExpressionSyntax memberAccess = (MemberAccessExpressionSyntax) lca;

                            if (name == null)
                            {
                                name = memberAccess.Name.ToFullString();
                            }
                            else if (!name.Equals(memberAccess.Name.ToFullString()))
                            {
                                isCommonName = false;
                            }
                        }
                        //else
                        //{
                        //    return GetIdentifierName2();
                        //}
                        break;
                }
            }
            if (name != null && isCommonName)
            {
                return name;
            }

            //return GetIdentifierName2();
            return null;
        }

        private static string GetName(InvocationExpressionSyntax lca, string name)
        {
            InvocationExpressionSyntax syntax = (InvocationExpressionSyntax)lca;
            ExpressionSyntax expression = syntax.Expression;
            if (expression is IdentifierNameSyntax)
            {
                IdentifierNameSyntax nameSyntax = (IdentifierNameSyntax)expression;
                if (name == null)
                {
                    name = nameSyntax.ToFullString();
                }
                else if (!name.Equals(nameSyntax.ToFullString()))
                {
                    return null;
                }
            }
            else if (expression is MemberAccessExpressionSyntax)
            {
                MemberAccessExpressionSyntax member = (MemberAccessExpressionSyntax)expression;
                if (name == null)
                {
                    name = member.Name.ToFullString();
                }
                else if (!name.Equals(member.Name.ToFullString()))
                {
                    return null;
                }

            }
            return name;
        }
        //private string GetIdentifierName()
        //{
        //    string name = null;

        //    EditorController controller = EditorController.GetInstance();
        //    foreach (SyntaxNode node in controller.Lcas)
        //    {
        //        node.
        //    }
        //}

        ///// <summary>
        ///// Syntax nodes correspondents to selection
        ///// </summary>
        ///// <param name="sourceCode">Source code.</param>
        ///// <param name="list">Selection location on source code.</param>
        ///// <returns>Syntax nodes correspondents to selection on source code</returns>
        //private IEnumerable<SyntaxNode> SyntaxNodesForFiltering(string sourceCode, List<TRegion> list)
        //{
        //    List<SyntaxNode> nodes = RegionManager.SyntaxNodesForFiltering(sourceCode, list);
        //    return nodes;
        //}

        ///// <summary>
        ///// Syntax nodes
        ///// </summary>
        ///// <param name="input">Source code</param>
        ///// <returns>Syntax nodes</returns>
        //protected abstract IEnumerable<SyntaxNode> SyntaxNodes(string input);

        /// <summary>
        /// Filter learner
        /// </summary>
        /// <returns>Filter learn base</returns>
        protected abstract FilterLearnerBase GetFilterLearner(List<TRegion> list);

        /// <summary>
        /// String representation
        /// </summary>
        /// <returns>String representation for this object.</returns>
        public override string ToString()
        {
            return Predicate.ToString();
        }

    }
}

///// <summary>
///// Retrieve regions
///// </summary>
///// <param name="input">Source code</param>
///// <returns>Retrieved regions</returns>
//public List<TRegion> RetrieveRegion(string input)
//{
//    IEnumerable<SyntaxNode> regions = SyntaxNodes(input, List);
//    return RetrieveRegionsBase(regions);
//    //List<TRegion> tRegions = new List<TRegion>();
//    //IEnumerable<SyntaxNode> regions = SyntaxNodes(input, List);

//    //foreach (SyntaxNode node in regions)
//    //{
//    //    List<SyntaxNodeOrToken> tokens = new List<SyntaxNodeOrToken>();
//    //    tokens = ASTManager.EnumerateSyntaxNodesAndTokens(node, tokens);
//    //    ListNode lNode = new ListNode(tokens);

//    //    TokenSeq regexs = ASTProgram.ConcatenateRegularExpression(Predicate.r1, Predicate.r2);
//    //    TokenSeq regex = regexs;

//    //    if (ASTManager.IsMatch(lNode, regex))
//    //    {
//    //        TRegion tRegion = new TRegion();

//    //        TextSpan span = node.Span;
//    //        tRegion.Text = node.GetText().ToString();
//    //        tRegion.Start = span.Start;
//    //        tRegion.Length = span.Length;
//    //        tRegion.Node = node;

//    //        tRegions.Add(tRegion);
//    //    }
//    //}
//    //return tRegions;
//}