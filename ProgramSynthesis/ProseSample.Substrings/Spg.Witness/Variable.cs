using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Specifications;
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
                var examples = spec.DisjunctiveExamples[input].ToList();
                //var kinds = new List<object>();
                for (int i = 0; i < examples.Count; i++)
                {
                    var node = (TreeNode<SyntaxNodeOrToken>)examples.ElementAt(i);
                    //kinds.Add(node.Value.Kind());

                    if (!dicMats.ContainsKey(i)) dicMats.Add(i, new List<SyntaxKind>());
                    if (!dicChil.ContainsKey(i)) dicChil.Add(i, new List<int>());

                    dicMats[i].Add(node.Value.Kind());
                    dicChil[i].Add(node.Children.Count);
                }
                treeExamples[input] = new List<object>();
            }
            return ConfigureKind(spec, dicMats, dicChil, treeExamples);
        }

        private static DisjunctiveExamplesSpec ConfigureKind(DisjunctiveExamplesSpec spec, Dictionary<int, List<SyntaxKind>> dicMats, Dictionary<int, List<int>> dicChil, Dictionary<State, IEnumerable<object>> treeExamples)
        {
            var isAtLeastOneCorrect = false;
            var exNum = spec.ProvidedInputs.Count();
            foreach (var pair in dicMats)
            {
                if (pair.Value.Count == exNum)
                {
                    var mats = pair.Value;
                    var childrenNums = dicChil[pair.Key];
                    if (!mats.Any()) continue;
                    var isChilNumEqual = childrenNums.All(o => o.Equals(childrenNums.First()));
                    var isTypeEqual = mats.All(o => o.Equals(mats.First()));
                    if (isTypeEqual && isChilNumEqual && childrenNums.First() != 0) continue;

                    if (!isTypeEqual)
                    {
                        foreach (var input in spec.ProvidedInputs)
                        {
                            var examples = (List<object>)treeExamples[input];
                            examples.Add(SyntaxKind.EmptyStatement);
                            //kinds[pair.Key] = SyntaxKind.EmptyStatement;
                            treeExamples[input] = examples;
                        }
                    }
                    else
                    {
                        foreach (var input in spec.ProvidedInputs)
                        {
                            var examples = (List<object>)treeExamples[input];
                            examples.Add(pair.Value.First());
                            treeExamples[input] = examples;
                        }
                    }
                    isAtLeastOneCorrect = true;
                }
            }
            if (!isAtLeastOneCorrect) return null;

            return DisjunctiveExamplesSpec.From(treeExamples);
        }
    }
}
