using System;
using ExampleRefactoring.Spg.LocationRefactoring.Tok;
using Microsoft.CodeAnalysis;
using Spg.ExampleRefactoring.Tok;

namespace ExampleRefactoring.Spg.ExampleRefactoring.Comparator
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
        public TokenSeq R { get; set; }

        /// <summary>
        /// current token
        /// </summary>
        /// <returns>Get or set current token</returns>
        public Token Current { get; set; }
        /// <summary>
        /// Next token
        /// </summary>
        /// <returns>Get or set next token</returns>
        public Token Next { get; set; }

        private int _indexNext;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="r">current token</param>
        public Automato(TokenSeq r)
        {
            this.R = r;
            Init();
        }

        /// <summary>
        /// Configure default state automaton
        /// </summary>
        private void Init()
        {
                Next = R.Tokens[0];
                Current = null;
                this._indexNext = 0;
        }

        /// <summary>
        /// Transition function
        /// </summary>
        /// <param name="node">Read node</param>
        /// <returns>Next state index</returns>
        public int Transition(SyntaxNodeOrToken node)
        {
            if (R.Tokens == null) throw new Exception("Tokens cannot be null");
           
            //Reach final state
            if (Next == null && !Current.Match(node))
            {
                Init();
                return Final;
            }

            //Current don't match node and next don't match node, returns inconsistent state
            if (Current != null && Next != null && !Current.Match(node) && !Next.Match(node))
            {
                Init();
                return Inconssistente;
            }

            //First matching detected, returns start state
            if (Current == null && Next.Match(node))
            {
                Current = Next;
                _indexNext++;

                if (_indexNext < R.Tokens.Count)
                {
                    Next = R.Tokens[_indexNext];
                }
                else
                {
                    Next = null;
                }

                return Start;
            }

            //Transition detected
            if (Next != null && Next.Match(node))
            {
                Current = Next;
                _indexNext++;

                if (_indexNext < R.Tokens.Count)
                {
                    Next = R.Tokens[_indexNext];
                }
                else
                {
                    Next = null;
                }
            }

            //no match occurred yet.
            if (Current == null)
            {
                return Inconssistente;
            }

            return Continue;
        }
    }
}
