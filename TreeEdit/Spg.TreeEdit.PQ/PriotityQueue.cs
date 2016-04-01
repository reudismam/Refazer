using C5;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeEdit.Spg.TreeEdit.PQ
{
    public class PriorityQueue
    {
        public IPriorityQueue<Tuple<int, SyntaxNodeOrToken>> pq { get; set; }

        public PriorityQueue()
        {
            var comp = new ComparerHeap();
            pq = new IntervalHeap<Tuple<int, SyntaxNodeOrToken>>(comp);
        }

        public int PeekMax()
        {
            return pq.FindMax().Item1;
        }

        public void Push(SyntaxNodeOrToken t)
        {
            SyntaxNode sn = t.AsNode();

            if (sn == null) return;

            int h = Height(t);
            Tuple<int, SyntaxNodeOrToken> tuple = Tuple.Create(h, t);
            pq.Add(tuple);
        }

        public int Height(SyntaxNodeOrToken t)
        {
            SyntaxNode sn = t.AsNode();

            //if (sn == null) return 0;

            if (!sn.ChildNodes().Any()) return 1;

            int max = 0;
            foreach(var i in sn.ChildNodes())
            {
                max = Math.Max(max, Height(i));
            }

            return 1 + max;
        }

        public void Open(SyntaxNodeOrToken t1)
        {
            foreach (var item in t1.ChildNodesAndTokens())
            {
                if (item.AsNode() != null)
                {
                    SyntaxNode sn = item.AsNode();
                    Push(item);
                    //int dsc = sn.DescendantNodes().Count();
                    //Tuple<int, SyntaxNodeOrToken> t = Tuple.Create(dsc, item);
                    //pq.Add(t);
                }
            }
        }

        public List<Tuple<int, SyntaxNodeOrToken>> Pop()
        {
            Tuple<int, SyntaxNodeOrToken> t = pq.FindMax();
            int top = t.Item1;

            List<Tuple<int, SyntaxNodeOrToken>> l = new List<Tuple<int, SyntaxNodeOrToken>>();
            while(t.Item1 == top)
            {
                l.Add(t);
                pq.DeleteMax();

                if (!pq.IsEmpty)
                {
                    t = pq.FindMax();
                }
                else
                {
                    break;
                }
            }

            return l;
        }

        class ComparerHeap : IComparer<Tuple<int, SyntaxNodeOrToken>>
        {
            public int Compare(Tuple<int, SyntaxNodeOrToken> x, Tuple<int, SyntaxNodeOrToken> y)
            {
                if (x.Item1 > y.Item1) return 1;

                if (x.Item1 == y.Item1) return 0;

                return -1;
            }
        }
    }
}
