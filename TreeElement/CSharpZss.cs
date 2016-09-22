using System.Collections.Generic;
using System.Linq;
using ProseSample.Substrings;

namespace TreeElement
{
    public class CSharpZss<T> : Zss<ITreeNode<T>>
    {
        
        public CSharpZss(ITreeNode<T> previousTree, ITreeNode<T> currentTree) : base(previousTree,currentTree)
        {
        }

        protected override void GenerateNodes(ITreeNode<T> t1, ITreeNode<T> t2)
        {
            TreeTraversal<T> traversal = new TreeTraversal<T>();
            var l1 = traversal.PostOrderTraversal(t1);
            var l2 = traversal.PostOrderTraversal(t2);

            A = ConvertToZZ(l1);
            T1 = Enumerable.Range(1, A.Count).ToArray();

            B = ConvertToZZ(l2);
            T2 = Enumerable.Range(1, B.Count).ToArray();

            return;
        }

        private List<ZssNode<ITreeNode<T>>> ConvertToZZ(List<ITreeNode<T>> list)
        {
            var zzlist = new List<ZssNode<ITreeNode<T>>>();
            foreach (var i in list)
            {
                var node = new CSharpZssNode<T>(i);
                zzlist.Add(node);
            }

            return zzlist;
        }
    }
}
