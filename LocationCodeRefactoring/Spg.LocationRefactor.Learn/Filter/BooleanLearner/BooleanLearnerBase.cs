using System;
using System.Collections.Generic;
using Spg.ExampleRefactoring.Position;
using Spg.ExampleRefactoring.Setting;
using Spg.ExampleRefactoring.Synthesis;
using Spg.LocationRefactor.Predicate;
using Spg.LocationRefactoring.Tok;

namespace Spg.LocationRefactor.Learn.Filter.BooleanLearner
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

                TokenSeq r1 = GetTokenSeq(positioncopy.R1);
                TokenSeq r2 = GetTokenSeq(positioncopy.R2);
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
                ListNode input = example.Item2; //input and output are equal.
                for (int k = 0; k < input.List.Count; k++)
                {
                    positions.AddRange(program.GeneratePos(input, k));
                }
            }
            return positions;
        }

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

        /// <summary>
        /// Return predicate
        /// </summary>
        /// <returns>Predicate</returns>
        protected abstract IPredicate GetPredicate();

        /// <summary>
        /// Return token sequence
        /// </summary>
        /// <param name="r1">TokenSeq</param>
        /// <returns>TokenSeq</returns>
        protected abstract TokenSeq GetTokenSeq(TokenSeq r1);
    }
}



