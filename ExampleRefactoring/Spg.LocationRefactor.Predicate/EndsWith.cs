using System.Collections.Generic;
using Spg.ExampleRefactoring.AST;
using Spg.ExampleRefactoring.Synthesis;
using Spg.ExampleRefactoring.Tok;
using Spg.LocationRefactoring.Tok;
using Spg.ExampleRefactoring.Position;

namespace Spg.LocationRefactor.Predicate
{
    public class EndsWith: IPredicate
    {
        /// <summary>
        /// Evaluate regex
        /// </summary>
        /// <param name="input">Source code</param>
        /// <param name="regex">Regex</param>
        /// <returns>Evaluation</returns>
        public override bool Evaluate(ListNode input, Pos regex) {
            int match = regex.GetPositionIndex(input);
            bool isMatch = match != -1;
            return isMatch;
        }

        /// <summary>
        /// String representation of this object
        /// </summary>
        /// <returns>String representation of this object</returns>
        public override string ToString()
        {
            return "EndSeqPos(" + regex + " x), Split(R0, \\n)";
        }
    }
}



