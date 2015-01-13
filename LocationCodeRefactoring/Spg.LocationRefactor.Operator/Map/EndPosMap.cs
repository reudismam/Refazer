using Microsoft.CodeAnalysis.Text;
using Spg.ExampleRefactoring.AST;
using Spg.ExampleRefactoring.Comparator;
using Spg.ExampleRefactoring.Expression;
using Spg.ExampleRefactoring.Position;
using Spg.ExampleRefactoring.Synthesis;
using Spg.ExampleRefactoring.Tok;
using Spg.LocationRefactor.TextRegion;
using System;
using System.Collections.Generic;

namespace Spg.LocationRefactor.Operator
{
    public class EndPosMap : MapBase
    {
        public EndPosMap(List<TRegion> list)
        {
        }

        public override string ToString()
        {
            return "EndSeqMap(" + ((Pair)scalarExpression.ioperator).expression.p1.ToString() + "\nLS=" + sequenceExpression.ToString() + ")";
        }

        /// <summary>
        /// Retrieve region
        /// </summary>
        /// <param name="sourceCode"></param>
        /// <returns></returns>
        public override List<TRegion> RetrieveRegion(String sourceCode)
        {
            List<TRegion> tRegions = new List<TRegion>();

            Tuple<String, String> t = Tuple.Create(sourceCode, sourceCode);
            Tuple<ListNode, ListNode> lNode = ASTProgram.Example(t);
            ListNode input = lNode.Item1;

            FilterBase filter = (FilterBase) sequenceExpression.ioperator;

            Pair pair = (Pair) scalarExpression.ioperator;

            SubStr synthesizer = pair.expression;

            TokenSeq tokens = ASTProgram.ConcatenateRegularExpression(filter.predicate.r1, filter.predicate.r2);
            TokenSeq regex2 = tokens;

            List<Tuple<int, ListNode>> matches = ASTManager.Matches(input, regex2, new RegexComparer());
            foreach (Tuple<int, ListNode> match in matches) {
                ListNode subNodes = ASTManager.SubNotes(input, 0, match.Item1 + regex2.Length());

                Pos p1 = (Pos)pair.expression.p1;
                p1.position = -1;

                int pofMatch = p1.GetPositionIndex(subNodes);

                ListNode matchNodes = ASTManager.SubNotes(subNodes, pofMatch, subNodes.Length() - pofMatch);

                int start = matchNodes.List[0].Span.Start;

                TextSpan span = matchNodes.List[matchNodes.Length() - 1].Span;

                int length = span.Start + span.Length - start;
                TRegion tRegion = new TRegion();
                tRegion.Start = start;
                tRegion.Length = length;

                tRegions.Add(tRegion);
            }
            return tRegions;
        }
    }
}
