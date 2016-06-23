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
                var matches = new List<object>();
                foreach (Node node in spec.DisjunctiveExamples[input])
                {
                    var sot = node.Value;
                    if (sot.Value.IsToken) return null;

                    if (!sot.Children.Any()) return null;
                    if (!sot.Children.Any()) return null;

                    var lsot = ExtractChildren(sot);

                    var childList = lsot.Select(item => new Node(item)).ToList();

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
                var matches = new List<object>();
                foreach (Node node in spec.DisjunctiveExamples[input])
                {
                    var sot = node.Value;
                    if (sot.Value.IsToken) return null;
                    if (!sot.Children.Any()) return null;
                    var lsot = ExtractChildren(sot);
                    var childList = lsot.Select(item => new Node(item)).ToList();
                    matches.Add(childList);
                }
                eExamples[input] = matches;
            }
            return DisjunctiveExamplesSpec.From(eExamples);
        }

        public static DisjunctiveExamplesSpec MatchK(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, ExampleSpec kind)
        {
            throw new NotImplementedException();
        }
    }
}
