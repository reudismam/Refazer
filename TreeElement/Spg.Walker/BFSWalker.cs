using System.Collections.Generic;
using System.Linq;
using ProseSample.Substrings;

namespace TreeElement.Spg.Walker
{
    public class BFSWalker<T>
    {
        /// <summary>
        /// Breadth First Search traversal
        /// </summary>
        /// <param name="u">Node to be traversed</param>
        /// <returns></returns>
        public static List<TreeNode<T>> BreadFirstSearch(TreeNode<T> u)
        {
            var result = new List<TreeNode<T>>();
            var dist = new Dictionary<TreeNode<T>, int> { [u] = 0 };
            var q = new Queue<TreeNode<T>>();
            q.Enqueue(u);

            while (q.Any())
            {
                TreeNode<T> v = q.Dequeue();
                foreach (var c in v.Children)
                {
                    if (!dist.ContainsKey(c))
                    {
                        dist[c] = dist[v] + 1;
                        result.Add(c);
                        q.Enqueue(c);
                    }
                }
            }

            return result;
        }

        public static Dictionary<TreeNode<T>, int> Dist(TreeNode<T> u)
        {
            var result = new List<TreeNode<T>>();
            var dist = new Dictionary<TreeNode<T>, int> { [u] = 0 };
            var q = new Queue<TreeNode<T>>();
            q.Enqueue(u);

            while (q.Any())
            {
                TreeNode<T> v = q.Dequeue();
                foreach (var c in v.Children)
                {
                    if (!dist.ContainsKey(c))
                    {
                        dist[c] = dist[v] + 1;
                        result.Add(c);
                        q.Enqueue(c);
                    }
                }
            }
            return dist;
        }
    }
}
