using System;
using System.Collections.Generic;
using Spg.ExampleRefactoring.Position;
using Spg.ExampleRefactoring.Setting;
using Spg.ExampleRefactoring.Synthesis;
using Spg.LocationRefactor.Predicate;
using Spg.LocationRefactoring.Tok;
using Spg.ExampleRefactoring.AST;
using System.Linq;
using Spg.ExampleRefactoring.Comparator;

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
        public readonly Dictionary<Pos, bool> Calculated;

        public bool _getFullQualifiedName;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="calculated">Calculated expressions</param>
        protected BooleanLearnerBase(Dictionary<Pos, bool> calculated, bool _getFullQualifiedName)
        {
            this._getFullQualifiedName = _getFullQualifiedName;
            Calculated = calculated;
        }

        public List<IPredicate> BooleanLearning(List<Tuple<ListNode, bool>> boolExamples, List<Tuple<ListNode, ListNode>> examples)
        {
            var predicates = new List<IPredicate>();

            IEnumerable<IPosition> positions = GetPositions(examples);
            foreach (IPosition position in positions)
            {
                Pos positioncopy = (Pos)position;  

                IPredicate clone = GetPredicate();
                clone.regex = positioncopy;
                //clone.r1 = r1;
                //clone.r2 = r2;
                TokenSeq r1 = GetTokenSeq(positioncopy.R1);
                TokenSeq r2 = GetTokenSeq(positioncopy.R2);

                positioncopy.R1 = r1;
                positioncopy.R2 = r2;
            
                TokenSeq merge = ASTProgram.ConcatenateRegularExpression(r1, r2);

                if (ContainsSubStrToken(merge))
                {
                    positioncopy.Position = ASTManager.Matches(examples.First().Item2, merge, new RegexComparer()).Count;
                }
                //TokenSeq regex = merge;

                if (!Calculated.ContainsKey(positioncopy))
                {
                    bool b = Indicator(clone, boolExamples, positioncopy);
                    if (b)
                    {
                        predicates.Add(clone);
                    }
                }
            }
            return predicates;
        }

        private bool ContainsSubStrToken(TokenSeq merge)
        {
            foreach(var item in merge.Tokens)
            {
                if(item is SubStrToken)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Get position expressions
        /// </summary>
        /// <param name="examples">Example list</param>
        /// <returns>Predicates set</returns>
        private IEnumerable<IPosition> GetPositions(List<Tuple<ListNode, ListNode>> examples)
        {
            SynthesizerSetting setting = new SynthesizerSetting { DynamicTokens = true, Deviation = 2, _getFullyQualifiedName = this._getFullQualifiedName};
            ASTProgram program = new ASTProgram(setting, examples);

            List<IPosition> positions = new List<IPosition>();
            foreach (var example in examples)
            {
                ListNode input = example.Item2; //input and output are equal.
                for (int k = 0; k < input.List.Count; k++)
                {
                    positions.AddRange(program.GeneratePosition(input, k, false));
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
        public bool Indicator(IPredicate predicate, List<Tuple<ListNode, bool>> examples, Pos regex)
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



