using System;
using System.Collections.Generic;
using ExampleRefactoring.Spg.ExampleRefactoring.AST;
using ExampleRefactoring.Spg.ExampleRefactoring.Synthesis;
using LocationCodeRefactoring.Spg.LocationCodeRefactoring.Controller;
using LocationCodeRefactoring.Spg.LocationRefactor.Operator.Filter;
using LocationCodeRefactoring.Spg.LocationRefactor.Program;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Spg.ExampleRefactoring.AST;
using Spg.ExampleRefactoring.Expression;
using Spg.ExampleRefactoring.Synthesis;
using Spg.ExampleRefactoring.Tok;
using Spg.LocationRefactor.Operator;
using Spg.LocationRefactor.TextRegion;
//using Spg.LocationRefactor.Program;

namespace LocationCodeRefactoring.Spg.LocationRefactor.Operator.Map
{
    /// <summary>
    /// Map operator
    /// </summary>
    public abstract class MapBase: IOperator
    {
        /// <summary>
        /// Scalar operator
        /// </summary>
        public Prog ScalarExpression;
        /// <summary>
        /// Sequence operator
        /// </summary>
        public Prog SequenceExpression;

        /// <summary>
        /// Execute map
        /// </summary>
        /// <param name="input">Input</param>
        /// <returns>Matched statements</returns>
        public ListNode Execute(String input) {
            ListNode filtered = null;
            if(SequenceExpression.Execute(input) != null){
                filtered = ScalarExpression.Execute(input);
            }
            return filtered;
        }

        public ListNode Execute(SyntaxNode input)
        {
            ListNode filtered = null;
            if (SequenceExpression.Execute(input) != null)
            {
                filtered = ScalarExpression.Execute(input);
            }
            return filtered;
        }


        /// <summary>
        /// Retrieve region from input
        /// </summary>
        /// <param name="input">Syntax tree</param>
        /// <returns>Region list</returns>
        private List<TRegion> RetrieveRegionBase(List<TRegion> filtereds, string input)
        {
            List<TRegion> tRegions = new List<TRegion>();
            //FilterBase filter = (FilterBase)SequenceExpression.Ioperator;

            Pair pair = (Pair)ScalarExpression.Ioperator;

            //List<TRegion> filtereds = filter.RetrieveRegion(input);
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
        /// Retrieve region from input
        /// </summary>
        /// <param name="input">Syntax tree</param>
        /// <returns>Region list</returns>
        public virtual List<TRegion> RetrieveRegion(string input)
        {
            List<TRegion> tRegions = new List<TRegion>();
            FilterBase filter = (FilterBase)SequenceExpression.Ioperator;

            List<TRegion> filtereds = filter.RetrieveRegion(input);
            tRegions = RetrieveRegionBase(filtereds, input);

            return tRegions;
        }

        /// <summary>
        /// Retrieve region of the source code
        /// </summary>
        /// <param name="syntaxNode">Syntax node to be considered</param>
        /// <param name="sourceCode">Source code</param>
        /// <returns>List of region on the source code</returns>
        public List<TRegion> RetrieveRegion(SyntaxNode syntaxNode, string sourceCode)
        {
            List<TRegion> tRegions = new List<TRegion>();
            FilterBase filter = (FilterBase)SequenceExpression.Ioperator;

            List<TRegion> filtereds = filter.RetrieveRegion(syntaxNode, sourceCode);

            tRegions = RetrieveRegionBase(filtereds, sourceCode);
            
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

///// <summary>
///// Retrieve region from input
///// </summary>
///// <param name="input">Syntax tree</param>
///// <returns>Region list</returns>
//private List<TRegion> RetrieveRegionBase(List<TRegion> filtereds, string input)
//{
//    List<TRegion> tRegions = new List<TRegion>();
//    FilterBase filter = (FilterBase)SequenceExpression.Ioperator;

//    Pair pair = (Pair)ScalarExpression.Ioperator;
//    SubStr synthesizer = pair.expression;

//    TokenSeq tokens = ASTProgram.ConcatenateRegularExpression(filter.Predicate.r1, filter.Predicate.r2);
//    List<Token> regex = tokens.Regex();

//    //List<TRegion> filtereds = filter.RetrieveRegion(input);
//    foreach (TRegion r in filtereds)
//    {
//        TRegion region = new TRegion();
//        List<SyntaxNodeOrToken> list = new List<SyntaxNodeOrToken>();
//        list = ASTManager.EnumerateSyntaxNodesAndTokens(r.Node, list);
//        ListNode node = new ListNode(list);
//        Tuple<ListNode, ListNode> tnodes = Tuple.Create(node, node);
//        ListNode lnode = new ListNode();

//        try
//        {
//            lnode = pair.expression.RetrieveSubNodes(tnodes.Item1);
//        }
//        catch (Exception e)
//        {
//            Console.WriteLine("Map cannot operate on this input." + e.Message);
//        }

//        if (lnode.Length() > 0)
//        {
//            SyntaxNodeOrToken first = lnode.List[0];
//            SyntaxNodeOrToken last = lnode.List[lnode.Length() - 1];

//            TextSpan span = first.Span;
//            int start = span.Start;
//            int length = last.Span.Start + last.Span.Length - span.Start;

//            region.Start = start;
//            region.Length = length;
//            region.Node = r.Node;
//            region.Text = input.Substring(start, length);

//            tRegions.Add(region);
//        }
//    }
//    return tRegions;
//}