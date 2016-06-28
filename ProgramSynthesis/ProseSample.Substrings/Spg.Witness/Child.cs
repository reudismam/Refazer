using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Specifications;
using TreeEdit.Spg.TreeEdit.Update;

namespace ProseSample.Substrings.Spg.Witness
{
    public class Child
    {
        public static DisjunctiveExamplesSpec ParentVariable(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var mats = new List<object>();
                var key = input[rule.Body[0]];
                var inpTree = WitnessFunctions.GetCurrentTree(key);
                foreach (Node node in spec.DisjunctiveExamples[input])
                {
                    var sot = node.Value;
                    var treeNode = TreeUpdate.FindNode(inpTree, sot.Value);

                    if (sot.Value.IsToken || treeNode != null) return null;

                    var child = inpTree.Children.Single();

                    var result = new Node(child);
                    mats.Add(result);
                }
                treeExamples[input] = mats.GetRange(0, 1);
            }
            return DisjunctiveExamplesSpec.From(treeExamples);
        }
    }
}
