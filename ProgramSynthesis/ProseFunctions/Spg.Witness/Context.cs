using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Specifications;
using ProseFunctions.Spg.Bean;
using TreeEdit.Spg.Match;
using TreeEdit.Spg.TreeEdit.Update;
using TreeElement.Spg.Node;
using TreeElement.Token;

namespace ProseFunctions.Spg.Witness
{
    public class Context
    {
        /// <summary>
        /// Specification for the pattern attribute of the Context operator.
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">parameter</param>
        /// <param name="spec">Specification</param>
        public DisjunctiveExamplesSpec ContextPattern(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var inputTree = (Node)input[rule.Grammar.InputSymbol];
                var mats = new List<Tuple<TreeNode<SyntaxNodeOrToken>, int >>();
                foreach (TreeNode<SyntaxNodeOrToken> node in spec.DisjunctiveExamples[input])
                {
                    //Insert node itself
                    Tuple<TreeNode<SyntaxNodeOrToken>, int> tnode = Tuple.Create(node, 0);
                    mats.Add(tnode);
                    //Insert ancestors
                    var t1Node = TreeUpdate.FindNode(inputTree.Value, node.Value);
                    var parent = t1Node?.Parent;
                    if (parent == null) continue;
                    Tuple<TreeNode<SyntaxNodeOrToken>, int> tparent = Tuple.Create(parent, 1);
                    AnalyseParent(tparent, mats);
                    var parentParent = parent.Parent;
                    if (parentParent == null) continue;
                    Tuple<TreeNode<SyntaxNodeOrToken>, int> tparentParent = Tuple.Create(parentParent, 2);
                    AnalyseParent(tparentParent, mats);
                }
                if (!mats.Any()) return null;
                treeExamples[input] = mats;
            }
            return new DisjunctiveExamplesSpec(treeExamples);
        }

        private static void AnalyseParent(Tuple<TreeNode<SyntaxNodeOrToken>, int> parent, List<Tuple<TreeNode<SyntaxNodeOrToken>, int>> mats)
        {
            int tolerance = 40;
            var parentDescendants = parent.Item1.DescendantNodesAndSelf();
            if (parentDescendants.Count < tolerance) mats.Add(parent);
        }

        /// <summary>
        /// Find the index of the child in the pattern node.
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">Rule parameter</param>
        /// <param name="spec">Example specification</param>
        /// <param name="kind">Parent binding</param>
        public ExampleSpec ContextXPath(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, ExampleSpec kind)
        {
            var kExamples = new Dictionary<State, object>();
            var matches = new List<object>();
            foreach (State input in spec.ProvidedInputs)
            {
                var inputTree = (Node)input[rule.Grammar.InputSymbol];
                var parent = (Pattern)kind.Examples[input];
                //If the pattern is Empty then return
                if (parent.Tree.Value.Label.Equals(new Label(Token.Expression))) return null;

                foreach (TreeNode<SyntaxNodeOrToken> node in spec.DisjunctiveExamples[input])
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
        /// Specification for the pattern attribute of the Context operator.
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">parameter</param>
        /// <param name="spec">Specification</param>
        public DisjunctiveExamplesSpec SContextPattern(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var mats = new List<Tuple<TreeNode<SyntaxNodeOrToken>, int>>();
                foreach (TreeNode<SyntaxNodeOrToken> node in spec.DisjunctiveExamples[input])
                {
                    //Insert node itself
                    Tuple<TreeNode<SyntaxNodeOrToken>, int> tnode = Tuple.Create(node, 0);
                    mats.Add(tnode);
                }
                if (!mats.Any()) return null;
                treeExamples[input] = mats;
            }
            return new DisjunctiveExamplesSpec(treeExamples);
        }

        /// <summary>
        /// Build an XPath expression for the target node. To build this XPath, 
        /// we keep get the pattern while the pattern is null and build an XPath 
        /// from the last pattern until the target node.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="pattern"></param>
        /// <returns>XPath</returns>
        public static string GetPath(TreeNode<SyntaxNodeOrToken> target, TreeNode<Token> pattern)
        {
            string path = "";
            TreeNode<SyntaxNodeOrToken> node;
            for (node = target; node != null && node.Value != null && !MatchManager.IsValueEachChild(node, pattern); node = node.Parent)
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

            //In case of pattern matches the node itself
            if (!path.Any()) return ".";
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
