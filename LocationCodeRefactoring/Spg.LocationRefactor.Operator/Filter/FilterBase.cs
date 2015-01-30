using System;
using System.Collections.Generic;
using ExampleRefactoring.Spg.ExampleRefactoring.AST;
using ExampleRefactoring.Spg.ExampleRefactoring.Synthesis;
using ExampleRefactoring.Spg.LocationRefactoring.Tok;
using LocationCodeRefactoring.Spg.LocationRefactor.Location;
using LocationCodeRefactoring.Spg.LocationRefactor.Operator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Spg.LocationRefactor.Learn;
using Spg.LocationRefactor.Predicate;
using Spg.LocationRefactor.TextRegion;

namespace Spg.LocationRefactor.Operator.Filter
{
    /// <summary>
    /// Filter base
    /// </summary>
    public abstract class FilterBase: IOperator
    {
        public IPredicate Predicate { get; set; }

        public List<TRegion> List;

        protected FilterBase(List<TRegion> list)
        {
            this.List = list;
        }

        public override string ToString()
        {
            return Predicate.ToString();
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

        public ListNode Execute(SyntaxNode input)
        {
            FilterLearnerBase learn = GetFilterLearner(List);
            TokenSeq tokenSeq = Predicate.r1;
            TokenSeq regex = tokenSeq;

            List<SyntaxNodeOrToken> inputNodes = new List<SyntaxNodeOrToken>();
            inputNodes = ASTManager.EnumerateSyntaxNodesAndTokens(input, inputNodes);

            ListNode listNode = new ListNode(inputNodes);
            Boolean indicator = learn.Indicator(Predicate, listNode, regex);

            if (indicator)
            {
                return listNode;
            }
            return null;
        }

        /// <summary>
        /// Retrieve regions
        /// </summary>
        /// <param name="input">Source code</param>
        /// <returns>Retrieved regions</returns>
        public List<TRegion> RetrieveRegion(string input)
        {
            IEnumerable<SyntaxNode> regions = SyntaxNodes(input, List);
            return RetrieveRegionsBase(regions);
        }

        /// <summary>
        /// Retrieve region given a syntax node
        /// </summary>
        /// <param name="syntaxNode">Syntax node</param>
        /// <param name="sourceCode">Source code</param>
        /// <returns></returns>
        public List<TRegion> RetrieveRegion(SyntaxNode syntaxNode, string sourceCode)
        {
            IEnumerable<SyntaxNode> regions = SyntaxNodes(syntaxNode, List);
            return RetrieveRegionsBase(regions);
        }

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

                TokenSeq regexs = ASTProgram.ConcatenateRegularExpression(Predicate.r1, Predicate.r2);
                TokenSeq regex = regexs;

                if (ASTManager.IsMatch(lNode, regex))
                {
                    TRegion tRegion = new TRegion();

                    TextSpan span = node.Span;
                    tRegion.Text = node.GetText().ToString();
                    tRegion.Start = span.Start;
                    tRegion.Length = span.Length;
                    tRegion.Node = node;

                    tRegions.Add(tRegion);
                }
            }
            return tRegions;
        }

        /// <summary>
        /// Syntax nodes correspondents to selection
        /// </summary>
        /// <param name="input">Source code.</param>
        /// <param name="list">Selection location on source code.</param>
        /// <returns>Syntax nodes correspondents to selection on source code</returns>
        private IEnumerable<SyntaxNode> SyntaxNodes(SyntaxNode input, List<TRegion> list)
        {
            return input.DescendantNodes(); //simply return descendants nodes
        }

        /// <summary>
        /// Syntax nodes correspondents to selection
        /// </summary>
        /// <param name="input">Source code.</param>
        /// <param name="list">Selection location on source code.</param>
        /// <returns>Syntax nodes correspondents to selection on source code</returns>
        private IEnumerable<SyntaxNode> SyntaxNodes(string input, List<TRegion> list)
        {
            List<SyntaxNode> nodes = RegionManager.SyntaxElements(input, list[0].Parent.Text, list);
            return nodes;
        }
        
        /// <summary>
        /// Syntax nodes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected abstract IEnumerable<SyntaxNode> SyntaxNodes(string input);

        /// <summary>
        /// Filter learner
        /// </summary>
        /// <returns></returns>
        protected abstract FilterLearnerBase GetFilterLearner(List<TRegion> list);

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