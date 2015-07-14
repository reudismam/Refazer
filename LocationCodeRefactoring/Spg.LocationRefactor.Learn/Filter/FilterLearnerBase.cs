using System;
using System.Collections.Generic;
using System.Linq;
using Spg.ExampleRefactoring.Synthesis;
using Spg.ExampleRefactoring.Tok;
using Spg.LocationRefactor.Learn.Filter.BooleanLearner;
using Spg.LocationRefactor.Operator.Filter;
using Spg.LocationRefactor.Operator.Map;
using Spg.LocationRefactor.Predicate;
using Spg.LocationRefactor.Program;
using Spg.LocationRefactor.TextRegion;
using Spg.LocationRefactoring.Tok;

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
        private readonly Dictionary<TokenSeq, bool> _calculated;

        /// <summary>
        /// Predicate of the filter
        /// </summary>
        public IPredicate Predicate { get; set; }

        public List<TRegion> List { get; set; }

        public FilterLearnerBase(List<TRegion> list)
        {
            this.List = list;
            _calculated = new Dictionary<TokenSeq, bool>();
        }

        /// <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="predicate">Predicate</param>
        public FilterLearnerBase(IPredicate predicate, List<TRegion> list)
        {
            this.List = list;
            _calculated = new Dictionary<TokenSeq, bool>();
            this.Predicate = predicate;
        }

        /// <summary>
        /// Learn a list filters
        /// </summary>
        /// <param name="examples">Examples</param>
        /// <returns>List of filters</returns>
        public List<Prog> Learn(List<Tuple<ListNode, ListNode>> examples)
        {
            List<Tuple<ListNode, ListNode>> S = MapBase.Decompose(examples);

            List<Tuple<Tuple<ListNode, ListNode>, bool>> QLine = new List<Tuple<Tuple<ListNode, ListNode>, bool>>();
            foreach (Tuple<ListNode, ListNode> tuple in S)
            {
                Tuple<Tuple<ListNode, ListNode>, bool> t = Tuple.Create(tuple, true);
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

            List<Tuple<Tuple<ListNode, ListNode>, bool>> QLine = new List<Tuple<Tuple<ListNode, ListNode>, bool>>();
            foreach (Tuple<ListNode, ListNode> tuple in S)
            {
                Tuple<Tuple<ListNode, ListNode>, bool> t = Tuple.Create(tuple, true);
                QLine.Add(t);
            }

            foreach (Tuple<ListNode, ListNode> tuple in SN)
            {
                Tuple<Tuple<ListNode, ListNode>, bool> t = Tuple.Create(tuple, false);
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
        public List<IPredicate> BooleanLearning(List<Tuple<Tuple<ListNode, ListNode>, bool>> examples)
        {
            List<Tuple<ListNode, bool>> boolExamples = new List<Tuple<ListNode, bool>>();
            List<Tuple<ListNode, ListNode>> positivesExamples = new List<Tuple<ListNode, ListNode>>();
            List<Tuple<ListNode, ListNode>> negativesExamples = new List<Tuple<ListNode, ListNode>>();

            foreach (Tuple<Tuple<ListNode, ListNode>, bool> e in examples)
            {
                boolExamples.Add(Tuple.Create(e.Item1.Item1, e.Item2));
                if (e.Item2 && positivesExamples.Count < 2)
                {
                    positivesExamples.Add(e.Item1);

                }
                else if (!e.Item2 && negativesExamples.Count < 2)
                {
                    negativesExamples.Add(e.Item1);
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
        public bool Indicator(IPredicate predicate, ListNode input, TokenSeq regex)
        {
            bool b = predicate.Evaluate(input, regex);
            return b;
        }

        protected abstract FilterBase GetFilter(List<TRegion> list);
    }
}


//public List<IPredicate> BooleanLearning(List<Tuple<Tuple<ListNode, ListNode>, Boolean>> examples)
//{
//    List<IPredicate> predicates = new List<Predicate.IPredicate>();

//    /*List<Tuple<ListNode, ListNode>> lnodes = new List<Tuple<ListNode, ListNode>>();
//    foreach (Tuple<Tuple<ListNode, ListNode>, Boolean> t in examples) {
//        Tuple<ListNode, ListNode> tp = Tuple.Create(t.Item1.Item2, t.Item1.Item2);
//        lnodes.Add(tp);
//    }

//    SynthesizerSetting setting = new SynthesizerSetting();
//    setting.dynamicTokens = true;
//    setting.deviation = lnodes[0].Item2.Length();
//    ASTProgram program = new ASTProgram(setting, lnodes);*/
//    //program.boundary = BoundaryManager.GetInstance().boundary;

//    List<Tuple<ListNode, bool>> boolExamples = new List<Tuple<ListNode, bool>>();

//    Dag T = null;
//    foreach (Tuple<Tuple<ListNode, ListNode>, Boolean> e in examples)
//    {
//        boolExamples.Add(Tuple.Create(e.Item1.Item1, e.Item2));
//    }

//    T = CreateDag(examples);

//    //T = program.GenerateString2(examples[0].Item1.Item2, examples[0].Item1.Item2);
//    foreach (KeyValuePair<Tuple<Vertex, Vertex>, List<IExpression>> entry in T.mapping)
//    {
//        List<IExpression> expressions = entry.Value;
//        foreach (IExpression exp in expressions)
//        {
//            if (exp is SubStr)
//            {
//                IPosition p1 = ((SubStr)exp).p1;
//                IPosition p2 = ((SubStr)exp).p2;
//                List<IPosition> positions = new List<IPosition>();
//                positions.Add(p1);
//                positions.Add(p2);
//                foreach (IPosition position in positions)
//                {
//                    if (position is Pos)
//                    {
//                        Pos positioncopy = (Pos)position;

//                        TokenSeq r1 = positioncopy.r1;
//                        TokenSeq r2 = positioncopy.r2;
//                        TokenSeq merge = ASTProgram.ConcatenateRegularExpression(r1, r2);
//                        TokenSeq regex = merge;

//                        Boolean b = Indicator(predicate, boolExamples, regex);
//                        if (b)
//                        {
//                            IPredicate clone = PredicateFactory.Create((IPredicate)predicate);
//                            clone.r1 = r1;
//                            clone.r2 = r2;

//                            predicates.Add(clone);
//                        }
//                    }
//                }
//            }
//        }
//    }
//    return predicates;
//}




