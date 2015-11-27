using System;
using System.Collections.Generic;
using Spg.ExampleRefactoring.Synthesis;
using Microsoft.CodeAnalysis;
using Spg.ExampleRefactoring.Tok;
using Spg.LocationRefactoring.Tok;
using System.Linq;

namespace Spg.LocationRefactoring.Tok
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
        /// <param name="tokens"></param>
        public TokenSeq(List<Token> tokens) {
            this.Tokens = tokens;
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
        public static List<Token> DymTokens(ListNode nodes, Dictionary<DymToken, List<DymToken>> dict)
        {
            List<Token> tokens = new List<Token>();
            foreach (SyntaxNodeOrToken st in nodes.List)
            {
                DymToken dtoken = new DymToken(st, false);
                RawDymToken rdtoken = new RawDymToken(st);
                if (dict.ContainsKey(dtoken))
                {
                    tokens.Add(dict[dtoken].First());
                }
                else if (dict.ContainsKey(rdtoken))
                {
                    tokens.Add(rdtoken);
                }
                else {
                    Token token = new Token(st);
                    tokens.Add(token);
                }
            }

            return tokens;
        }

        //public static List<List<Token>> DymTokens(ListNode nodes, Dictionary<DymToken, List<DymToken>> dict)
        //{
        //    List<Token> tokensDymToken = new List<Token>();
        //    List<Token> tokensRawDymToken = new List<Token>();
        //    foreach (SyntaxNodeOrToken st in nodes.List)
        //    {
        //        DymToken dtoken = new DymToken(st, false);
        //        RawDymToken rdtoken = new RawDymToken(st);
        //        if (dict.ContainsKey(dtoken))
        //        {
        //            tokensDymToken.Add(dict[dtoken].First());
        //            if (dict.ContainsKey(rdtoken))
        //            {
        //                tokensRawDymToken.Add(dict[rdtoken].First());
        //            }
        //        }
        //        else
        //        {
        //            Token token = new Token(st);
        //            tokensDymToken.Add(token);
        //            tokensRawDymToken.Add(token);
        //        }
        //    }

        //    return new List<List<Token>> { tokensDymToken, tokensRawDymToken };
        //}

        /// <summary>
        /// String representation
        /// </summary>
        /// <returns>String representation</returns>
        public override string ToString()
        {
            string str = "";
            if (Length() > 1)
            {
                int i;
                str += "TokenSeq(";
                for (i = 0; i < Tokens.Count - 1; i++)
                {
                    str += Tokens[i] + ", ";
                }
                str += Tokens[i] + ")";
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



