using System;
using System.Collections.Generic;
using System.Linq;
using TreeEdit.Spg.TreeEdit.PQ;
using Spg.TreeEdit.Node;
using TreeEdit.Spg.TreeEdit.Isomorphic;
using Tutor;

namespace TreeEdit.Spg.TreeEdit.Mapping
{
    public class GumTreeMapping<T> : ITreeMapping<T>
    {
        /// <summary>
        /// Top-down phase from GumTree algorithm
        /// </summary>
        /// <param name="t1">First rooted tree.</param>
        /// <param name="t2">Second rooted tree.</param>
        /// <returns>Mapping from the nodes from t1 and t2.</returns>
        public Dictionary<ITreeNode<T>, ITreeNode<T>> TopDown(ITreeNode<T> t1, ITreeNode<T> t2)
        {
            var M = new Dictionary<ITreeNode<T>, ITreeNode<T>>();
            var A = new Dictionary<ITreeNode<T>, ITreeNode<T>>();

            var l1 = new PriorityQueue<T>();
            var l2 = new PriorityQueue<T>();
            l1.Push(t1);
            l2.Push(t2);

            int minH = 1;

            while (l1.PeekMax() > minH && l2.PeekMax() > minH)
            {
                if (l1.PeekMax() != l2.PeekMax())
                {
                    if (l1.PeekMax() > l2.PeekMax())
                    {
                        foreach (var item in l1.Pop())
                        {
                            l1.Open(item.Item2);
                        }
                    }
                    else
                    {
                        foreach (var item in l2.Pop())
                        {
                            l2.Open(item.Item2);
                        }
                    }
                }
                else
                {
                    List<Tuple<int, ITreeNode<T>>> h1 = l1.Pop();
                    List<Tuple<int, ITreeNode<T>>> h2 = l2.Pop();

                    var dict1 = new Dictionary<ITreeNode<T>, List<ITreeNode<T>>>();
                    var dict2 = new Dictionary<ITreeNode<T>, List<ITreeNode<T>>>();
                    foreach (var item1 in h1)
                    {
                        if (!dict1.ContainsKey(item1.Item2)) dict1.Add(item1.Item2, new List<ITreeNode<T>>());
                        foreach (var item2 in h2)
                        {
                            if (!dict2.ContainsKey(item2.Item2)) dict2.Add(item2.Item2, new List<ITreeNode<T>>());

                            if (IsomorphicManager<T>.IsIsomorphic(item1.Item2, item2.Item2))
                            {
                                dict1[item1.Item2].Add(item2.Item2);
                                dict2[item2.Item2].Add(item1.Item2);
                            }
                        }
                    }

                    foreach (var item1 in h1)
                    {
                        foreach (var item2 in h2)
                        {
                            if (IsomorphicManager<T>.IsIsomorphic(item1.Item2, item2.Item2))
                            {
                                if (dict1[item1.Item2].Count > 1 || dict2[item2.Item2].Count > 1)
                                {
                                    A.Add(item1.Item2, item2.Item2);
                                }
                                else
                                {
                                    var dict = IsomorphicManager<T>.AllPairOfIsomorphic(item1.Item2, item2.Item2);
                                    foreach (var v in dict)
                                    {
                                        M.Add(v.Key, v.Value);
                                    }
                                }
                            }
                        }
                    }

                    foreach (var item1 in h1)
                    {
                        if (!dict1[item1.Item2].Any())
                        {
                            l1.Open(item1.Item2);
                        }
                    }

                    foreach (var item2 in h2)
                    {
                        if (!dict2[item2.Item2].Any())
                        {
                            l2.Open(item2.Item2);
                        }
                    }
                }
            }

            var sortList = A.ToList();
            sortList = sortList.OrderByDescending(o => Dice(o.Key, o.Value, M)).ToList();

            //Insert code the remaiding implementation.

            return M;
        }

        public Dictionary<ITreeNode<T>, ITreeNode<T>> BottomUP(ITreeNode<T> t1, ITreeNode<T> t2, Dictionary<ITreeNode<T>, ITreeNode<T>> M)
        {
            var traversal = new TreeTraversal<T>();
            var list = traversal.PostOrderTraversal(t1);

            int maxSize = 100;
            double minDice = 0.25;

            var MT = new Dictionary<ITreeNode<T>, ITreeNode<T>>();
            foreach (var i in list)
            {
                if (!M.ContainsKey(i) && HasMatchedChildren(i, M))
                {
                    ITreeNode<T> snt2 = Candidate(i, t2, M);

                    double dice = Dice(i, snt2, M);

                    if (snt2 != null && dice > minDice)
                    {
                        if (Math.Max(i.DescendantNodes().Count(), t2.DescendantNodes().Count()) < maxSize)
                        {
                            var zss = new CSharpZss<T>(i, snt2);

                            var result = zss.Compute();
                            var edits = result.Item2;

                            RemoveFromM(i, M, MT);
                            var R = Opt(edits, i, snt2);

                            foreach (var edt in R)
                            {
                                if (!M.ContainsKey(edt.Key))
                                {
                                    if (edt.Key.IsLabel(edt.Value.Label))
                                    {
                                        M.Add(edt.Key, edt.Value);
                                        MT.Add(edt.Key, edt.Value);
                                    }
                                }
                            }                                   
                        }
                    }
                }
            }
            return M;
        }

        private void RemoveFromM(ITreeNode<T> t, Dictionary<ITreeNode<T>, ITreeNode<T>> M, Dictionary<ITreeNode<T>, ITreeNode<T>> MT)
        {
            var traversal = new TreeTraversal<T>();
            var elements = traversal.PostOrderTraversal(t);

            foreach(var i in elements)
            {
                if (M.ContainsKey(i) && !MT.ContainsKey(i))
                {
                    M.Remove(i);
                }
            }
        }

        private Dictionary<ITreeNode<T>, ITreeNode<T>> Opt(List<Tuple<ZssNode<ITreeNode<T>>, ZssNode<ITreeNode<T>>>> edits, ITreeNode<T> t1, ITreeNode<T> t2)
        {
            Dictionary<ITreeNode<T>, ITreeNode<T>> dict = new Dictionary<ITreeNode<T>, ITreeNode<T>>();

            foreach (var edit in edits)
            {
                if(edit.Item1 != null && edit.Item2 != null)
                {
                    dict.Add(edit.Item1.InternalNode, edit.Item2.InternalNode);
                    var x = IsomorphicManager<T>.AllPairOfIsomorphic(edit.Item1.InternalNode, edit.Item2.InternalNode);
                    
                    foreach(var dici in x)
                    {
                        if(!dict.ContainsKey(dici.Key))
                        {
                            dict.Add(dici.Key, dici.Value);
                        }
                    }

                    Console.WriteLine();
                }
            }

            return dict;
        }


        /// <summary>
        /// This method verify if tree t has some matching children.
        /// </summary>
        /// <param name="t">Tree t to be analyzed</param>
        /// <param name="M">Computed mapping</param>
        /// <returns>Tree t2 with the disired characteristic.</returns>
        private bool HasMatchedChildren(ITreeNode<T> t, Dictionary<ITreeNode<T>, ITreeNode<T>> M)
        {
            return t.DescendantNodes().Any(ch => M.ContainsKey(ch));
        }

        private ITreeNode<T> Candidate(ITreeNode<T> t1, ITreeNode<T> t2, Dictionary<ITreeNode<T>, ITreeNode<T>> M)
        {
            var label = t1.Label;

            var list = from i in t2.DescendantNodesAndSelf()
                       where i.IsLabel(label)
                       select i;

            double max = -1;
            ITreeNode<T> smax = null;
            foreach (var i in list)
            {
                double dice = Dice(t1, i, M);

                if (dice > max)
                {
                    max = dice;
                    smax = i;
                }
            }

            return smax;
        }

        private double Dice(ITreeNode<T> t1, ITreeNode<T> t2, Dictionary<ITreeNode<T>, ITreeNode<T>> M)
        {
            if (t2 == null) return 0.0;

            double count = 0.0;
            foreach (var item in t1.DescendantNodes())
            {
                if (M.ContainsKey(item) && t2.DescendantNodesAndSelf().Contains(M[item]))
                {
                    count++;
                }
            }

            double den = t1.DescendantNodes().Count + t2.DescendantNodes().Count();

            double dice = (2.0 * count) / den;
            return dice;
        }

        public Dictionary<ITreeNode<T>, ITreeNode<T>> Mapping(ITreeNode<T> t1, ITreeNode<T> t2)
        {
            Dictionary<ITreeNode<T>, ITreeNode<T>> M = TopDown(t1, t2);
            M = BottomUP(t1, t2, M);
            return M;
        }
    }
}
