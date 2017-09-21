using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Specifications;
using TreeEdit.Spg.Isomorphic;
using TreeElement.Spg.Node;

namespace RefazerFunctions.Spg.Witness
{
    public class Literal
    {
        public static DisjunctiveExamplesSpec LiteralTreeDisjunctive(GrammarRule rule, DisjunctiveExamplesSpec spec)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();
            var @intersect = spec.DisjunctiveExamples.First().Value.Cast<Tuple<TreeNode<SyntaxNodeOrToken>, int>>();
            var comparer = new LiteralComparator();
            foreach (State input in spec.ProvidedInputs)
            {
                var kinds = spec.DisjunctiveExamples[input].Cast<Tuple<TreeNode<SyntaxNodeOrToken>, int>>();
                @intersect = @intersect.Intersect(kinds, comparer);
            }
            var list = new List<object>();
            if (!@intersect.Any()) return null;
            @intersect.ForEach(o => list.Add(o.Item1.Value));
            spec.ProvidedInputs.ForEach(o => treeExamples[o] = list);
            return DisjunctiveExamplesSpec.From(treeExamples);
        }

        public static DisjunctiveExamplesSpec LiteralTree(GrammarRule rule, DisjunctiveExamplesSpec spec)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();
            var matches = new List<TreeNode<SyntaxNodeOrToken>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var mats = new List<object>();
                foreach(Tuple<TreeNode<SyntaxNodeOrToken>, int> tsot in spec.DisjunctiveExamples[input].ToList())
                {
                    var sot = tsot.Item1;
                    matches.Add(sot);
                    mats.Add(sot);
                }
                if (!mats.Any()) return null;
            }
            var first = matches.First();
            if (!matches.All(sot => IsomorphicManager<SyntaxNodeOrToken>.IsIsomorphic(first, sot))) return null;
            spec.ProvidedInputs.ForEach(input => treeExamples[input] = new List<object> {first.Value});
            return DisjunctiveExamplesSpec.From(treeExamples);
        }

        public class LiteralComparator : IEqualityComparer<Tuple<TreeNode<SyntaxNodeOrToken>, int>>
        {
            public bool Equals(Tuple<TreeNode<SyntaxNodeOrToken>, int> x, Tuple<TreeNode<SyntaxNodeOrToken>, int> y)
            {
                bool isIsomorphic = IsomorphicManager<SyntaxNodeOrToken>.IsIsomorphic(x.Item1, y.Item1);
                bool sameLevel = x.Item2 == y.Item2;
                return isIsomorphic && sameLevel;
            }

            public int GetHashCode(Tuple<TreeNode<SyntaxNodeOrToken>, int> x)
            {
                return x.Item1.ToString().GetHashCode();
            }
        }
    }
}
