using System.Collections.Generic;
using ProseSample.Substrings;

namespace TreeElement
{
    public class TreeTraversal<T>
    {
        public List<ITreeNode<T>> List;

        public ITreeNode<T> Root;

        public List<ITreeNode<T>> PostOrderTraversal(ITreeNode<T> t)
        {
            List = new List<ITreeNode<T>>();

            PostOrder(t);

            return List;
        }

        private void PostOrder(ITreeNode<T> t)
        {

            foreach (var ch in t.Children)
            {
                PostOrder(ch);
            }

            List.Add(t);
        }
    }
}
