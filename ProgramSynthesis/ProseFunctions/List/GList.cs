using System.Collections.Generic;
using System.Linq;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Specifications;

namespace ProseFunctions.List
{
    public class GList<T>
    {
        /// <summary>
        /// Return the first element of the list.
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">Paraeter</param>
        /// <param name="spec">Specification</param>
        public static DisjunctiveExamplesSpec List0(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var matches = new List<object>();
                foreach (List<T> matchResult in spec.DisjunctiveExamples[input])
                {
                    if (!matchResult.Any()) return null;
                    if (matchResult.Count == 1) return null;

                    matches.Add(matchResult.First());
                }
                treeExamples[input] = matches;
            }
            return DisjunctiveExamplesSpec.From(treeExamples);
        }

        /// <summary>
        /// Remove the first element and return the remaining of the list.
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="spec">Example specification</param>
        public static DisjunctiveExamplesSpec List1(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var matches = new List<object>();
                foreach (List<T> matchResult in spec.DisjunctiveExamples[input])
                {
                    if (!matchResult.Any()) return null;
                    if (matchResult.Count == 1) return null;

                    var copy = new List<T>(matchResult);
                    copy.RemoveAt(0);
                    matches.Add(copy);
                }
                treeExamples[input] = matches;
            }
            return DisjunctiveExamplesSpec.From(treeExamples);
        }

        /// <summary>
        /// Return the first element of the list when the list has a single element
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="spec">Specification</param>
        /// <returns></returns>
        public static DisjunctiveExamplesSpec Single(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var matches = new List<object>();
                foreach (List<T> matchResult in spec.DisjunctiveExamples[input])
                {
                    if (!matchResult.Any()) return null;
                    if (matchResult.Count != 1) return null;

                    matches.Add(matchResult.First());
                }
                treeExamples[input] = matches;
            }
            return DisjunctiveExamplesSpec.From(treeExamples);
        }

        /// <summary>
        /// Insert the child as first child of clist
        /// </summary>
        /// <param name="child">Child</param>
        /// <param name="clist">Child list</param>
        public static IEnumerable<T> List(T child, IEnumerable<T> clist)
        {
            var list = clist.ToList();
            list.Insert(0, child);
            return list;
        }

        /// <summary>
        /// Return a list with a single element
        /// </summary>
        /// <param name="child">Child</param>
        public static IEnumerable<T> Single(T child)
        {
            var list = new List<T> { child };
            return list;
        }
    }
}
