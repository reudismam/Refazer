using Spg.ExampleRefactoring.AST;
using Spg.ExampleRefactoring.Synthesis;
using Spg.ExampleRefactoring.Tok;
using System;
using ExampleRefactoring.Spg.ExampleRefactoring.AST;
using ExampleRefactoring.Spg.ExampleRefactoring.Synthesis;
using ExampleRefactoring.Spg.LocationRefactoring.Tok;

namespace Spg.LocationRefactor.Predicate
{
    /// <summary>
    /// Predicate contains
    /// </summary>
    public class Contains: IPredicate
    {
        /// <summary>
        /// Evaluate regex
        /// </summary>
        /// <param name="input">Input</param>
        /// <param name="regex">Regex</param>
        /// <returns>True if input contains the regex</returns>
        public override Boolean Evaluate(ListNode input, TokenSeq regex)
        {
            Boolean isMatch = ASTManager.IsMatch(input, regex);
            return isMatch;
        }

        public override string ToString()
        {
            return "Contains("+ base.r1 + base.r2 +", S), Split(R0, S)";
        }
    }
}
