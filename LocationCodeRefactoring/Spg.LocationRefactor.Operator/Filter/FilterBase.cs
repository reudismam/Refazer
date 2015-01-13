using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Spg.ExampleRefactoring.AST;
using Spg.ExampleRefactoring.Synthesis;
using Spg.ExampleRefactoring.Tok;
using Spg.LocationRefactor.Learn;
using Spg.LocationRefactor.Predicate;
using Spg.LocationRefactor.TextRegion;
using System;
using System.Collections.Generic;
using LocationCodeRefactoring.Br.Spg.Location;

namespace Spg.LocationRefactor.Operator
{
    /// <summary>
    /// Filter base
    /// </summary>
    public abstract class FilterBase:IOperator
    {
        public IPredicate predicate { get; set; }

        private Dictionary<String, Boolean> calculated;

        public List<TRegion> list;

        public FilterBase(List<TRegion> list)
        {
            this.list = list;
            calculated = new Dictionary<String, Boolean>();
        }

        public override string ToString()
        {
            return predicate.ToString();
        }

        /// <summary>
        /// Execute filter base
        /// </summary>
        /// <param name="input">Source code</param>
        /// <returns>Statements that follows the predicate</returns>
        public ListNode Execute(String input)
        {
            FilterLearnerBase learn = GetFilterLearner(list);
            TokenSeq tokenSeq = predicate.r1;
            TokenSeq regex = tokenSeq;

            SyntaxTree tree1 = CSharpSyntaxTree.ParseText(input);
            List<SyntaxNodeOrToken> inputNodes = new List<SyntaxNodeOrToken>();
            inputNodes = ASTManager.EnumerateSyntaxNodesAndTokens(tree1.GetRoot(), inputNodes);

            ListNode listNode = new ListNode(inputNodes);
            Boolean indicator = learn.Indicator(predicate, listNode, regex);

            if(indicator){
                return listNode;
            }
            return null;
        }

        /// <summary>
        /// Retrieve regions
        /// </summary>
        /// <param name="input">Source code</param>
        /// <returns>Retrieved regions</returns>
        public List<TRegion> RetrieveRegion(String input)
        {
            List<TRegion> tRegions = new List<TRegion>();
            //IEnumerable<SyntaxNode> regions = SyntaxNodes(input);
            IEnumerable<SyntaxNode> regions = SyntaxNodes(input, list);

            foreach (SyntaxNode node in regions)
            {
                List<SyntaxNodeOrToken> tokens = new List<SyntaxNodeOrToken>();
                tokens = ASTManager.EnumerateSyntaxNodesAndTokens(node, tokens);
                ListNode lNode = new ListNode(tokens);

                TokenSeq regexs = ASTProgram.ConcatenateRegularExpression(predicate.r1, predicate.r2);
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

        private IEnumerable<SyntaxNode> SyntaxNodes(string input, List<TRegion> list)
        {
            List<SyntaxNode> nodes = Strategy.SyntaxElements(input, list[0].Parent.Text, list);
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


    /*/// <summary>
        /// Retrieve regions
        /// </summary>
        /// <param name="input">Source code</param>
        /// <returns>Retrieved regions</returns>
        public List<TRegion> RetrieveRegion(String input)
        {
            List<TRegion> tRegions = new List<TRegion>();
            IEnumerable<SyntaxNode> regions = SyntaxNodes(input);

            foreach (SyntaxNode node in regions){
                List<SyntaxNodeOrToken> tokens = new List<SyntaxNodeOrToken>();
                tokens = ASTManager.EnumerateSyntaxNodesAndTokens(node, tokens);
                ListNode lNode = new ListNode(tokens);

                TokenSeq regexs = ASTProgram.ConcatenateRegularExpression(predicate.r1, predicate.r2);
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
        }*/
}
