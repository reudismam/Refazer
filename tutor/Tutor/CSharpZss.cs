using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tutor;

namespace Tutor
{
    public class CSharpZss : Zss<SyntaxNodeOrToken>
    {
        
        public CSharpZss(SyntaxNodeOrToken previousTree, SyntaxNodeOrToken currentTree) : base(previousTree,currentTree)
        {
        }

        protected override void GenerateNodes(SyntaxNodeOrToken t1, SyntaxNodeOrToken t2)
        {
            TreeTraversal traversal = new TreeTraversal();
            var l1 = traversal.PostOrderTraversal(t1);
            var l2 = traversal.PostOrderTraversal(t2);

            A = ConvertToZZ(l1);
            T1 = Enumerable.Range(1, A.Count).ToArray();

            B = ConvertToZZ(l2);
            T2 = Enumerable.Range(1, B.Count).ToArray();

            return;
        }

        private List<ZssNode<SyntaxNodeOrToken>> ConvertToZZ(List<SyntaxNodeOrToken> list)
        {
            var zzlist = new List<ZssNode<SyntaxNodeOrToken>>();
            foreach (var i in list)
            {
                var node = new CSharpZssNode(i);
                zzlist.Add(node);
            }

            return zzlist;
        }
    }
}
