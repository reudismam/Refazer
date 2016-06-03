using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Specifications;
using TreeEdit.Spg.TreeEdit.Update;

namespace ProseSample.Substrings.Spg.Witness
{
    public class Parent
    {
        public static DisjunctiveExamplesSpec ParentVariable(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var mats = new List<object>();
                var key = input[rule.Body[0]];
                var inpTree = WitnessFunctions.GetCurrentTree(key);
                foreach (var sot in from MatchResult matchResult in spec.DisjunctiveExamples[input] select matchResult.Match.Item1)
                {
                    var parent = TreeUpdate.FindNode(inpTree, sot.Value);
                    if (sot.Value.IsToken || parent == null) return null;

                    var result = new MatchResult(Tuple.Create(parent, new Bindings(new List<SyntaxNodeOrToken> { parent.Value })));
                    mats.Add(result);
                }
                treeExamples[input] = mats.GetRange(0, 1);
            }
            return DisjunctiveExamplesSpec.From(treeExamples);
        }

        public static DisjunctiveExamplesSpec ParentK(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, ExampleSpec kindBinding)
        {
            var kExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var matches = new List<object>();
                foreach (MatchResult matchResult in spec.DisjunctiveExamples[input])
                {
                    var sot = matchResult.Match.Item1;
                    var parent = sot.Parent;

                    if (sot.Value.IsToken || parent == null) return null;

                    var children = parent.Children;

                    for (int i = 0; i < children.Count; i++)
                    {
                        var item = children.ElementAt(i);
                        if (item.Equals(sot))
                        {
                            matches.Add(i + 1);
                        }
                    }
                }

                kExamples[input] = matches;
                var value = kExamples.Values;

                if (value.Any(sequence => !sequence.SequenceEqual(value.First()))) return null;

            }
            return DisjunctiveExamplesSpec.From(kExamples);
        }
    }
}
