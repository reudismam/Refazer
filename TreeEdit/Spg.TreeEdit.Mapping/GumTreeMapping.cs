using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using TreeEdit.Spg.TreeEdit.PQ;
using Microsoft.CodeAnalysis.CSharp;
using TreeEdit.Spg.TreeEdit.Isomorphic;
using Tutor;

namespace TreeEdit.Spg.TreeEdit.Mapping
{
    public class GumTreeMapping : ITreeMapping
    {
        /// <summary>
        /// Top-down phase from GumTree algorithm
        /// </summary>
        /// <param name="t1">First rooted tree.</param>
        /// <param name="t2">Second rooted tree.</param>
        /// <returns>Mapping from the nodes from t1 and t2.</returns>
        public Dictionary<SyntaxNodeOrToken, SyntaxNodeOrToken> TopDown(SyntaxNodeOrToken t1, SyntaxNodeOrToken t2)
        {
            var M = new Dictionary<SyntaxNodeOrToken, SyntaxNodeOrToken>();
            var A = new Dictionary<SyntaxNodeOrToken, SyntaxNodeOrToken>();

            var l1 = new PriorityQueue();
            var l2 = new PriorityQueue();
            l1.Push(t1);
            l2.Push(t2);

            int minH = 1;

            while (l1.PeekMax() > minH && l2.PeekMax() > minH)
            {
                PrintPQ(l1);
                PrintPQ(l2);

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
                    List<Tuple<int, SyntaxNodeOrToken>> h1 = l1.Pop();
                    List<Tuple<int, SyntaxNodeOrToken>> h2 = l2.Pop();

                    var dict1 = new Dictionary<SyntaxNodeOrToken, List<SyntaxNodeOrToken>>();
                    var dict2 = new Dictionary<SyntaxNodeOrToken, List<SyntaxNodeOrToken>>();
                    foreach (var item1 in h1)
                    {
                        if (!dict1.ContainsKey(item1.Item2)) dict1.Add(item1.Item2, new List<SyntaxNodeOrToken>());
                        foreach (var item2 in h2)
                        {
                            if (!dict2.ContainsKey(item2.Item2)) dict2.Add(item2.Item2, new List<SyntaxNodeOrToken>());

                            if (IsomorphicManager.IsIsomorphic(item1.Item2, item2.Item2))
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
                            if (IsomorphicManager.IsIsomorphic(item1.Item2, item2.Item2))
                            {
                                if (dict1[item1.Item2].Count() > 1 || dict2[item2.Item2].Count() > 1)
                                {
                                    A.Add(item1.Item2, item2.Item2);
                                }
                                else
                                {
                                    var dict = IsomorphicManager.AllPairOfIsomorphic(item1.Item2, item2.Item2);
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

        public Dictionary<SyntaxNodeOrToken, SyntaxNodeOrToken> BottomUP(SyntaxNodeOrToken t1, SyntaxNodeOrToken t2, Dictionary<SyntaxNodeOrToken, SyntaxNodeOrToken> M)
        {
            TreeTraversal traversal = new TreeTraversal();
            var list = traversal.PostOrderTraversal(t1);

            int maxSize = 100;
            double minDice = 0.3;

            var MT = new Dictionary<SyntaxNodeOrToken, SyntaxNodeOrToken>();
            foreach (var i in list)
            {
                if (!M.ContainsKey(i) && HasMatchedChildren(i, M))
                {
                    SyntaxNodeOrToken snt2 = Candidate(i, t2, M);

                    double dice = Dice(i, snt2, M);

                    if (snt2 != null && dice > minDice)
                    {
                        if (Math.Max(i.AsNode().DescendantNodes().Count(), t2.AsNode().DescendantNodes().Count()) < maxSize)
                        {
                            var zss = new CSharpZss(i, snt2);

                            var result = zss.Compute();
                            var edits = result.Item2;

                            RemoveFromM(i, M, MT);
                            var R = Opt(edits, i, snt2);

                            foreach (var edt in R)
                            {
                                if (!M.ContainsKey(edt.Key))
                                {
                                    M.Add(edt.Key, edt.Value);
                                    MT.Add(edt.Key, edt.Value);
                                }
                            }                                   
                        }
                    }
                }
            }
            return M;
        }

        private void RemoveFromM(SyntaxNodeOrToken t, Dictionary<SyntaxNodeOrToken, SyntaxNodeOrToken> M, Dictionary<SyntaxNodeOrToken, SyntaxNodeOrToken> MT)
        {
            var traversal = new TreeTraversal();
            var elements = traversal.PostOrderTraversal(t);

            foreach(var i in elements)
            {
                if (M.ContainsKey(i) && !MT.ContainsKey(i))
                {
                    M.Remove(i);
                }
            }
        }

        private Dictionary<SyntaxNodeOrToken, SyntaxNodeOrToken> Opt(List<Tuple<ZssNode<SyntaxNodeOrToken>, ZssNode<SyntaxNodeOrToken>>> edits, SyntaxNodeOrToken t1, SyntaxNodeOrToken t2)
        {
            Dictionary<SyntaxNodeOrToken, SyntaxNodeOrToken> dict = new Dictionary<SyntaxNodeOrToken, SyntaxNodeOrToken>();

            foreach (var edit in edits)
            {
                if(edit.Item1 != null && edit.Item2 != null)
                {
                    dict.Add(edit.Item1.InternalNode, edit.Item2.InternalNode);
                    var x = IsomorphicManager.AllPairOfIsomorphic(edit.Item1.InternalNode, edit.Item2.InternalNode);
                    
                    foreach(var dici in x)
                    {
                        dict.Add(dici.Key, dici.Value);
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
        private bool HasMatchedChildren(SyntaxNodeOrToken t, Dictionary<SyntaxNodeOrToken, SyntaxNodeOrToken> M)
        {
            SyntaxNode sn = t.AsNode();
            foreach (var ch in sn.DescendantNodes())
            {
                if (M.ContainsKey(ch)) return true;
            }

            return false;
        }

        private SyntaxNodeOrToken Candidate(SyntaxNodeOrToken t1, SyntaxNodeOrToken t2, Dictionary<SyntaxNodeOrToken, SyntaxNodeOrToken> M)
        {
            SyntaxNode sn = t1.AsNode();
            SyntaxNode sn2 = t2.AsNode();

            if (sn == null || sn2 == null) throw new Exception("Tree must be a syntax node nor a token.");

            SyntaxKind kind = sn.Kind();

            var list = from i in sn2.DescendantNodesAndSelf()
                       where i.IsKind(kind)
                       select i;

            double max = -1;
            SyntaxNodeOrToken smax = null;
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

        private void PrintPQ(PriorityQueue pq)
        {
            Console.WriteLine("===================== Begin PQ ========================");
            var list = pq.pq.ToList();
            foreach (var x in list)
            {
                Console.WriteLine(x);
            }
            Console.WriteLine("===================== End PQ ==========================");
        }


        private double Dice(SyntaxNodeOrToken t1, SyntaxNodeOrToken t2, Dictionary<SyntaxNodeOrToken, SyntaxNodeOrToken> M)
        {
            SyntaxNode s1 = t1.AsNode();
            SyntaxNode s2 = t2.AsNode();

            if (s1 == null || s2 == null) return 0.0;

            double count = 0.0;
            foreach (var item in s1.DescendantNodes())
            {
                if (M.ContainsKey(item))
                {
                    count++;
                }
            }

            double den = s1.DescendantNodes().Count() + s2.DescendantNodes().Count();

            double dice = (2.0 * count) / den;
            return dice;
        }

        public Dictionary<SyntaxNodeOrToken, SyntaxNodeOrToken> Mapping(SyntaxNodeOrToken t1, SyntaxNodeOrToken t2)
        {
            Dictionary<SyntaxNodeOrToken, SyntaxNodeOrToken> M = TopDown(t1, t2);
            M = BottomUP(t1, t2, M);
            return M;
        }

    }
}
