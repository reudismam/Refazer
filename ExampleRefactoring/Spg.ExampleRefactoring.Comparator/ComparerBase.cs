using System;
using System.Collections.Generic;
using ExampleRefactoring.Spg.ExampleRefactoring.Synthesis;
using Microsoft.CodeAnalysis;

namespace Spg.ExampleRefactoring.Comparator
{
    /// <summary>
    /// Comparer base class
    /// </summary>
    public abstract class ComparerBase
    {
        /// <summary>
        /// Matches of subNodes on input ListNode
        /// </summary>
        /// <param name="input">ListNode</param>
        /// <param name="subNodes">Substring</param>
        /// <returns>Match indexes</returns>
        public virtual List<int> Matches(ListNode input, ListNode subNodes)
        {
            if(input == null || subNodes == null){
                throw new Exception("Input or subnodes cannot be null");
            }

            if (subNodes.List.Count == 0)
            {
                throw new Exception("subNodes cannot be empty");
            }

            List<int> matches = new List<int>();
            SyntaxNodeOrToken firstNode = subNodes.List[0];
            for (int i = 0; i < input.Length(); i++)
            {
                if (IsEqual(input.List[i], firstNode))
                {
                    matches.Add(i);
                }
            }
            matches = EvaluateMatches(input, subNodes, matches);
            return matches;
        }

       /* /// <summary>
        /// Regex matches
        /// </summary>
        /// <param name="input">Input nodes</param>
        /// <param name="regex">Regex</param>
        /// <returns>Match starts</returns>
        public virtual List<int> Matches(ListNode input, TokenSeq regex)
        {
            List<int> matches = new List<int>();

            IEnumerator<SyntaxNodeOrToken> enumerator = input.List.GetEnumerator();

            //First token
            Token firstNode = regex.Tokens[0];
            for (int i = 0; i < input.Length(); i++)
            {
                if (firstNode.Comparer().IsEqual(input.List[i], firstNode.token))
                {
                    matches.Add(i);
                }
            }

            matches = EvaluateMatches(input, regex, matches);
            return matches;
        }*/

        private List<int> EvaluateMatches(ListNode input, ListNode subNodes, List<int> matches)
        {
            List<int> removes = new List<int>();
            foreach (int match in matches)
            {
                //Is not a match because the match is out of bound of 
                //the input string.
                if (match + subNodes.Length() > input.Length())
                {
                    removes.Add(match);
                }
                //Evalute if the sequence of syntax nodes is on the input.
                else
                {
                    bool isMatch = true;
                    int i = 0;
                    foreach (SyntaxNodeOrToken node in subNodes.List)
                    {
                        if (!IsEqual(input.List[match + i], node))
                        {
                            isMatch = false;
                            break;
                        }
                        i++;
                    }

                    if (!isMatch)
                    {
                        removes.Add(match);
                    }
                }
            }

            matches.RemoveAll(i => removes.Contains(i));
            return matches;
        }

        /*private List<int> EvaluateMatches(ListNode input, TokenSeq subNodes, List<int> matches)
        {
            List<int> removes = new List<int>();
            foreach (int match in matches)
            {
                //Is not a match because the match is out of bound of 
                //the input string.
                if (match + subNodes.Tokens.Count() > input.Length())
                {
                    removes.Add(match);
                }
                //Evalute if the sequence of syntax nodes is on the input.
                else
                {
                    Boolean isMatch = true;
                    int i = 0;
                    foreach(Token node in subNodes.Tokens)
                    {
                        if (!node.Comparer().IsEqual(input.List[match + i], node.token))
                        {
                            isMatch = false;
                            break;
                        }
                        i++;
                    }

                    if (!isMatch)
                    {
                        removes.Add(match);
                    }
                }
            }

            matches.RemoveAll(i => removes.Contains(i));
            return matches;
        }*/

        /// <summary>
        /// Is Equal
        /// </summary>
        /// <param name="first">Syntax node or token</param>
        /// <param name="regex">Regex token</param>
        /// <returns>True if first is equal to regex</returns>
        public bool IsEqual(SyntaxNodeOrToken first, SyntaxNodeOrToken regex)
        {
            return Match(first, regex);
        }

        /// <summary>
        /// Two sequences are equals
        /// </summary>
        /// <param name="seq1">First sequence</param>
        /// <param name="seq2">Second sequence</param>
        /// <returns>True if seq1 and seq2 are equal</returns>
        public bool SequenceEqual(ListNode seq1, ListNode seq2)
        {
            if (seq1 == null || seq2 == null) return false;

            //List size are different
            if (seq1.Length() != seq2.Length())
            {
                return false;
            }

            for (int i = 0; i < seq1.Length(); i++)
            {
                if (!IsEqual(seq1.List[i], seq2.List[i]))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// First and second syntax node or token are equal
        /// </summary>
        /// <param name="first">First sequence</param>
        /// <param name="second">Second sequence</param>
        /// <returns>True if first and second syntax node or token are equal</returns>
        public abstract bool Match(SyntaxNodeOrToken first, SyntaxNodeOrToken second);
    }
}


