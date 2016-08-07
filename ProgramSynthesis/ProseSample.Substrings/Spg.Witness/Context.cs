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
                var mats = new List<ITreeNode<Token>>();
                foreach (ITreeNode<Token> node in spec.DisjunctiveExamples[input])
                {
                    var target = Target(node);
                    if (target == null) continue;
                    mats.Add(target);
                }
                if (!mats.Any()) return null;
                treeExamples[input] = mats;
            }
            return DisjunctiveExamplesSpec.From(treeExamples);
        }

        /// <summary>
        /// Find the index of the child in the parent node.
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">Rule parameter</param>
        /// <param name="spec">Example specification</param>
        /// <param name="kindBinding">Parent binding</param>
        public ExampleSpec ParentK(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, DisjunctiveExamplesSpec kindBinding)
        {
            var kExamples = new Dictionary<State, object>();
            foreach (State input in spec.ProvidedInputs)
            {
                var matches = new List<object>();
                foreach (ITreeNode<Token> node in spec.DisjunctiveExamples[input])
                {
                    var target = Target(node);
                    if (target == null) continue;
                    var parent = target;
                    var children = parent.Children;
                    var targetIndex = children.FindIndex(o => o.Equals(node));

                    if (targetIndex == -1) continue;
                    matches.Add(targetIndex + 1);
                    //if (!ConverterHelper.Valid(sot.Value)) return null;

                    //for (int i = 0; i < children.Count; i++)
                    //{
                    //    var item = children.ElementAt(i);
                    //    if (TreeNode<SyntaxNodeOrToken>.IsEqual(item, sot))
                    //    {
                    //        matches.Add(i + 1);
                    //    }
                    //}
                }
                if (!matches.Any()) return null;    
                if (matches.Any(sequence => !sequence.Equals(matches.First()))) return null;
                kExamples[input] = matches.First();
            }
            return new ExampleSpec(kExamples);
        }

        public abstract ITreeNode<Token> Target(ITreeNode<Token> sot);
    }
}
