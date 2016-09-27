using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Specifications;
using ProseSample.Substrings;
using TreeElement.Spg.Node;

namespace ProseSample.Substrings.Spg.Witness
{
    public class Variable
    {
        public static ExampleSpec VariableKind(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var treeExamples = new Dictionary<State, object>();
            foreach (State input in spec.ProvidedInputs)
            {
                var mats = new List<SyntaxKind>();
                foreach (TreeNode<SyntaxNodeOrToken> node in spec.DisjunctiveExamples[input])
                {
                    //if (node.Children.Any()) continue;
                    mats.Add(node.Value.Kind());
                }
                if (!mats.Any()) return null;
                treeExamples[input] = mats.First();
            }
            return new ExampleSpec(treeExamples);
        }

        public static ExampleSpec LeafKind(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var treeExamples = new Dictionary<State, object>();
            foreach (State input in spec.ProvidedInputs)
            {
                var mats = new List<SyntaxKind>();
                foreach (TreeNode<Token> node in spec.DisjunctiveExamples[input])
                {
                    if (!(node.Value is LeafToken)) continue;
                    if (node.Children.Any()) continue;
                    mats.Add(node.Value.Kind);
                }
                if (!mats.Any()) return null;
                treeExamples[input] = mats.First();
            }
            return new ExampleSpec(treeExamples);
        }
    }
}
