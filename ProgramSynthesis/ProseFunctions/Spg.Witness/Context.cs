using System;
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
                var mats = new List<TreeNode<SyntaxNodeOrToken>>();
                foreach(TreeNode<SyntaxNodeOrToken> node in spec.DisjunctiveExamples[input])
                {                   
                    var parent = node.Parent;
                    if (parent == null) continue;
                    mats.Add(parent);
                   
                    var pParent = node.Parent.Parent;
                    if (pParent != null)
                    {
                        mats.Add(pParent);
                    }
                }
                if (!mats.Any()) return null;
                treeExamples[input] = mats;
            }
            return new DisjunctiveExamplesSpec(treeExamples);
        }

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
                var parent = (Pattern) kind.Examples[input]; 
                foreach(TreeNode<SyntaxNodeOrToken> node in spec.DisjunctiveExamples[input])
                {
                    var path = GetPath(node, parent.Tree);
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
            for (TreeNode<SyntaxNodeOrToken> node = target; !node.Value.IsKind(parent.Value.Kind); node = node.Parent)
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

        private static TreeNode<SyntaxNodeOrToken> PreviousSibling(TreeNode<SyntaxNodeOrToken> node)
        {
            var parent = node.Parent;
            var parentIndex = parent.Children.FindIndex(o => o.Equals(node));
            if (parentIndex == 0) return null;
            return parent.Children[parentIndex - 1];
        }
    }
}
