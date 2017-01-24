using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Specifications;
using ProseFunctions.Spg.Bean;
using ProseFunctions.Substrings;
using TreeEdit.Spg.Isomorphic;
using TreeEdit.Spg.TreeEdit.Update;
using TreeElement.Spg.Node;

namespace ProseFunctions.Spg.Witness
{
    public class Match
    {
        /// <summary>
        /// Witness function for the parameter kind of the NewNode specification.
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="spec">Specification</param>
        public static DisjunctiveExamplesSpec CKind(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var kdExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var syntaxKinds = new List<object>();
                foreach (TreeNode<SyntaxNodeOrToken> node in spec.DisjunctiveExamples[input])
                {
                    var sot = node.Value;
                    syntaxKinds.Add(sot.Kind());
                }
                kdExamples[input] = syntaxKinds;
            }
            return DisjunctiveExamplesSpec.From(kdExamples);
        }

        /// <summary>
        /// Witness function for the parameter children of the NewNode specification.
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="spec">Specification</param>
        /// <param name="kind">Kind specification</param>
        public static DisjunctiveExamplesSpec CChildren(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, ExampleSpec kind)
        {
            var eExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var matches = new List<List<TreeNode<SyntaxNodeOrToken>>>();
                foreach (TreeNode<SyntaxNodeOrToken> node in spec.DisjunctiveExamples[input])
                {
                    if (!node.Children.Any()) continue;
                    var lsot = node.Children;
                    matches.Add(lsot);
                }
                if (!matches.Any()) return null;
                eExamples[input] = matches;
            }
            return DisjunctiveExamplesSpec.From(eExamples);
        }

        public static DisjunctiveExamplesSpec MatchPattern(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            var eExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var kMatches = new List<TreeNode<SyntaxNodeOrToken>>();
                var target = (TreeNode<SyntaxNodeOrToken>)input[rule.Body[0]];
                foreach (TreeNode<SyntaxNodeOrToken> node in spec.DisjunctiveExamples[input])
                {
                    //var currentTree = GetAnchorTree(target, node);
                    var found = TreeUpdate.FindNode(target, node.Value);
                    if (found == null)
                    {
                        var currentTree = ConverterHelper.ConvertCSharpToTreeNode(target.Value.Parent.Parent);
                        found = TreeUpdate.FindNode(currentTree, node);
                        if (currentTree.DescendantNodesAndSelf().Count() > 50) continue;
                    }
                    if (found == null) continue;
                    if (!IsomorphicManager<SyntaxNodeOrToken>.IsIsomorphic(found, node)) continue;
                    kMatches.Add(found);
                }
                if (!kMatches.Any()) return null;
                eExamples[input] = kMatches;
            }
            return new DisjunctiveExamplesSpec(eExamples);
        }    

        //public static DisjunctiveExamplesSpec MatchK(GrammarRule rule, int parameter, ExampleSpec spec, ExampleSpec kind)
        //{
        //    var kExamples = new Dictionary<State, IEnumerable<object>>();
        //    foreach (State input in spec.ProvidedInputs)
        //    {
        //        var mats = new List<object>();
        //        var patternExample = (Pattern)kind.Examples[input];
        //        var pattern = patternExample.Tree;
        //        foreach (TreeNode<SyntaxNodeOrToken> node in spec.DisjunctiveExamples[input])
        //        {
        //            var currentTree = WitnessFunctions.GetCurrentTree(node.SyntaxTree);
        //            var matches = MatchManager.Matches(currentTree, pattern);

        //            for (int i = 0; i < matches.Count; i++)
        //            {
        //                var match = matches[i];
        //                var compare = Semantics.FindChild(match, patternExample.K);
        //                if (compare != null && IsEqual(compare.Value, node.Value))
        //                {
        //                    mats.Add(i + 1);
        //                }
        //            }
        //        }
        //        if (!mats.Any()) return null;
        //        kExamples[input] = mats;
        //    }
        //    return DisjunctiveExamplesSpec.From(kExamples);
        //}

        public static DisjunctiveExamplesSpec MatchK(GrammarRule rule, int parameter, ExampleSpec spec, ExampleSpec kind)
        {
            var kExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var mats = new List<object>();
                var target = (TreeNode<SyntaxNodeOrToken>)input[rule.Body[0]];
                var pattern = (Pattern)kind.Examples[input];
                foreach (TreeNode<SyntaxNodeOrToken> node in spec.DisjunctiveExamples[input])
                {
                    var found = TreeUpdate.FindNode(target, node.Value);
                    if (found == null)
                    {
                        var currentTree = ConverterHelper.ConvertCSharpToTreeNode(target.Value.Parent.Parent);
                        found = TreeUpdate.FindNode(currentTree, node);
                    }
                    K ki = new K(target, found);
                    var k = ki.GetK(pattern);
                    if (k == -K.INF)
                    {
                        k = ki.GetKParent(pattern);
                        if (k == -K.INF) continue;
                        k = k*-1;
                    }
                    mats.Add(k);
                }
                kExamples[input] = mats;
            }
            return DisjunctiveExamplesSpec.From(kExamples);
        }

        /// <summary>
        /// Get anchor node
        /// </summary>
        /// <param name="target">Current target</param>
        /// <param name="node">Node</param>
        private static TreeNode<SyntaxNodeOrToken> GetAnchorTree(TreeNode<SyntaxNodeOrToken> target, TreeNode<SyntaxNodeOrToken> node)
        {
            var found = TreeUpdate.FindNode(target, node.Value);
            if (found == null)
            {
                var currentTree = ConverterHelper.ConvertCSharpToTreeNode(target.Value.Parent.Parent);
                return currentTree;
            }
            return target;
        }

        public static bool IsEqual(SyntaxNodeOrToken x, SyntaxNodeOrToken y)
        {
            if (!x.IsKind(y.Kind())) return false;
            if (!x.ToString().Equals(y.ToString())) return false;

            return true;
        }
    }
}

