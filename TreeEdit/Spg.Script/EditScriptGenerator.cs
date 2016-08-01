using System;
using System.Collections.Generic;
using System.Linq;
using TreeEdit.Spg.Print;
using TreeElement;
using TreeElement.Spg.Node;
using TreeElement.Spg.Walker;

namespace TreeEdit.Spg.Script
{
    public class EditScriptGenerator<T>
    {
        /// <summary>
        /// Create an edit script
        /// </summary>
        /// <param name="t1">Source tree</param>
        /// <param name="t2">Target tree</param>
        /// <param name="M">Mapping between source and target tree nodes</param>
        /// <returns></returns>
        public List<EditOperation<T>> EditScript(ITreeNode<T> t1, ITreeNode<T> t2, Dictionary<ITreeNode<T>, ITreeNode<T>> M)
        {
            var editScript = new List<EditOperation<T>>();
            var bfs = BFSWalker<T>.BreadFirstSearch(t2);

            foreach (var x in bfs)
            {
                //Combines the update, insert, align, and move phases
                var y = x.Parent;
                var z = M.ToList().Find(o => o.Value.Equals(y)).Key;
                var w = M.ToList().Find(o => o.Value.Equals(x)).Key;

                if (w == null)
                {
                    //if (!M.ContainsKey(x)) continue;
                    int k = FindPos(x, M);

                    var xnode = new TreeNode<T>(x.Value, x.Label);
                    var insert = new Insert<T>(xnode, z, k);
                    z.AddChild(xnode, k - 1);
                    M.Add(xnode, x);

                    //PrintUtil<T>.PrintPretty(t1, "", true);
                    editScript.Add(insert);
                }
                else //x has a partner in M
                {
                    var v = w.Parent;
                    if (!w.Children.Any() && !w.ToString().Equals(x.ToString()))
                    {
                        var update = new Update<T>(w, x, z);
                        int index = v.Children.TakeWhile(item => !item.Equals(w)).Count();
                        v.RemoveNode(index);
                        var xnode = new TreeNode<T>(x.Value, x.Label);
                        v.AddChild(xnode, index);
                        M.Add(xnode, x);
                        M.Remove(w);
                        w = xnode;
                        //PrintUtil<T>.PrintPretty(t1, "", true);
                        editScript.Add(update);
                    }

                   
                    if (z != null && M.ToList().Find(o => o.Value.Equals(y) && o.Key.Equals(v)).Key == null)
                    {
                        int k = FindPos(x, M);
                        var znode = ConverterHelper.MakeACopy(z);
                        var wnode = ConverterHelper.MakeACopy(w);
                        var move = new Move<T>(wnode, znode, k);
                        move.PreviousParent = w.Parent;
                        z.AddChild(w, k - 1);
                        int index = v.Children.TakeWhile(item => !item.Equals(w)).Count();
                        v.RemoveNode(index);
                        //PrintUtil<T>.PrintPretty(t1, "", true);
                        editScript.Add(move);
                    }
                }

                //AlignChildren(x, w);   
            }

            var traversal = new TreeTraversal<T>();
            var nodes = traversal.PostOrderTraversal(t1); //the delete phase

            foreach (var w in nodes)
            {
                if (!M.ContainsKey(w))
                {
                    var delete = new Delete<T>(w);
                    var v = delete.Parent;
                    int index = v.Children.TakeWhile(item => !item.Equals(w)).Count();
                    v.RemoveNode(index);
                    //PrintUtil<T>.PrintPretty(t1, "", true);
                    editScript.Add(delete);
                }
            }

            for (int i = 0; i < editScript.Count; i++)
            {
                var v = editScript[i];
                if (v is Insert<T>)
                {
                    var xnode = new TreeNode<T>(v.T1Node.Value, v.T1Node.Label);
                    var znode = new TreeNode<T>(v.Parent.Value, v.Parent.Label);

                    var insert = new Insert<T>(xnode, znode, v.K);
                    editScript[i] = insert;
                }
            }

            var removes = new List<Tuple<EditOperation<T>, EditOperation<T>>>();
            var deletes = new List<EditOperation<T>>();
            foreach (var v in editScript)
            {
                if (v is Move<T>)
                {
                    var move = (Move<T>)v;
                    var t1copy = ConverterHelper.MakeACopy(v.T1Node);
                    var parentcopy = ConverterHelper.MakeACopy(v.Parent);
                    var previousParentCopy = ConverterHelper.MakeACopy(move.PreviousParent);
                    EditOperation<T> insert = new Insert<T>(t1copy, parentcopy, v.K);
                    removes.Add(Tuple.Create(v, insert));
                    var delete = new Delete<T>(ConverterHelper.MakeACopy(t1copy));
                    delete.Parent = previousParentCopy;
                    deletes.Add(delete);
                }
            }

            editScript.AddRange(deletes);
            foreach (var v in removes)
            {
                var index = editScript.FindIndex(o => o.Equals(v.Item1));
                editScript.Insert(index, v.Item2);
                editScript.Remove(v.Item1);
            }
            return editScript;
        }

        /// <summary>
        /// Find the index in which the edit operations will be executed.
        /// </summary>
        /// <param name="w">w is the patner of x (w in T1)</param>
        /// <param name="x">Node in t2</param>
        /// <returns>Index to be updated</returns>
        private int FindPos(ITreeNode<T> x, Dictionary<ITreeNode<T>, ITreeNode<T>> M)
        {
            ITreeNode<T> y = x.Parent; ITreeNode<T> w = M.ToList().Find(o => o.Value.Equals(x)).Key;

            ITreeNode<T> firstChild = y.Children.ElementAt(0);

            if (firstChild.Equals(x)) return 1;
            //if (!y.Children.Any()) return 1;

            ITreeNode<T> v = null;
            foreach (ITreeNode<T> c in y.Children)
            {
                if (c.Equals(x))
                {
                    break;
                }

                v = c;
            }

            ITreeNode<T> u = M.ToList().Find(o => o.Value.Equals(v)).Key;//Mline.Values.ToList().Find(o => o.Equals(x));

            int count = 1;
            foreach (ITreeNode<T> c in u.Parent.Children)
            {
                if (c.Equals(u)) return count + 1;

                count++;
            }

            return -1;
        }

        ///// <summary>
        ///// Breadth First Search traversal
        ///// </summary>
        ///// <param name="u">Node to be traversed</param>
        ///// <returns></returns>
        //private List<ITreeNode<T>> BreadFirstSearch(ITreeNode<T> u)
        //{
        //    var result = new List<ITreeNode<T>>();
        //    var dist = new Dictionary<ITreeNode<T>, int> { [u] = 0 };
        //    var q = new Queue<ITreeNode<T>>();
        //    q.Enqueue(u);

        //    while (q.Any())
        //    {
        //        ITreeNode<T> v = q.Dequeue();

        //        foreach (var c in v.Children)
        //        {
        //            if (!dist.ContainsKey(c))
        //            {
        //                dist[v] = dist[u] + 1;
        //                result.Add(c);
        //                q.Enqueue(c);
        //            }
        //        }
        //    }

        //    return result;
        //}
    }
}
