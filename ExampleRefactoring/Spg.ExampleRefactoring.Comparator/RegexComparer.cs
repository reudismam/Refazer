using System;
using System.Collections.Generic;
using System.Linq;
using ExampleRefactoring.Spg.ExampleRefactoring.AST;
using ExampleRefactoring.Spg.ExampleRefactoring.RegularExpression;
using ExampleRefactoring.Spg.ExampleRefactoring.Synthesis;
using ExampleRefactoring.Spg.LocationRefactoring.Tok;
using Microsoft.CodeAnalysis;
using Spg.ExampleRefactoring.AST;
using Spg.ExampleRefactoring.Synthesis;
using Spg.ExampleRefactoring.Tok;

namespace Spg.ExampleRefactoring.Comparator
{
    /// <summary>
    /// Regular expression comparer
    /// </summary>
    public class RegexComparer:ComparerBase
    {
        /// <summary>
        /// First and second syntax node or token type are equal
        /// </summary>
        /// <param name="first">First syntax node or token</param>
        /// <param name="second">second syntax node or token</param>
        /// <returns>true if first and second syntax node or token type are equal</returns>
        public override bool Match(SyntaxNodeOrToken first, SyntaxNodeOrToken second)
        {
            if(first == null)  throw new ArgumentNullException("first");
            if (second == null) throw new ArgumentNullException("second");

            if (second.AsNode() == null && second.AsNode() != null)
            {
                return ASTManager.Parent(first).RawKind == second.RawKind;
            }

            if (second.AsNode() != null && second.AsNode() == null)
            {
                return first.RawKind == ASTManager.Parent(second).RawKind;
            }

            return first.RawKind == second.RawKind;
           
        }

        /// <summary>
        /// Regular expression matches
        /// </summary>
        /// <param name="input">Input nodes</param>
        /// <param name="regex">Regular expression</param>
        /// <returns>Match starts</returns>
        public virtual List<Tuple<int, ListNode>> Matches(ListNode input, TokenSeq regex)
        {
            if(input == null) throw new ArgumentNullException("input");
            if (regex == null) throw new ArgumentNullException("regex");

            if (regex.Tokens == null)
            {
                throw new Exception("Regular expression must have at least one token");
            }

            /*List<Tuple<int, ListNode>> matches = new List<Tuple<int, ListNode>>();
            if (regex.Length() == 0)
            {
                Tuple<int, ListNode> emptyTuple = Tuple.Create(0, input);
                matches.Add(emptyTuple);
                return matches;
            }

            Token firstToken = regex.Tokens[0];
            int i = 0;
            while(i < input.Length())
            {
                if(firstToken.Comparer().IsEqual(input.List[i], firstToken.token))
                {
                    matches.Add(i);
                }

                ListNode lnodes = ASTManager.SubNotes(input, i, input.Length() - i);
                ListNode regexMatch = firstToken.Match(lnodes);
                if (regexMatch.List.Count > 0)
                {
                    i += regexMatch.List.Count();
                }
                else {
                    i++;
                }
            }

            matches = EvaluateMatches(input, regex, matches);
            List<Tuple<int, ListNode>> rMatches = new List<Tuple<int, ListNode>>();
            List<Tuple<int, ListNode>> mnodes = Regex.Matches(input, regex);
            foreach (Tuple<int, ListNode> tuple in mnodes)
            {
                rMatches.Add(tuple);
            }
            return rMatches;*/
            List<Tuple<int, ListNode>> mnodes = Regex.Matches(input, regex);
            return mnodes;
        }

        /*/// <summary>
        /// Evaluate correct match
        /// </summary>
        /// <param name="input">Input</param>
        /// <param name="regex">Regular expression</param>
        /// <param name="matches">Matches</param>
        /// <returns>Correct matches</returns>
        private static List<int> EvaluateMatches(ListNode input, TokenSeq regex, List<int> matches)
        {
            List<int> removes = new List<int>();
            foreach (int match in matches)
            {
                if (match + regex.Tokens.Count() > input.Length())
                {
                    removes.Add(match);
                }
                else
                {
                    Boolean isMatch = true;
                    int i = 0;
                    for (int j = 0; j < regex.Tokens.Count; j++)
                    {
                        Token regexNode = regex.Tokens[j];
                        if (i < regex.Tokens.Count && !regexNode.Comparer().IsEqual(input.List[match + i], regexNode.token))
                        {
                            isMatch = false;
                            break;
                        }

                        ListNode lnodes = ASTManager.SubNotes(input, (match + i), input.Length() - (match + i));
                        ListNode regexMatch = regexNode.Match(lnodes);

                        int regexSize = regexMatch.List.Count;

                        int deltaToNext = regexSize;

                        if (j + 1 < regex.Tokens.Count())
                        {
                            List<Token> tokens = new List<Token>();
                            tokens.Add(regex.Tokens[j + 1]);
                            TokenSeq tseq = new TokenSeq(tokens);
                            var mats = new RegexComparer().Matches(regexMatch, tseq);

                            if (mats.Count > 0)
                            {
                                deltaToNext = Math.Max(mats[0], 1);
                            }
                        }
                    
                        i += deltaToNext;
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
    }
}

