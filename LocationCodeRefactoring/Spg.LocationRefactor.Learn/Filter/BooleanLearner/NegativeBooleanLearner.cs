using System.Collections.Generic;
using ExampleRefactoring.Spg.LocationRefactoring.Tok;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Spg.ExampleRefactoring.Tok;
using Spg.LocationRefactor.Predicate;
using Spg.LocationRefactoring.Tok;

namespace LocationCodeRefactoring.Spg.LocationRefactor.Learn.Filter.BooleanLearner
{
    /// <summary>
    /// Negative boolean learner
    /// </summary>
    public class NegativeBooleanLearner :BooleanLearnerBase
    {

        public NegativeBooleanLearner(Dictionary<TokenSeq, bool> calculated) : base(calculated)
        {
        }
        /// <summary>
        /// IPredicate for this boolean learner
        /// </summary>
        /// <returns></returns>
        protected override IPredicate GetPredicate()
        {
            return PredicateFactory.CreateInv(new Contains());
        }

        /// <summary>
        /// Token sequence for this boolean learner
        /// </summary>
        /// <param name="r1">Token sequence</param>
        /// <returns>Return token sequence with string token replaced by SubStrToken.</returns>
        protected override TokenSeq GetTokenSeq(TokenSeq r1)
        {
            return CreateSubStrToken(r1);
        }

        /// <summary>
        /// Create sub string token
        /// </summary>
        /// <param name="r1">Token sequence</param>
        /// <returns>A new token sequence with string literal token replaced by sub string token</returns>
        private TokenSeq CreateSubStrToken(TokenSeq r1)
        {
            List<Token> tokens = new List<Token>();
            for (int i = 0; i < r1.Tokens.Count; i++)
            {
                var token = r1.Tokens[i];
                if (token.token.IsKind(SyntaxKind.StringLiteralToken))
                {
                    Token substrToken = new SubStrToken(token.token);
                    tokens.Add(substrToken);
                    continue;
                }
                tokens.Add(token);
            }

            TokenSeq clone = new TokenSeq(tokens);
            return clone;
        }

    }
}
