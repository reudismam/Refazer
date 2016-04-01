using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeEdit.Spg.TreeEdit.Mapping
{
    public class IsomorphicPairs
    {
        Dictionary<SyntaxNodeOrToken, string> dict1;
        Dictionary<SyntaxNodeOrToken, string> dict2;

        Dictionary<SyntaxNodeOrToken, SyntaxNodeOrToken> alg;

        public IsomorphicPairs()
        {        
        }

        private void allPairOfIsomorphic(SyntaxNodeOrToken t1, SyntaxNodeOrToken t2)
        {
            SyntaxNode s1 = t1.AsNode();
            SyntaxNode s2 = t2.AsNode();

            if (s1 == null || s2 == null)
            {
                return;
            }

            if (dict1[s1].Equals(dict2[s2]))
            {
                alg.Add(t1, t2);
            }

            if (!s1.ChildNodes().Any() || !s2.ChildNodes().Any())
            {
                return;
            }

            foreach (var ci in s1.ChildNodes())
            {
                foreach (var cj in s2.ChildNodes())
                {
                    if (dict1[ci].Equals(dict2[cj]))
                    {
                        allPairOfIsomorphic(ci, cj);
                    }
                }
            }
        }

        public Dictionary<SyntaxNodeOrToken, SyntaxNodeOrToken> pairs(SyntaxNodeOrToken t1, SyntaxNodeOrToken t2)
        {
            TreeAlignment talg = new TreeAlignment();
            dict1 = talg.align(t1);
            dict2 = talg.align(t2);

            alg = new Dictionary<SyntaxNodeOrToken, SyntaxNodeOrToken>();

            allPairOfIsomorphic(t1, t2);
            return alg;
        }
    }
}
