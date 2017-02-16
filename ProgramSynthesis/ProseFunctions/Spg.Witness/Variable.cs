using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Specifications;
using ProseFunctions.Substrings;
using TreeElement.Spg.Node;

namespace ProseFunctions.Spg.Witness
{
    public class Variable
    {
        public static DisjunctiveExamplesSpec VariableKindDisjunctive(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();
            var @intersect = spec.DisjunctiveExamples.First().Value.Cast<TreeNode<SyntaxNodeOrToken>>().Select(o => o.Value.Kind().ToString());
            foreach (State input in spec.ProvidedInputs)
            {
                var kids = spec.DisjunctiveExamples[input].Cast<TreeNode<SyntaxNodeOrToken>>().Select(o => o.Value.Kind().ToString());
                @intersect = @intersect.Intersect(kids);
            }
            var list = new List<object>();
            @intersect.ForEach(o => list.Add(o));
            list.Add(Token.Expression);

            spec.ProvidedInputs.ForEach(o => treeExamples[o] = list);
            return DisjunctiveExamplesSpec.From(treeExamples);
        }

        public static ExampleSpec VariableKind(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            var first = (TreeNode<SyntaxNodeOrToken>)spec.Examples.First().Value;
            var mats = spec.Examples.Values.Cast<TreeNode<SyntaxNodeOrToken>>();
            //queries
            var isChilNumEqual = mats.All(o => o.Children.Count == mats.First().Children.Count);
            var isTypeEqual = mats.All(o => o.Value.Kind().ToString().Equals(mats.First().Value.Kind().ToString()));
            var hasChildren = mats.First().Children.Count != 0;

            if (isTypeEqual && isChilNumEqual && hasChildren) return null;
            var treeExamples = new Dictionary<State, object>();
            if (!isTypeEqual)
            {
                spec.ProvidedInputs.ForEach(o => treeExamples[o] = Token.Expression);
                return new ExampleSpec(treeExamples);
            }
            spec.ProvidedInputs.ForEach(o => treeExamples[o] = first.Value.Kind().ToString());
            return new ExampleSpec(treeExamples);
        }
    }
}
