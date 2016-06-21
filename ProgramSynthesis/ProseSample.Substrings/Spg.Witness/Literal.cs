using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Specifications;
using TreeEdit.Spg.Isomorphic;
using TreeEdit.Spg.Match;
using TreeElement.Spg.Node;

namespace ProseSample.Substrings.Spg.Witness
{
    public class Literal
    {
        public static DisjunctiveExamplesSpec LiteralK(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, ExampleSpec treeBinding)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (var input in spec.ProvidedInputs)
            {
                var mats = new List<object>();
                var key = input[rule.Body[0]];
                var inpTree = WitnessFunctions.GetCurrentTree(key);

                foreach (MatchResult matchResult in spec.DisjunctiveExamples[input])
                {
                    var sot = matchResult.Match.Item1;
                    if (sot.Value.IsToken || sot.Children.Any()) return null;

                    var matches = MatchManager.ConcreteMatches(inpTree, sot.Value);

                    for (int i = 0; i < matches.Count; i++)
                    {
                        var item = matches[i].Value;
                        if (item.Span.Contains(sot.Value.Span) && sot.Value.Span.Contains(item.Span))
                        {
                            mats.Add(i + 1);
                        }
                    }
                }

                treeExamples[input] = mats;
            }
            var values = treeExamples.Values;
            return values.Any(sequence => !sequence.SequenceEqual(values.First())) ? null : DisjunctiveExamplesSpec.From(treeExamples);
        }


        public static DisjunctiveExamplesSpec LiteralTree(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();
            var literalExamples = new List<object>();
            foreach (var input in spec.ProvidedInputs)
            {
                var key = input[rule.Body[0]];
                var inpTree = WitnessFunctions.GetCurrentTree(key);

                foreach (MatchResult matchResult in spec.DisjunctiveExamples[input])
                {
                    var sot = matchResult.Match.Item1;
                    if (sot.Value.IsToken || sot.Children.Any()) return null;

                    var matches = MatchManager.ConcreteMatches(inpTree, sot.Value);

                    if (!matches.Any()) return null;

                    //literalExamples.Add(matches.First());
                    literalExamples.Add(sot);

                    var first = (ITreeNode<SyntaxNodeOrToken>)literalExamples.First();
                    if (!IsomorphicManager<SyntaxNodeOrToken>.IsIsomorphic(sot, first)) return null;
                }

                var examples = (from ITreeNode<SyntaxNodeOrToken> v in literalExamples.GetRange(0, 1) select v.Value).Cast<object>().ToList();

                treeExamples[input] = examples;
            }
            return DisjunctiveExamplesSpec.From(treeExamples);
        }
    }
}
