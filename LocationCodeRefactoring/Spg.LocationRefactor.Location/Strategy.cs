using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Spg.ExampleRefactoring.AST;
using Spg.ExampleRefactoring.Synthesis;
using Spg.LocationRefactor.TextRegion;

namespace LocationCodeRefactoring.Br.Spg.Location
{
    public abstract class Strategy
    {
        public List<Tuple<ListNode, ListNode>> Extract(List<TRegion> list)
        {
            List<Tuple<ListNode, ListNode>> examples = new List<Tuple<ListNode, ListNode>>();

            TRegion region = list[0];
            List<SyntaxNode> statements = SyntaxNodes(region.Parent.Text);
            Dictionary<SyntaxNode, Tuple<ListNode, ListNode>> methodsDic = new Dictionary<SyntaxNode, Tuple<ListNode, ListNode>>();
            Dictionary<TRegion, SyntaxNode> pairs = ChoosePairs(statements, list);
            foreach (KeyValuePair<TRegion, SyntaxNode> pair in pairs)
            {
                TRegion re = pair.Key;
                SyntaxNode node = pair.Value;
                //foreach (SyntaxNode node in statements)
                //{
                //    TextSpan span = node.Span;
                //    int start = span.Start;
                //    int length = span.Length;
                //    foreach (TRegion re in list)
                //    {
                //        if (start <= re.Start && re.Start <= start + length)
                //        {
                Tuple<ListNode, ListNode> val = null;
                Tuple<ListNode, ListNode> te = Example(node, re);
                if (!methodsDic.TryGetValue(node, out val))
                {
                    examples.Add(te);
                    methodsDic.Add(node, te);
                }
                else
                {
                    val.Item2.List.AddRange(te.Item2.List);
                }
            }
            //        }
            //    }
            //}
            return examples;
        }

        /// <summary>
        /// Choose corresponding syntax node for the region
        /// </summary>
        /// <param name="statements">Statement list</param>
        /// <param name="region">Region</param>
        /// <returns></returns>
        private Dictionary<TRegion, SyntaxNode> ChoosePairs(List<SyntaxNode> statements, List<TRegion> regions)
        {
            Dictionary<TRegion, SyntaxNode> dicRegions = new Dictionary<TRegion, SyntaxNode>();

            foreach (SyntaxNode statment in statements)
            {
                foreach (TRegion region in regions)
                {
                    string text = region.Text;
                    string pattern = System.Text.RegularExpressions.Regex.Escape(text);
                    string statmentText = statment.GetText().ToString();
                    bool contains = System.Text.RegularExpressions.Regex.IsMatch(statmentText, pattern);
                    if (contains)
                    {
                        if (statment.SpanStart <= region.Start && region.Start <= statment.SpanStart + statment.Span.Length)
                        {
                            if (!dicRegions.ContainsKey(region))
                            {
                                dicRegions.Add(region, statment);
                            }
                            else if (statment.GetText().Length < dicRegions[region].GetText().Length)
                            {
                                dicRegions[region] = statment;
                            }
                        }
                    }
                }
            }
            return dicRegions;
        }

        public List<Tuple<String, String>> ExtractText(List<TRegion> list)
        {
            List<Tuple<String, String>> examples = new List<Tuple<String, String>>();
            TRegion region = list[0];
            List<SyntaxNode> statements = SyntaxNodes(region.Parent.Text);
            Dictionary<String, String> methodsDic = new Dictionary<String, String>();
            Dictionary<TRegion, SyntaxNode> pairs = ChoosePairs(statements, list);
            foreach (KeyValuePair<TRegion, SyntaxNode> pair in pairs)
            {
                TRegion re = pair.Key;
                SyntaxNode statment = pair.Value;

                //foreach (SyntaxNode statement in statements)
                //{
                //    TextSpan span = statement.Span;
                //    int start = span.Start;
                //    int length = span.Length;
                //    foreach (TRegion re in list)
                //    {
                //        if (start <= re.Start && re.Start <= start + length)
                //        {
                String value = null;
                if (!methodsDic.TryGetValue(statment.GetText().ToString(), out value))
                {
                    Tuple<String, String> tuple = Tuple.Create(re.Text, statment.GetText().ToString());
                    examples.Add(tuple);
                    methodsDic.Add(statment.GetText().ToString(), value);
                }
                //}
                //    }
                //}
            }
            return examples;
        }

        public abstract List<SyntaxNode> SyntaxNodes(string sourceCode);

        /// <summary>
        /// Covert the region on a method to an example ListNode
        /// </summary>
        /// <param name="me">Method</param>
        /// <param name="re">Region within the method</param>
        /// <returns>A example</returns>
        private Tuple<ListNode, ListNode> Example(SyntaxNode me, TRegion re)
        {
            List<SyntaxNodeOrToken> list = new List<SyntaxNodeOrToken>();
            list = ASTManager.EnumerateSyntaxNodesAndTokens(me, list);
            ListNode listNode = new ListNode(list);
            listNode.OriginalText = me.GetText().ToString();

            //TextSpan span = list[0].Span;
            SyntaxNodeOrToken node = list[0];

            int i = 0;
            while (re.Start > node.Span.Start)
            {
                node = list[i++];
            }

            int j = i;
            while (re.Start + re.Length >= node.Span.Start)
            {
                if (j == list.Count)
                    break;
                node = list[Math.Max(j++, 0)];
            }

            if (i == 0 && j == 0)
            {
                j = list.Count;
            }

            ListNode subNodes = ASTManager.SubNotes(listNode, Math.Max(i - 1, 0), ((j) - i));
            subNodes.OriginalText = re.Text;
            Tuple<ListNode, ListNode> t = Tuple.Create(listNode, subNodes);

            return t;
        }
    }
}