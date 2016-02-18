using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Extraction.Text.Semantics;
using Microsoft.ProgramSynthesis.Learning;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Specifications;
using Microsoft.ProgramSynthesis.Utils;
using static ProseSample.Substrings.RegexUtils;
using Microsoft.CodeAnalysis;
using Spg.ExampleRefactoring.Synthesis;
using Spg.LocationRefactoring.Tok;
using Spg.ExampleRefactoring.LCS;
using Microsoft.CodeAnalysis.CSharp;
using Spg.ExampleRefactoring.AST;

namespace ProseSample.Substrings
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public static class WitnessFunctions
    {
        [WitnessFunction("SubStr", 1)]
        public static DisjunctiveExamplesSpec WitnessPositionPair(GrammarRule rule, int parameter,
                                                                  ExampleSpec spec)
        {
            var ppExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var v = (StringRegion) input[rule.Body[0]];
                var desiredOutput = (StringRegion) spec.Examples[input];
                var occurrences = new List<object>();
                for (int i = v.Value.IndexOf(desiredOutput.Value, StringComparison.Ordinal);
                     i >= 0;
                     i = v.Value.IndexOf(desiredOutput.Value, i + 1, StringComparison.Ordinal))
                {
                    occurrences.Add(Tuple.Create(v.Start + (uint?) i, v.Start + (uint?) i + desiredOutput.Length));
                }
                ppExamples[input] = occurrences;
            }
            return DisjunctiveExamplesSpec.From(ppExamples);
        }

        [WitnessFunction("AbsPos", 1)]
        public static DisjunctiveExamplesSpec WitnessK(GrammarRule rule, int parameter,
                                                       DisjunctiveExamplesSpec spec)
        {
            var kExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var v = (StringRegion) input[rule.Body[0]];
                var positions = new List<object>();
                foreach (uint pos in spec.DisjunctiveExamples[input])
                {
                    positions.Add((int) pos + 1 - (int) v.Start);
                    positions.Add((int) pos - (int) v.End - 1);
                }
                kExamples[input] = positions;
            }
            return DisjunctiveExamplesSpec.From(kExamples);
        }

        [WitnessFunction("RegPos", 1)]
        public static DisjunctiveExamplesSpec WitnessRegexPair(GrammarRule rule, int parameter,
                                                               DisjunctiveExamplesSpec spec)
        {
            var rrExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var v = (StringRegion) input[rule.Body[0]];
                var regexes = new List<Tuple<RegularExpression, RegularExpression>>();
                foreach (uint pos in spec.DisjunctiveExamples[input])
                {
                    Dictionary<Token, TokenMatch> rightMatches;
                    if (!v.Cache.TryGetAllMatchesStartingAt(pos, out rightMatches)) continue;
                    Dictionary<Token, TokenMatch> leftMatches;
                    if (!v.Cache.TryGetAllMatchesEndingAt(pos, out leftMatches)) continue;
                    var leftRegexes = leftMatches.Keys.Select(RegularExpression.Create).Append(Epsilon);
                    var rightRegexes = rightMatches.Keys.Select(RegularExpression.Create).Append(Epsilon);
                    var regexPairs = from l in leftRegexes from r in rightRegexes select Tuple.Create(l, r);
                    regexes.AddRange(regexPairs);
                }
                rrExamples[input] = regexes;
            }
            return DisjunctiveExamplesSpec.From(rrExamples);
        }

        [WitnessFunction("RegPos", 2, DependsOnParameters = new[] { 1 })]
        public static DisjunctiveExamplesSpec WitnessRegexCount(GrammarRule rule, int parameter,
                                                                DisjunctiveExamplesSpec spec,
                                                                ExampleSpec regexBinding)
        {
            var kExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var v = (StringRegion) input[rule.Body[0]];
                var rr = (Tuple<RegularExpression, RegularExpression>) regexBinding.Examples[input];
                var ks = new List<object>();
                foreach (uint pos in spec.DisjunctiveExamples[input])
                {
                    var ms = rr.Item1.Run(v).Where(m => rr.Item2.MatchesAt(v, m.Right)).ToArray();
                    int index = ms.BinarySearchBy(m => m.Right.CompareTo(pos));
                    if (index < 0) return null;
                    ks.Add(index + 1);
                    ks.Add(index - ms.Length);
                }
                kExamples[input] = ks;
            }
            return DisjunctiveExamplesSpec.From(kExamples);
        }

        [WitnessFunction("Literal", 1)]
        public static DisjunctiveExamplesSpec WitnessTree(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();

            var mats = new List<object>();
            foreach (State input in spec.ProvidedInputs)
            {
                var v = (SyntaxNodeOrToken)input[rule.Body[0]];
                var desiredOutput = (MatchResult) spec.Examples[input];

                Tuple<SyntaxNodeOrToken, SyntaxNodeOrToken> tuple = Tuple.Create(v, desiredOutput.match.Item1);
                Tuple<ListNode, ListNode> lnode = ASTProgram.Example(tuple);

                TokenSeq seq = DymTokens(lnode.Item2.List);
                var matches = Spg.ExampleRefactoring.RegularExpression.Regex.Matches(lnode.Item1, seq);

                foreach(var item in matches)
                {
                    if(item.Item2.Length() == 1)
                    {
                        mats.Add(item.Item2.List.Single());
                    }
                    else
                    {
                        SyntaxNodeOrToken parent = LCAManager.GetInstance().LeastCommonAncestor(item.Item2.List, item.Item2.List.First().SyntaxTree).AsNode();
                        mats.Add(parent);
                    }
                }

                treeExamples[input] = mats;
            }
            return DisjunctiveExamplesSpec.From(treeExamples);
        }


        [WitnessFunction("Identifier", 0)]
        public static DisjunctiveExamplesSpec WitnessId(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var IdExamples = new Dictionary<State, IEnumerable<object>>();

            foreach (State input in spec.ProvidedInputs)
            {
                var strings = new List<object>();
                foreach (SyntaxNodeOrToken sot in spec.DisjunctiveExamples[input])
                {
                    if (sot.IsToken && sot.IsKind(SyntaxKind.IdentifierToken))
                    {
                        strings.Add(sot.ToString());
                    }
                    else
                    {
                        return null;
                    }
                }
                IdExamples[input] = strings;
            }

            return DisjunctiveExamplesSpec.From(IdExamples);
        }


        [WitnessFunction("PredefinedType", 0)]
        public static DisjunctiveExamplesSpec WitnessPredefinedTypeId(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var IdExamples = new Dictionary<State, IEnumerable<object>>();

            foreach (State input in spec.ProvidedInputs)
            {
                var strings = new List<object>();
                foreach (SyntaxNodeOrToken sot in spec.DisjunctiveExamples[input])
                {
                    if (sot.Parent.IsKind(SyntaxKind.PredefinedType))
                    {
                        strings.Add(sot.ToString());
                    }
                    else
                    {
                        return null;
                    }
                }
                IdExamples[input] = strings;
            }

            return DisjunctiveExamplesSpec.From(IdExamples);
        }

        [WitnessFunction("NumericLiteralExpression", 0)]
        public static DisjunctiveExamplesSpec WitnessNumericLiteralId(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var IdExamples = new Dictionary<State, IEnumerable<object>>();

            foreach (State input in spec.ProvidedInputs)
            {
                var strings = new List<object>();
                foreach (SyntaxNodeOrToken sot in spec.DisjunctiveExamples[input])
                {
                    if (sot.AsToken() != null && ((SyntaxToken)sot).IsKind(SyntaxKind.NumericLiteralToken))
                    {
                        strings.Add(sot.ToString());
                    }
                    else
                    {
                        return null;
                    }
                }
                IdExamples[input] = strings;
            }

            return DisjunctiveExamplesSpec.From(IdExamples);
        }

        [WitnessFunction("C1", 1)]
        public static DisjunctiveExamplesSpec WitnessC1Kd(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var kdExamples = new Dictionary<State, IEnumerable<object>>();

            foreach (State input in spec.ProvidedInputs)
            {
                var strings = new List<object>();
                foreach (MatchResult mt in spec.DisjunctiveExamples[input])
                {
                    SyntaxNodeOrToken sot = mt.match.Item1;

                    if (sot.IsToken) return null;

                    strings.Add(sot.Kind().ToString());
                }
                kdExamples[input] = strings;
            }

            return DisjunctiveExamplesSpec.From(kdExamples);
        }

        [WitnessFunction("C2", 1)]
        public static DisjunctiveExamplesSpec WitnessC2Kd(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            return WitnessC1Kd(rule, parameter, spec);
        }

        [WitnessFunction("C1", 2, DependsOnParameters = new int[]{1})]
        public static DisjunctiveExamplesSpec WitnessC1Expression1(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, ExampleSpec kind)
        {
            var eExamples = new Dictionary<State, IEnumerable<object>>();

            foreach (State input in spec.ProvidedInputs)
            {
                var v = (SyntaxNodeOrToken)input[rule.Body[0]];

                var x = kind.Examples[input];
                var matches = new List<object>();

                foreach (MatchResult ma in spec.DisjunctiveExamples[input])
                {
                    SyntaxNodeOrToken sot = ma.match.Item1;

                    if (sot.IsToken) return null;

                    Bindings bs = null;

                    var l = sot.ChildNodesAndTokens();

                    List<SyntaxNodeOrToken> lsot = new List<SyntaxNodeOrToken>();
                    foreach (var item in sot.ChildNodesAndTokens())
                    {
                        SyntaxNodeOrToken st = item;
                        if (st.IsNode)
                        {
                            lsot.Add(st);
                        }else if (st.IsToken && st.IsKind(SyntaxKind.IdentifierToken))
                        {
                            lsot.Add(st);
                        }
                    }

                    if (lsot.Count != 1) return null;

                    Tuple<SyntaxNodeOrToken, Bindings> t = Tuple.Create(lsot.First(), bs); 
                    MatchResult m = new MatchResult(t);

                    matches.Add(m);
                }
                eExamples[input] = matches;
            }

            return DisjunctiveExamplesSpec.From(eExamples);
        }

        [WitnessFunction("C2", 2, DependsOnParameters = new int[] { 1 })]
        public static DisjunctiveExamplesSpec WitnessC2Expression1(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, ExampleSpec kind)
        {
            var eExamples = new Dictionary<State, IEnumerable<object>>();

            foreach (State input in spec.ProvidedInputs)
            {
                var v = (SyntaxNodeOrToken)input[rule.Body[0]];

                var x = kind.Examples[input];
                var matches = new List<object>();

                foreach (MatchResult ma in spec.DisjunctiveExamples[input])
                {
                    SyntaxNodeOrToken sot = ma.match.Item1;

                    if (sot.IsToken) return null;

                    Bindings bs = null;

                    var l = sot.ChildNodesAndTokens();

                    List<SyntaxNodeOrToken> lsot = new List<SyntaxNodeOrToken>();
                    foreach (var item in sot.ChildNodesAndTokens())
                    {
                        SyntaxNodeOrToken st = item;
                        if (st.IsNode)
                        {
                            lsot.Add(st);
                        }
                        else if (st.IsToken && st.IsKind(SyntaxKind.IdentifierToken))
                        {
                            lsot.Add(st);
                        }
                    }

                    if (lsot.Count != 2) return null;

                    Tuple<SyntaxNodeOrToken, Bindings> t = Tuple.Create(lsot.First(), bs);
                    MatchResult m = new MatchResult(t);

                    matches.Add(m);
                }
                eExamples[input] = matches;
            }

            return DisjunctiveExamplesSpec.From(eExamples);
        }

        private static TokenSeq DymTokens(List<SyntaxNodeOrToken> list)
        {
            var tokens = new List<Spg.ExampleRefactoring.Tok.Token>();
            foreach (var item in list)
            {
                RawDymToken t = new RawDymToken(item);
                tokens.Add(t);
            }

            TokenSeq seq = new TokenSeq(tokens);
            return seq;
        }

        [WitnessFunction("C2", 3, DependsOnParameters = new int[] { 1, 2 })]
        public static DisjunctiveExamplesSpec WitnessC2Expression2(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, ExampleSpec kind, ExampleSpec expression1)
        {
            var eExamples = new Dictionary<State, IEnumerable<object>>();

            foreach (State input in spec.ProvidedInputs)
            {
                var v = (SyntaxNodeOrToken)input[rule.Body[0]];

                var skind = kind.Examples[input];
                var exp1 = expression1.Examples[input];

                var matches = new List<object>();

                foreach (MatchResult ma in spec.DisjunctiveExamples[input])
                {
                    SyntaxNodeOrToken sot = ma.match.Item1;

                    if (sot.IsToken) return null;

                    Bindings bs = null;

                    var l = sot.ChildNodesAndTokens();

                    List<SyntaxNodeOrToken> lsot = new List<SyntaxNodeOrToken>();
                    foreach (var item in sot.ChildNodesAndTokens())
                    {
                        SyntaxNodeOrToken st = item;
                        if (st.IsNode)
                        {
                            lsot.Add(st);
                        }
                        else if (st.IsToken && st.IsKind(SyntaxKind.IdentifierToken))
                        {
                            lsot.Add(st);
                        }
                    }

                    if (lsot.Count != 2) return null;

                    Tuple<SyntaxNodeOrToken, Bindings> t = Tuple.Create(lsot[1], bs);
                    MatchResult m = new MatchResult(t);

                    matches.Add(m);
                }
                eExamples[input] = matches;
            }

            return DisjunctiveExamplesSpec.From(eExamples);
        }
    }
}
