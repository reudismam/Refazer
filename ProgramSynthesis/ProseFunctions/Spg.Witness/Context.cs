using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Specifications;
using ProseFunctions.Spg.Bean;
using ProseFunctions.Substrings;
using TreeEdit.Spg.Match;
using TreeEdit.Spg.TreeEdit.Update;
using TreeElement.Spg.Node;

namespace ProseFunctions.Spg.Witness
{
    public class Context
    {

        /// <summary>
        /// Specification for the parent attribute of the Context operator.
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">parameter</param>
        /// <param name="spec">Specification</param>
        public DisjunctiveExamplesSpec ParentVariable(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var inputTree = (Node)input[rule.Grammar.InputSymbol];
                var mats = new List<TreeNode<SyntaxNodeOrToken>>();
                foreach (TreeNode<SyntaxNodeOrToken> node in spec.DisjunctiveExamples[input])
                {
                    var t1Node = TreeUpdate.FindNode(inputTree.Value, node.Value);
                    var parentT1Node = t1Node?.Parent;
                    if (parentT1Node == null) continue;

                    var parentParent = parentT1Node.Parent;
                    if (parentParent != null)
                    {
                        AnalyseParent(parentParent, t1Node, mats);
                    }

                    //AnalyseParent(parentT1Node, t1Node, mats);
                }
                if (!mats.Any()) return null;
                treeExamples[input] = mats;
            }
            return new DisjunctiveExamplesSpec(treeExamples);
        }

        /// <summary>
        /// Specification for the parent attribute of the Context operator.
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">parameter</param>
        /// <param name="spec">Specification</param>
        public DisjunctiveExamplesSpec ParentVariableP(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var inputTree = (Node)input[rule.Grammar.InputSymbol];
                var mats = new List<TreeNode<SyntaxNodeOrToken>>();
                foreach (TreeNode<SyntaxNodeOrToken> node in spec.DisjunctiveExamples[input])
                {
                    var t1Node = TreeUpdate.FindNode(inputTree.Value, node.Value);
                    var parentT1Node = t1Node?.Parent;
                    if (parentT1Node == null) continue;

                    //var parentParent = parentT1Node.Parent;
                    //if (parentParent != null)
                    //{
                    //    AnalyseParent(parentParent, t1Node, mats);
                    //}

                    AnalyseParent(parentT1Node, t1Node, mats);
                }
                if (!mats.Any()) return null;
                treeExamples[input] = mats;
            }
            return new DisjunctiveExamplesSpec(treeExamples);
        }

        /// <summary>
        /// Specification for the parent attribute of the Context operator.
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">parameter</param>
        /// <param name="spec">Specification</param>
        public DisjunctiveExamplesSpec ParentVariablePPP(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var inputTree = (Node)input[rule.Grammar.InputSymbol];
                var mats = new List<TreeNode<SyntaxNodeOrToken>>();
                foreach (TreeNode<SyntaxNodeOrToken> node in spec.DisjunctiveExamples[input])
                {
                    var t1Node = TreeUpdate.FindNode(inputTree.Value, node.Value);
                    var parentT1Node = t1Node?.Parent;
                    if (parentT1Node == null) continue;

                    var parentParent = parentT1Node.Parent;
                    if (parentParent == null) continue;

                    var parentParentParent = parentParent.Parent;

                    //var parentParent = parentT1Node.Parent;
                    if (parentParentParent != null)
                    {
                        AnalyseParent(parentParentParent, t1Node, mats);
                    }

                    //AnalyseParent(parentT1Node, t1Node, mats);
                }
                if (!mats.Any()) return null;
                treeExamples[input] = mats;
            }
            return new DisjunctiveExamplesSpec(treeExamples);
        }

        private static void AnalyseParent(TreeNode<SyntaxNodeOrToken> parentT1Node, TreeNode<SyntaxNodeOrToken> t1Node, List<TreeNode<SyntaxNodeOrToken>> mats)
        {
            int tolerance = 100;
            var parentDescendants = parentT1Node.DescendantNodesAndSelf();
            if (parentDescendants.Count < tolerance)
            {
                if (t1Node.Value.AsNode() == null)
                {
                    if (parentT1Node.Parent != null)
                    {
                        var descendantsAndSelf = parentT1Node.Parent.DescendantNodesAndSelf();
                        if (descendantsAndSelf.Count < tolerance)
                        {
                            mats.Add(parentT1Node.Parent);
                        }
                    }
                }
                else
                {
                    mats.Add(parentT1Node);
                }
            }
        }

        ///// <summary>
        ///// Specification for the parent attribute of the Context operator.
        ///// </summary>
        ///// <param name="rule">Grammar rule</param>
        ///// <param name="parameter">parameter</param>
        ///// <param name="spec">Specification</param>
        //public DisjunctiveExamplesSpec ParentVariable(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        //{
        //    var treeExamples = new Dictionary<State, IEnumerable<object>>();
        //    var examplesDisjunction = new Dictionary<int, List<Tuple<State, TreeNode<SyntaxNodeOrToken>>>>();
        //    Enumerable.Range(0, 2).ForEach(o => examplesDisjunction.Add(o, new List<Tuple<State, TreeNode<SyntaxNodeOrToken>>>()));
        //    foreach (State input in spec.ProvidedInputs)
        //    {
        //        var inputTree = (Node)input[rule.Grammar.InputSymbol];
        //        foreach(TreeNode<SyntaxNodeOrToken> node in spec.DisjunctiveExamples[input])
        //        {                   
        //            var t1Node = TreeUpdate.FindNode(inputTree.Value, node.Value);
        //            var parentT1Node = t1Node?.Parent;
        //            if (parentT1Node?.DescendantNodesAndSelf().Count < 40)
        //            {
        //                if (node.Value.AsNode() == null)
        //                {
        //                    if (parentT1Node.Parent != null)
        //                    {
        //                        var descendantsAndSelf = parentT1Node.Parent.DescendantNodesAndSelf();
        //                        if (descendantsAndSelf.Count < 40)
        //                        {
        //                            examplesDisjunction[0].Add(Tuple.Create(input, parentT1Node.Parent));
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    examplesDisjunction[0].Add(Tuple.Create(input, parentT1Node.Parent));
        //                }
        //            }
        //            //examplesDisjunction[1].Add(Tuple.Create(input, node));
        //        }
        //        treeExamples[input] = new List<TreeNode<SyntaxNodeOrToken>>(); ;
        //    }

        //    var exampleNumber = spec.ProvidedInputs.Count();
        //    foreach (var key in examplesDisjunction.Keys)
        //    {
        //        var value = examplesDisjunction[key];
        //        if (!value.Any()) continue;
        //        if (value.Count == exampleNumber)
        //        { 
        //            foreach (State input in spec.ProvidedInputs)
        //            {
        //                var examples = (List<TreeNode<SyntaxNodeOrToken>>)treeExamples[input];
        //                examples.Add(value.Find(o => o.Item1.Equals(input)).Item2);
        //                treeExamples[input] = examples;
        //            }
        //        }
        //    }
        //    return new DisjunctiveExamplesSpec(treeExamples);
        //}

        /// <summary>
        /// Find the index of the child in the parent node.
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">Rule parameter</param>
        /// <param name="spec">Example specification</param>
        /// <param name="kind">Parent binding</param>
        public ExampleSpec ParentK(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, ExampleSpec kind)
        {
            var kExamples = new Dictionary<State, object>();
            var matches = new List<object>();
            foreach (State input in spec.ProvidedInputs)
            {
                var inputTree = (Node)input[rule.Grammar.InputSymbol];
                var parent = (Pattern) kind.Examples[input];
                //If the pattern is Empty then return
                if (parent.Tree.Value.Kind == SyntaxKind.EmptyStatement) return null;

                foreach(TreeNode<SyntaxNodeOrToken> node in spec.DisjunctiveExamples[input])
                {
                    var t1Node = TreeUpdate.FindNode(inputTree.Value, node.Value);
                    if (t1Node == null) continue;
                    var path = GetPath(t1Node, parent.Tree);
                    if (path == null) continue;
                    matches.Add(path);
                }
                if (!matches.Any()) return null;    
                if (matches.Any(sequence => !sequence.Equals(matches.First()))) return null;
                kExamples[input] = matches.First();
            }
            return new ExampleSpec(kExamples);
        }

        /// <summary>
        /// Build an XPath expression for the target node. To build this XPath, 
        /// we keep get the parent while the parent is null and build an XPath 
        /// from the last parent until the target node.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="parent"></param>
        /// <returns>XPath</returns>
        public static string GetPath(TreeNode<SyntaxNodeOrToken> target, TreeNode<SyntaxNodeOrToken> parent)
        {
            string path = "";
            for (TreeNode<SyntaxNodeOrToken> node = target; !node.Equals(parent); node = node.Parent)
            {
                string append = "/";

                if (node.Parent != null && node.Parent.Children.Count >= 1)
                {
                    append += "[";

                    int index = 1;
                    var previousSibling = PreviousSibling(node);
                    while (previousSibling != null)
                    {
                        index++;
                        previousSibling = PreviousSibling(previousSibling);
                    }

                    append += $"{index}]";
                    path = append + path;
                }
            }
            return path;
        }

        /// <summary>
        /// Build an XPath expression for the target node. To build this XPath, 
        /// we keep get the parent while the parent is null and build an XPath 
        /// from the last parent until the target node.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="parent"></param>
        /// <returns>XPath</returns>
        public static string GetPath(TreeNode<SyntaxNodeOrToken> target, TreeNode<Token> parent)
        {
            string path = "";
            TreeNode<SyntaxNodeOrToken> node;
            for (node = target; node != null && node.Value != null && !MatchManager.IsValueEachChild(node, parent); node = node.Parent)
            {
                string append = "/";
                if (node.Parent != null && node.Parent.Children.Count >= 1)
                {
                    append += "[";
                    int index = 1;
                    var previousSibling = PreviousSibling(node);
                    while (previousSibling != null)
                    {
                        index++;
                        previousSibling = PreviousSibling(previousSibling);
                    }
                    append += $"{index}]";
                    path = append + path;
                }
            }
            if (node == null) return null;
            return path;
        }

        private static TreeNode<SyntaxNodeOrToken> PreviousSibling(TreeNode<SyntaxNodeOrToken> node)
        {
            var parent = node.Parent;
            var parentIndex = parent.Children.FindIndex(o => o.Equals(node));
            if (parentIndex == 0) return null;
            return parent.Children[parentIndex - 1];
        }
    }
}
