using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Spg.ExampleRefactoring.Expression;
using Spg.ExampleRefactoring.Synthesis;
using Spg.LocationCodeRefactoring.Controller;
using Spg.LocationRefactor.Operator;
using Spg.LocationRefactor.Predicate;
using Spg.LocationRefactor.Program;
using Spg.LocationRefactor.TextRegion;

namespace Spg.LocationRefactor.Learn
{
    /// <summary>
    /// Merge Learner
    /// </summary>
    public class MergeLearner: ILearn
    {
        /// <summary>
        /// Learn merge
        /// </summary>
        /// <param name="examples">Examples</param>
        /// <returns>Merge operators</returns>
        public List<Prog> Learn(List<Tuple<ListNode, ListNode>> examples)
        {
            List<Prog> programs = new List<Prog>();
            List<Tuple<ListNode, ListNode>> Q = MapBase.Decompose(examples);

            ASTProgram P = new ASTProgram();
            SynthesizedProgram h = P.GenerateStringProgram(examples).Single();

            List<IExpression> X = new List<IExpression>();

            Predicate.IPredicate pred = GetPredicate();
            EditorController contoller = EditorController.GetInstance();
            List<TRegion> list = contoller.RegionsBeforeEdition;
            FilterLearnerBase S = GetFilter(list);
            S.predicate = pred;

            List<Prog> predicates = S.Learn(examples);
            foreach (IExpression e in X)
            {
                foreach (Prog predicate in predicates)
                {
                    Prog scalar = new Prog();
                    Pair pair = new Pair();
                    pair.expression = (SubStr)e;

                    scalar.ioperator = pair;

                    
                    MapBase map = GetMap(list);
                    map.scalarExpression = scalar;
                    map.sequenceExpression = predicate;
                    Prog prog = new Prog();
                    prog.ioperator = map;
                    programs.Add(prog);
                }
            }
            return programs;
        }

        /// <summary>
        /// Predicate
        /// </summary>
        /// <returns>Predicate</returns>
        protected IPredicate GetPredicate() {
            return new Contains();
        }

        /// <summary>
        /// Map
        /// </summary>
        /// <returns>Map</returns>
        protected MapBase GetMap(List<TRegion> list) {
            return new MethodMap(list);
        }

        /// <summary>
        /// Filter
        /// </summary>
        /// <returns>Filter</returns>
        protected FilterLearnerBase GetFilter(List<TRegion> list) {
            return new StatementFilterLearner(list);
        }
    }
}
