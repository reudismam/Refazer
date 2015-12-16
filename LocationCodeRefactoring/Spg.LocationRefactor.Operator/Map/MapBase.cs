using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Spg.ExampleRefactoring.Synthesis;
using Spg.LocationRefactor.Operator.Filter;
using Spg.LocationRefactor.Program;
using Spg.LocationRefactor.TextRegion;
using Spg.ExampleRefactoring.Expression;
using System.Linq;

//using Spg.LocationRefactor.Program;

namespace Spg.LocationRefactor.Operator.Map
{
    /// <summary>
    /// Map operator
    /// </summary>
    public abstract class MapBase: IPredicateOperator
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
        public ListNode Execute(string input) {
            ListNode filtered = null;
            if(SequenceExpression.Execute(input) != null){
                filtered = ScalarExpression.Execute(input);
            }
            return filtered;
        }

        /// <summary>
        /// Execute map
        /// </summary>
        /// <param name="input">Syntax node input</param>
        /// <returns>Result of map execution</returns>
        public ListNode Execute(SyntaxNode input)
        {
            ListNode filtered = null;
            if (SequenceExpression.Execute(input) != null)
            {
                filtered = ScalarExpression.Execute(input);
            }
            return filtered;
        }

        ///// <summary>
        ///// Retrieve region from input
        ///// </summary>
        ///// <param name="filtereds">Filtered regions</param>
        ///// <param name="input">Syntax tree</param>
        ///// <returns>Region list</returns>
        //private List<TRegion> RetrieveRegionBase(List<TRegion> filtereds, string input2)
        //{
        //    List<TRegion> tRegions = new List<TRegion>();
        //    //FilterBase filter = (FilterBase)SequenceExpression.Ioperator;

        //    Pair pair = (Pair)ScalarExpression.Ioperator;

        //    //List<TRegion> filtereds = filter.RetrieveRegion(input);
        //    foreach (TRegion r in filtereds)
        //    {
        //        TRegion region = new TRegion();
        //        Tuple<SyntaxNode, SyntaxNode> tuplesn = Tuple.Create(r.Node, r.Node);
        //        var tnodes = ASTProgram.Example(tuplesn);

        //        ListNode lnode = new ListNode();
        //        try
        //        {
        //            lnode = pair.Expression.RetrieveSubNodes(tnodes.Item1);
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

        /// <summary>
        /// Retrieve region from input
        /// </summary>
        /// <param name="filtereds">Filtered regions</param>
        /// <returns>Region list</returns>
        public List<TRegion> RetrieveRegionBase(List<TRegion> filtereds)
        {
            List<TRegion> tRegions = new List<TRegion>();
            Pair pair = (Pair)ScalarExpression.Ioperator;

            foreach (TRegion r in filtereds)
            {
                TRegion region = new TRegion();
                Tuple<SyntaxNode, SyntaxNode> tuplesn = Tuple.Create(r.Node, r.Node);
                var tnodes = ASTProgram.Example(tuplesn);

                ListNode lnode = new ListNode();
                try
                {
                    //lnode = pair.Expression.RetrieveSubNodes(tnodes.Item1);
                    lnode = pair.Expression.TransformInput(tnodes.Item1);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Map cannot operate on this input. " + e.Message);
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
                    region.Text = r.Parent.Text.Substring(start, length);
                    region.Parent = r.Parent;
                    region.Path = r.Path;

                    tRegions.Add(region);
                }
            }
            return tRegions;
        }

        ///// <summary>
        ///// Retrieve region from input
        ///// </summary>
        ///// <param name="input">Syntax tree</param>
        ///// <returns>Region list</returns>
        //public virtual List<TRegion> RetrieveRegion(string input)
        //{
        //    FilterBase filter = (FilterBase)SequenceExpression.Ioperator;

        //    List<TRegion> filtereds = filter.RetrieveRegion(input);
        //    var tRegions = RetrieveRegionBase(filtereds, input);

        //    return tRegions;
        //}

        /// <summary>
        /// Retrieve region from input
        /// </summary>
        /// <returns>Region list</returns>
        public virtual List<TRegion> RetrieveRegion()
        {
            FilterBase filter = (FilterBase)SequenceExpression.Ioperator;

            List<TRegion> filtereds = filter.RetrieveRegion();
            var tRegions = RetrieveRegionBase(filtereds);

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
            FilterBase filter = (FilterBase)SequenceExpression.Ioperator;

            List<TRegion> filtereds = filter.RetrieveRegion(syntaxNode, sourceCode);

            var tRegions = RetrieveRegionBase(filtereds);
            
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

        public override string ToString()
        {
            Pair pair = (Pair)ScalarExpression.Ioperator;

            if (pair.Expression.Solutions.Count > 1) { throw new Exception("Map can contains only an expression."); }

            SubTokens expression = (SubTokens)pair.Expression.Solutions.First();

            return "NodeMap(λx: Pair(Pos(x, p1), Pos(x, p2)), NSeq)"
                + "\n\tp1 = " + expression.P1
                + "\n\tp2 = " + expression.P2
                + "\n\tNSeq=" + SequenceExpression;
        }

        public void AddPredicate()
        {

        }
    }
}


