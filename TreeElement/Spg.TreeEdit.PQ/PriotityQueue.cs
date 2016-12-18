using C5;
using System;
using System.Collections.Generic;
using System.Linq;
using ProseFunctions.Substrings;
using TreeElement.Spg.Node;

namespace TreeEdit.Spg.TreeEdit.PQ
{
    public class PriorityQueue<T>
    {
        public IPriorityQueue<Tuple<int, TreeNode<T>>> pq { get; set; }

        public PriorityQueue()
        {
            var comp = new ComparerHeap();
            pq = new IntervalHeap<Tuple<int, TreeNode<T>>>(comp);
        }

        public int PeekMax()
        {
            if (pq.IsEmpty) return -1;

            return pq.FindMax().Item1;
        }

        public void Push(TreeNode<T> t)
        {
            int h = Height(t);
            Tuple<int, TreeNode<T>> tuple = Tuple.Create(h, t);
            pq.Add(tuple);
        }

        public int Height(TreeNode<T> t)
        {
            if (!t.Children.Any()) return 1;

            int max = 0;
            foreach (var i in t.Children)
            {
                max = Math.Max(max, Height(i));
            }

            return 1 + max;
        }

        public void Open(TreeNode<T> t1)
        {
            foreach (var item in t1.Children)
            {
                Push(item);
            }
        }

        public List<Tuple<int, TreeNode<T>>> Pop()
        {
            Tuple<int, TreeNode<T>> t = pq.FindMax();
            int top = t.Item1;

            List<Tuple<int, TreeNode<T>>> l = new List<Tuple<int, TreeNode<T>>>();
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

        class ComparerHeap : IComparer<Tuple<int, TreeNode<T>>>
        {
            public int Compare(Tuple<int, TreeNode<T>> x, Tuple<int, TreeNode<T>> y)
            {
                if (x.Item1 > y.Item1) return 1;

                if (x.Item1 == y.Item1 && x.Item2.Start > y.Item2.Start) return 1;

                if (x.Item1 == y.Item1 && x.Item2.Start > y.Item2.Start) return 0;

                return -1;
            }
        }
    }
}
