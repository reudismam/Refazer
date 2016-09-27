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
                    if (node.Parent == null) continue;
                    mats.Add(node.Parent);
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
        /// <param name="kind">Parent binding</param>
        public ExampleSpec ParentK(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, ExampleSpec kind)
        {
            var kExamples = new Dictionary<State, object>();
            var matches = new List<object>();
            foreach (State input in spec.ProvidedInputs)
            {
                var parent = (TreeNode<SyntaxNodeOrToken>)kind.Examples[input]; 
                foreach (TreeNode<SyntaxNodeOrToken> node in spec.DisjunctiveExamples[input])
                {
                    matches.Add("[0]");
                }
                if (!matches.Any()) return null;    
                if (matches.Any(sequence => !sequence.Equals(matches.First()))) return null;
                kExamples[input] = matches.First();
            }
            return new ExampleSpec(kExamples);
        }
    }
}
