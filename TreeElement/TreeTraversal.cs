using System.Collections.Generic;
using ProseSample.Substrings;
using TreeElement.Spg.Node;

namespace TreeElement
{
    public class TreeTraversal<T>
    {
        public List<TreeNode<T>> List;

        public TreeNode<T> Root;

        public List<TreeNode<T>> PostOrderTraversal(TreeNode<T> t)
        {
            List = new List<TreeNode<T>>();

            PostOrder(t);

            return List;
        }

        private void PostOrder(TreeNode<T> t)
        {

            foreach (var ch in t.Children)
            {
                PostOrder(ch);
            }

            List.Add(t);
        }
    }
}
