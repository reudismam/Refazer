﻿using System;
using System.Collections.Generic;
using System.Linq;
using TreeEdit.Spg.TreeEdit.PQ;
using Spg.TreeEdit.Node;
using TreeEdit.Spg.Print;
using TreeEdit.Spg.Isomorphic;
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
            var A = new List<Tuple<ITreeNode<T>, ITreeNode<T>>>();

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
                                    A.Add(Tuple.Create(item1.Item2, item2.Item2));
                                }
                                else
                                {
                                    var dict = IsomorphicManager<T>.AllPairOfIsomorphic(item1.Item2, item2.Item2);
                                    foreach (var v in dict)
                                    {
                                        if (!M.ContainsKey(v.Item1) && !M.ContainsValue(v.Item2))
                                        {
                                            M.Add(v.Item1, v.Item2);
                                        }
                                        else
                                        {
                                            A.Add(v);
                                        }
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

            var list = A.ToList();
            var sortList = list.OrderByDescending(o => Dice(o.Item1, o.Item2, M)).ToList();

            while (sortList.Any())
            {
                var item = sortList.First();
                var dict = IsomorphicManager<T>.AllPairOfIsomorphic(item.Item1, item.Item2);
                foreach (var v in dict)
                {
                    if(!M.ContainsKey(v.Item1))
                    {
                        M.Add(v.Item1, v.Item2);
                    }
                }

                var removes = sortList.Where(elm => elm.Item1.Equals(item.Item1) || elm.Item2.Equals(item.Item2)).ToList();

                sortList = sortList.Except(removes).ToList();
            }

            return M;
        }

        /// <summary>
        /// GumTree bottom up approach
        /// </summary>
        /// <param name="t1">First tree</param>
        /// <param name="t2">Second tree</param>
        /// <param name="M">Previous mapping</param>
        /// <returns></returns>
        public Dictionary<ITreeNode<T>, ITreeNode<T>> BottomUp(ITreeNode<T> t1, ITreeNode<T> t2, Dictionary<ITreeNode<T>, ITreeNode<T>> M)
        {
            var traversal = new TreeTraversal<T>();
            var t1Nodes = traversal.PostOrderTraversal(t1);
            foreach (var t1Node in t1Nodes)
            {
                if (!M.ContainsKey(t1Node) && HasMatchedChildren(t1Node, M))
                {
                    ITreeNode<T> t2Node = Candidate(t1Node, t2, M);
                    double dice = Dice(t1Node, t2Node, M);

                    if (t2Node != null && dice > 0.25)
                    {
                        if (Math.Max(t1Node.DescendantNodes().Count, t2.DescendantNodes().Count) < 1000)
                        {
                            RemoveFromM(t1Node, M);
                            var R = Opt(t1Node, t2Node);
                            foreach (var edt in R.Where(edt => !M.ContainsKey(edt.Key)).Where(edt => edt.Key.IsLabel(edt.Value.Label)))
                            {
                                M.Add(edt.Key, edt.Value);
                            }
                        }
                    }
                }
            }
            PrintUtil<T>.PrettyPrintString(t1, t2, M);
            return M;
        }

        /// <summary>
        /// Remove previous mapped nodes.
        /// </summary>
        /// <param name="t">Tree</param>
        /// <param name="M">Mapping</param>
        private void RemoveFromM(ITreeNode<T> t, Dictionary<ITreeNode<T>, ITreeNode<T>> M)
        {
            var traversal = new TreeTraversal<T>();
            var elements = traversal.PostOrderTraversal(t);

            foreach (var snode in elements.Where(M.ContainsKey))
            {
                M.Remove(snode);
            }
        }

        /// <summary>
        /// Mapping between nodes without move operation
        /// </summary>
        /// <param name="t1">T1 tree</param>
        /// <param name="t2">T2 tree</param>
        /// <returns></returns>
        private Dictionary<ITreeNode<T>, ITreeNode<T>> Opt(ITreeNode<T> t1, ITreeNode<T> t2)
        {
            var zss = new CSharpZss<T>(t1, t2);
            var result = zss.Compute();
            var script = result.Item2;
            var dict = new Dictionary<ITreeNode<T>, ITreeNode<T>>();

            foreach (var editOperation in script.Where(editOperation => editOperation.Item1 != null && editOperation.Item2 != null))
            {
                dict.Add(editOperation.Item1.InternalNode, editOperation.Item2.InternalNode);
                var isomorphicPairs = IsomorphicManager<T>.AllPairOfIsomorphic(editOperation.Item1.InternalNode, editOperation.Item2.InternalNode);

                if (IsomorphicManager<T>.IsIsomorphic(editOperation.Item1.InternalNode,
                    editOperation.Item2.InternalNode))
                {
                    dict.Add(editOperation.Item1.InternalNode, editOperation.Item2.InternalNode);
                }

                foreach (var pair in isomorphicPairs.Where(pair => !dict.ContainsKey(pair.Item1) && !dict.ContainsValue(pair.Item2)))
                {
                    dict.Add(pair.Item1, pair.Item2);
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
        private static bool HasMatchedChildren(ITreeNode<T> t, Dictionary<ITreeNode<T>, ITreeNode<T>> M)
        {
            return t.DescendantNodes().Any(M.ContainsKey);
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

        /// <summary>
        /// Compute the dice function between two trees.
        /// </summary>
        /// <param name="t1">First tree</param>
        /// <param name="t2">Second tree</param>
        /// <param name="M">Mapping</param>
        /// <returns></returns>
        private static double Dice(ITreeNode<T> t1, ITreeNode<T> t2, Dictionary<ITreeNode<T>, ITreeNode<T>> M)
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

            double dice = 2.0 * count / den;
            return dice;
        }

        /// <summary>
        /// Compute the mapping between two trees.
        /// </summary>
        /// <param name="t1">First tree.</param>
        /// <param name="t2">Second tree</param>
        /// <returns>Mapping between two trees.</returns>
        public Dictionary<ITreeNode<T>, ITreeNode<T>> Mapping(ITreeNode<T> t1, ITreeNode<T> t2)
        {
            var M = TopDown(t1, t2);
            M = BottomUp(t1, t2, M);
            return M;
        }
    }
}
