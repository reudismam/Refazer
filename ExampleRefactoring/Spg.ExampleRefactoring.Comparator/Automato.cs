using System;
using Microsoft.CodeAnalysis;
using Spg.ExampleRefactoring.Tok;

namespace Spg.ExampleRefactoring.Comparator
{
    /// <summary>
    /// Automaton representation
    /// </summary>
    public class Automato
    {
        /// <summary>
        /// Inconsistent state
        /// </summary>
        public const int Inconssistente = -1;
        /// <summary>
        /// Final state
        /// </summary>
        public const int Final = 0;
        /// <summary>
        /// Looking for next token state
        /// </summary>
        public const int Continue = 2;
        /// <summary>
        /// Start state
        /// </summary>
        public const int Start = 1;

        /// <summary>
        /// read token
        /// </summary>
        /// <returns>Get or set read token</returns>
        public TokenSeq r { get; set; }

        /// <summary>
        /// current token
        /// </summary>
        /// <returns>Get or set current token</returns>
        public Token current { get; set; }
        /// <summary>
        /// Next token
        /// </summary>
        /// <returns>Get or set next token</returns>
        public Token next { get; set; }

        private int indexNext;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="r">current token</param>
        public Automato(TokenSeq r)
        {
            this.r = r;
            Init();
        }

        /// <summary>
        /// Configure default state automaton
        /// </summary>
        private void Init()
        {
                next = r.Tokens[0];
                current = null;
                this.indexNext = 0;
        }

        /// <summary>
        /// Transition function
        /// </summary>
        /// <param name="node">Read node</param>
        /// <returns>Next state index</returns>
        public int Transition(SyntaxNodeOrToken node)
        {
            if (r.Tokens == null) throw new Exception("Tokens cannot be null");
           
            //Reach final state
            if (next == null && !current.Match(node))
            {
                Init();
                return Final;
            }

            //Current don't match node and next don't match node, returns inconsistent state
            if (current != null && next != null && !current.Match(node) && !next.Match(node))
            {
                Init();
                return Inconssistente;
            }

            //First matching detected, returns start state
            if (current == null && next.Match(node))
            {
                current = next;
                indexNext++;

                if (indexNext < r.Tokens.Count)
                {
                    next = r.Tokens[indexNext];
                }
                else
                {
                    next = null;
                }

                return Start;
            }

            //Transition detected
            if (next != null && next.Match(node))
            {
                current = next;
                indexNext++;

                if (indexNext < r.Tokens.Count)
                {
                    next = r.Tokens[indexNext];
                }
                else
                {
                    next = null;
                }
            }

            //no match occurred yet.
            if (current == null)
            {
                return Inconssistente;
            }

            return Continue;
        }
    }
}
