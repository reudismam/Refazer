using C5;
using System;
using System.Collections.Generic;
using System.Linq;
using TreeElement.Spg.Node;

namespace TreeEdit.Spg.TreeEdit.PQ
{
    public class PriorityQueue<T>
    {
        public IPriorityQueue<Tuple<int, ITreeNode<T>>> pq { get; set; }

        public PriorityQueue()
        {
            var comp = new ComparerHeap();
            pq = new IntervalHeap<Tuple<int, ITreeNode<T>>>(comp);
        }

        public int PeekMax()
        {
            if (pq.IsEmpty) return -1;

            return pq.FindMax().Item1;
        }

        public void Push(ITreeNode<T> t)
        {
            int h = Height(t);
            Tuple<int, ITreeNode<T>> tuple = Tuple.Create(h, t);
            pq.Add(tuple);
        }

        public int Height(ITreeNode<T> t)
        {
            if (!t.Children.Any()) return 1;

            int max = 0;
            foreach (var i in t.Children)
            {
                max = Math.Max(max, Height(i));
            }

            return 1 + max;
        }

        public void Open(ITreeNode<T> t1)
        {
            foreach (var item in t1.Children)
            {
                Push(item);
            }
        }

        public List<Tuple<int, ITreeNode<T>>> Pop()
        {
            Tuple<int, ITreeNode<T>> t = pq.FindMax();
            int top = t.Item1;

            List<Tuple<int, ITreeNode<T>>> l = new List<Tuple<int, ITreeNode<T>>>();
            while (t.Item1 == top)
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

        class ComparerHeap : IComparer<Tuple<int, ITreeNode<T>>>
        {
            public int Compare(Tuple<int, ITreeNode<T>> x, Tuple<int, ITreeNode<T>> y)
            {
                if (x.Item1 > y.Item1) return 1;

                if (x.Item1 == y.Item1) return 0;

                return -1;
            }
        }
    }
}
