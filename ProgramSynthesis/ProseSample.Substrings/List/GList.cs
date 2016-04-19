using System.Collections.Generic;
using System.Linq;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Specifications;


namespace ProseSample.Substrings.List
{
    public class GList<T>
    {
        public static DisjunctiveExamplesSpec List0(GrammarRule rule, int parameter, ExampleSpec spec)
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


        public static DisjunctiveExamplesSpec List1(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            var treeExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var matches = new List<object>();
                foreach (List<T> matchResult in spec.DisjunctiveExamples[input])
                {
                    if (!matchResult.Any()) return null;
                    if (matchResult.Count == 1) return null;

                    matchResult.RemoveAt(0);
                    matches.Add(matchResult);
                }
                treeExamples[input] = matches;
            }
            return DisjunctiveExamplesSpec.From(treeExamples);
        }

        public static DisjunctiveExamplesSpec Single(GrammarRule rule, int parameter, ExampleSpec spec)
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

        public static IEnumerable<T> List(T child, IEnumerable<T> clist)
        {
            var list = clist.ToList();
            list.Insert(0, child);
            return list;
        }

        public static IEnumerable<T> Single(T child)
        {
            var list = new List<T> { child };
            return list;
        }
    }
}
