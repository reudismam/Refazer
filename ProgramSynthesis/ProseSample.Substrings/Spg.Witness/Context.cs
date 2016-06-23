using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Specifications;
using TreeElement.Spg.Node;

namespace ProseSample.Substrings.Spg.Witness
{
    public abstract class Context
    {
        public DisjunctiveExamplesSpec ParentVariable(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var mats = new List<object>();
                foreach (var sot in from Node node in spec.DisjunctiveExamples[input] select node.Value)
                {
                    var target = Target(sot);
                    if (sot.Value.IsToken || target == null) return null;
                    mats.Add(new Node(target));
                }
                treeExamples[input] = mats;
            }
            return DisjunctiveExamplesSpec.From(treeExamples);
        }

        public DisjunctiveExamplesSpec ParentK(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, DisjunctiveExamplesSpec kindBinding)
        {
            var kExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var matches = new List<object>();
                //var key = input[rule.Body[0]];
                //var inpTree = WitnessFunctions.GetCurrentTree(key);
                foreach (Node node in spec.DisjunctiveExamples[input])
                {
                    var sot = node.Value;
                    var target = Target(sot);
                    if (target == null) return null;
                    var parent = target;//TreeUpdate.FindNode(inpTree, target);

                    if (sot.Value.IsToken || parent == null) return null;

                    var children = parent.Children;

                    for (int i = 0; i < children.Count; i++)
                    {
                        var item = children.ElementAt(i);
                        if (item.Equals(sot))
                        {
                            matches.Add(i + 1);
                        }
                    }
                }

                kExamples[input] = matches;
                var value = kExamples.Values;

                if (value.Any(sequence => !sequence.SequenceEqual(value.First()))) return null;

            }
            return DisjunctiveExamplesSpec.From(kExamples);
        }

        public abstract ITreeNode<SyntaxNodeOrToken> Target(ITreeNode<SyntaxNodeOrToken> sot);
    }
}
