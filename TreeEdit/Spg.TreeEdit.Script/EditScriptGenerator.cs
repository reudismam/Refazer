using System.Collections.Generic;
using System.Linq;
using Spg.TreeEdit.Node;
using TreeEdit.Spg.TreeEdit.Mapping;

namespace TreeEdit.Spg.TreeEdit.Script
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
            var bfs = BreadFirstSearch(t2);
            //Obs: we do not need to apply transformation as described in the paper because
            //we don't use t1 in the process.
            foreach (var x in bfs)
            {
                //Combines the update, insert, align, and move phases
                var y = x.Parent;
                var z = M.ToList().Find(o => o.Value.Equals(y)).Key;
                var w = M.ToList().Find(o => o.Value.Equals(x)).Key;

                if (w == null)
                {
                    int k = FindPos(x, M);
                    //x.Children = new List<ITreeNode<T>>();
                    var insert = new Insert<T>(x, z, k);
                    M.Add(x, x);
                    editScript.Add(insert);
                }
                else //x has a partner in M
                {
                    var v = w.Parent;
                    if (!w.Children.Any() && !w.ToString().Equals(x.ToString()))
                    {
                        var update = new Update<T>(w, x, z);
                        editScript.Add(update);
                    }


                    if (z == null || !z.Equals(v))
                    {
                        int k = FindPos(x, M);
                        var move = new Move<T>(w, z, k);
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
                    editScript.Add(delete);
                }
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

        /// <summary>
        /// Breadth First Search traversal
        /// </summary>
        /// <param name="u">Node to be traversed</param>
        /// <returns></returns>
        private List<ITreeNode<T>> BreadFirstSearch(ITreeNode<T> u)
        {
            var result = new List<ITreeNode<T>>();
            var dist = new Dictionary<ITreeNode<T>, int> { [u] = 0 };
            var q = new Queue<ITreeNode<T>>();
            q.Enqueue(u);

            while (q.Any())
            {
                ITreeNode<T> v = q.Dequeue();

                foreach (var c in v.Children)
                {
                    if (!dist.ContainsKey(c))
                    {
                        dist[v] = dist[u] + 1;
                        result.Add(c);
                        q.Enqueue(c);
                    }
                }
            }

            return result;
        }
    }
}
