using Microsoft.CodeAnalysis;
using Spg.ExampleRefactoring.Synthesis;
using Spg.LocationRefactor.Operator;
using Spg.LocationRefactor.Predicate;
using Spg.LocationRefactor.Program;
using Spg.LocationRefactor.TextRegion;
using System;
using System.Collections.Generic;

namespace Spg.LocationRefactor.Learn
{
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

            Predicate.IPredicate pred = GetPredicate();
            FilterLearnerBase S = GetFilter();
            S.predicate = pred;

            List<Prog> predicates = S.Learn(examples);

            if (hypo.Count == 1)
            {

                foreach (Prog h in hypo)
                {
                    foreach (Prog predicate in predicates)
                    {
                        MapBase map = GetMap();
                        map.scalarExpression = h;
                        map.sequenceExpression = predicate;
                        Prog prog = new Prog();
                        prog.ioperator = map;
                        programs.Add(prog);
                    }
                }
            }
            else {
                Boolean firstSynthesizedProg = true;
                List<Merge> merges = new List<Merge>();
                foreach(Prog h in hypo)
                {
                    programs = new List<Prog>();
                    foreach (Prog predicate in predicates)
                    {
                        MapBase map = GetMap();
                        map.scalarExpression = h;
                        map.sequenceExpression = predicate;
                        Prog prog = new Prog();
                        prog.ioperator = map;
                        programs.Add(prog);
                    }

                    if (firstSynthesizedProg)
                    {
                        foreach (Prog prog in programs)
                        {
                            Merge merge = new Merge();
                            merge.AddMap((MapBase) prog.ioperator);
                            merges.Add(merge);
                        }
                        firstSynthesizedProg = false;

                    }else
                    {
                        for (int i = 0; i < merges.Count; i++)
                        {
                            merges[i].AddMap((MapBase) programs[i].ioperator);
                        }
                    }
                }

                programs = new List<Prog>();
                foreach (Merge merge in merges)
                {
                    Prog prog = new Prog();
                    prog.ioperator = merge;
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
        public abstract List<Tuple<ListNode, ListNode>> Decompose(List<TRegion> list);
  
        /// <summary>
        /// SyntaxNode specific for map
        /// </summary>
        /// <param name="sourceCode">Source code</param>
        /// <returns>Syntax nodes</returns>
        public abstract List<SyntaxNode> SyntaxNodes(string sourceCode);

        /// <summary>
        /// Predicate for map
        /// </summary>
        /// <returns>Predicate for map</returns>
        protected abstract IPredicate GetPredicate();

        /// <summary>
        /// Map
        /// </summary>
        /// <returns>Map</returns>
        protected abstract MapBase GetMap();

        /// <summary>
        /// Filter for map
        /// </summary>
        /// <returns>Filter for map</returns>
        protected abstract FilterLearnerBase GetFilter();
    }
}
