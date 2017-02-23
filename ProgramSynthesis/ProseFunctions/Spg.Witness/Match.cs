﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Specifications;
using ProseFunctions.Spg.Bean;
using ProseFunctions.Substrings;
using TreeEdit.Spg.Isomorphic;
using TreeEdit.Spg.TreeEdit.Update;
using TreeElement.Spg.Node;

namespace ProseFunctions.Spg.Witness
{
    public class Match
    {
        /// <summary>
        /// Witness function for the parameter kind of the NewNode specification.
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="spec">Specification</param>
        public static DisjunctiveExamplesSpec CKind(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();
            var @intersect = spec.DisjunctiveExamples.First().Value.Cast<Tuple<TreeNode<SyntaxNodeOrToken>, int>>().Select(o => o.Item1.Value.Kind().ToString());
            foreach (State input in spec.ProvidedInputs)
            {
                var kids = spec.DisjunctiveExamples[input].Cast<Tuple<TreeNode<SyntaxNodeOrToken>, int>>().Select(o => o.Item1.Value.Kind().ToString());
                @intersect = @intersect.Intersect(kids);
            }
            var list = new List<object>();
            @intersect.ForEach(o => list.Add(o));
            spec.ProvidedInputs.ForEach(o => treeExamples[o] = list);
            return DisjunctiveExamplesSpec.From(treeExamples);
        }

        /// <summary>
        /// Witness function for the parameter children of the NewNode specification.
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="spec">Specification</param>
        /// <param name="kind">Label specification</param>
        public static DisjunctiveExamplesSpec CChildren(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, ExampleSpec kind)
        {
            var eExamples = new Dictionary<State, IEnumerable<object>>();
            var dicMat = new Dictionary<State, List<TreeNode<SyntaxNodeOrToken>>>();
            foreach (State input in spec.ProvidedInputs)
            {
                dicMat.Add(input, new List<TreeNode<SyntaxNodeOrToken>>());
                foreach (Tuple<TreeNode<SyntaxNodeOrToken>, int> node in spec.DisjunctiveExamples[input])
                {
                    dicMat[input].Add(node.Item1);
                }
            }
            foreach (State input in spec.ProvidedInputs)
            {
                var matches = new List<List<Tuple<TreeNode<SyntaxNodeOrToken>, int>>>();
                foreach (Tuple<TreeNode<SyntaxNodeOrToken>, int> node in spec.DisjunctiveExamples[input])
                {
                    var sot = node.Item1.Value;
                    if (dicMat.All(o => dicMat[o.Key].Any(e => e.Value.Kind() == sot.Kind() && e.Children.Count == node.Item1.Children.Count)))
                    {
                        if (!node.Item1.Children.Any()) continue;
                        var lsot = node.Item1.Children;
                        var tlsot = lsot.Select(o => Tuple.Create(o, node.Item2)).ToList();
                        matches.Add(tlsot);
                    }
                }
                if (!matches.Any()) return null;
                eExamples[input] = matches;
            }
            return DisjunctiveExamplesSpec.From(eExamples);
        }


        public static DisjunctiveExamplesSpec MatchPattern(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            var eExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var kMatches = new List<TreeNode<SyntaxNodeOrToken>>();
                var target = (TreeNode<SyntaxNodeOrToken>)input[rule.Body[0]];
                foreach (TreeNode<SyntaxNodeOrToken> node in spec.DisjunctiveExamples[input])
                {
                    var list = target.DescendantNodesAndSelf().FindAll(o => IsomorphicManager<SyntaxNodeOrToken>.IsIsomorphic(o, node));
                    if (!list.Any())
                    {
                        return MatchPatternParent(rule, parameter, spec);
                    }
                    if (!list.Any()) continue;
                    kMatches.AddRange(list);
                }
                if (!kMatches.Any()) return null;
                eExamples[input] = kMatches;
            }
            return new DisjunctiveExamplesSpec(eExamples);
        }


        public static DisjunctiveExamplesSpec MatchPatternParent(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            var eExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var kMatches = new List<TreeNode<SyntaxNodeOrToken>>();
                var target = (TreeNode<SyntaxNodeOrToken>)input[rule.Body[0]];
                foreach (TreeNode<SyntaxNodeOrToken> node in spec.DisjunctiveExamples[input])
                {
                    var currentTree = ConverterHelper.ConvertCSharpToTreeNode(target.Value.Parent.Parent);
                    var list = currentTree.DescendantNodesAndSelf().FindAll(o => IsomorphicManager<SyntaxNodeOrToken>.IsIsomorphic(o, node));
                    if (currentTree.DescendantNodesAndSelf().Count > 50) continue;

                    if (!list.Any()) continue;
                    kMatches.AddRange(list);
                }
                if (!kMatches.Any()) return null;
                eExamples[input] = kMatches;
            }
            return new DisjunctiveExamplesSpec(eExamples);
        }

        public static DisjunctiveExamplesSpec MatchK(GrammarRule rule, int parameter, ExampleSpec spec, ExampleSpec kind)
        {
            var kExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var mats = new List<object>();
                var target = (TreeNode<SyntaxNodeOrToken>)input[rule.Body[0]];
                var pattern = (Pattern)kind.Examples[input];
                foreach (TreeNode<SyntaxNodeOrToken> node in spec.DisjunctiveExamples[input])
                {
                    var found = TreeUpdate.FindNode(target, node.Value);
                    if (found == null)
                    {
                        var currentTree = ConverterHelper.ConvertCSharpToTreeNode(target.Value.Parent.Parent);
                        found = TreeUpdate.FindNode(currentTree, node);
                    }
                    K ki = new K(target, found);
                    var k = ki.GetK(pattern);
                    if (k == -K.INF)
                    {
                        k = ki.GetKParent(pattern);
                        if (k == -K.INF) continue;
                        k = k * -1;
                    }
                    mats.Add(k);
                }
                if (!mats.Any()) return null;
                kExamples[input] = mats;
            }
            return DisjunctiveExamplesSpec.From(kExamples);
        }

        public static bool IsEqual(SyntaxNodeOrToken x, SyntaxNodeOrToken y)
        {
            if (!x.IsKind(y.Kind())) return false;
            if (!x.ToString().Equals(y.ToString())) return false;

            return true;
        }
    }
}

