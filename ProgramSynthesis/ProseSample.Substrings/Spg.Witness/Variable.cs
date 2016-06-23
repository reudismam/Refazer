using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Specifications;
using TreeEdit.Spg.Match;
using TreeEdit.Spg.Print;

namespace ProseSample.Substrings.Spg.Witness
{
    public class Variable
    {
        public static DisjunctiveExamplesSpec VariableKind(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            //var treeExamples = new Dictionary<State, IEnumerable<object>>();
            //foreach (State input in spec.ProvidedInputs)
            //{
            //    var mats = new List<object>();
            //    //var key = input[rule.Body[0]];
            //    //var inpTree = WitnessFunctions.GetCurrentTree(key);
            //    foreach (MatchResult matchResult in spec.DisjunctiveExamples[input])
            //    {
            //        var sot = matchResult.Match.Item1;
            //        var matches = MatchManager.AbstractMatches(inpTree, sot.Value.Kind());

            //        foreach (var item in matches.Where(item => item.ToString().Equals(sot.ToString())))
            //        {
            //            mats.Add(item.Kind());
            //            if (!mats.First().Equals(item.Kind())) return null;
            //        }

            //        if (!mats.Any()) return null;
            //    }
            //    treeExamples[input] = mats.GetRange(0, 1);
            //}

            //var values = treeExamples.Values;
            //return values.Any(sequence => !sequence.SequenceEqual(values.First())) ? null : DisjunctiveExamplesSpec.From(treeExamples);
            return Match.CKind(rule, parameter, spec);
        }


        public static DisjunctiveExamplesSpec VariableK(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, ExampleSpec kindBinding)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var mats = new List<object>();
                var key = input[rule.Body[0]];
                var inpTree = WitnessFunctions.GetCurrentTree(key);
                //PrintUtil<SyntaxNodeOrToken>.PrintPretty(inpTree, "", true);
                foreach (MatchResult matchResult in spec.DisjunctiveExamples[input])
                {
                    var sot = matchResult.Match.Item1;

                    var kind = (SyntaxKind)kindBinding.Examples[input];
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
