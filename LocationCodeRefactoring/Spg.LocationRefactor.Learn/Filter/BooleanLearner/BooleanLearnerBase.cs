using System;
using System.Collections.Generic;
using ExampleRefactoring.Spg.ExampleRefactoring.Position;
using ExampleRefactoring.Spg.ExampleRefactoring.Setting;
using ExampleRefactoring.Spg.ExampleRefactoring.Synthesis;
using ExampleRefactoring.Spg.LocationRefactoring.Tok;
using Spg.ExampleRefactoring.Position;
using Spg.LocationRefactor.Predicate;

namespace LocationCodeRefactoring.Spg.LocationRefactor.Learn.Filter.BooleanLearner
{
    /// <summary>
    /// Learn a set of predicates
    /// </summary>
    public abstract class BooleanLearnerBase
    {
        /// <summary>
        /// Store the filters calculated
        /// </summary>
        public readonly Dictionary<TokenSeq, bool> Calculated;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="calculated">Calculated expressions</param>
        protected BooleanLearnerBase(Dictionary<TokenSeq, bool> calculated)
        {
            Calculated = calculated;
        }

        public List<IPredicate> BooleanLearning(List<Tuple<ListNode, bool>> boolExamples, List<Tuple<ListNode, ListNode>> examples)
        {
            var predicates = new List<IPredicate>();

            IEnumerable<IPosition> positions = GetPositions(examples);
            foreach (IPosition position in positions)
            {
                Pos positioncopy = (Pos)position;

                TokenSeq r1 = GetTokenSeq(positioncopy.r1);
                TokenSeq r2 = GetTokenSeq(positioncopy.r2);
                TokenSeq merge = ASTProgram.ConcatenateRegularExpression(r1, r2);
                TokenSeq regex = merge;

                IPredicate clone = GetPredicate();
                clone.r1 = r1;
                clone.r2 = r2;

                if (!Calculated.ContainsKey(regex))
                {
                    bool b = Indicator(clone, boolExamples, regex);
                    if (b)
                    {
                        predicates.Add(clone);
                    }
                }
            }
            return predicates;
        }

        //public List<IPredicate> BooleanLearning(List<Tuple<ListNode, bool>> boolExamples, List<Tuple<ListNode, ListNode>> examples)
        //{
        //    List<IPredicate> predicates = new List<IPredicate>();

        //    //Dag T = CreateDag(examples);

        //    //foreach (KeyValuePair<Tuple<Vertex, Vertex>, List<IExpression>> entry in T.Mapping)
        //    //{
        //    //List<IExpression> expressions = entry.Value;
        //    //foreach (IExpression exp in expressions)
        //    //{
        //    //    if (exp is SubStr)
        //    //    {
        //    //        IPosition p1 = ((SubStr)exp).p1;
        //    //        IPosition p2 = ((SubStr)exp).p2;
        //    //        List<IPosition> positions = new List<IPosition>();
        //    //        positions.Add(p1);
        //    //        positions.Add(p2);
        //    List<IPosition> positions = GetPositions(examples);
        //    foreach (IPosition position in positions)
        //    {
        //        //if (position is Pos)
        //        //{
        //        Pos positioncopy = (Pos)position;

        //        TokenSeq r1 = GetTokenSeq(positioncopy.r1);
        //        TokenSeq r2 = GetTokenSeq(positioncopy.r2);
        //        TokenSeq merge = ASTProgram.ConcatenateRegularExpression(r1, r2);
        //        TokenSeq regex = merge;

        //        IPredicate clone = GetPredicate();//PredicateFactory.Create(new Contains());
        //        clone.r1 = r1;
        //        clone.r2 = r2;


        //        if (!Calculated.ContainsKey(regex))
        //        {
        //            bool b = Indicator(clone, boolExamples, regex);
        //            if (b)
        //            {
        //                predicates.Add(clone);
        //            }
        //        }
        //    }
        //    //        }
        //    //    }
        //    //}
        //    //}
        //    return predicates;
        //}

        /// <summary>
        /// Get position expressions
        /// </summary>
        /// <param name="examples">Example list</param>
        /// <returns>Predicates set</returns>
        private IEnumerable<IPosition> GetPositions(List<Tuple<ListNode, ListNode>> examples)
        {
            SynthesizerSetting setting = new SynthesizerSetting { DynamicTokens = true, Deviation = 2 };
            ASTProgram program = new ASTProgram(setting, examples);

            List<IPosition> positions = new List<IPosition>();
            foreach (var example in examples)
            {
                ListNode input = example.Item1; //input and output are equal.
                for (int k = 0; k < input.List.Count; k++)
                {
                    positions.AddRange(program.GeneratePos(input, k));
                }
            }
            return positions;
        }

        //public List<IPredicate> BooleanLearning2(List<Tuple<ListNode, bool>> boolExamples, List<Tuple<ListNode, ListNode>> examples)
        //{
        //    List<IPredicate> predicates = new List<IPredicate>();

        //    Dag T = CreateDag(examples);

        //    foreach (KeyValuePair<Tuple<Vertex, Vertex>, List<IExpression>> entry in T.Mapping)
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

        //                        TokenSeq r1 = GetTokenSeq(positioncopy.r1);
        //                        TokenSeq r2 = GetTokenSeq(positioncopy.r2);
        //                        TokenSeq merge = ASTProgram.ConcatenateRegularExpression(r1, r2);
        //                        TokenSeq regex = merge;

        //                        IPredicate clone = GetPredicate();//PredicateFactory.Create(new Contains());
        //                        clone.r1 = r1;
        //                        clone.r2 = r2;


        //                        if (!Calculated.ContainsKey(regex))
        //                        {
        //                            bool b = Indicator(clone, boolExamples, regex);
        //                            if (b)
        //                            {
        //                                predicates.Add(clone);
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return predicates;
        //}

        protected abstract IPredicate GetPredicate();


        protected abstract TokenSeq GetTokenSeq(TokenSeq r1);


        ///// <summary>
        ///// Create a Dag.
        ///// </summary>
        ///// <param name="examples"></param>
        ///// <returns></returns>
        //private Dag CreateDag(List<Tuple<ListNode, ListNode>> examples)
        //{
        //    List<Dag> dags = new List<Dag>();
        //    SynthesizerSetting setting = new SynthesizerSetting {DynamicTokens = true, Deviation = 2};
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

        //    IntersectManager intManager = new IntersectManager();
        //    Dag T = intManager.Intersect(dags);

        //    ExpressionManager expmanager = new ExpressionManager();
        //    expmanager.FilterExpressions(T, examples);

        //    ASTProgram.Clear(T);

        //    return T;
        //}

        /// <summary>
        /// True if regular expression matches the input
        /// </summary>
        /// <param name="predicate">Predicate</param>
        /// <param name="examples">Examples</param>
        /// <param name="regex">Regular expression</param>
        /// <returns>True if regular expression matches the input</returns>
        public bool Indicator(IPredicate predicate, List<Tuple<ListNode, bool>> examples, TokenSeq regex)
        {
            bool entry;
            if (!Calculated.TryGetValue(regex, out entry))
            {
                foreach (Tuple<ListNode, bool> example in examples)
                {
                    bool b02 = predicate.Evaluate(example.Item1, regex);
                    if (b02 != example.Item2)
                    {
                        Calculated[regex] = false;
                        return false;
                    }
                }
                Calculated[regex] = true;
                entry = true;
            }
            return entry;
        }
    }
}
