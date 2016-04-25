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
        public List<SyntaxNodeOrToken> List;

        public SyntaxNodeOrToken Root;

        public List<SyntaxNodeOrToken> PostOrderTraversal(SyntaxNodeOrToken t)
        {
            List = new List<SyntaxNodeOrToken>();

            PostOrder(t);

            return List;
        }

        private void PostOrder(SyntaxNodeOrToken t)
        {
            SyntaxNode sn = t.AsNode();

            if (sn == null) return;

            foreach(var ch in sn.ChildNodes())
            {
                PostOrder(ch);
            }

            List.Add(sn);
        }
    }
}
