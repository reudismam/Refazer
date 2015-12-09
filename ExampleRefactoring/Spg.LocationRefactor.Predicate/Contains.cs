using Spg.ExampleRefactoring.AST;
using Spg.ExampleRefactoring.Position;
using Spg.ExampleRefactoring.Synthesis;
using Spg.LocationRefactoring.Tok;

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
        public override bool Evaluate(ListNode input, Pos regex)
        {
            int match = regex.GetPositionIndex(input);
            bool isMatch = match != -1;
            return isMatch;
        }

        /// <summary>
        /// String representation 
        /// </summary>
        /// <returns>String representation</returns>
        public override string ToString()
        {
            TokenSeq comb = ASTProgram.ConcatenateRegularExpression(regex.R1, regex.R2);
            return "Contains(x, " + comb +")";
        }
    }
}


