using System;
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
using Tutor.Spg.Node;

namespace ProseSample.Substrings.Spg.Witness
{
    public class Template
    {
        public static DisjunctiveExamplesSpec PKind(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var kdExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var syntaxKinds = new List<object>();
                foreach (List<ITreeNode<SyntaxNodeOrToken>> mt in spec.DisjunctiveExamples[input])
                {
                    syntaxKinds.Add(mt.First().Value.Kind());
                }
                kdExamples[input] = syntaxKinds.GetRange(0, 1);
            }
            return DisjunctiveExamplesSpec.From(kdExamples);
        }


        public static DisjunctiveExamplesSpec PChildren(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, ExampleSpec kind)
        {
            var eExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var matches = new List<object>();

                foreach (List<ITreeNode<SyntaxNodeOrToken>> matchResultList in spec.DisjunctiveExamples[input])
                {
                    var firstExample = matchResultList.First();

                    var children = firstExample.Children.Select(v => new List<ITreeNode<SyntaxNodeOrToken>>()).ToList();

                    foreach (var matchResult in matchResultList)
                    {
                        var sot = matchResult;
                        if (sot.Value.IsToken || !sot.Children.Any()) return null;

                        for (int i = 0; i < sot.Children.Count; i++)
                        {
                            var item = sot.Children[i];
                            var m = item;
                            children.ElementAt(i).Add(m);
                        }
                    }

                    matches.Add(children);
                }

                eExamples[input] = matches;
            }
            return DisjunctiveExamplesSpec.From(eExamples);
        }

        public static DisjunctiveExamplesSpec ConcreteTree(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();

            foreach (var input in spec.ProvidedInputs)
            {
                var literalExamples = new List<object>();
                foreach (List<ITreeNode<SyntaxNodeOrToken>> treeNodes in spec.DisjunctiveExamples[input])
                {
                    var tree = treeNodes.First();

                    foreach (var sot in treeNodes.Select(matchResult => matchResult))
                    {
                        if (sot.Value.IsToken || sot.Children.Any()) return null;

                        if (!IsomorphicManager<SyntaxNodeOrToken>.IsIsomorphic(tree, sot)) return null;
                    }
                    literalExamples.Add(tree.Value);
                }
                treeExamples[input] = literalExamples;
            }
            return DisjunctiveExamplesSpec.From(treeExamples);
        }

        public static DisjunctiveExamplesSpec AbstractKind(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            var kindExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (var input in spec.ProvidedInputs)
            {
                var matches = new List<object>();
                foreach (List<ITreeNode<SyntaxNodeOrToken>> treeNodes in spec.DisjunctiveExamples[input])
                {
                    var first = treeNodes.First().Value;
                    var kind = first.Kind();
                    if (treeNodes.Any(t => !t.Value.IsKind(kind) || t.Children.Any())) return null;

                    if (treeNodes.All(matchResult => matchResult.Value.ToString().Equals(first.ToString()))) return null;

                    matches.Add(kind);
                }
                kindExamples[input] = matches;
            }
            return DisjunctiveExamplesSpec.From(kindExamples);
        }
    }
}
