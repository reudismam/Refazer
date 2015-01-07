using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Spg.ExampleRefactoring.AST;

namespace LocationCodeRefactoring.Br.Spg.Location
{
    public class StatementStrategy : Strategy
    {
        /*public List<Tuple<ListNode, ListNode>> Extract(List<TRegion> list)
        {
            List<Tuple<ListNode, ListNode>> examples = new List<Tuple<ListNode, ListNode>>();

            TRegion region = list[0];

            List<SyntaxNode> statements = ASTManager.SyntaxElements(region.Parent.Text, SyntaxKind.IfStatement);
            Dictionary<SyntaxNode, Tuple<ListNode, ListNode>> methodsDic = new Dictionary<SyntaxNode, Tuple<ListNode, ListNode>>();
            foreach(SyntaxNode me in statements)
            {
                TextSpan span = me.Span;
                int start = span.Start;
                int length = span.Length;
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

        /*  /// <summary>
          /// Covert the region on a method to a example listnode
          /// </summary>
          /// <param name="me">Method</param>
          /// <param name="re">Region within the method</param>
          /// <returns>A example</returns>
          private Tuple<ListNode, ListNode> Example(SyntaxNode me, TRegion re)
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
            return ASTManager.SyntaxElements(sourceCode, SyntaxKind.IfStatement);
        }
    }
}
