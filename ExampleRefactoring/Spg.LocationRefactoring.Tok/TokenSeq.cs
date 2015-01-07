using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Spg.ExampleRefactoring.Synthesis;
using Spg.LocationRefactoring.Tok;

namespace Spg.ExampleRefactoring.Tok
{
    /// <summary>
    /// Token sequence
    /// </summary>
    public class TokenSeq
    {
        
        /// <summary>
        /// Token list
        /// </summary>
        /// <returns>Get or set token list</returns>
        public List<Token> Tokens { get; set;}
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Tokens"></param>
        public TokenSeq(List<Token> Tokens) {
            this.Tokens = Tokens;
        }


        /// <summary>
        /// Converts the LisNode object to a list of tokens.
        /// </summary>
        /// <param name="nodes">Nodes</param>
        /// <returns>List of tokens</returns>
        public static List<Token> GetTokens(ListNode nodes) {
            List<Token> tokens = new List<Token>();

            foreach(SyntaxNodeOrToken st in nodes.List){
                Token token = new Token(st);
                tokens.Add(token);
            }

            return tokens;
        }

        /// <summary>
        /// Regular expression list
        /// </summary>
        /// <returns>Regular expression list</returns>
        public List<Token> Regex()
        {
            return Tokens;
        }

        /// <summary>
        /// TokenSeq length
        /// </summary>
        /// <returns>TokenSeq length</returns>
        public int Length(){
            return Tokens.Count;
        }

        /// <summary>
        /// List nodes to dynamic tokens
        /// </summary>
        /// <param name="nodes">Nodes</param>
        /// <param name="dict">Dictionary with previous dynamic tokens</param>
        /// <returns>Dynamic tokens</returns>
        public static List<Token> DymTokens(ListNode nodes, Dictionary<DymToken, int> dict)
        {
            List<Token> tokens = new List<Token>();

            foreach (SyntaxNodeOrToken st in nodes.List)
            {
                DymToken dtoken = new DymToken(st);
                if (dict.ContainsKey(dtoken))
                {
                    tokens.Add(dtoken);
                }
                else {
                    Token token = new Token(st);
                    tokens.Add(token);
                }
            }

            return tokens;
        }

        /// <summary>
        /// String representation
        /// </summary>
        /// <returns>String representation</returns>
        public override string ToString()
        {
            String str = "";
            if (Length() > 1)
            {
                int i = 0;

                str += "TokenSeq(";
                for (i = 0; i < Tokens.Count - 1; i++)
                {
                    str += Tokens[i].ToString() + ", ";
                }
                str += Tokens[i].ToString() + ")";
            }
            else
            {
                if (Length() == 1)
                {
                    str += Tokens[0].ToString();
                }
            }

            return str;
        }

        /// <summary>
        /// Equals 
        /// </summary>
        /// <param name="obj">Another object to be compared</param>
        /// <returns>True is object obj is equals to this instance</returns>
        public override bool Equals(object obj)
        {
            if(!(obj is TokenSeq))
            {
                return false;
            }

            TokenSeq another = obj as TokenSeq;

            if (another.Length() != this.Length())
            {
                return false;
            }

            bool equal = true;

            for (int i = 0; i < this.Length(); i++)
            {
                if (!this.Tokens[i].Equals(another.Tokens[i]))
                {
                    return false;
                }
            }

            return equal;
        }

        /// <summary>
        /// Hash code
        /// </summary>
        /// <returns>Hash code for this object</returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}
