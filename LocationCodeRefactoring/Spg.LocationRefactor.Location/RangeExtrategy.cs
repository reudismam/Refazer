using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using FastColoredTextBoxNS;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Spg.ExampleRefactoring.AST;
using Spg.ExampleRefactoring.Synthesis;
using Spg.LocationRefactor.TextRegion;

namespace LocationCodeRefactoring.Br.Spg.Location
{
    public class RangeExtrategy : Strategy
    {
        /*public List<Tuple<ListNode, ListNode>> Extract(List<TRegion> list)
        {
            List<Tuple<ListNode, ListNode>> examples = new List<Tuple<ListNode, ListNode>>();

            TRegion region = list[0];

            IEnumerable<MethodDeclarationSyntax> methods = ASTManager.SyntaxElements(region.Parent.Text);
            Dictionary<MethodDeclarationSyntax, Tuple<ListNode, ListNode>> methodsDic = new Dictionary<MethodDeclarationSyntax, Tuple<ListNode, ListNode>>();

            foreach (MethodDeclarationSyntax me in methods)
            {
                TextSpan span = me.Span;
                int start = span.Start;
                int length = span.Length;

                String method = region.Parent.Text.Substring(start, length);
                FastColoredTextBox textbox = new FastColoredTextBox();
                textbox.Text = region.Parent.Text;

                foreach (TRegion re in list)
                {
                    if (start <= re.Start && re.Start <= start + length)
                    {
                        Tuple<ListNode, ListNode> val = null;
                        Tuple<ListNode, ListNode> te = Example(me, re);
                        if (!methodsDic.TryGetValue(me, out val))
                        {
                            examples.Add(te);
                            methodsDic.Add(me, te);
                        }
                        else
                        {
                            val.Item2.List.AddRange(te.Item2.List);
                        }
                    }
                }
            }
            return examples;
        }*/


        /*/// <summary>
        /// Covert the region on a method to a example listnode
        /// </summary>
        /// <param name="me">Method</param>
        /// <param name="re">Region within the method</param>
        /// <returns>A example</returns>
        private Tuple<ListNode, ListNode> Example(MethodDeclarationSyntax me, TRegion re)
        {
            List<SyntaxNodeOrToken> list = new List<SyntaxNodeOrToken>();
            list = ASTManager.EnumerateSyntaxNodesAndTokens(me, list);
            ListNode listNode = new ListNode(list);

            TextSpan span = list[0].Span;

            int i = 0;
            while (re.Start > span.Start)
            {
                span = list[i++].Span;
            }

            int j = i - 1;
            while (re.Start + re.Length >= span.Start)
            {
                if (j == list.Count)
                    break;
                span = list[j++].Span;
            }

            ListNode subNodes = ASTManager.SubNotes(listNode, i - 1, (j - i));
            Tuple<ListNode, ListNode> t = Tuple.Create(listNode, subNodes);

            return t;
        }*/


        public override List<SyntaxNode> SyntaxNodes(string sourceCode)
        {
            throw new NotImplementedException();
        }
    }
}
