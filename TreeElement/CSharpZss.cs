using System.Collections.Generic;
using System.Linq;
using ProseSample.Substrings;
using TreeElement.Spg.Node;

namespace TreeElement
{
    public class CSharpZss<T> : Zss<TreeNode<T>>
    {
        
        public CSharpZss(TreeNode<T> previousTree, TreeNode<T> currentTree) : base(previousTree,currentTree)
        {
        }

        protected override void GenerateNodes(TreeNode<T> t1, TreeNode<T> t2)
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

        private List<ZssNode<TreeNode<T>>> ConvertToZZ(List<TreeNode<T>> list)
        {
            var zzlist = new List<ZssNode<TreeNode<T>>>();
            foreach (var i in list)
            {
                var node = new CSharpZssNode<T>(i);
                zzlist.Add(node);
            }

            return zzlist;
        }
    }
}
