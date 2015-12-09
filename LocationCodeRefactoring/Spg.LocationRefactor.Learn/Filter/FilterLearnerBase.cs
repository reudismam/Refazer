using Microsoft.CodeAnalysis;
using Spg.ExampleRefactoring.Position;
using Spg.ExampleRefactoring.Synthesis;
using Spg.ExampleRefactoring.Tok;
using Spg.LocationRefactor.Learn.Filter.BooleanLearner;
using Spg.LocationRefactor.Location;
using Spg.LocationRefactor.Operator.Filter;
using Spg.LocationRefactor.Operator.Map;
using Spg.LocationRefactor.Predicate;
using Spg.LocationRefactor.Program;
using Spg.LocationRefactor.TextRegion;
using Spg.LocationRefactoring.Tok;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Spg.LocationRefactor.Learn
{
    /// <summary>
    /// Represents a filter operator
    /// </summary>
    public abstract class FilterLearnerBase : ILearn
    {
        /// <summary>
        /// Store the filters calculated
        /// </summary>
        private readonly Dictionary<Pos, bool> _calculated;

        /// <summary>
        /// Predicate of the filter
        /// </summary>
        public IPredicate Predicate { get; set; }

        public List<TRegion> List { get; set; }

        public FilterLearnerBase(List<TRegion> list)
        {
            this.List = list;
            _calculated = new Dictionary<Pos, bool>();
        }

        /// <summary>
        /// Learn a list filters
        /// </summary>
        /// <param name="examples">Examples</param>
        /// <returns>List of filters</returns>
        public List<Prog> Learn(List<Tuple<ListNode, ListNode>> examples)
        {
            List<Tuple<ListNode, ListNode>> S = MapBase.Decompose(examples);

            List<Tuple<ListNode, ListNode, bool>> QLine = new List<Tuple<ListNode, ListNode, bool>>();
            foreach (Tuple<ListNode, ListNode> tuple in S)
            {
                Tuple<ListNode, ListNode, bool> t = Tuple.Create(tuple.Item1, tuple.Item2, true);
                QLine.Add(t);
            }

            List<Prog> programs = new List<Prog>();

            List<SyntaxNode> Lcas = LearnLcas(examples);

            Console.WriteLine("Learning predicates for filter.");
            List<IPredicate> predicates = BooleanLearning(QLine);
            Console.WriteLine("Predicated learning completed.");
            var items = from pair in predicates
                        orderby pair.Regex().Count() descending, Order(pair) descending
                        select pair;
            Dictionary<IPredicate, Prog> dic = new Dictionary<IPredicate, Prog>();
            foreach (IPredicate ipredicate in items)
            {
                Prog prog;
                if (!dic.TryGetValue(ipredicate, out prog))
                {
                    prog = new Prog();
                    FilterBase filter = GetFilter(List);
                    filter.Lcas = Lcas;
                    filter.Predicate = ipredicate;
                    prog.Ioperator = filter;
                    dic.Add(ipredicate, prog);
                    programs.Add(prog);
                }
            }
            return programs;
        }

        private List<SyntaxNode> LearnLcas(List<Tuple<ListNode, ListNode>> examples)
        {
            List<ListNode> llnode = new List<ListNode>();
            foreach (var item in examples)
            {
                llnode.Add(item.Item2);
            }

            List<SyntaxNode> Lcas = RegionManager.LeastCommonAncestors(llnode);

            return Lcas;
        }

        /// <summary>
        /// Learn filter
        /// </summary>
        /// <param name="positiveExamples">Positive examples</param>
        /// <param name="negativeExamples">Negative examples</param>
        /// <returns>Learned filters</returns>
        public List<Prog> Learn(List<Tuple<ListNode, ListNode>> positiveExamples, List<Tuple<ListNode, ListNode>> negativeExamples)
        {
            List<Tuple<ListNode, ListNode>> S = MapBase.Decompose(positiveExamples);
            List<Tuple<ListNode, ListNode>> SN = MapBase.Decompose(negativeExamples);

            List<Tuple<ListNode, ListNode, bool>> QLine = new List<Tuple<ListNode, ListNode, bool>>();
            foreach (Tuple<ListNode, ListNode> tuple in S)
            {
                Tuple<ListNode, ListNode, bool> t = Tuple.Create(tuple.Item1, tuple.Item2, true);
                QLine.Add(t);
            }

            foreach (Tuple<ListNode, ListNode> tuple in SN)
            {
                Tuple<ListNode, ListNode, bool> t = Tuple.Create(tuple.Item1, tuple.Item2, false);
                QLine.Add(t);
            }

            List<Prog> programs = new List<Prog>();

            List<IPredicate> predicates = BooleanLearning(QLine);
            var items = from pair in predicates
                        orderby pair.Regex().Count() descending, Order(pair) descending
                        select pair;

            Dictionary<IPredicate, Prog> dic = new Dictionary<IPredicate, Prog>();
            foreach (IPredicate ipredicate in items)
            {
                Prog prog;
                if (!dic.TryGetValue(ipredicate, out prog))
                {
                    prog = new Prog();
                    FilterBase filter = GetFilter(List);
                    filter.Predicate = ipredicate;
                    prog.Ioperator = filter;
                    dic.Add(ipredicate, prog);
                    programs.Add(prog);
                }
            }
            return programs;
        }

        private object Order(IPredicate pair)
        {
            int count = 0;
            foreach (Token t in pair.Regex())
            {
                if (t is DymToken)
                {
                    count++;
                }
            }
            return count;
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
                if (e.Item3 /*&& positivesExamples.Count < 2*/)
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

        /// <summary>
        /// True if the regex match the input
        /// </summary>
        /// <param name="predicate">Predicate</param>
        /// <param name="input">Input</param>
        /// <param name="regex">Regular expression</param>
        /// <returns>True if the regex match the input</returns>
        public bool Indicator(IPredicate predicate, ListNode input, Pos regex)
        {
            bool b = predicate.Evaluate(input, regex);
            return b;
        }

        /// <summary>
        /// Get filter for specific learner
        /// </summary>
        /// <param name="list">List of regions</param>
        /// <returns>Get filter for specific learner</returns>
        protected abstract FilterBase GetFilter(List<TRegion> list);
    }
}





