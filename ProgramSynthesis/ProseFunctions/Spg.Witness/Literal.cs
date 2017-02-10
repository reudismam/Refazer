using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Specifications;
using TreeEdit.Spg.Isomorphic;
using TreeElement.Spg.Node;

namespace ProseFunctions.Spg.Witness
{
    public class Literal
    {
        public static DisjunctiveExamplesSpec LiteralTreeDisjunctive(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();
            var @intersect = spec.DisjunctiveExamples.First().Value.Cast<TreeNode<SyntaxNodeOrToken>>();
            var comparer = new LiteralCompater();
            foreach (State input in spec.ProvidedInputs)
            {
                var kids = spec.DisjunctiveExamples[input].Cast<TreeNode<SyntaxNodeOrToken>>();
                @intersect = @intersect.Intersect(kids, comparer);
            }
            var list = new List<object>();
            @intersect = @intersect.Where(o => !o.Children.Any());
            if (!@intersect.Any()) return null;

            @intersect.ForEach(o => list.Add(o.Value));
            spec.ProvidedInputs.ForEach(o => treeExamples[o] = list);
            return DisjunctiveExamplesSpec.From(treeExamples);
        }

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

        public class LiteralCompater : IEqualityComparer<TreeNode<SyntaxNodeOrToken>>
        {
            public bool Equals(TreeNode<SyntaxNodeOrToken> x, TreeNode<SyntaxNodeOrToken> y)
            {
                return IsomorphicManager<SyntaxNodeOrToken>.IsIsomorphic(x, y);
            }

            public int GetHashCode(TreeNode<SyntaxNodeOrToken> x)
            {
                return x.Value.GetHashCode();
            }
        }
    }
}
