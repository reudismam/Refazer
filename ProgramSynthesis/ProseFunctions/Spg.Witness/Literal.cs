using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Specifications;
using TreeEdit.Spg.Isomorphic;
using TreeElement.Spg.Node;

namespace ProseFunctions.Spg.Witness
{
    public class Literal
    {
        public static DisjunctiveExamplesSpec LiteralTree(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();
            var examplesDisjunction = new Dictionary<int, List<TreeNode<SyntaxNodeOrToken>>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var examples = spec.DisjunctiveExamples[input].ToList();
                for (int i = 0; i < examples.Count; i++)
                {
                    var sot = (TreeNode<SyntaxNodeOrToken>)examples.ElementAt(i);
                    if (!sot.Children.Any())
                    {
                        if (!examplesDisjunction.ContainsKey(i)) examplesDisjunction.Add(i, new List<TreeNode<SyntaxNodeOrToken>>());
                        examplesDisjunction[i].Add(sot);
                    }
                }
                treeExamples[input] = new List<object>();
            }

            var exampleNumber = spec.ProvidedInputs.Count();
            var containValidDisjunction = false;
            foreach (var pair in examplesDisjunction)
            { 
                if (!pair.Value.Any()) continue;
                if (pair.Value.Count == exampleNumber)
                {
                    var first = pair.Value.First();
                    if (!pair.Value.All(sot => IsomorphicManager<SyntaxNodeOrToken>.IsIsomorphic(first, sot))) continue;
                    foreach (State input in spec.ProvidedInputs)
                    {
                        var examples = (List<object>)treeExamples[input];
                        examples.Add(pair.Value.First().Value);
                        treeExamples[input] = examples;
                    }
                    containValidDisjunction = true;
                }
            }
            if (!containValidDisjunction) return null;
            return DisjunctiveExamplesSpec.From(treeExamples);
        }
    }
}
