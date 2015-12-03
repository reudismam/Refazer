using System;
using System.Collections.Generic;
using Spg.ExampleRefactoring.Synthesis;
using Spg.LocationRefactor.Controller;
using Spg.LocationRefactor.Node;
using Spg.LocationRefactor.Operator;
using Spg.LocationRefactor.Operator.Map;
using Spg.LocationRefactor.Predicate;
using Spg.LocationRefactor.Program;
using Spg.LocationRefactor.TextRegion;
using System.Linq;
using Spg.ExampleRefactoring.Comparator;

namespace Spg.LocationRefactor.Learn.Map
{
    /// <summary>
    /// Map Learner base
    /// </summary>
    public class MergeLearnerBase : ILearn
    {
        /// <summary>
        /// Learn for map
        /// </summary>
        /// <param name="examples">Examples</param>
        /// <returns>Learned programs</returns>
        public List<Prog> Learn(List<Tuple<ListNode, ListNode>> examples)
        {
            List<Prog> programs = new List<Prog>();
            List<Tuple<ListNode, ListNode>> Q = MapBase.Decompose(examples);

            PairLearn F = new PairLearn();
            List<Prog> hypo = F.Learn(Q);

            Pair pair = hypo.First().Ioperator as Pair;

            if (!(pair.Expression is Switch))
            {
                StatementMapLearner mapLearner = new StatementMapLearner();
                List<Prog> progs = mapLearner.Learn(examples);
                return progs;
            }

            Dictionary<IPredicate, List<Tuple<ListNode, ListNode>>> segExamples = GetExamples(pair.Expression as Switch, examples);

            List<List<Prog>> maps = new List<List<Prog>>();
            foreach (var item in segExamples)
            {
                MapLearnerBase mapLearner = new StatementMapLearner();
                List<Prog> progs = mapLearner.Learn(item.Value);
                maps.Add(progs);
            }

            programs = CombinePrograms(maps);
            return programs;
        }

        private List<Prog> CombinePrograms(List<List<Prog>> maps)
        {
            List<List<Prog>> merges = new List<List<Prog>>();
            List<Prog> progs = new List<Prog>();

            foreach (var item in maps)
            {
                merges = AddProgs(item, merges);
            }

            foreach (var merge in merges)
            {
                MergeBase mergebase = new MergeBase();
                mergebase.maps = merge;

                Prog prog = new Prog();
                prog.Ioperator = mergebase;
                progs.Add(prog);
            }

            return progs;
        }

        private List<List<Prog>> AddProgs(List<Prog> progs, List<List<Prog>> merges)
        {
            List<List<Prog>> result = new List<List<Prog>>();
            if (!merges.Any())
            {
                foreach (var prog in progs)
                {
                    List<Prog> temp = new List<Prog>();
                    temp.Add(prog);
                    result.Add(temp);
                }
                return result;
            }

            foreach (var prog in progs)
            {
                foreach (var merge in merges)
                {
                    List<Prog> temp = new List<Prog>(merge);
                    temp.Add(prog);
                    result.Add(temp);
                }
            }
            return result;
        }

        private Dictionary<IPredicate, List<Tuple<ListNode, ListNode>>> GetExamples(Switch expression, List<Tuple<ListNode, ListNode>> examples)
        {
            Dictionary<IPredicate, List<Tuple<ListNode, ListNode>>> dict = new Dictionary<IPredicate, List<Tuple<ListNode, ListNode>>>();
            foreach (var example in examples)
            {
                foreach (var item in expression.Gates)
                {
                    SynthesizedProgram program = item.Item2;
                    ListNode lnode = null;
                    try
                    {
                        lnode = program.TransformInput(example.Item1);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                    NodeComparer comparer = new NodeComparer();

                    if (lnode != null && comparer.SequenceEqual(lnode, example.Item2))
                    {
                        if (!dict.ContainsKey(item.Item1))
                        {
                            List<Tuple<ListNode, ListNode>> l = new List<Tuple<ListNode, ListNode>>();
                            dict.Add(item.Item1, l);
                        }
                        dict[item.Item1].Add(example);
                    }
                }
            }
            return dict;
        }

        public List<Prog> Learn(List<Tuple<ListNode, ListNode>> positiveExamples, List<Tuple<ListNode, ListNode>> negativeExamples)
        {
            List<Prog> programs = new List<Prog>();

            EditorController contoller = EditorController.GetInstance();
            List<TRegion> list = contoller.SelectedLocations;
            Decomposer deco = Decomposer.GetInstance();
            List<Tuple<ListNode, ListNode>> exampleList = deco.Decompose(list);
            List<Tuple<ListNode, ListNode>> Q = MapBase.Decompose(exampleList);

            PairLearn F = new PairLearn();
            List<Prog> hypo = F.Learn(Q);

            IPredicate pred = GetPredicate();

            FilterLearnerBase S = GetFilter(list);
            S.Predicate = pred;

            List<Prog> predicates = S.Learn(positiveExamples, negativeExamples);
            if (hypo.Count == 1)
            {
                foreach (Prog h in hypo)
                {
                    foreach (Prog predicate in predicates)
                    {
                        MapBase map = GetMap(list);
                        map.ScalarExpression = h;
                        map.SequenceExpression = predicate;
                        Prog prog = new Prog();
                        prog.Ioperator = map;
                        programs.Add(prog);
                    }
                }
            }
            else
            {
                bool firstSynthesizedProg = true;
                List<Merge> merges = new List<Merge>();
                foreach (Prog h in hypo)
                {
                    programs = new List<Prog>();
                    foreach (Prog predicate in predicates)
                    {
                        MapBase map = GetMap(list);
                        map.ScalarExpression = h;
                        map.SequenceExpression = predicate;
                        Prog prog = new Prog();
                        prog.Ioperator = map;
                        programs.Add(prog);
                    }

                    if (firstSynthesizedProg)
                    {
                        foreach (Prog prog in programs)
                        {
                            Merge merge = new Merge();
                            merge.AddMap((MapBase)prog.Ioperator);
                            merges.Add(merge);
                        }
                        firstSynthesizedProg = false;

                    }
                    else
                    {
                        for (int i = 0; i < merges.Count; i++)
                        {
                            merges[i].AddMap((MapBase)programs[i].Ioperator);
                        }
                    }
                }

                programs = new List<Prog>();
                foreach (Merge merge in merges)
                {
                    Prog prog = new Prog();
                    prog.Ioperator = merge;
                    programs.Add(prog);
                }
            }
            return programs;
        }

        /// <summary>
        /// Decompose the examples
        /// </summary>
        /// <param name="list">Selection</param>
        /// <returns>Decomposition</returns>
        public List<Tuple<ListNode, ListNode>> Decompose(List<TRegion> list)
        {
            StatementMapLearner mapLearner = new StatementMapLearner();
            return mapLearner.Decompose(list);
        }


        /// <summary>
        /// Predicate for map
        /// </summary>
        /// <returns>Predicate for map</returns>
        protected IPredicate GetPredicate() { throw new NotImplementedException(""); }

        /// <summary>
        /// Map
        /// </summary>
        /// <returns>Map</returns>
        protected MapBase GetMap(List<TRegion> list) { throw new NotImplementedException(""); }

        /// <summary>
        /// Filter for map
        /// </summary>
        /// <returns>Filter for map</returns>
        protected FilterLearnerBase GetFilter(List<TRegion> list) { throw new NotImplementedException(""); }
    }
}





