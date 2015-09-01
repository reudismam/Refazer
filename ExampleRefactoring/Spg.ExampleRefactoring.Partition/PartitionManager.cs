using System;
using System.Collections.Generic;
using System.Linq;
using Spg.ExampleRefactoring.Digraph;
using Spg.ExampleRefactoring.Intersect;
using Spg.ExampleRefactoring.Synthesis;
using Spg.LocationRefactor.Learn.Filter.BooleanLearner;
using Spg.LocationRefactor.Predicate;
using Spg.LocationRefactoring.Tok;

namespace Spg.ExampleRefactoring.Partition
{
    internal class PartitionManager
    {
        /// <summary>
        /// Store the filters calculated
        /// </summary>
        private readonly Dictionary<TokenSeq, bool> _calculated = new Dictionary<TokenSeq, bool>();

        public Dictionary<Dag, List<Tuple<ListNode, ListNode>>> GeneratePartition(Dictionary<Dag, List<Tuple<ListNode, ListNode>>> dags)
        {
            List<Dag> T = new List<Dag>(dags.Keys);
            Dictionary<Dag, List<Tuple<ListNode, ListNode>>> dictionary = new Dictionary<Dag, List<Tuple<ListNode, ListNode>>>(dags);
            while (ExistComp(T))
            {
                Tuple<Dag, Dag, Dag> dag = CS(T);
                T.Remove(dag.Item1);
                T.Remove(dag.Item2);
                T.Add(dag.Item3);

                if (!dictionary.ContainsKey(dag.Item3))
                {
                    dictionary.Add(dag.Item3, new List<Tuple<ListNode, ListNode>>());
                }
                dictionary[dag.Item3].AddRange(dictionary[dag.Item1]);
                dictionary[dag.Item3].AddRange(dictionary[dag.Item2]);
            }

            Dictionary<Dag, List<Tuple<ListNode, ListNode>>> rt = new Dictionary<Dag, List<Tuple<ListNode, ListNode>>>();

            foreach (Dag dag in T)
            {
                rt.Add(dag, dictionary[dag]);
            }

            return rt;
        }

        private static bool ExistComp(List<Dag> T)
        {
            if (T.Count == 1) return false;

            IntersectManager intersectManager = new IntersectManager();
            for (int i = 0; i < T.Count; i++)
            {
                for (int j = i + 1; j < T.Count; j++)
                {
                    List<Dag> comp = new List<Dag>();
                    comp.Add(T[i]);
                    comp.Add(T[j]);
                    Dag inter = intersectManager.Intersect(comp);
                    if (inter != null)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static Tuple<Dag, Dag, Dag> CS(List<Dag> T)
        {
            IntersectManager intersectManager = new IntersectManager();
            //first dag, second dag, intersection
            List<Tuple<Dag, Dag, Dag>> dags = new List<Tuple<Dag, Dag, Dag>>();
            for (int i = 0; i < T.Count; i++)
            {
                for (int j = i + 1; j < T.Count; j++)
                {
                    List<Dag> comp = new List<Dag> { T[i], T[j] };
                    Dag inter = intersectManager.Intersect(comp);
                    if (inter != null)
                    {
                        Tuple<Dag, Dag, Dag> tuple = Tuple.Create(T[i], T[j], inter);
                        dags.Add(tuple);
                    }
                }
            }

            if (dags.Count == 1) return dags.First();

            Dictionary<Tuple<Dag, Dag, Dag>, int> scores = new Dictionary<Tuple<Dag, Dag, Dag>, int>();
            //compute CS1
            foreach (Tuple<Dag, Dag, Dag> tuple in dags)
            {
                foreach (Dag dag in T)
                {
                    if (!scores.ContainsKey(tuple))
                    {
                        scores.Add(tuple, 0);
                    }
                    if (dag != tuple.Item1 && dag != tuple.Item2)
                    {
                        List<Dag> de1 = new List<Dag> { tuple.Item1, dag };
                        List<Dag> de2 = new List<Dag> { tuple.Item2, dag };
                        List<Dag> dei = new List<Dag> { tuple.Item3, dag };

                        bool ede1 = ExistComp(de1);
                        bool ede2 = ExistComp(de2);
                        bool edei = ExistComp(dei);
                        if (ede1 == ede2 && ede2 == edei)
                        {
                            scores[tuple]++;
                        }
                    }
                }
            }

            int maxVal = scores.First().Value;
            Tuple<Dag, Dag, Dag> max = scores.First().Key;
            foreach (var item in scores)
            {
                if (item.Value > maxVal)
                {
                    max = item.Key;
                    maxVal = item.Value;
                }
            }

            return max;
        }

        /// <summary>
        /// Learn boolean operators
        /// </summary>
        /// <param name="examples">Examples</param>
        /// <returns>List of boolean operators</returns>
        public List<IPredicate> BooleanLearning(List<Tuple<ListNode, ListNode, bool>> examples)
        {
            List<Tuple<ListNode, bool>> boolExamples = new List<Tuple<ListNode, bool>>();
            List<Tuple<ListNode, ListNode>> positivesExamples = new List<Tuple<ListNode, ListNode>>();
            List<Tuple<ListNode, ListNode>> negativesExamples = new List<Tuple<ListNode, ListNode>>();

            foreach (Tuple<ListNode, ListNode, bool> e in examples)
            {
                boolExamples.Add(Tuple.Create(e.Item1, e.Item3));
                if (e.Item3 && positivesExamples.Count < 2)
                {
                    Tuple<ListNode, ListNode> tuple = Tuple.Create(e.Item1, e.Item2);
                    positivesExamples.Add(tuple);

                }
                else if (!e.Item3 && negativesExamples.Count < 2)
                {
                    Tuple<ListNode, ListNode> tuple = Tuple.Create(e.Item1, e.Item2);
                    negativesExamples.Add(tuple);
                }
            }

            BooleanLearnerBase bbase = new PositiveBooleanLearner(_calculated);
            var predicates = bbase.BooleanLearning(boolExamples, positivesExamples);


            if (!negativesExamples.Any())
            {
                return predicates;
            }

            BooleanLearnerBase nbase = new NegativeBooleanLearner(bbase.Calculated);
            predicates.AddRange(nbase.BooleanLearning(boolExamples, negativesExamples));

            return predicates;
        }
    }
}
