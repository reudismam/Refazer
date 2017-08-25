using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Specifications;
using RefazerFunctions.Spg.Config;
using TreeElement.Spg.Node;
using TreeElement.Token;

namespace RefazerFunctions.Spg.Witness
{
    public class Variable
    {
        /// <summary>
        /// Defines the back-propagation function of the kind parameter of the Abstract operator.
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="spec">Specification</param>
        [SuppressMessage("ReSharper", "LoopCanBeConvertedToQuery")]
        [SuppressMessage("ReSharper", "ImplicitlyCapturedClosure")]
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

        /// <summary>
        /// Defines the back-propagation function of the kind parameter of the Abstract operator.
        /// </summary>
        /// <param name="rule">Grammar</param>
        /// <param name="spec">Specification for Abstract operator</param>
        public static DisjunctiveExamplesSpec VariableKind(GrammarRule rule, ExampleSpec spec)
        {
            var matches = spec.Examples.Values.Cast<Tuple<TreeNode<SyntaxNodeOrToken>, int>>().ToList();
            var first = matches.First().Item1;
            //queries       
            var isTypeEqual = matches.All(o => o.Item1.Value.Kind().ToString().Equals(first.Value.Kind().ToString()));
            if (SynthesisConfig.GetInstance().BoundGeneratedPrograms)
            {
                var isChildrenNumberEquals = matches.All(o => o.Item1.Children.Count == first.Children.Count);
                var hasChildren = first.Children.Any();
                if (isTypeEqual && isChildrenNumberEquals && hasChildren) return null;
            }
            var treeExamples = new Dictionary<State, IEnumerable<object>>();
            if (!isTypeEqual)
            {
                spec.ProvidedInputs.ForEach(o => treeExamples[o] = new List<object> { Token.Expression });
                return new DisjunctiveExamplesSpec(treeExamples);
            }
            spec.ProvidedInputs.ForEach(o => treeExamples[o] = new List<object> { first.Value.Kind().ToString()});
            if (!SynthesisConfig.GetInstance().BoundGeneratedPrograms)
            {
                spec.ProvidedInputs.ForEach(o => ((List<object>) treeExamples[o]).Add(Token.Expression));
            }
            return new DisjunctiveExamplesSpec(treeExamples);
        }
    }
}
