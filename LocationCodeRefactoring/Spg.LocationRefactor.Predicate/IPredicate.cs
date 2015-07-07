using System.Collections.Generic;
using Spg.ExampleRefactoring.Synthesis;
using Spg.ExampleRefactoring.Tok;
using Spg.LocationRefactoring.Tok;

namespace Spg.LocationRefactor.Predicate
{
    /// <summary>
    /// IPredicate
    /// </summary>
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
        public abstract bool Evaluate(ListNode input, TokenSeq regex);

        /// <summary>
        /// Evalute input
        /// </summary>
        /// <param name="input">Input data</param>
        /// <returns>Evaluation of regex for this input</returns>
        public bool Evaluate(ListNode input)
        {
            TokenSeq regex = ASTProgram.ConcatenateRegularExpression(r1, r2);
            return Evaluate(input, regex);
        }

        /// <summary>
        /// Regex
        /// </summary>
        /// <returns>Regex</returns>
        public virtual List<Token> Regex()
        {
            TokenSeq tokens = ASTProgram.ConcatenateRegularExpression(r1, r2);
            return tokens.Regex();
        }

        /// <summary>
        /// Calculate if object is equal to this object
        /// </summary>
        /// <param name="obj">Another object</param>
        /// <returns>True if obj is equal to this object</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is IPredicate)) { return false; }

            IPredicate other = obj as IPredicate;

            if (r1 == null || r2 == null || other.r1 == null || other.r2 == null) return false;

            return r1.Equals(other.r1) && r2.Equals(other.r2);
        }

        /// <summary>
        /// Hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}

