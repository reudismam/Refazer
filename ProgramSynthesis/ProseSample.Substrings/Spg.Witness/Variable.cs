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
        public static ExampleSpec VariableKind(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var treeExamples = new Dictionary<State, object>();
            var mats = new List<SyntaxKind>();
            var childrenNums = new List<int>();
            foreach (State input in spec.ProvidedInputs)
            {
                var defaultValue = default(SyntaxKind);
                SyntaxKind kind = defaultValue;
                foreach (TreeNode<SyntaxNodeOrToken> node in spec.DisjunctiveExamples[input])
                {
                    kind = node.Value.Kind();
                    mats.Add(kind);
                    childrenNums.Add(node.Children.Count);
                }
                if (kind == defaultValue) return null;
                treeExamples[input] = kind;
            }

            if (!mats.Any()) return null;
            var isChilNumEqual = childrenNums.All(o => o.Equals(childrenNums.First()));
            var isTypeEqual = mats.All(o => o.Equals(mats.First()));

            if (!isTypeEqual)
            {
                foreach (var input in spec.ProvidedInputs)
                {
                    treeExamples[input] = SyntaxKind.EmptyStatement;
                }
            }

            if (isTypeEqual && isChilNumEqual) return null;

            return new ExampleSpec(treeExamples);
        }
    }
}
