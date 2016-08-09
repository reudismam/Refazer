using System.Collections.Generic;
using System.Linq;
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
                foreach(Pattern node in spec.DisjunctiveExamples[input])
                {
                    if (!(node is PatternP)) continue;
                    if (node.Tree == null) continue;
                    mats.Add(node.Tree);
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
                foreach (Pattern node in spec.DisjunctiveExamples[input])
                {
                    if (!(node is PatternP)) continue;
                    var patternP = (PatternP) node;
                    if (patternP.K == -1) continue;
                    matches.Add(patternP.K + 1);
                }
                if (!matches.Any()) return null;    
                if (matches.Any(sequence => !sequence.Equals(matches.First()))) return null;
                kExamples[input] = matches.First();
            }
            return new ExampleSpec(kExamples);
        }
    }
}
