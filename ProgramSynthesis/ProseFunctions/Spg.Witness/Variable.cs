using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Specifications;
using TreeElement.Spg.Node;

namespace ProseFunctions.Spg.Witness
{
    public class Variable
    {
        public static DisjunctiveExamplesSpec VariableKindD(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();
            var dicMat = new Dictionary<State, List<TreeNode<SyntaxNodeOrToken>>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var examples = spec.DisjunctiveExamples[input].ToList();
                dicMat.Add(input, new List<TreeNode<SyntaxNodeOrToken>>());
                for (int i = 0; i < examples.Count; i++)
                {
                    var node = (TreeNode<SyntaxNodeOrToken>) examples.ElementAt(i);
                    dicMat[input].Add(node);
                }
            }

            foreach (State input in spec.ProvidedInputs)
            {
                var matches = new List<object>();
                var examples = spec.DisjunctiveExamples[input].ToList();
                for (int i = 0; i < examples.Count; i++)
                {                 
                    var node = (TreeNode<SyntaxNodeOrToken>)examples.ElementAt(i);
                    var sot = node.Value;
                    var kind = node.Value.Kind();

                    if (dicMat.All(o => dicMat[o.Key].Any(e => e.Value.Kind() == sot.Kind() && e.Children.Count != node.Children.Count)))
                    {
                        matches.Add(kind);
                    }

                    if (dicMat.All(o => dicMat[o.Key].Any(e => e.Value.Kind() == sot.Kind() && !node.Children.Any() && e.Children.Count == node.Children.Count)))
                    {
                        matches.Add(kind);
                    }
                }
                if (!matches.Any())
                {
                    for (int i = 0; i < examples.Count; i++)
                    {
                        var node = (TreeNode<SyntaxNodeOrToken>)examples.ElementAt(i);
                        var sot = node.Value;

                        if (dicMat.Any(o => dicMat[o.Key].All(e => e.Value.Kind() != sot.Kind())))
                        {
                            matches.Add(SyntaxKind.EmptyStatement);
                        }
                    }
                }
                if (!matches.Any())
                {
                    var result = Match.CChildren(rule, parameter, spec, null);
                    return null;
                }
                treeExamples[input] = matches.Distinct(o => o).ToList();
            }

            spec.ProvidedInputs.ForEach(o => treeExamples[o] = treeExamples.Values.First());
            return DisjunctiveExamplesSpec.From(treeExamples);
        }

        public static DisjunctiveExamplesSpec VariableKind(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();
            var dicMats = new Dictionary<int, List<TreeNode<SyntaxNodeOrToken>>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var examples = spec.DisjunctiveExamples[input].ToList();
                for (int i = 0; i < examples.Count; i++)
                {
                    var node = (TreeNode<SyntaxNodeOrToken>)examples.ElementAt(i);
                    if (!dicMats.ContainsKey(i))
                    {
                        dicMats.Add(i, new List<TreeNode<SyntaxNodeOrToken>>());
                    }
                    dicMats[i].Add(node);
                }
                treeExamples[input] = new List<object>();
            }
            return ConfigureKind(spec, dicMats, treeExamples);
        }

        private static DisjunctiveExamplesSpec ConfigureKind(DisjunctiveExamplesSpec spec, Dictionary<int, List<TreeNode<SyntaxNodeOrToken>>> dicMats, Dictionary<State, IEnumerable<object>> treeExamples)
        {
            var isAtLeastOneCorrect = false;
            var exNum = spec.ProvidedInputs.Count();
            foreach (var pair in dicMats)
            {
                if (pair.Value.Count == exNum)
                {
                    var mats = pair.Value;
                    if (!mats.Any()) continue;
                    var isChilNumEqual = mats.All(o => o.Children.Count == mats.First().Children.Count);
                    var isTypeEqual = mats.All(o => o.IsLabel(mats.First().Label));
                    var hasChildren = mats.First().Children.Count != 0;
                    if (isTypeEqual && isChilNumEqual && hasChildren) continue;

                    if (!isTypeEqual)
                    {
                        foreach (var input in spec.ProvidedInputs)
                        {
                            var examples = (List<object>)treeExamples[input];
                            examples.Add(SyntaxKind.EmptyStatement);
                            treeExamples[input] = examples;
                        }
                    }
                    else
                    {
                        foreach (var input in spec.ProvidedInputs)
                        {
                            var examples = (List<object>)treeExamples[input];
                            examples.Add(pair.Value.First().Value.Kind());
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
