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
        //public static DisjunctiveExamplesSpec LiteralK(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, DisjunctiveExamplesSpec treeBinding)
        //{
        //    var treeExamples = new Dictionary<State, IEnumerable<object>>();
        //    foreach (var input in spec.ProvidedInputs)
        //    {
        //        var mats = new List<object>();
        //        var key = (Node) input[rule.Body[0]];
        //        var inpTree = WitnessFunctions.GetCurrentTree(key.Value);
        //        foreach (Node node in spec.DisjunctiveExamples[input])
        //        {
        //            var sot = node.Value;
        //            if (sot.Value.IsToken || sot.Children.Any()) return null;

        //            var matches = MatchManager.ConcreteMatches(inpTree, sot.Value);

        //            for (int i = 0; i < matches.Count; i++)
        //            {
        //                var item = matches[i].Value;
        //                if (item.Span.Contains(sot.Value.Span) && sot.Value.Span.Contains(item.Span))
        //                {
        //                    mats.Add(i + 1);
        //                }
        //            }
        //        }
        //        treeExamples[input] = mats;
        //    }
        //    var values = treeExamples.Values;
        //    return values.Any(sequence => !sequence.SequenceEqual(values.First())) ? null : DisjunctiveExamplesSpec.From(treeExamples);
        //}


        public static ExampleSpec LiteralTree(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var treeExamples = new Dictionary<State, object>();
            var literalExamples = new List<ITreeNode<SyntaxNodeOrToken>>();
            foreach (var input in spec.ProvidedInputs)
            {
                //var key = input[rule.Body[0]];
                //var inpTree = WitnessFunctions.GetCurrentTree(key);

                foreach (Node node in spec.DisjunctiveExamples[input])
                {
                    var sot = node.Value;
                    if (sot.Value.IsToken || sot.Children.Any()) return null;

                    //var matches = MatchManager.ConcreteMatches(inpTree, sot.Value);

                    //if (!matches.Any()) return null;

                    //literalExamples.Add(matches.First());
                    literalExamples.Add(sot);

                    var first = literalExamples.First();
                    if (!IsomorphicManager<SyntaxNodeOrToken>.IsIsomorphic(sot, first)) return null;
                }

                treeExamples[input] = literalExamples.First().Value;
            }
            return new ExampleSpec(treeExamples);
        }
    }
}
