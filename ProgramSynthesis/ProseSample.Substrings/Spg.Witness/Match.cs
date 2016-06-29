using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Specifications;
using TreeElement.Spg.Node;

namespace ProseSample.Substrings.Spg.Witness
{
    public class Match
    {
        public static DisjunctiveExamplesSpec TreeKindRef(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            return spec;
        }

        public static ExampleSpec CKind(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var pattern = MatchPattern(rule, parameter, spec);
            var kdExamples = new Dictionary<State, object>();
            foreach (State input in spec.ProvidedInputs)
            {
                var syntaxKinds = new List<SyntaxKind>();
                foreach (Node node in spec.DisjunctiveExamples[input])
                {
                    var sot = node.Value;
                    if (sot.Value.IsToken) return null;

                    syntaxKinds.Add(sot.Value.Kind());
                    if (!sot.Value.Kind().Equals(syntaxKinds.First())) return null;
                }
                kdExamples[input] = syntaxKinds.First();
            }
            return new ExampleSpec(kdExamples);
        }

        public static DisjunctiveExamplesSpec CChildren(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, ExampleSpec kind)
        {
            var eExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var matches = new List<List<Node>>();
                foreach (Node node in spec.DisjunctiveExamples[input])
                {
                    var sot = node.Value;
                    if (sot.Value.IsToken) return null;
                    if (!sot.Children.Any()) return null;

                    var lsot = ExtractChildren(sot);
                    var childList = lsot.Select(item => new Node(item)).ToList();
                    childList.ForEach(item => item.Value.SyntaxTree = sot.SyntaxTree);
                    matches.Add(childList);
                }
                eExamples[input] = matches;
            }
            return DisjunctiveExamplesSpec.From(eExamples);
        }

        /// <summary>
        /// Extract relevant child
        /// </summary>
        /// <param name="parent">Parent</param>
        /// <returns>Relevant child</returns>
        private static List<ITreeNode<SyntaxNodeOrToken>> ExtractChildren(ITreeNode<SyntaxNodeOrToken> parent)
        {
            var lsot = new List<ITreeNode<SyntaxNodeOrToken>>();
            foreach (var item in parent.Children)
            {
                ITreeNode<SyntaxNodeOrToken> st = item;
                if (st.Value.IsNode)
                {
                    lsot.Add(st);
                }
                else if (st.Value.IsToken && st.Value.IsKind(SyntaxKind.IdentifierToken))
                {
                    lsot.Add(st);
                }
            }
            return lsot;
        }

        public static DisjunctiveExamplesSpec MatchPattern(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var eExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var matches = new List<ITreeNode<SyntaxNodeOrToken>>();
                foreach (Node node in spec.DisjunctiveExamples[input])
                {
                    var sot = node.Value;
                    matches.Add(sot);
                }

                var pattern = BuildPattern(ConverterHelper.ConvertITreeNodeToToken(matches.First()), ConverterHelper.ConvertITreeNodeToToken(matches.Last()));
                eExamples[input] = new List<ITreeNode<Token>> {pattern.Tree};
            }
            return DisjunctiveExamplesSpec.From(eExamples);
        }

        private static Pattern BuildPattern(ITreeNode<Token> t1, ITreeNode<Token> t2)
        {
            var token = new Token(t1.Value.Kind);
            var itreeNode = new TreeNode<Token>(token, new TLabel(token.Kind));
            Pattern pattern = new Pattern(itreeNode);
            if (t1.Value.Kind != t2.Value.Kind) return null;
            if (!t1.Children.Any() || !t2.Children.Any())
            {
                if (t1.Children.Any() && !t2.Children.Any() && t1 is DynToken && t2 is DynToken)
                {
                    if (t1.ToString().Equals(t2.ToString()))
                    {
                        var dtoken = new DynToken(t1.Value.Kind, ((DynToken) t1.Value).Value);
                        var ditreeNode = new TreeNode<Token>(dtoken, new TLabel(dtoken.Kind));
                        return new Pattern(ditreeNode);
                    }
                }
                else
                {
                    return pattern;
                }
            }


            if (t1.Children.Count == t2.Children.Count)
            {
                for (int j = 0; j < t1.Children.Count; j++)
                {
                    Pattern patternChild = BuildPattern(t1.Children.ElementAt(j), t2.Children.ElementAt(j));
                    if (patternChild == null) return null;
                    pattern.Tree.AddChild(patternChild.Tree, j);
                }
            }
            return pattern;
        }

        //private static List<Pattern> BuildPattern(List<ITreeNode<SyntaxNodeOrToken>> values)
        //{
        //    if (!values.Any()) return null;

        //    var patterns = new List<Pattern>();
        //    bool childrenEmpty = values.All(v => v.Children.Any());        
        //    if (childrenEmpty)
        //    {
        //        if (values.All(v => v.Value.IsKind(values.First().Value.Kind())))
        //        {
        //            var token = new Token(values.First().Value.Kind());
        //            var itreeNode = new TreeNode<Token>(token, new TLabel(token.Kind));
        //            patterns.Add(new Pattern(itreeNode));

        //            if (values.All(v => v.Value.ToString().Equals(values.First().Value.ToString())))
        //            {
        //                var dtoken = new DynToken(values.First().Value.Kind(), values.First().Value);
        //                var ditreeNode = new TreeNode<Token>(dtoken, new TLabel(token.Kind));
        //                patterns.Add(new Pattern(ditreeNode));
        //            }
        //        }

        //        return patterns;
        //    }

        //    if (values.All(v => v.Children.Count == values.First().Children.Count))
        //    {
        //        var children = values.First().Children.Select(i => new List<ITreeNode<SyntaxNodeOrToken>>()).ToList();

        //        foreach (var i in values)
        //        {
        //            for (int j = 0; j < i.Children.Count; j++)
        //            {
        //                children.ElementAt(j).Add(i.Children[j]);
        //            }
        //        }

        //        foreach (var v in children)
        //        {
        //            List<Pattern> patternList = BuildPattern(v);
        //            if (patternList == null) return null;


        //        }
        //    }
        //    return null;
        //}

        public static DisjunctiveExamplesSpec MatchK(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, DisjunctiveExamplesSpec kind)
        {
            var kExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var v = (Node)input[rule.Body[0]];
                var rr = (Pattern)kind.DisjunctiveExamples[input];
                var ks = new List<object>();
                //foreach (uint pos in spec.DisjunctiveExamples[input])
                //{
                //    var ms = rr.Item1.Run(v).Where(m => rr.Item2.MatchesAt(v, m.Right)).ToArray();
                //    int index = ms.BinarySearchBy(m => m.Right.CompareTo(pos));
                //    if (index < 0) return null;
                //    ks.Add(index + 1);
                //    ks.Add(index - ms.Length);
                //}
                kExamples[input] = ks;
            }
            return DisjunctiveExamplesSpec.From(kExamples);
        }
    }
}
