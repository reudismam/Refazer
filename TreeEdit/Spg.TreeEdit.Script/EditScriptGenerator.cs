using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using TreeEdit.Spg.TreeEdit.Mapping;

namespace TreeEdit.Spg.TreeEdit.Script
{
    public class EditScriptGenerator
    {
        /// <summary>
        /// Create an edit script
        /// </summary>
        /// <param name="t1">Source tree</param>
        /// <param name="t2">Target tree</param>
        /// <param name="M">Mapping between source and target tree nodes</param>
        /// <returns></returns>
        public List<EditOperation> EditScript(SyntaxNodeOrToken t1, SyntaxNodeOrToken t2, Dictionary<SyntaxNodeOrToken, SyntaxNodeOrToken> M)
        {
            var editScript = new List<EditOperation>();
            var bfs = BreadFirstSearch(t2.AsNode());

            foreach (var x in bfs)
            {
                //Combines the update, insert, align, and move phases
                var y = x.Parent;
                var z = M.ToList().Find(o => o.Value.Equals(y)).Key;
                var w = M.ToList().Find(o => o.Value.Equals(x)).Key;

                if (w.IsKind(SyntaxKind.None))
                {
                    //if (y.IsKind(z.Parent.Kind()))
                    //{
                        Console.WriteLine();
                        int k = FindPos(x, M);
                        Insert insert = new Insert(x, z, k);
                        M.Add(x, x);
                        editScript.Add(insert);
                    //}
                }
                else //x has a partner in M
                {
                    var v = w.Parent;
                    if (!w.AsNode().ChildNodes().Any() && !w.ToString().Equals(x.ToString()))
                    {
                        Update update = new Update(w, x, z);
                        editScript.Add(update);
                    }

                    //var vmap = M.ToList().Find(o => o.Value.Equals(y)).Key;
                    if ((z.IsKind(SyntaxKind.None) || !z.Equals(v)) && y.IsKind(z.Parent.Kind()))
                    {
                        Console.WriteLine();

                        int k = FindPos(x, M);
                        Move move = new Move(w, z, k);
                        editScript.Add(move);
                    }
                }

                //AlignChildren(x, w);   
            }

            var traversal = new TreeTraversal();
            var nodes = traversal.PostOrderTraversal(t1); //the delete phase

            foreach (var w in nodes)
            {
                if (!M.ContainsKey(w))
                {
                    Delete delete = new Delete(w);
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
        private int FindPos(SyntaxNodeOrToken x, Dictionary<SyntaxNodeOrToken, SyntaxNodeOrToken> M)
        {
            SyntaxNode y = x.Parent; SyntaxNodeOrToken w = M.ToList().Find(o => o.Value.Equals(x)).Key;

            SyntaxNodeOrToken firstChild = y.ChildNodes().ElementAt(0);

            if (firstChild.Equals(x)) return 1;

            SyntaxNodeOrToken v = null;
            foreach (SyntaxNodeOrToken c in y.ChildNodes())
            {
                if (c.Equals(x))
                {
                    break;
                }
                else
                {
                    v = c;
                }
            }

            SyntaxNodeOrToken u = M.ToList().Find(o => o.Value.Equals(v)).Key;//Mline.Values.ToList().Find(o => o.Equals(x));

            int count = 1;
            foreach (SyntaxNodeOrToken c in u.Parent.ChildNodes())
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
        private List<SyntaxNodeOrToken> BreadFirstSearch(SyntaxNode u)
        {
            var result = new List<SyntaxNodeOrToken>();
            var dist = new Dictionary<SyntaxNode, int>(); dist[u] = 0;
            var q = new Queue<SyntaxNode>();
            q.Enqueue(u);

            while (q.Any())
            {
                SyntaxNode v = q.Dequeue();

                foreach (var c in v.ChildNodes())
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
