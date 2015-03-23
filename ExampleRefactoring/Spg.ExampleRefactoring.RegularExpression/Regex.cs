using System;
using System.Collections.Generic;
using ExampleRefactoring.Spg.ExampleRefactoring.AST;
using ExampleRefactoring.Spg.ExampleRefactoring.Comparator;
using ExampleRefactoring.Spg.ExampleRefactoring.Synthesis;
using ExampleRefactoring.Spg.LocationRefactoring.Tok;
using Microsoft.CodeAnalysis;

namespace ExampleRefactoring.Spg.ExampleRefactoring.RegularExpression
{
    /// <summary>
    /// Regular expression
    /// </summary>
    public class Regex
    {

        /// <summary>
        /// All matches
        /// </summary>
        /// <returns>Matches</returns>
        public static bool IsMatch(ListNode input, TokenSeq regex)
        {
            List<Tuple<int, ListNode>> matches = new List<Tuple<int, ListNode>>();
            if (regex.Length() == 0) //equivalent to empty that matches everything
            {
                return true;
            }

            Automato aut = new Automato(regex);
            int signal = -1;
            for (int i = 0; i < input.Length(); i++)
            {
                SyntaxNodeOrToken node = input.List[i];
                signal = aut.Transition(node);

                if (signal == Automato.Final)
                {
                    return true;
                }
            }

            if ((signal == Automato.Start || signal == Automato.Continue) && aut.Next == null)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// All matches
        /// </summary>
        /// <returns>Matches</returns>
        public static List<Tuple<int, ListNode>> Matches(ListNode input, TokenSeq regex)
        {
            List<Tuple<int, ListNode>> matches = new List<Tuple<int, ListNode>>();
            if (regex.Length() == 0)
            {
                Tuple<int, ListNode> tuple = Tuple.Create(0, input);
                matches.Add(tuple);
                return matches;
            }

            int begin = -1;
            int length;

            Automato aut = new Automato(regex);
            int signal = -1;
            for(int i = 0; i < input.Length(); i++){
                SyntaxNodeOrToken node = input.List[i];
                signal = aut.Transition(node);

                switch(signal){
                    case Automato.Start:
                        begin = i; break;
                    case Automato.Inconssistente:
                        begin = -1; break;
                    case Automato.Final:
                        length = i - begin;
                        Tuple<int, ListNode> tuple = Tuple.Create(begin, ASTManager.SubNotes(input, begin, length));
                        matches.Add(tuple);
                        i --;
                        break;
                }
            }

            if ((signal == Automato.Start || signal == Automato.Continue) && aut.Next == null)
            {
                length = (input.Length()) - begin;
                Tuple<int, ListNode> tuple = Tuple.Create(begin, ASTManager.SubNotes(input, begin, length));
                matches.Add(tuple);
            }
            return matches;
            /*IEnumerator<SyntaxNodeOrToken> enumerator = input.List.GetEnumerator();
            //First token
            //Token firstNode = regex.Tokens[0];
            int i = 0;
            int match = -1;
            while (i < input.Length())
            {
                for (int j = 0; j < regex.Tokens.Count; j++)
                {
                    Token token = regex.Tokens[j];
                    IAutomato automato = AutomatoFactory.CreateAutomato(token);
                    ListNode nodes = ASTManager.SubNotes(input, i, input.Length() - i);
                    Tuple<int, ListNode> tuple = automato.Match(nodes);

                    if (tuple.Item1 == -1)
                    {
                        i++;
                        break;
                    }

                    if (j == 0)
                    {
                        match = i + tuple.Item1;
                    }


                    if ((j + 1) < regex.Tokens.Count && (i + 1) < input.Length())
                    {
                        Token nextToken = regex.Tokens[j + 1];
                        IAutomato nextAut = AutomatoFactory.CreateAutomato(nextToken);
                        ListNode nodesLeft = ASTManager.SubNotes(input, (i + 1), input.Length() - (i + 1));

                        Tuple<int, ListNode> nextTuple = nextAut.Match(nodesLeft);

                        if (nextTuple.Item1 == -1) { i++; break; }

                        i += Math.Min(tuple.Item2.Length(), nextTuple.Item2.Length());
                        continue;
                    }

                    ListNode nodeMatch = ASTManager.SubNotes(input, match, (i + tuple.Item2.Length()) - match);
                    Tuple<int, ListNode> tmatch = Tuple.Create(match, nodeMatch);
                    matches.Add(tmatch);
                    i += tuple.Item2.Length();
                }
            }*/

            /*while (i < input.Length())
            {
                if (firstNode.Comparer().IsEqual(input.List[i], firstNode.token))
                {
                    matches.Add(i);
                }

                ListNode lnodes = ASTManager.SubNotes(input, i, input.Length() - i);
                ListNode regexMatch = firstNode.Match(lnodes);
                if (regexMatch.List.Count > 0)
                {
                    i += regexMatch.List.Count();
                }
                else
                {
                    i++;
                }
            }

            matches = EvaluateMatches(input, regex, matches);*/

            //List<Tuple<int, ListNode>> tuples = new List<Tuple<int, ListNode>>();
            //return matches;
            //return tuples;
        }

        /*[Obsolete]
        /// <summary>
        /// Evaluate correct match
        /// </summary>
        /// <param name="input">Input</param>
        /// <param name="regex">Regex</param>
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
                        //i++;
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
