using Microsoft.CodeAnalysis.CSharp;
using Spg.ExampleRefactoring.Synthesis;
using Spg.LocationRefactor.Operator;
using Spg.LocationRefactor.Program;
using Spg.LocationRefactor.TextRegion;
using System;
using System.Collections.Generic;

namespace Spg.LocationRefactor.Learn
{
    /// <summary>
    /// Learner
    /// </summary>
    public class Learner
    {
        /// <summary>
        /// Map
        /// </summary>
        /// <returns>map</returns>
        public MapLearnerBase map { get; set; }

        /// <summary>
        /// Syntax kind
        /// </summary>
        /// <returns>Syntax kind</returns>
        public SyntaxKind syntaxKind { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="syntaxKind"></param>
        public Learner(SyntaxKind syntaxKind)
        {
            this.syntaxKind = syntaxKind;
            map = new StatementMapLearner(syntaxKind);
        }

        /// <summary>
        /// Learn a region sequence
        /// </summary>
        /// <param name="examples">Examples</param>
        /// <returns>Region sequence learners</returns>
        public List<Prog> LearnSeqRegion(List<Tuple<ListNode, ListNode>> examples)
        {
            List<Prog> programs = new List<Prog>();
            List<IOperator> operators = new List<IOperator>();
            List<ILearn> learns = new List<ILearn>();

            learns.Add(map);
            foreach (ILearn learn in learns)
            {
                programs = learn.Learn(examples);
            }
            return programs;
        }

        /// <summary>
        /// Tuples (Statement, Selection)
        /// </summary>
        /// <param name="list">User selection regions</param>
        /// <returns>Examples</returns>
        public List<Tuple<ListNode, ListNode>> Decompose(List<TRegion> list)
        {
            List<Tuple<ListNode, ListNode>> decomposition = map.Decompose(list);
            return decomposition;
        }

        /// <summary>
        /// Learn a region
        /// </summary>
        /// <param name="examples">Examples</param>
        /// <returns>Region learn</returns>
        public List<Prog> LearnRegion(List<Tuple<ListNode, ListNode>> examples) {
            PairLearn pair = new PairLearn();
            return pair.Learn(examples);
        }
    }
}
