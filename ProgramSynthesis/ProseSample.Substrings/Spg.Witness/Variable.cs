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
        public static DisjunctiveExamplesSpec VariableKind(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();
            var dicMats = new Dictionary<int, List<SyntaxKind>>();
            var dicChil = new Dictionary<int, List<int>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var defaultValue = default(SyntaxKind);
                SyntaxKind kind = defaultValue;
                var examples = spec.DisjunctiveExamples[input].ToList();
                var kinds = new List<object>();
                for (int i = 0; i < examples.Count(); i++)
                {
                    var node = (TreeNode<SyntaxNodeOrToken>) examples.ElementAt(i);
                    kind = node.Value.Kind();
                    kinds.Add(kind);
                    if (!dicMats.ContainsKey(i)) dicMats.Add(i, new List<SyntaxKind>());
                    if (!dicChil.ContainsKey(i)) dicChil.Add(i, new List<int>());
                    dicMats[i].Add(kind);
                    dicChil[i].Add(node.Children.Count);
                }
                if (kind == defaultValue) return null;
                treeExamples[input] = kinds;
            }

            foreach (var pair in dicMats)
            {
                var mats = pair.Value;
                var childrenNums = dicChil[pair.Key];
                if (!mats.Any()) continue;
                var isChilNumEqual = childrenNums.All(o => o.Equals(childrenNums.First()));
                var isTypeEqual = mats.All(o => o.Equals(mats.First()));
                if (isTypeEqual && isChilNumEqual) continue;

                if (!isTypeEqual)
                {
                    foreach (var input in spec.ProvidedInputs)
                    {
                        var kinds = (List<object>) treeExamples[input];
                        kinds[pair.Key] = SyntaxKind.EmptyStatement;
                        treeExamples[input] = kinds;
                    }
                }
            }
            return DisjunctiveExamplesSpec.From(treeExamples);
        }
    }
}
