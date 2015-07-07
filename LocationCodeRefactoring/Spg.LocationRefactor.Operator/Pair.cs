using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Spg.ExampleRefactoring.AST;
using Spg.ExampleRefactoring.Expression;
using Spg.ExampleRefactoring.Synthesis;
using Spg.LocationRefactor.TextRegion;
using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;

namespace Spg.LocationRefactor.Operator
{
    public class Pair:IOperator
    {
        /// <summary>
        /// Sub expression
        /// </summary>
        public SubStr Expression { get; set; }

        public Pair(SubStr expression) {
            this.Expression = expression;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Pair() { 
        }

        /// <summary>
        /// Pair execution
        /// </summary>
        /// <param name="input">Source code</param>
        /// <returns>Execution result</returns>
        public ListNode Execute(string input)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(input);
            return Execute(tree.GetRoot());
        }

        public ListNode Execute(SyntaxNode input)
        {
            SynthesizedProgram hypothesis = new SynthesizedProgram();
            List<IExpression> expressions = new List<IExpression>();
            expressions.Add(Expression);

            hypothesis.Solutions = expressions;

            List<SyntaxNodeOrToken> list = new List<SyntaxNodeOrToken>();
            
            list = ASTManager.EnumerateSyntaxNodesAndTokens(input, list);
            ListNode lnode = new ListNode(list);

            SyntaxTree result = ASTProgram.TransformString(lnode, hypothesis).Tree;

            List<SyntaxNodeOrToken> nodes = new List<SyntaxNodeOrToken>();
            nodes = ASTManager.EnumerateSyntaxNodesAndTokens(result.GetRoot(), nodes);

            ListNode listNode = new ListNode(nodes);
            return listNode;
        }


        /// <summary>
        /// Retrieve region
        /// </summary>
        /// <param name="sourceCode">Source code</param>
        /// <returns>Execution result</returns>
        public List<TRegion> RetrieveRegion(string sourceCode) {
            List<TRegion> tRegions = new List<TRegion>();

            Tuple<String, String> t = Tuple.Create(sourceCode, sourceCode);
            Tuple<ListNode, ListNode> lNode = ASTProgram.Example(t);
            ListNode input = lNode.Item1;

            int pOfMatch1 = Expression.p1.GetPositionIndex(input);
            int pOfMatch2 = Expression.p2.GetPositionIndex(input);

            ListNode matchNodes = ASTManager.SubNotes(input, pOfMatch1, (pOfMatch2 - pOfMatch1));

            int start = matchNodes.List[0].Span.Start;

            TextSpan span = matchNodes.List[matchNodes.Length() - 1].Span;

            int length = span.Start + span.Length - start;

            TRegion tRegion = new TRegion();
            tRegion.Start = start;
            tRegion.Length = length;

            tRegions.Add(tRegion);

            return tRegions;
        }

        /// <summary>
        /// Retrieve region
        /// </summary>
        /// <returns>Regions retrieved</returns>
        public List<TRegion> RetrieveRegion()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieve regions
        /// </summary>
        /// <param name="syntaxNode">Syntax node</param>
        /// <param name="sourceCode">Source code</param>
        /// <returns>Regions retrieved</returns>
        public List<TRegion> RetrieveRegion(SyntaxNode syntaxNode, string sourceCode)
        {
            List<TRegion> tRegions = new List<TRegion>();

            Tuple<String, String> t = Tuple.Create(sourceCode, sourceCode);
            Tuple<ListNode, ListNode> lNode = ASTProgram.Example(t);
            ListNode input = lNode.Item1;

            int pOfMatch1 = Expression.p1.GetPositionIndex(input);
            int pOfMatch2 = Expression.p2.GetPositionIndex(input);

            ListNode matchNodes = ASTManager.SubNotes(input, pOfMatch1, (pOfMatch2 - pOfMatch1));

            int start = matchNodes.List[0].Span.Start;

            TextSpan span = matchNodes.List[matchNodes.Length() - 1].Span;

            int length = span.Start + span.Length - start;

            TRegion tRegion = new TRegion();
            tRegion.Start = start;
            tRegion.Length = length;

            tRegions.Add(tRegion);

            return tRegions;
        }

        public override string ToString()
        {
            return "Pair(p1, p2, LS)\n" +
            "p1 = " + Expression.p1 + "\n" +
            "p2 = " + Expression.p2;
        }
    }
}




