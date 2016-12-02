using System;
using System.Collections.Generic;
using System.Linq;
using TreeElement;
using ProseSample.Substrings;
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
        public List<EditOperation<T>> EditScript(TreeNode<T> t1, TreeNode<T> t2, Dictionary<TreeNode<T>, TreeNode<T>> M)
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
                    int k = FindPos(x, M);

                    var xnode = new TreeNode<T>(x.Value, x.Label);
                    var insert = new Insert<T>(xnode, z, k);
                    z.AddChild(xnode, k - 1);
                    M.Add(xnode, x);

                    editScript.Add(insert);
                }
                else //x has a partner in M
                {
                    var v = w.Parent;
                    if (!w.Children.Any() && !w.ToString().Equals(x.ToString()))
                    {
                        var update = new Update<T>(w, x, z, y);
                        int index = v.Children.TakeWhile(item => !item.Equals(w)).Count();
                        v.RemoveNode(index);
                        var xnode = new TreeNode<T>(x.Value, x.Label);
                        v.AddChild(xnode, index);
                        M.Add(xnode, x);
                        M.Remove(w);
                        w = xnode;
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
                    editScript.Add(delete);
                }
            }

            foreach (var s in editScript)
            {
                if (s is Move<T>) continue;
                var t1copy = ConverterHelper.MakeACopy(s.T1Node);
                t1copy.Children = new List<TreeNode<T>>();
                var parentcopy = ConverterHelper.MakeACopy(s.Parent);
                parentcopy.Children = new List<TreeNode<T>>();

                s.T1Node = t1copy;
                s.Parent = parentcopy;
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

            var removes = new List<Tuple<EditOperation<T>, List<EditOperation<T>>>>();
            foreach (var v in editScript)
            {
                if (v is Move<T>)
                {
                    var move = (Move<T>)v;
                    var t1copy = ConverterHelper.MakeACopy(v.T1Node);
                    var parentcopy = ConverterHelper.MakeACopy(v.Parent);
                    var previousParentCopy = ConverterHelper.MakeACopy(move.PreviousParent);
                    EditOperation<T> insert = new Insert<T>(t1copy, parentcopy, v.K);
                    var delete = new Delete<T>(ConverterHelper.MakeACopy(t1copy));
                    delete.Parent = previousParentCopy;
                    var list = new List<EditOperation<T>>();
                    list.Add(delete);
                    list.Add(insert);
                    removes.Add(Tuple.Create(v, list));           
                }
            }
            foreach (var v in removes)
            {
                var index = editScript.FindIndex(o => o.Equals(v.Item1));
                editScript.InsertRange(index, v.Item2);
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
        private int FindPos(TreeNode<T> x, Dictionary<TreeNode<T>, TreeNode<T>> M)
        {
            TreeNode<T> y = x.Parent; TreeNode<T> w = M.ToList().Find(o => o.Value.Equals(x)).Key;

            TreeNode<T> firstChild = y.Children.ElementAt(0);

            if (firstChild.Equals(x)) return 1;

            TreeNode<T> v = null;
            foreach (TreeNode<T> c in y.Children)
            {
                if (c.Equals(x))
                {
                    break;
                }

                v = c;
            }
            TreeNode<T> u = M.ToList().Find(o => o.Value.Equals(v)).Key;
            int count = 1;
            foreach (TreeNode<T> c in u.Parent.Children)
            {
                if (c.Equals(u)) return count + 1;

                count++;
            }
            return -1;
        }
    }
}
