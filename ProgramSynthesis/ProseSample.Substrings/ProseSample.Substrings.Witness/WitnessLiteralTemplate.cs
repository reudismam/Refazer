using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Specifications;

namespace ProseSample.Substrings.ProseSample.Substrings.Witness
{
    public abstract class WitnessLiteralTemplate
    {
        protected List<SyntaxKind> Kinds;


        /// <summary>
        /// Literal parameter base learner
        /// </summary>
        /// <param name="rule">Rule in the grammar</param>
        /// <param name="parameter">Rule parameter</param>
        /// <param name="spec">Example specification</param>
        /// <returns>Disjunctive example specification</returns>
        public DisjunctiveExamplesSpec LiteralParameterBase(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            InitializeKinds();
            var idExamples = new Dictionary<State, IEnumerable<object>>();

            foreach (State input in spec.ProvidedInputs)
            {
                var strings = new List<object>();
                foreach (SyntaxNodeOrToken sot in spec.DisjunctiveExamples[input])
                {
                    if (IsKind(sot, Kinds))
                    {
                        strings.Add(sot.ToString());
                    }
                    else
                    {
                        return null;
                    }
                }
                idExamples[input] = strings;
            }

            return DisjunctiveExamplesSpec.From(idExamples);
        }

        public abstract bool Match(SyntaxNodeOrToken toCompare, List<SyntaxKind> kinds);

        public abstract void InitializeKinds();

        /// <summary>
        /// Verify if toCompater is of one of the specified kinds.
        /// </summary>
        /// <param name="toCompare">Node to be compared</param>
        /// <param name="kinds">SyntaxKinds evaluated</param>
        /// <returns>True if toCompater is of one of the specified kinds.</returns>
        public static bool IsKind(SyntaxNodeOrToken toCompare, List<SyntaxKind> kinds)
        {
            return kinds.Any(kind => toCompare.IsKind(kind));
        }
    } 
}
