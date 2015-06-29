using Spg.ExampleRefactoring.AST;
using Spg.ExampleRefactoring.Synthesis;
using Spg.ExampleRefactoring.Tok;
using System;
using System.Collections.Generic;
using ExampleRefactoring.Spg.ExampleRefactoring.AST;
using ExampleRefactoring.Spg.ExampleRefactoring.Synthesis;
using ExampleRefactoring.Spg.LocationRefactoring.Tok;

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
        public override bool Evaluate(ListNode input, TokenSeq regex) {
            bool isMatch = ASTManager.IsMatch(input, regex);
            return isMatch;
        }

        /// <summary>
        /// Regex
        /// </summary>
        /// <returns>Token list</returns>
        public override List<Token> Regex()
        {
            return base.Regex();
        }

        public override string ToString()
        {
            return "EndSeqPos(" + r1 + ", " + r2 + " x), Split(R0, \\n)";
        }
    }
}


