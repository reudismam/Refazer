using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Specifications;
using ProseFunctions.Substrings;
using TreeEdit.Spg.Isomorphic;
using TreeElement.Spg.Node;

namespace ProseFunctions.Spg.Witness
{
    public class AST
    {
        /// <summary>
        /// Witness function to compute the kind of the node.
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">parameter</param>
        /// <param name="spec">Specification</param>
        public static DisjunctiveExamplesSpec NodeKind(GrammarRule rule, DisjunctiveExamplesSpec spec)
        {
            var kExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var kMatches = new List<object>();
                foreach (TreeNode<SyntaxNodeOrToken> sot in spec.DisjunctiveExamples[input])
                {
                    if (!ConverterHelper.Valid(sot.Value)) return null;
                    if (!sot.Children.Any()) return null;
                    kMatches.Add(sot.Value.Kind());
                }
                kExamples[input] = kMatches;
            }
            return DisjunctiveExamplesSpec.From(kExamples);
        }

        /// <summary>
        /// Witness function to return the specification for the children of a node.
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">parameter</param>
        /// <param name="spec">Specification</param>
        /// <param name="kind">Label specification</param>
        public static DisjunctiveExamplesSpec NodeChildren(GrammarRule rule, DisjunctiveExamplesSpec spec, ExampleSpec kind)
        {
            var eExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var matches = new List<object>();
                foreach (TreeNode<SyntaxNodeOrToken> sot in spec.DisjunctiveExamples[input])
                {
                    if (!ConverterHelper.Valid(sot.Value)) return null;
                    var lsot = sot.Children;
                    lsot.ForEach(o => o.SyntaxTree = sot.SyntaxTree);
                    matches.Add(lsot);
                }
                eExamples[input] = matches;
            }
            return DisjunctiveExamplesSpec.From(eExamples);
        }

        /// <summary>
        /// Return a specification for the ConstNode witness function.
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">parameter</param>
        /// <param name="spec">Specification</param>
        public static ExampleSpec Const(GrammarRule rule, ExampleSpec spec)
        {
            var treeExamples = new Dictionary<State, object>();
            var mats = new List<TreeNode<SyntaxNodeOrToken>>();
            foreach (State input in spec.ProvidedInputs)
            {
                foreach (TreeNode<SyntaxNodeOrToken> sot in spec.DisjunctiveExamples[input])
                {
                    if (sot.Children.Any()) return null;
                    mats.Add(sot);

                    var first = mats.First();
                    if (!IsomorphicManager<SyntaxNodeOrToken>.IsIsomorphic(first, sot)) return null;
                }
                treeExamples[input] = mats.First().Value;
            }
            return new ExampleSpec(treeExamples);
        }
    }
}
