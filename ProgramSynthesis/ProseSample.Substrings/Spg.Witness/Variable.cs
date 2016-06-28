using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Specifications;
using TreeEdit.Spg.Match;

namespace ProseSample.Substrings.Spg.Witness
{
    public class Variable
    {
        public static ExampleSpec VariableKind(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var treeExamples = new Dictionary<State, object>();
            foreach (State input in spec.ProvidedInputs)
            {
                var mats = new List<SyntaxKind>();
                foreach (Node node in spec.DisjunctiveExamples[input])
                {
                    var sot = node.Value;
                    var syntaxTree = WitnessFunctions.GetCurrentTree(sot.SyntaxTree);
                    var matches = MatchManager.AbstractMatches(syntaxTree, sot.Value.Kind());

                    if (sot.Children.Any() && matches.Count > 1) continue;
                    if (!matches.Any()) continue;

                    mats.Add(matches.First().Kind());
                }

                if (!mats.Any()) return null;

                if (mats.Any(v => !v.Equals(mats.First()))) return null;
                treeExamples[input] = mats.First();
            }

            return new ExampleSpec(treeExamples);
        }

        public static DisjunctiveExamplesSpec VariableK(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, ExampleSpec kindBinding)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var mats = new List<object>();
                var key = (Node) input[rule.Body[0]];
                var inpTree = WitnessFunctions.GetCurrentTree(key.Value);
                foreach (Node node in spec.DisjunctiveExamples[input])
                {
                    var sot = node.Value;

                    var kind = (SyntaxKind) kindBinding.Examples[input];
                    var matches = MatchManager.AbstractMatches(inpTree, kind);

                    for (int i = 0; i < matches.Count; i++)
                    {
                        var item = matches[i];
                        if (item.Equals(sot.Value))
                        {
                            mats.Add(i + 1);
                        }
                    }
                    if (mats.Count > 1) return null;
                }

                treeExamples[input] = mats;
            }
            var values = treeExamples.Values;
            return values.Any(sequence => !sequence.SequenceEqual(values.First())) ? null : DisjunctiveExamplesSpec.From(treeExamples);
        }
    }
}
