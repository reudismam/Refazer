using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Specifications;
using TreeEdit.Spg.Isomorphic;
using TreeEdit.Spg.TreeEdit.Update;
using TreeElement.Spg.Node;

namespace ProseSample.Substrings.Spg.Witness
{
    public class AST
    {
        public static DisjunctiveExamplesSpec NodeKind(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var kExamples = new Dictionary<State, IEnumerable<object>>();

            foreach (State input in spec.ProvidedInputs)
            {
                var kMatches = new List<object>();
                foreach (ITreeNode<SyntaxNodeOrToken> sot in spec.DisjunctiveExamples[input])
                {
                    if (!ConverterHelper.Valid(sot.Value)) return null;
                    if (!sot.Children.Any()) return null;
                    kMatches.Add(sot.Value.Kind());
                }
                kExamples[input] = kMatches;
            }

            return DisjunctiveExamplesSpec.From(kExamples);
        }

        public static DisjunctiveExamplesSpec NodeChildren(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, ExampleSpec kind)
        {
            var eExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var matches = new List<object>();

                foreach (ITreeNode<SyntaxNodeOrToken> sot in spec.DisjunctiveExamples[input])
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

        public static ExampleSpec Const(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            var treeExamples = new Dictionary<State, object>();
            var mats = new List<ITreeNode<SyntaxNodeOrToken>>();
            foreach (State input in spec.ProvidedInputs)
            {
                foreach (ITreeNode<SyntaxNodeOrToken> sot in spec.DisjunctiveExamples[input])
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

        public static DisjunctiveExamplesSpec Ref(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();
            //Todo Bug correct the witness function Ref
            foreach (State input in spec.ProvidedInputs)
            {
                var target = (Node) input[rule.Body[0]];
                var currentTree = WitnessFunctions.GetCurrentTree(target.Value);
                var mats = new List<object>();
                foreach (ITreeNode<SyntaxNodeOrToken> sot in spec.DisjunctiveExamples[input])
                {
                    if (!ConverterHelper.Valid(sot.Value)) return null;
                    //var subTree = ConverterHelper.ConvertCSharpToTreeNode(sot.Value);
                    var node = TreeUpdate.FindNode(currentTree, sot.Value);//IsomorphicManager<SyntaxNodeOrToken>.FindIsomorphicSubTree(inpTree.Value, subTree);
                    if (node == null) return null;
                    var result = new Node(sot);
                    mats.Add(result);
                }
                treeExamples[input] = mats;
            }
            return DisjunctiveExamplesSpec.From(treeExamples);
        }
    }
}
