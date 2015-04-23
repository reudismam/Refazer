using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Spg.ExampleRefactoring.AST;
using Spg.ExampleRefactoring.Comparator;
using Spg.ExampleRefactoring.Synthesis;
using Spg.ExampleRefactoring.Tok;
using Spg.LocationRefactor.TextRegion;
using System;
using System.Collections.Generic;
using ExampleRefactoring.Spg.ExampleRefactoring.AST;
using ExampleRefactoring.Spg.ExampleRefactoring.Synthesis;
using ExampleRefactoring.Spg.LocationRefactoring.Tok;
using LocationCodeRefactoring.Spg.LocationRefactor.Operator;
using Spg.LocationRefactor.Operator.Filter;
using LocationCodeRefactoring.Spg.LocationRefactor.Operator.Map;
using LocationCodeRefactoring.Spg.LocationRefactor.Program;

namespace Spg.LocationRefactor.Operator
{
    /// <summary>
    /// IOperator
    /// </summary>
    public class Merge : IOperator
    {
        /// <summary>
        /// Program learned
        /// </summary>
        private Prog prog;

        /// <summary>
        /// operator
        /// </summary>
        private IOperator ioperator;

        /// <summary>
        /// Map list
        /// </summary>
        /// <returns></returns>
        public List<MapBase> maps { get; set; }


        public Merge(List<MapBase> maps)
        {
            this.maps = maps;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Merge()
        {
            this.maps = new List<MapBase>();
        }

        /// <summary>
        /// Add map
        /// </summary>
        /// <param name="map">Map</param>
        public void AddMap(MapBase map)
        {
            this.maps.Add(map);
        }

        /// <summary>
        /// Execute merge
        /// </summary>
        /// <param name="input">Source code</param>
        /// <returns>Execution result</returns>
        public ListNode Execute(string input)
        {
            throw new NotImplementedException();
        }

        public ListNode Execute(SyntaxNode input)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieve region
        /// </summary>
        /// <param name="input">Source code</param>
        /// <returns>Retrieve regions</returns>
        public List<TRegion> RetrieveRegion(string input)
        {
            List<TRegion> tRegions = new List<TRegion>();

            FilterBase filter = (FilterBase)maps[0].SequenceExpression.Ioperator;

            //TokenSeq tokens = ASTProgram.ConcatenateRegularExpression(filter.Predicate.r1, filter.Predicate.r2);
            //List<Token> regex = tokens.Regex();

            List<TRegion> filtereds = filter.RetrieveRegion(input);

            SynthesizedProgram synt = new SynthesizedProgram();
            foreach (MapBase map in maps)
            {
                Pair pair = (Pair)map.ScalarExpression.Ioperator;
                synt.Add(pair.expression);
            }

            //ASTProgram program = new ASTProgram();
            foreach (TRegion r in filtereds)
            {
                TRegion region = new TRegion();
                Tuple<string, string> tu = Tuple.Create(input.Substring(r.Start, r.Length), input.Substring(r.Start, r.Length));
                Tuple<ListNode, ListNode> tnodes = ASTProgram.Example(tu);

                ListNode lnode = ASTProgram.RetrieveNodes(tnodes, synt.Solutions);

                List<int> matches = ASTManager.Matches(tnodes.Item1, lnode, new NodeComparer());
                if (matches.Count == 1)
                {
                    lnode = ASTManager.SubNotes(tnodes.Item1, matches[0], lnode.Length());
                }

                if (lnode.Length() > 0)
                {
                    SyntaxNodeOrToken first = lnode.List[0];
                    SyntaxNodeOrToken last = lnode.List[lnode.Length() - 1];

                    TextSpan span = first.Span;

                    int start = r.Start + span.Start;
                    int length = last.Span.Start + last.Span.Length - span.Start;

                    region.Start = start;
                    region.Length = length;

                    tRegions.Add(region);
                }
            }
            return tRegions;
        }

        //public List<TRegion> RetrieveRegion(SyntaxNode input)
        //{
        //    throw new NotImplementedException();
        //}

        /// <summary>
        /// String representation
        /// </summary>
        /// <returns>String representation</returns>
        public override string ToString()
        {
            String s = "Merge(";
            foreach (MapBase hypothesis in maps)
            {
                s += hypothesis + ", ";
            }
            s += ")";

            return s;
        }

        public List<TRegion> RetrieveRegion(SyntaxNode input, string sourceCode)
        {
            throw new NotImplementedException();
        }

        public List<TRegion> RetrieveRegion()
        {
            throw new NotImplementedException();
        }
    }
}
