using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.ProgramSynthesis.Extraction.Text.Semantics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Spg.ExampleRefactoring.Synthesis;
using Spg.ExampleRefactoring.AST;
using Spg.ExampleRefactoring.Comparator;
using Spg.LocationRefactoring.Tok;
using Spg.ExampleRefactoring.RegularExpression;
using Spg.ExampleRefactoring.LCS;

namespace ProseSample.Substrings
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public static class Semantics
    {
        public static StringRegion SubStr(StringRegion v, Tuple<uint?, uint?> posPair)
        {
            uint? start = posPair.Item1, end = posPair.Item2;
            if (start == null || end == null || start < v.Start || start > v.End || end < v.Start || end > v.End)
                return null;
            return v.Slice((uint) start, (uint) end);
        }

        public static uint? AbsPos(StringRegion v, int k)
        {
            if (Math.Abs(k) > v.Length + 1) return null;
            return (uint) (k > 0 ? (v.Start + k - 1) : (v.End + k + 1));
        }

        public static uint? RegPos(StringRegion v, Tuple<RegularExpression, RegularExpression> rr, int k)
        {
            List<PositionMatch> ms = rr.Item1.Run(v).Where(m => rr.Item2.MatchesAt(v, m.Right)).ToList();
            int index = k > 0 ? (k - 1) : (ms.Count + k);
            return index < 0 || index >= ms.Count ? null : (uint?) ms[index].Right;
        }

        public static MatchResult C1(SyntaxNodeOrToken n, string kind, MatchResult expression)
        {
            SyntaxKind skind;
            if (Enum.TryParse(kind, out skind))
            {
                SyntaxNode parent = ASTManager.Parent(expression.match.Item1).AsNode();
                if (parent.IsKind(skind))
                {
                    Tuple<SyntaxNodeOrToken, Bindings> match = Tuple.Create<SyntaxNodeOrToken, Bindings>(parent, null);
                    MatchResult matchResult = new MatchResult(match);
                    return matchResult;
                }
            }
     
            return null;
        }

        public static MatchResult C2(SyntaxNodeOrToken n, string kind, MatchResult expression1, MatchResult expression2)
        {
            SyntaxKind skind;
           
            if (Enum.TryParse(kind, out skind))
            {
                List<SyntaxNodeOrToken> list = new List<SyntaxNodeOrToken>();
                list.Add(expression1.match.Item1); list.Add(expression2.match.Item1);
                SyntaxNode parent = LCAManager.GetInstance().LeastCommonAncestor(list, list.First().SyntaxTree).AsNode();
                if (parent.IsKind(skind))
                {
                    Tuple<SyntaxNodeOrToken, Bindings> match = Tuple.Create<SyntaxNodeOrToken, Bindings>(parent, null);
                    MatchResult matchResult = new MatchResult(match);
                    return matchResult;
                }
            }

            return null;
        }

        public static SyntaxNodeOrToken Identifier(string id)
        {
            SyntaxNode n = SyntaxFactory.IdentifierName(id);
            return n;
        }

        public static SyntaxNode PredefinedType(string id)
        {
            SyntaxToken predType = SyntaxFactory.ParseToken(id);
            SyntaxNode n = SyntaxFactory.PredefinedType(predType);
            return n;
        }

        public static SyntaxNode NumericLiteralExpression(string id)
        {
            double d = double.Parse(id);
            SyntaxNode numericLiteral = SyntaxFactory.ParseExpression(d.ToString());
            return numericLiteral;
        }

        public static MatchResult Literal(SyntaxNodeOrToken n, SyntaxNodeOrToken node)
        {
            Tuple<SyntaxNodeOrToken, SyntaxNodeOrToken> tuple = Tuple.Create(n, (SyntaxNodeOrToken) node);
            Tuple<ListNode, ListNode> lnode = ASTProgram.Example(tuple);

            TokenSeq seq = DymTokens(lnode.Item2.List);
            var matches = Regex.Matches(lnode.Item1, seq);

            bool m = matches.Any();
            if (m)
            {
                Tuple<SyntaxNodeOrToken, Bindings> match = Tuple.Create<SyntaxNodeOrToken, Bindings>(matches.First().Item2.List.First(), null);
                MatchResult matchResult = new MatchResult(match);
                return matchResult;
            }

            return null;
        }

        private static TokenSeq DymTokens(List<SyntaxNodeOrToken> list)
        {
            var tokens = new List<Spg.ExampleRefactoring.Tok.Token>();
            foreach(var item in list)
            {
                RawDymToken t = new RawDymToken(item);
                tokens.Add(t);
            }

            TokenSeq seq = new TokenSeq(tokens);
            return seq;
        }
    }
}
