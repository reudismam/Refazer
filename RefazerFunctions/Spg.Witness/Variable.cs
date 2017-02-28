using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Specifications;
using TreeElement.Spg.Node;
using TreeElement.Token;

namespace ProseFunctions.Spg.Witness
{
    public class Variable
    {
        public static DisjunctiveExamplesSpec VariableKindDisjunctive(GrammarRule rule, DisjunctiveExamplesSpec spec)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();
            var @intersect = spec.DisjunctiveExamples.First().Value.Cast<Tuple<TreeNode<SyntaxNodeOrToken>, int>>().Select(o => o.Item1.Value.Kind().ToString());
            foreach (State input in spec.ProvidedInputs)
            {
                var kids = spec.DisjunctiveExamples[input].Cast<Tuple<TreeNode<SyntaxNodeOrToken>, int>>().Select(o => o.Item1.Value.Kind().ToString());
                @intersect = @intersect.Intersect(kids);
            }
            var list = new List<object>();
            @intersect.ForEach(o => list.Add(o));
            list.Add(Token.Expression);

            spec.ProvidedInputs.ForEach(o => treeExamples[o] = list);
            return DisjunctiveExamplesSpec.From(treeExamples);
        }

        public static ExampleSpec VariableKind(GrammarRule rule, ExampleSpec spec)
        {
            var first = (Tuple<TreeNode<SyntaxNodeOrToken>, int>)spec.Examples.First().Value;
            var mats = spec.Examples.Values.Cast<Tuple<TreeNode<SyntaxNodeOrToken>, int>>();
            //queries
            var isChilNumEqual = mats.All(o => o.Item1.Children.Count == mats.First().Item1.Children.Count);
            var isTypeEqual = mats.All(o => o.Item1.Value.Kind().ToString().Equals(mats.First().Item1.Value.Kind().ToString()));
            var hasChildren = mats.First().Item1.Children.Count != 0;

            if (isTypeEqual && isChilNumEqual && hasChildren) return null;
            var treeExamples = new Dictionary<State, object>();
            if (!isTypeEqual)
            {
                spec.ProvidedInputs.ForEach(o => treeExamples[o] = Token.Expression);
                return new ExampleSpec(treeExamples);
            }
            spec.ProvidedInputs.ForEach(o => treeExamples[o] = first.Item1.Value.Kind().ToString());
            return new ExampleSpec(treeExamples);
        }
    }
}
