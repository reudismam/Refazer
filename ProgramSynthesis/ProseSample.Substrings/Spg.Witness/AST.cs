﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Specifications;
using TreeEdit.Spg.Isomorphic;
using TreeElement.Spg.Node;

namespace ProseSample.Substrings.Spg.Witness
{
    public class AST
    {
        public static DisjunctiveExamplesSpec NodeKind(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var kExamples = new Dictionary<State, IEnumerable<object>>();

            foreach (State input in spec.ProvidedInputs)
            {
                var kMatches = new List<object>();
                foreach (ITreeNode<SyntaxNodeOrToken> sot in spec.DisjunctiveExamples[input])
                {
                    if (sot.Value.IsToken) return null;
                    if (!sot.Children.Any()) return null;
                    kMatches.Add(sot.Value.Kind());
                }
                kExamples[input] = kMatches;
            }

            return DisjunctiveExamplesSpec.From(kExamples);
        }

        public static DisjunctiveExamplesSpec NodeChildren(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, ExampleSpec kind)
        {
            var eExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var matches = new List<object>();

                foreach (ITreeNode<SyntaxNodeOrToken> sot in spec.DisjunctiveExamples[input])
                {
                    if (sot.Value.IsToken) return null;
                    var lsot = sot.Children;
                    matches.Add(lsot);
                }
                eExamples[input] = matches;
            }
            return DisjunctiveExamplesSpec.From(eExamples);
        }   

        public static DisjunctiveExamplesSpec Const(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();

            var mats = new List<object>();
            foreach (State input in spec.ProvidedInputs)
            {
                foreach (ITreeNode<SyntaxNodeOrToken> sot in spec.DisjunctiveExamples[input])
                {
                    if (sot.Children.Any()) return null;
                    mats.Add(sot.Value);
                }

                treeExamples[input] = mats.GetRange(0, 1); //We do not need to pass more than a constant.
            }
            return DisjunctiveExamplesSpec.From(treeExamples);
        }

        public static DisjunctiveExamplesSpec Ref(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();

            foreach (State input in spec.ProvidedInputs)
            {
                var key = input[rule.Body[0]];
                var inpTree = WitnessFunctions.GetCurrentTree(key);
                var mats = new List<object>();
                foreach (ITreeNode<SyntaxNodeOrToken> sot in spec.DisjunctiveExamples[input])
                {
                    if (!sot.Value.IsNode) return null;

                    var subTree = ConverterHelper.ConvertCSharpToTreeNode(sot.Value);
                    var node = IsomorphicManager<SyntaxNodeOrToken>.FindIsomorphicSubTree(inpTree, subTree);

                    if (node == null) return null;

                    var result = new Node(sot);

                    mats.Add(result);
                }

                treeExamples[input] = mats;
            }
            return DisjunctiveExamplesSpec.From(treeExamples);
        }
    }
}
