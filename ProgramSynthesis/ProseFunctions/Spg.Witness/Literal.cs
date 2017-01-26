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
            var matches = new List<TreeNode<SyntaxNodeOrToken>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var examples = spec.DisjunctiveExamples[input].ToList();
                var mats = new List<object>();
                for (int i = 0; i < examples.Count; i++)
                {
                    var sot = (TreeNode<SyntaxNodeOrToken>)examples.ElementAt(i);
                    if (!sot.Children.Any())
                    {
                        matches.Add(sot);
                        mats.Add(sot);
                    }
                }
                if (!mats.Any()) return null;
            }
            var first = matches.First();
            if (!matches.All(sot => IsomorphicManager<SyntaxNodeOrToken>.IsIsomorphic(first, sot))) return null;
            spec.ProvidedInputs.ForEach(input => treeExamples[input] = new List<object> {first.Value});
            return DisjunctiveExamplesSpec.From(treeExamples);
        }
    }
}
