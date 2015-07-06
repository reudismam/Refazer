using System;
using Spg.ExampleRefactoring.AST;
using Spg.ExampleRefactoring.Synthesis;
using Spg.LocationRefactoring.Tok;

namespace Spg.LocationRefactor.Predicate
{
    /// <summary>
    /// Predicate contains
    /// </summary>
    public class NotContains: IPredicate
    {
        /// <summary>
        /// Evaluate regex
        /// </summary>
        /// <param name="input">Input</param>
        /// <param name="regex">Regex</param>
        /// <returns>True if input contains the regex</returns>
        public override bool Evaluate(ListNode input, TokenSeq regex)
        {
            bool isMatch = ASTManager.IsMatch(input, regex);
            return !isMatch;
        }

        public override string ToString()
        {
            return "Not Contains("+ base.r1 + base.r2 +", S), Split(R0, S)";
        }
    }
}

