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
using TreeEdit.Spg.TreeEdit.Update;
using Tutor.Spg.Node;

namespace ProseSample.Substrings.Spg.Witness
{
    public class Match
    {
        public static DisjunctiveExamplesSpec TreeKindRef(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (var input in spec.ProvidedInputs)
            {
                var matches = spec.DisjunctiveExamples[input].Cast<MatchResult>().Cast<object>().ToList();
                treeExamples[input] = matches;
            }
            return DisjunctiveExamplesSpec.From(treeExamples);
        }

        public static DisjunctiveExamplesSpec CKind(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var kdExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var syntaxKinds = new List<object>();
                foreach (MatchResult mt in spec.DisjunctiveExamples[input])
                {
                    var sot = mt.Match.Item1;
                    if (sot.Value.IsToken) return null;

                    syntaxKinds.Add(sot.Value.Kind());
                }
                kdExamples[input] = syntaxKinds;
            }
            return DisjunctiveExamplesSpec.From(kdExamples);
        }


        public static DisjunctiveExamplesSpec CChildren(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, ExampleSpec kind)
        {
            var eExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var matches = new List<object>();
                foreach (MatchResult matchResult in spec.DisjunctiveExamples[input])
                {
                    var sot = matchResult.Match.Item1;
                    if (sot.Value.IsToken) return null;

                    var key = input[rule.Body[0]];
                    var currentTree = WitnessFunctions.GetCurrentTree(key);
                    var snode = TreeUpdate.FindNode(currentTree, sot.Value);
                    if (snode == null || !snode.Children.Any()) return null;

                    var lsot = ExtractChildren(snode);

                    var childList = new List<MatchResult>();
                    foreach (var item in lsot)
                    {
                        var binding = matchResult.Match.Item2;
                        binding.bindings.Add(item.Value);

                        MatchResult m = new MatchResult(Tuple.Create(item, binding));
                        childList.Add(m);
                    }

                    matches.Add(childList);
                }
                eExamples[input] = matches;
            }
            return DisjunctiveExamplesSpec.From(eExamples);
        }

        /// <summary>
        /// Extract relevant child
        /// </summary>
        /// <param name="parent">Parent</param>
        /// <returns>Relevant child</returns>
        private static List<ITreeNode<SyntaxNodeOrToken>> ExtractChildren(ITreeNode<SyntaxNodeOrToken> parent)
        {
            var lsot = new List<ITreeNode<SyntaxNodeOrToken>>();
            foreach (var item in parent.Children)
            {
                ITreeNode<SyntaxNodeOrToken> st = item;
                if (st.Value.IsNode)
                {
                    lsot.Add(st);
                }
                else if (st.Value.IsToken && st.Value.IsKind(SyntaxKind.IdentifierToken))
                {
                    lsot.Add(st);
                }
            }
            return lsot;
        }

        


    }
}
