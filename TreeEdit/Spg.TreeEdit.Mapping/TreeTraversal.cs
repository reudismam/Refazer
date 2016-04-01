using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeEdit.Spg.TreeEdit.Mapping
{
    public class TreeTraversal
    {
        public List<SyntaxNodeOrToken> list;

        public SyntaxNodeOrToken root;

        public TreeTraversal()
        {
        }

        public List<SyntaxNodeOrToken> PostOrderTraversal(SyntaxNodeOrToken t)
        {
            list = new List<SyntaxNodeOrToken>();

            PostOrder(t);

            return list;
        }

        private void PostOrder(SyntaxNodeOrToken t)
        {
            SyntaxNode sn = t.AsNode();

            if (sn == null) return;

            foreach(var ch in sn.ChildNodes())
            {
                PostOrder(ch);
            }

            list.Add(sn);
        }

        /*private void InOrder(SyntaxNodeOrToken t)
        {
            SyntaxNode sn = t.AsNode();

            if (sn == null) return;

            foreach (var ch in sn.ChildNodes())
            {
                PostOrder(ch);
            }

            list.Add(sn);
        }*/
    }
}
