using Spg.ExampleRefactoring.Synthesis;
using Spg.ExampleRefactoring.Tok;
using System;
using System.Collections.Generic;

namespace Spg.LocationRefactor.Predicate
{
    public abstract class IPredicate
    {
        /// <summary>
        /// Second regular expression
        /// </summary>
        public TokenSeq r1 { get; set; }

        /// <summary>
        /// First regular expression
        /// </summary>
        public TokenSeq r2 { get; set; }

        /// <summary>
        /// Evaluate
        /// </summary>
        /// <param name="input">Source code</param>
        /// <param name="regex">Regex</param>
        /// <returns>Evaluation</returns>
        public abstract Boolean Evaluate(ListNode input, TokenSeq regex);

        /// <summary>
        /// Regex
        /// </summary>
        /// <returns>Regex</returns>
        public virtual List<Token> Regex()
        {
            TokenSeq tokens = ASTProgram.ConcatenateRegularExpression(r1, r2);
            return tokens.Regex();
        }
    }
}
