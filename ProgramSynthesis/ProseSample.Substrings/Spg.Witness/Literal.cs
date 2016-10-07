using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Specifications;
using TreeEdit.Spg.Isomorphic;
using TreeElement.Spg.Node;

namespace ProseSample.Substrings.Spg.Witness
{
    public class Literal
    {
        public static DisjunctiveExamplesSpec LiteralTree(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();
            var dicMats = new Dictionary<int, List<TreeNode<SyntaxNodeOrToken>>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var examples = spec.DisjunctiveExamples[input].ToList();
                for (int i = 0; i < examples.Count; i++)
                {
                    var sot = (TreeNode<SyntaxNodeOrToken>)examples.ElementAt(i);
                    if (sot.Children.Any())
                    {
                        if (!dicMats.ContainsKey(i)) dicMats.Add(i, new List<TreeNode<SyntaxNodeOrToken>>());
                        dicMats[i].Add(sot);
                    }
                }
                treeExamples[input] = new List<object>();
            }

            var exNum = spec.ProvidedInputs.Count();
            var isOneIncluded = false;
            foreach (var pair in dicMats)
            {
                if (pair.Value.First().Children.Any()) continue;
                if (!pair.Value.Any()) continue;
                if (pair.Value.Count == exNum)
                {
                    var first = pair.Value.First();
                    if (!pair.Value.All(sot => IsomorphicManager<SyntaxNodeOrToken>.IsIsomorphic(first, sot))) continue;
                    foreach (State input in spec.ProvidedInputs)
                    {
                        var examples = (List<object>)treeExamples[input];
                        examples.Add(pair.Value.First().Value);
                        treeExamples[input] = examples;
                    }
                    isOneIncluded = true;
                }
            }
            if (!isOneIncluded) return null;
            return DisjunctiveExamplesSpec.From(treeExamples);
        }

        //public static ExampleSpec LiteralTree(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        //{
        //    var treeExamples = new Dictionary<State, object>();
        //    var mats = new List<TreeNode<SyntaxNodeOrToken>>();
        //    foreach (State input in spec.ProvidedInputs)
        //    {
        //        foreach (TreeNode<SyntaxNodeOrToken> sot in spec.DisjunctiveExamples[input])
        //        {
        //            if (sot.Children.Any()) return null;
        //            mats.Add(sot);

        //            var first = mats.First();
        //            if (!IsomorphicManager<SyntaxNodeOrToken>.IsIsomorphic(first, sot)) return null;
        //        }
        //        treeExamples[input] = mats.First().Value;
        //    }
        //    return new ExampleSpec(treeExamples);
        //}
    }
}
