using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace Tutor
{
    public class TreeTraversal
    {
        public List<SyntaxNodeOrToken> list;

        public SyntaxNodeOrToken root;

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
    }
}
