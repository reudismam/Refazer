using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Spg.ExampleRefactoring.Synthesis;
using Spg.LocationRefactor.Controller;
using Spg.LocationRefactor.Node;
using Spg.LocationRefactor.Operator;
using Spg.LocationRefactor.Operator.Map;
using Spg.LocationRefactor.Predicate;
using Spg.LocationRefactor.Program;
using Spg.LocationRefactor.TextRegion;
using Spg.LocationRefactor.Location;

namespace Spg.LocationRefactor.Learn.Map
{
    /// <summary>
    /// Map Learner base
    /// </summary>
    public abstract class MapLearnerBase:ILearn
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

            IPredicate pred = GetPredicate();
            EditorController contoller = EditorController.GetInstance();
            List<TRegion> list = contoller.SelectedLocations;
            FilterLearnerBase S = GetFilter(list);
            S.Predicate = pred;

            //List<ListNode> llnode = new List<ListNode>();
            //foreach(var item in examples)
            //{
            //    llnode.Add(item.Item2);
            //}

            //contoller.Lcas = RegionManager.LeastCommonAncestors(llnode);

            List<Prog> predicates = S.Learn(examples);
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
            //else {
            //    bool firstSynthesizedProg = true;
            //    List<Merge> merges = new List<Merge>();
            //    foreach(Prog h in hypo)
            //    {
            //        programs = new List<Prog>();
            //        foreach (Prog predicate in predicates)
            //        {
            //            MapBase map = GetMap(list);
            //            map.ScalarExpression = h;
            //            map.SequenceExpression = predicate;
            //            Prog prog = new Prog();
            //            prog.Ioperator = map;
            //            programs.Add(prog);
            //        }

            //        if (firstSynthesizedProg)
            //        {
            //            foreach (Prog prog in programs)
            //            {
            //                Merge merge = new Merge();
            //                merge.AddMap((MapBase) prog.Ioperator);
            //                merges.Add(merge);
            //            }
            //            firstSynthesizedProg = false;

            //        }else
            //        {
            //            for (int i = 0; i < merges.Count; i++)
            //            {
            //                merges[i].AddMap((MapBase) programs[i].Ioperator);
            //            }
            //        }
            //    }

            //    programs = new List<Prog>();
            //    foreach (Merge merge in merges)
            //    {
            //        Prog prog = new Prog();
            //        prog.Ioperator = merge;
            //        programs.Add(prog);
            //    }
            //}
            return programs;
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
            //else
            //{
            //    bool firstSynthesizedProg = true;
            //    List<Merge> merges = new List<Merge>();
            //    foreach (Prog h in hypo)
            //    {
            //        programs = new List<Prog>();
            //        foreach (Prog predicate in predicates)
            //        {
            //            MapBase map = GetMap(list);
            //            map.ScalarExpression = h;
            //            map.SequenceExpression = predicate;
            //            Prog prog = new Prog();
            //            prog.Ioperator = map;
            //            programs.Add(prog);
            //        }

            //        if (firstSynthesizedProg)
            //        {
            //            foreach (Prog prog in programs)
            //            {
            //                Merge merge = new Merge();
            //                merge.AddMap((MapBase)prog.Ioperator);
            //                merges.Add(merge);
            //            }
            //            firstSynthesizedProg = false;

            //        }
            //        else
            //        {
            //            for (int i = 0; i < merges.Count; i++)
            //            {
            //                merges[i].AddMap((MapBase)programs[i].Ioperator);
            //            }
            //        }
            //    }

            //    programs = new List<Prog>();
            //    foreach (Merge merge in merges)
            //    {
            //        Prog prog = new Prog();
            //        prog.Ioperator = merge;
            //        programs.Add(prog);
            //    }
            //}
            return programs;
        }

        /// <summary>
        /// Decompose the examples
        /// </summary>
        /// <param name="list">Selection</param>
        /// <returns>Decomposition</returns>
        public abstract List<Tuple<ListNode, ListNode>> Decompose(List<TRegion> list);

        /// <summary>
        /// SyntaxNode specific for map
        /// </summary>
        /// <param name="sourceCode">Source code</param>
        /// <returns>Syntax nodes</returns>
        public abstract List<SyntaxNode> SyntaxNodes(string sourceCode, List<TRegion> list);

        /// <summary>
        /// Predicate for map
        /// </summary>
        /// <returns>Predicate for map</returns>
        protected abstract IPredicate GetPredicate();

        /// <summary>
        /// Map
        /// </summary>
        /// <returns>Map</returns>
        protected abstract MapBase GetMap(List<TRegion> list);

        /// <summary>
        /// Filter for map
        /// </summary>
        /// <returns>Filter for map</returns>
        protected abstract FilterLearnerBase GetFilter(List<TRegion> list);
    }
}





