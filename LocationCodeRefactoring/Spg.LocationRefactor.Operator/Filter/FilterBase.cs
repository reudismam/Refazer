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

namespace Spg.LocationRefactor.Operator
{
    public abstract class FilterBase:IOperator
    {
        public IPredicate predicate { get; set; }

        private Dictionary<String, Boolean> calculated; 

        public FilterBase()
        {
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
            FilterLearnerBase learn = GetFilterLearner();
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
        protected abstract FilterLearnerBase GetFilterLearner();
    }
}
