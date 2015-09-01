using System;
using System.Collections.Generic;
using System.Linq;
using Spg.ExampleRefactoring.Digraph;
using Spg.ExampleRefactoring.Intersect;
using Spg.ExampleRefactoring.Synthesis;

namespace Spg.ExampleRefactoring.Partition
{
    internal class PartitionManager
    {
        public List<Dag> GeneratePartition(Dictionary<Dag, List<Tuple<ListNode, ListNode>>> dags)
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

            return T;
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
                    List<Dag> comp = new List<Dag> {T[i], T[j]};
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
                        List<Dag> de1 = new List<Dag> {tuple.Item1, dag};
                        List<Dag> de2 = new List<Dag> {tuple.Item2, dag};
                        List<Dag> dei = new List<Dag> {tuple.Item3, dag};

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
            Tuple<Dag,Dag,Dag> max = scores.First().Key;
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
    }
}
