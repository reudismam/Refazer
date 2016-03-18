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
using Microsoft.CodeAnalysis.CSharp.Syntax;

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
            bool validKind = Enum.TryParse(kind, out skind);

            var kinds = from k in n.AsNode().DescendantNodes()
                        where k.IsKind(skind)
                        select k;

            var klist = kinds.ToList();
 
            foreach(var kindMatch in klist)
            {
                if (MatchChildren(kindMatch, expression.match.Item1))
                {
                    Tuple<SyntaxNodeOrToken, Bindings> match = Tuple.Create<SyntaxNodeOrToken, Bindings>(kindMatch, null);
                    MatchResult matchResult = new MatchResult(match);
                    return matchResult;
                }
            }

            /* if (Enum.TryParse(kind, out skind))
             {
                 //SyntaxNode parent = ASTManager.Parent(expression.match.Item1).AsNode();
                 if (parent.IsKind(skind))
                 {
                     Tuple<SyntaxNodeOrToken, Bindings> match = Tuple.Create<SyntaxNodeOrToken, Bindings>(parent, null);
                     MatchResult matchResult = new MatchResult(match);
                     return matchResult;
                 }
             }*/

            return null;

        }

        private static bool MatchChildren(SyntaxNodeOrToken parent, SyntaxNodeOrToken child)
        {
            foreach(var item in parent.ChildNodesAndTokens())
            {
                if (item.IsKind(child.Kind()) && item.ToString().Equals(child.ToString()))
                {
                    return true;
                }

                if (child.IsKind(SyntaxKind.IdentifierToken) && item.IsKind(SyntaxKind.IdentifierName))
                {
                    string itemString = item.ToString();
                    string childString = child.ToString();
                    return itemString.Equals(childString);
                }
            }

            return false;

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

        public static SyntaxNodeOrToken InsertBefore(SyntaxNodeOrToken n, MatchResult mresult, SyntaxNodeOrToken ast)
        {
            SyntaxNodeOrToken node = mresult.match.Item1;

            List<SyntaxNode> nodes = new List<SyntaxNode>() { ast.AsNode() };

            var root = n.AsNode();

            root = root.InsertNodesBefore(root.FindNode(node.Span), nodes);

            return root.NormalizeWhitespace();
        }

        public static SyntaxNodeOrToken Node1(string kind, SyntaxNodeOrToken child)
        {
            SyntaxKind skind;
            bool validKind = Enum.TryParse(kind, out skind);
            List<SyntaxNodeOrToken> children = new List<SyntaxNodeOrToken>();
            children.Add(child);
            var node = GetSyntaxElement(skind, children);
            return node;
        }

        public static SyntaxNodeOrToken Node2(string kind, SyntaxNodeOrToken child, SyntaxNodeOrToken child2)
        {
            SyntaxKind skind;
            bool validKind = Enum.TryParse(kind, out skind);
            List<SyntaxNodeOrToken> children = new List<SyntaxNodeOrToken>();
            children.Add(child);
            children.Add(child2);
            var node = GetSyntaxElement(skind, children);
            return node;
        }

        public static SyntaxNodeOrToken Const(SyntaxNodeOrToken cst)
        {
            return cst;
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

        private static SyntaxNodeOrToken GetSyntaxElement(SyntaxKind kind, List<SyntaxNodeOrToken> children)
        {
            if(kind == SyntaxKind.ExpressionStatement)
            {
                ExpressionSyntax expression = (ExpressionSyntax) children.First();
                ExpressionStatementSyntax expressionStatement = SyntaxFactory.ExpressionStatement(expression);
                return expressionStatement;
            }

            if(kind == SyntaxKind.InvocationExpression)
            {
                IdentifierNameSyntax identifier = (IdentifierNameSyntax)children[0]; //identifier name
                ArgumentListSyntax argumentList = (ArgumentListSyntax)children[1]; //argument list
                var invocation = SyntaxFactory.InvocationExpression(identifier, argumentList);
                return invocation;
            }

            if (kind == SyntaxKind.ArgumentList)
            {
                ArgumentSyntax argument = (ArgumentSyntax) children.First();
                var spal = SyntaxFactory.SeparatedList(new[] { argument });
                var argumentList = SyntaxFactory.ArgumentList(spal);
                return argumentList;
            }

            if (kind == SyntaxKind.Argument)
            {
                IdentifierNameSyntax s = (IdentifierNameSyntax) children.First();
                var argument = SyntaxFactory.Argument(s);
                return argument;
            }

            return null;
        }
    }
}
