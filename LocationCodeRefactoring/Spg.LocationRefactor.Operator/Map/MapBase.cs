using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Spg.ExampleRefactoring.AST;
using Spg.ExampleRefactoring.Expression;
using Spg.ExampleRefactoring.Synthesis;
using Spg.ExampleRefactoring.Tok;
//using Spg.LocationRefactor.Program;
using Spg.LocationRefactor.TextRegion;
using System;
using System.Collections.Generic;
using Spg.LocationRefactor.Program;

namespace Spg.LocationRefactor.Operator
{
    /// <summary>
    /// Map operator
    /// </summary>
    public abstract class MapBase: IOperator
    {
        /// <summary>
        /// Scalar operator
        /// </summary>
        public Prog scalarExpression;
        /// <summary>
        /// Sequence operator
        /// </summary>
        public Prog sequenceExpression;

        /// <summary>
        /// Execute map
        /// </summary>
        /// <param name="input">Input</param>
        /// <returns>Matched statements</returns>
        public ListNode Execute(String input) {
            ListNode filtered = null;
            if(sequenceExpression.Execute(input) != null){
                filtered = scalarExpression.Execute(input);
            }
            return filtered;
        }

        /// <summary>
        /// Retrieve region from input
        /// </summary>
        /// <param name="input">Syntax tree</param>
        /// <returns>Region list</returns>
        public virtual List<TRegion> RetrieveRegion(string input)
        {
            List<TRegion> tRegions = new List<TRegion>();
            FilterBase filter = (FilterBase)sequenceExpression.ioperator;

            Pair pair = (Pair) scalarExpression.ioperator;
            SubStr synthesizer = pair.expression;

            TokenSeq tokens = ASTProgram.ConcatenateRegularExpression(filter.predicate.r1, filter.predicate.r2);
            List<Token> regex = tokens.Regex();

            List<TRegion> filtereds = filter.RetrieveRegion(input);
            foreach (TRegion r in filtereds)
            {
                TRegion region = new TRegion();
                List<SyntaxNodeOrToken> list = new List<SyntaxNodeOrToken>();
                list = ASTManager.EnumerateSyntaxNodesAndTokens(r.Node, list);
                ListNode node = new ListNode(list);
                Tuple<ListNode, ListNode> tnodes = Tuple.Create(node, node);
                ListNode lnode = new ListNode();

                try
                {
                    lnode = pair.expression.RetrieveSubNodes(tnodes.Item1);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Map cannot operate on this input." + e.Message);
                }

                if (lnode.Length() > 0)
                {
                    SyntaxNodeOrToken first = lnode.List[0];
                    SyntaxNodeOrToken last = lnode.List[lnode.Length() - 1];

                    TextSpan span = first.Span;
                    int start = span.Start;
                    int length = last.Span.Start + last.Span.Length - span.Start;

                    region.Start = start;
                    region.Length = length;
                    region.Node = r.Node;
                    region.Text = input.Substring(start, length);

                    tRegions.Add(region);
                }
            }
            return tRegions;
        }

        /// <summary>
        /// Example list
        /// </summary>
        /// <param name="examples">Example list</param>
        /// <returns>Example list</returns>
        public static List<Tuple<ListNode, ListNode>> Decompose(List<Tuple<ListNode, ListNode>> examples)
        {
            return examples;
        }
    }
}
