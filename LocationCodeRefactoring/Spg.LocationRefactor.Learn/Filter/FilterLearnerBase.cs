using System;
using System.Collections.Generic;
using System.Linq;
using ExampleRefactoring.Spg.ExampleRefactoring.Synthesis;
using ExampleRefactoring.Spg.LocationRefactoring.Tok;
using LocationCodeRefactoring.Spg.LocationRefactor.Learn;
using LocationCodeRefactoring.Spg.LocationRefactor.Learn.Filter.BooleanLearner;
using LocationCodeRefactoring.Spg.LocationRefactor.Operator.Map;
using LocationCodeRefactoring.Spg.LocationRefactor.Program;
using Spg.ExampleRefactoring.Tok;
using Spg.LocationRefactor.Operator.Filter;
using Spg.LocationRefactor.Predicate;
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
        public IPredicate predicate { get; set; }

        public List<TRegion> list { get; set; }

        public FilterLearnerBase(List<TRegion> list)
        {
            this.list = list;
            _calculated = new Dictionary<TokenSeq, Boolean>();
        }

        /// <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="predicate">Predicate</param>
        public FilterLearnerBase(IPredicate predicate, List<TRegion> list)
        {
            this.list = list;
            _calculated = new Dictionary<TokenSeq, Boolean>();
            this.predicate = predicate;
        }

        /// <summary>
        /// Learn a list filters
        /// </summary>
        /// <param name="examples">Examples</param>
        /// <returns>List of filters</returns>
        public List<Prog> Learn(List<Tuple<ListNode, ListNode>> examples)
        {
            List<Tuple<ListNode, ListNode>> S = MapBase.Decompose(examples);

            List<Tuple<Tuple<ListNode, ListNode>, Boolean>> QLine = new List<Tuple<Tuple<ListNode, ListNode>, Boolean>>();
            foreach (Tuple<ListNode, ListNode> tuple in S)
            {
                Tuple<Tuple<ListNode, ListNode>, Boolean> t = Tuple.Create(tuple, true);
                QLine.Add(t);
            }
            List<Prog> programs = new List<Prog>();

            List<IPredicate> predicates = BooleanLearning(QLine);
            var items = from pair in predicates
                        orderby pair.Regex().Count() descending, Order(pair) descending
                        select pair;
            Dictionary<IPredicate, Prog> dic = new Dictionary<IPredicate, Prog>();
            foreach (IPredicate ipredicate in predicates)
            {
                Prog prog;
                if (!dic.TryGetValue(ipredicate, out prog))
                {
                    prog = new Prog();
                    FilterBase filter = GetFilter(list);
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

            List<Tuple<Tuple<ListNode, ListNode>, Boolean>> QLine = new List<Tuple<Tuple<ListNode, ListNode>, Boolean>>();
            foreach (Tuple<ListNode, ListNode> tuple in S)
            {
                Tuple<Tuple<ListNode, ListNode>, Boolean> t = Tuple.Create(tuple, true);
                QLine.Add(t);
            }

            foreach (Tuple<ListNode, ListNode> tuple in SN)
            {
                Tuple<Tuple<ListNode, ListNode>, Boolean> t = Tuple.Create(tuple, false);
                QLine.Add(t);
            }

            List<Prog> programs = new List<Prog>();

            List<IPredicate> predicates = BooleanLearning(QLine);
            var items = from pair in predicates
                        orderby pair.Regex().Count() descending, Order(pair) descending
                        select pair;

            Dictionary<IPredicate, Prog> dic = new Dictionary<IPredicate, Prog>();
            foreach (IPredicate ipredicate in predicates)
            {
                Prog prog;
                if (!dic.TryGetValue(ipredicate, out prog))
                {
                    prog = new Prog();
                    FilterBase filter = GetFilter(list);
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

        ///// <summary>
        ///// Learn boolean operators
        ///// </summary>
        ///// <param name="examples">Examples</param>
        ///// <returns>List of boolean operators</returns>
        //public List<IPredicate> BooleanLearning(List<Tuple<Tuple<ListNode, ListNode>, bool>> examples)
        //{
        //    List<Tuple<ListNode, bool>> boolExamples = new List<Tuple<ListNode, bool>>();
        //    List<Tuple<ListNode, ListNode>> positivesExamples = new List<Tuple<ListNode, ListNode>>();
        //    List<Tuple<ListNode, ListNode>> negativesExamples = new List<Tuple<ListNode, ListNode>>();

        //    Dag T, NT = null;
        //    foreach (Tuple<Tuple<ListNode, ListNode>, bool> e in examples)
        //    {
        //        boolExamples.Add(Tuple.Create(e.Item1.Item1, e.Item2));
        //        if (e.Item2 && positivesExamples.Count < 2)
        //        {
        //            positivesExamples.Add(e.Item1);

        //        }
        //        else if (!e.Item2 && negativesExamples.Count < 2)
        //        {
        //            negativesExamples.Add(e.Item1);
        //        }
        //    }

        //    BooleanLearnerBase bbase = new PositiveBooleanLearner();
        //    var predicates = bbase.BooleanLearning(boolExamples, positivesExamples);


        //    //T = CreateDag(positivesExamples);

        //    //foreach (KeyValuePair<Tuple<Vertex, Vertex>, List<IExpression>> entry in T.Mapping)
        //    //{
        //    //    List<IExpression> expressions = entry.Value;
        //    //    foreach (IExpression exp in expressions)
        //    //    {
        //    //        if (exp is SubStr)
        //    //        {
        //    //            IPosition p1 = ((SubStr)exp).p1;
        //    //            IPosition p2 = ((SubStr)exp).p2;
        //    //            List<IPosition> positions = new List<IPosition>();
        //    //            positions.Add(p1);
        //    //            positions.Add(p2);
        //    //            foreach (IPosition position in positions)
        //    //            {
        //    //                if (position is Pos)
        //    //                {
        //    //                    Pos positioncopy = (Pos)position;

        //    //                    TokenSeq r1 = positioncopy.r1;
        //    //                    TokenSeq r2 = positioncopy.r2;
        //    //                    TokenSeq merge = ASTProgram.ConcatenateRegularExpression(r1, r2);
        //    //                    TokenSeq regex = merge;

        //    //                    IPredicate clone = PredicateFactory.Create(predicate);
        //    //                    clone.r1 = r1;
        //    //                    clone.r2 = r2;


        //    //                    if (!calculated.ContainsKey(regex))
        //    //                    {
        //    //                        bool b = Indicator(clone, boolExamples, regex);
        //    //                        if (b)
        //    //                        {
        //    //                            predicates.Add(clone);
        //    //                        }
        //    //                    }
        //    //                }
        //    //            }
        //    //        }
        //    //    }
        //    //}

        //    if (!negativesExamples.Any())
        //    {
        //        return predicates;
        //    }

        //    BooleanLearnerBase nbase = new NegativeBooleanLearner();
        //    predicates.AddRange(nbase.BooleanLearning(boolExamples, negativesExamples));
        //    //NT = CreateDag(negativesExamples);

        //    //foreach (KeyValuePair<Tuple<Vertex, Vertex>, List<IExpression>> entry in NT.Mapping)
        //    //{
        //    //    List<IExpression> expressions = entry.Value;
        //    //    foreach (IExpression exp in expressions)
        //    //    {
        //    //        if (exp is SubStr)
        //    //        {
        //    //            IPosition p1 = ((SubStr)exp).p1;
        //    //            IPosition p2 = ((SubStr)exp).p2;
        //    //            List<IPosition> positions = new List<IPosition>();
        //    //            positions.Add(p1);
        //    //            positions.Add(p2);
        //    //            foreach (IPosition position in positions)
        //    //            {
        //    //                if (position is Pos)
        //    //                {
        //    //                    Pos positioncopy = (Pos)position;

        //    //                    TokenSeq r1 = positioncopy.r1;
        //    //                    TokenSeq r2 = positioncopy.r2;
        //    //                    TokenSeq merge = ASTProgram.ConcatenateRegularExpression(r1, r2);

        //    //                    TokenSeq nr1 = CreateSubStrToken(r1);
        //    //                    TokenSeq nr2 = CreateSubStrToken(r2);
        //    //                    TokenSeq nmerge = ASTProgram.ConcatenateRegularExpression(nr1, nr2);
        //    //                    TokenSeq nregex = nmerge;

        //    //                    if (ContainsSubStrToken(nmerge))
        //    //                    {
        //    //                        IPredicate nclone = PredicateFactory.CreateInv(predicate);
        //    //                        nclone.r1 = nr1;
        //    //                        nclone.r2 = nr2;

        //    //                        if (!calculated.ContainsKey(nregex))
        //    //                        {
        //    //                            bool nb = Indicator(nclone, boolExamples, nregex);
        //    //                            if (nb)
        //    //                            {
        //    //                                predicates.Add(nclone);
        //    //                            }
        //    //                        }
        //    //                    }

        //    //                }
        //    //            }
        //    //        }
        //    //    }
        //    //}

        //    return predicates;
        //}

        //private bool ContainsSubStrToken(TokenSeq nmerge)
        //{
        //    foreach (var token in nmerge.Tokens)
        //    {
        //        if (token is SubStrToken) return true;
        //    }
        //    return false;
        //}

        //private TokenSeq CreateSubStrToken(TokenSeq r1)
        //{
        //    List<Token> tokens = new List<Token>();
        //    for (int i = 0; i < r1.Tokens.Count; i++)
        //    {
        //        var token = r1.Tokens[i];
        //        if (token.token.IsKind(SyntaxKind.StringLiteralToken))
        //        {
        //            Token substrToken = new SubStrToken(token.token);
        //            tokens.Add(substrToken);
        //            continue;
        //        }
        //        tokens.Add(token);
        //    }

        //    TokenSeq clone = new TokenSeq(tokens);
        //    return clone;
        //}

        //private Dag CreateDag(List<Tuple<ListNode, ListNode>> examples)
        //{
        //    //List<Tuple<ListNode, ListNode>> exs = new List<Tuple<ListNode, ListNode>>();
        //    //foreach (Tuple<Tuple<ListNode, ListNode>, Boolean> e in examples)
        //    //{
        //    //    if (e.Item2)
        //    //    {
        //    //        exs.Add(e.Item1);
        //    //    }
        //    //}

        //    List<Dag> dags = new List<Dag>();
        //    SynthesizerSetting setting = new SynthesizerSetting();
        //    setting.DynamicTokens = true;
        //    setting.Deviation = 2;
        //    ASTProgram program = new ASTProgram(setting, examples);
        //    foreach (Tuple<ListNode, ListNode> e in examples)
        //    {
        //        List<int> boundary = new List<int>();
        //        for (int i = 0; i <= e.Item2.Length(); i++)
        //        {
        //            boundary.Add(i);
        //        }
        //        Dag dag = program.GenerateStringBoundary(e.Item2, e.Item2, boundary);
        //        dags.Add(dag);
        //    }

        //    IntersectManager IntManager = new IntersectManager();
        //    Dag T = IntManager.Intersect(dags);

        //    ExpressionManager expmanager = new ExpressionManager();
        //    expmanager.FilterExpressions(T, examples);

        //    ASTProgram.Clear(T);

        //    return T;
        //}

        ///// <summary>
        ///// True if regex match the input
        ///// </summary>
        ///// <param name="predicate">Predicate</param>
        ///// <param name="examples">Examples</param>
        ///// <param name="regex">Regular expression</param>
        ///// <returns>True if regex match the input</returns>
        //public bool Indicator(IPredicate predicate, List<Tuple<ListNode, bool>> examples, TokenSeq regex)
        //{
        //    bool b = true;
        //    bool entry;

        //    if (!calculated.TryGetValue(regex, out entry))
        //    {
        //        foreach (Tuple<ListNode, bool> example in examples)
        //        {
        //            bool b02 = predicate.Evaluate(example.Item1, regex);
        //            if (!(b02 == example.Item2))
        //            {
        //                calculated[regex] = false;
        //                return false;
        //            }
        //        }
        //        calculated[regex] = b;
        //        entry = b;
        //    }
        //    return entry;
        //}

        /// <summary>
        /// True if the regex match the input
        /// </summary>
        /// <param name="predicate">Predicate</param>
        /// <param name="input">Input</param>
        /// <param name="regex">Regular expression</param>
        /// <returns>True if the regex match the input</returns>
        public Boolean Indicator(IPredicate predicate, ListNode input, TokenSeq regex)
        {
            Boolean b = predicate.Evaluate(input, regex);
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