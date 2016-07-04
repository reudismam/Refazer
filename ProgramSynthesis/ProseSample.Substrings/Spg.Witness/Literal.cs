using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Specifications;
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
            var literalExamples = new List<SyntaxNodeOrToken>();
            foreach (var input in spec.ProvidedInputs)
            {
                //var key = input[rule.Body[0]];
                //var inpTree = WitnessFunctions.GetCurrentTree(key);

                foreach (ITreeNode<Token> node in spec.DisjunctiveExamples[input])
                {
                    //var sot = node.Value;
                    if (node.Children.Any()) return null;

                    if (!(node.Value is DynToken)) return null;

                    var dyn = (DynToken) node.Value;

                    //var matches = MatchManager.ConcreteMatches(inpTree, sot.Value);

                    //if (!matches.Any()) return null;

                    //literalExamples.Add(matches.First());
                    literalExamples.Add(dyn.Value);

                    //var first = literalExamples.First();
                    //if (!IsomorphicManager<SyntaxNodeOrToken>.IsIsomorphic(sot, first)) return null;
                }

                treeExamples[input] = literalExamples.First();
            }
            return new ExampleSpec(treeExamples);
        }
    }
}
