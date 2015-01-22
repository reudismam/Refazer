using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Spg.ExampleRefactoring.AST;
using Spg.ExampleRefactoring.Expression;
using Spg.ExampleRefactoring.Synthesis;
using Spg.LocationRefactor.TextRegion;
using System;
using System.Collections.Generic;

namespace Spg.LocationRefactor.Operator
{
    public class Pair:IOperator
    {
        public SubStr expression { get; set; }

        public Pair(SubStr expression) {
            this.expression = expression;
        }

        public Pair() { 
        }

        /// <summary>
        /// Pair execution
        /// </summary>
        /// <param name="input">Source code</param>
        /// <returns>Execution result</returns>
        public ListNode Execute(String input)
        {
            ASTProgram program = new ASTProgram();
            SynthesizedProgram hypothesis = new SynthesizedProgram();
            List<IExpression> expressions = new List<IExpression>();
            expressions.Add(expression);

            hypothesis.solutions = expressions;

            SyntaxTree result = ASTProgram.TransformString(input, hypothesis).tree;

            List<SyntaxNodeOrToken> nodes = new List<SyntaxNodeOrToken>();
            nodes = ASTManager.EnumerateSyntaxNodesAndTokens(result.GetRoot(), nodes);

            ListNode listNode = new ListNode(nodes);
            return listNode;
        }


        /// <summary>
        /// Retrieve region
        /// </summary>
        /// <param name="input">Source code</param>
        /// <returns>Execution result</returns>
        public List<TRegion> RetrieveRegion(String sourceCode) {
            List<TRegion> tRegions = new List<TRegion>();

            Tuple<String, String> t = Tuple.Create(sourceCode, sourceCode);
            Tuple<ListNode, ListNode> lNode = ASTProgram.Example(t);
            ListNode input = lNode.Item1;

            int pOfMatch1 = expression.p1.GetPositionIndex(input);
            int pOfMatch2 = expression.p2.GetPositionIndex(input);

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
            "p1 = " + expression.p1 + "\n" +
            "p2 = " + expression.p2;
        }
    }
}
