using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiGraph;
using Spg.ExampleRefactoring.AST;
using Spg.ExampleRefactoring.Digraph;
using Spg.ExampleRefactoring.Expression;
using Spg.ExampleRefactoring.Intersect;
using Spg.ExampleRefactoring.Synthesis;
using Spg.LocationRefactor.Learn.Filter.BooleanLearner;
using Spg.LocationRefactor.Predicate;
using Spg.LocationRefactoring.Tok;

namespace Spg.ExampleRefactoring.Partition
{
    internal class PartitionManager
    {
        public Dictionary<Dag, List<Tuple<ListNode, ListNode>>> GeneratePartition(Dictionary<Dag, List<Tuple<ListNode, ListNode>>> dags)
        {
            List<Dag> T = new List<Dag>(dags.Keys);
            Dictionary<Dag, List<Tuple<ListNode, ListNode>>> dictionary = new Dictionary<Dag, List<Tuple<ListNode, ListNode>>>(dags);
            while (ExistComp(T, dictionary))
            {
                Tuple<Dag, Dag, Dag> dag = CS(T, dictionary);
                T.Remove(dag.Item1);
                T.Remove(dag.Item2);
                T.Add(dag.Item3);

                //if (!dictionary.ContainsKey(dag.Item3))
                //{
                //    dictionary.Add(dag.Item3, new List<Tuple<ListNode, ListNode>>());
                //}
                //dictionary[dag.Item3].AddRange(dictionary[dag.Item1]);
                //dictionary[dag.Item3].AddRange(dictionary[dag.Item2]);
            }

            Dictionary<Dag, List<Tuple<ListNode, ListNode>>> rt = new Dictionary<Dag, List<Tuple<ListNode, ListNode>>>();

            foreach (Dag dag in T)
            {
                rt.Add(dag, dictionary[dag]);
            }

            return rt;
        }

        private static bool ExistComp(List<Dag> T, Dictionary<Dag, List<Tuple<ListNode, ListNode>>> dags)
        {
            if (T.Count == 1) return false;

            IntersectManager intersectManager = new IntersectManager();
            for (int i = 0; i < T.Count; i++)
            {
                for (int j = i + 1; j < T.Count; j++)
                {
                    List<Tuple<ListNode, ListNode>> examples =  new List<Tuple<ListNode, ListNode>>();
                    examples.AddRange(dags[T[i]]);
                    examples.AddRange(dags[T[j]]);
                    List<Dag> comp = new List<Dag>();
                    comp.Add(T[i]);
                    comp.Add(T[j]);
                    Dag inter = intersectManager.Intersect(comp);
                    if (inter != null)
                    {
                        ExpressionManager expmanager = new ExpressionManager();
                        expmanager.FilterExpressions(inter, examples);
                        ASTProgram.Clear(inter);
                        BreadthFirstDirectedPaths bfs = new BreadthFirstDirectedPaths(inter.dag, inter.Init.Id);
                        if (bfs.HasPathTo(inter.End.Id))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private Tuple<Dag, Dag, Dag> CS(List<Dag> T, Dictionary<Dag, List<Tuple<ListNode, ListNode>>> dictionary)
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
                        ASTProgram.Clear(inter);
                        BreadthFirstDirectedPaths bfs = new BreadthFirstDirectedPaths(inter.dag, inter.Init.Id);
                        if (bfs.HasPathTo(inter.End.Id))
                        {
                            Tuple<Dag, Dag, Dag> tuple = Tuple.Create(T[i], T[j], inter);
                            dags.Add(tuple);

                            List<Tuple<ListNode, ListNode>> examples = new List<Tuple<ListNode, ListNode>>();
                            examples.AddRange(dictionary[T[i]]);
                            examples.AddRange(dictionary[T[j]]);

                            if (!dictionary.ContainsKey(inter))
                            {
                                dictionary.Add(inter, new List<Tuple<ListNode, ListNode>>());
                            }

                            dictionary[inter] = examples;
                        }
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

                        bool ede1 = ExistComp(de1, dictionary);
                        bool ede2 = ExistComp(de2, dictionary);
                        bool edei = ExistComp(dei, dictionary);
                        //bool ede1 = CanCreateFilter(de1, dictionary);
                        //bool ede2 = CanCreateFilter(de2, dictionary);
                        //bool edei = CanCreateFilter(dei, dictionary);

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

                if (item.Value == maxVal)
                {
                    int cs2Item = item.Key.Item3.Mapping.Count / Math.Max(item.Key.Item1.Mapping.Count, item.Key.Item2.Mapping.Count);
                    int cs2MaxVal = max.Item3.Mapping.Count / Math.Max(max.Item1.Mapping.Count, max.Item2.Mapping.Count);

                    if (cs2Item > cs2MaxVal)
                    {
                        max = item.Key;
                    }
                }
            }

            return max;
        }

        public bool CanCreateFilter(List<Dag> dags, Dictionary<Dag, List<Tuple<ListNode, ListNode>>> Ts)
        {
            if (!ExistComp(dags, Ts)) return false; 

            List<Tuple<ListNode, ListNode, bool>> ln = new List<Tuple<ListNode, ListNode, bool>>();

            foreach (Tuple<ListNode, ListNode> example in Ts[dags.First()])
            {
                Tuple<ListNode, ListNode, bool> tuple = Tuple.Create(example.Item2, example.Item2, true);
                ln.Add(tuple);
            }

            foreach (var example in Ts[dags[1]])
            {
                Tuple<ListNode, ListNode, bool> tuple = Tuple.Create(example.Item2, example.Item2, false);
                ln.Add(tuple);
            }

            List<IPredicate> predicates = BooleanLearning(ln);

            return predicates.Any();
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

            Dictionary<TokenSeq, bool> _calculated = new Dictionary<TokenSeq, bool>();
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
