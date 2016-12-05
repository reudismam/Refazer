using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using ProseSample.Substrings;
using TreeEdit.Spg.Isomorphic;
using TreeEdit.Spg.Print;
using TreeEdit.Spg.TreeEdit.Mapping;
using TreeEdit.Spg.TreeEdit.PQ;
using TreeElement.Spg.Node;

namespace TreeElement.Spg.TreeEdit.Mapping
{
    public class GumTreeMapping<T> : ITreeMapping<T>
    {
        /// <summary>
        /// Top-down phase from GumTree algorithm
        /// </summary>
        /// <param name="t1">First rooted tree.</param>
        /// <param name="t2">Second rooted tree.</param>
        /// <returns>Mapping from the nodes from t1 and t2.</returns>
        public Dictionary<TreeNode<T>, TreeNode<T>> TopDown(TreeNode<T> t1, TreeNode<T> t2)
        {
            var M = new Dictionary<TreeNode<T>, TreeNode<T>>();
            var A = new List<Tuple<TreeNode<T>, TreeNode<T>>>();

            var l1 = new PriorityQueue<T>();
            var l2 = new PriorityQueue<T>();
            l1.Push(t1);
            l2.Push(t2);

            const int minH = 0;

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
                    List<Tuple<int, TreeNode<T>>> h1 = l1.Pop();
                    List<Tuple<int, TreeNode<T>>> h2 = l2.Pop();

                    var dict1 = new Dictionary<TreeNode<T>, List<TreeNode<T>>>();
                    var dict2 = new Dictionary<TreeNode<T>, List<TreeNode<T>>>();
                    foreach (var item1 in h1)
                    {
                        if (!dict1.ContainsKey(item1.Item2)) dict1.Add(item1.Item2, new List<TreeNode<T>>());
                        foreach (var item2 in h2)
                        {
                            if (!dict2.ContainsKey(item2.Item2)) dict2.Add(item2.Item2, new List<TreeNode<T>>());

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
                                    PrintUtil<T>.PrettyPrintString(t1, t2, M);
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
            var sortList = list.OrderByDescending(o => Dice(o.Item1.Parent, o.Item2.Parent, M)).ToList();

            while (sortList.Any())
            {
                var item = sortList.First();
                var dict = IsomorphicManager<T>.AllPairOfIsomorphic(item.Item1, item.Item2);
                foreach (var v in dict)
                {
                    if (!M.ContainsKey(v.Item1))
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
        public Dictionary<TreeNode<T>, TreeNode<T>> BottomUp(TreeNode<T> t1, TreeNode<T> t2, Dictionary<TreeNode<T>, TreeNode<T>> M)
        {
            var traversal = new TreeTraversal<T>();
            var t1Nodes = traversal.PostOrderTraversal(t1);
            foreach (var t1Node in t1Nodes)
            {
                if (!M.ContainsKey(t1Node) && HasMatchedChildren(t1Node, M))
                {
                    TreeNode<T> t2Node = Candidate(t1Node, t2, M);
                    double dice = Dice(t1Node, t2Node, M);

                    if (t2Node != null && dice > 0.25)
                    {
                        M.Add(t1Node, t2Node);
                        var t1Descendants = t1Node.DescendantNodes();
                        var t2Descendants = t2Node.DescendantNodes();
                        if (Math.Max(t1Descendants.Count, t2Descendants.Count) < 110)
                        {
                            var R = Opt(t1Node, t2Node);
                            if (R.Any())
                            {
                                RemoveFromM(t1Node, M);
                                foreach (var edt in R.Where(edt => !M.ContainsKey(edt.Key)))
                                {
                                    if (SameLabelOrEquivalent(edt.Key, edt.Value))
                                    {
                                        M.Add(edt.Key, edt.Value);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            PrintUtil<T>.PrettyPrintString(t1, t2, M);
            return M;
        }

        private static bool SameLabelOrEquivalent(TreeNode<T> a, TreeNode<T> b)
        {
            return a.IsLabel(b.Label) ||
                   (a.IsLabel(new TLabel(SyntaxKind.MethodDeclaration)) && b.IsLabel(new TLabel(SyntaxKind.PropertyDeclaration))) ||
                   (a.IsLabel(new TLabel(SyntaxKind.PropertyDeclaration)) && b.IsLabel(new TLabel(SyntaxKind.MethodDeclaration)));
        }

        /// <summary>
        /// Remove previous mapped nodes.
        /// </summary>
        /// <param name="t">Tree</param>
        /// <param name="M">Mapping</param>
        private void RemoveFromM(TreeNode<T> t, Dictionary<TreeNode<T>, TreeNode<T>> M)
        {
            var traversal = new TreeTraversal<T>();
            var elements = traversal.PostOrderTraversal(t);

            foreach (var snode in elements.Where(M.ContainsKey))
            {
                M.Remove(snode);
            }
        }

        ///// <summary>
        ///// Mapping between nodes without move operation
        ///// </summary>
        ///// <param name="t1">T1 tree</param>
        ///// <param name="t2">T2 tree</param>
        ///// <returns></returns>
        //private Dictionary<TreeNode<T>, TreeNode<T>> Opt(TreeNode<T> t1, TreeNode<T> t2)
        //{
        //    var zss = new CSharpZss<T>(t1, t2);
        //    var result = zss.Compute();
        //    var script = result.Item2;
        //    var dict = new Dictionary<TreeNode<T>, TreeNode<T>>();

        //    foreach (var editOperation in script.Where(editOperation => editOperation.Item1 != null && editOperation.Item2 != null))
        //    {
        //        dict.Add(editOperation.Item1.InternalNode, editOperation.Item2.InternalNode);
        //        var isomorphicPairs = IsomorphicManager<T>.AllPairOfIsomorphic(editOperation.Item1.InternalNode, editOperation.Item2.InternalNode);

        //        if (IsomorphicManager<T>.IsIsomorphic(editOperation.Item1.InternalNode,
        //            editOperation.Item2.InternalNode))
        //        {
        //            dict.Add(editOperation.Item1.InternalNode, editOperation.Item2.InternalNode);
        //        }

        //        foreach (var pair in isomorphicPairs.Where(pair => !dict.ContainsKey(pair.Item1) && !dict.ContainsValue(pair.Item2)))
        //        {
        //            dict.Add(pair.Item1, pair.Item2);
        //        }
        //    }
        //    return dict;
        //}

        /// <summary>
        /// Mapping between nodes without move operation
        /// </summary>
        /// <param name="t1">T1 tree</param>
        /// <param name="t2">T2 tree</param>
        /// <returns></returns>
        private Dictionary<TreeNode<T>, TreeNode<T>> Opt(TreeNode<T> t1, TreeNode<T> t2)
        {
            var t1String = ConverterHelper.ConvertTreeNodeToString(t1);
            //t1String = Regex.Replace(t1String, "[^0-9a-zA-Z}{@\"]+", " ");
            //t1String = Regex.Replace(t1String, "(?:(?:@\"[^\"]*\")+)", "\"code\"");
            var t2String = ConverterHelper.ConvertTreeNodeToString(t2);
            //t2String = Regex.Replace(t2String, "(?:(?:@\"[^\"]*\")+)", "\"code\"");
            //t2String = Regex.Replace(t2String, "[^0-9a-zA-Z}{@\"]+", " ");

            var f1 = GetTestDataFolder(@"\") + @"\a.t";
            var f2 = GetTestDataFolder(@"\") + @"\b.t";

            File.WriteAllText(f1, t1String);
            File.WriteAllText(f2, t2String);

            string cmd = @"/c java -jar """ + GetTestDataFolder(@"\libs") + $@"\RTED_v1.1.jar"" -f ""{f1}"" ""{f2}"" -c 1 1 1 -s heavy --switch -m";
            Process proc = new Process();
            proc.StartInfo.FileName = "cmd.exe";
            proc.StartInfo.Arguments = cmd;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.Start();
            string output = proc.StandardOutput.ReadToEnd();

            var t1Traversal = new TreeTraversal<T>();
            var t1List = t1Traversal.PostOrderTraversal(t1);

            var t2Traversal = new TreeTraversal<T>();
            var t2List = t2Traversal.PostOrderTraversal(t2);


            var dic1 = new Dictionary<int, TreeNode<T>>();
            var dic2 = new Dictionary<int, TreeNode<T>>();

            for (int i = 0; i < t1List.Count; i++)
            {
                dic1.Add(i + 1, t1List[i]);
            }

            for (int i = 0; i < t2List.Count; i++)
            {
                dic2.Add(i + 1, t2List[i]);
            }

            var dictionary = new Dictionary<TreeNode<T>, TreeNode<T>>();
            StringReader strReader = new StringReader(output);
            strReader.ReadLine(); //discard files line
            while (true)
            {
                var aLine = strReader.ReadLine();
                if (aLine == null) break;

                int first = int.Parse(aLine.Substring(0, aLine.IndexOf("->", StringComparison.Ordinal)));
                var substr = aLine.IndexOf("->", StringComparison.Ordinal) + 2;

                int second = int.Parse(aLine.Substring(substr));
                if (first != 0 && second != 0)
                {
                    dictionary.Add(dic1[first], dic2[second]);
                }
            }
            return dictionary;
        }

        static string GetTestDataFolder(string testDataLocation)
        {
            string startupPath = System.AppDomain.CurrentDomain.BaseDirectory;
            var pathItems = startupPath.Split(Path.DirectorySeparatorChar);
            string projectPath = String.Join(Path.DirectorySeparatorChar.ToString(), pathItems.Take(pathItems.Length - 4));
            string result = projectPath + testDataLocation;
            return result;
        }

        /// <summary>
        /// This method verify if tree t has some matching children.
        /// </summary>
        /// <param name="t">Tree t to be analyzed</param>
        /// <param name="M">Computed mapping</param>
        /// <returns>Tree t2 with the disired characteristic.</returns>
        private static bool HasMatchedChildren(TreeNode<T> t, Dictionary<TreeNode<T>, TreeNode<T>> M)
        {
            return t.DescendantNodes().Any(M.ContainsKey);
        }

        private TreeNode<T> Candidate(TreeNode<T> t1, TreeNode<T> t2, Dictionary<TreeNode<T>, TreeNode<T>> M)
        {
            var list = from i in t2.DescendantNodesAndSelf()
                       where SameLabelOrEquivalent(i, t1)
                       select i;

            double max = -1;
            TreeNode<T> smax = null;
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
        private static double Dice(TreeNode<T> t1, TreeNode<T> t2, Dictionary<TreeNode<T>, TreeNode<T>> M)
        {
            if (t2 == null) return 0.0;

            var t2Descendants = t2.DescendantNodes();
            var t1Descendants = t1.DescendantNodes();

            double count = 0.0;
            foreach (var item in t1Descendants)
            {
                if (M.ContainsKey(item) && t2Descendants.Contains(M[item]))
                {
                    count++;
                }
            }

            double den = t1Descendants.Count + t2Descendants.Count;

            double dice = 2.0 * count / den;
            return dice;
        }

        /// <summary>
        /// Compute the mapping between two trees.
        /// </summary>
        /// <param name="t1">First tree.</param>
        /// <param name="t2">Second tree</param>
        /// <returns>Mapping between two trees.</returns>
        public Dictionary<TreeNode<T>, TreeNode<T>> Mapping(TreeNode<T> t1, TreeNode<T> t2)
        {
            //return Opt(t1, t2);
            var M = TopDown(t1, t2);
            M = BottomUp(t1, t2, M);
            return M;
        }
    }
}
