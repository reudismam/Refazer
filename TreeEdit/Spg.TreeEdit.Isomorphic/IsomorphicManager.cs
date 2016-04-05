using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using TreeEdit.Spg.TreeEdit.Mapping;

namespace TreeEdit.Spg.TreeEdit.Isomorphic
{
    public class IsomorphicManager
    {

        public static bool IsIsomorphic(SyntaxNodeOrToken t1, SyntaxNodeOrToken t2)
        {
            return AhuTreeIsomorphism(t1, t2);
        }

        public static bool AhuTreeIsomorphism(SyntaxNodeOrToken t1, SyntaxNodeOrToken t2)
        {
            TreeAlignment talg = new TreeAlignment();
            Dictionary<SyntaxNodeOrToken, string> dict1 = talg.align(t1);
            Dictionary<SyntaxNodeOrToken, string> dict2 = talg.align(t2);

            if (dict1[t1].Equals(dict2[t2]))
            {
                return true;
            }

            return false;
        }

        public static Dictionary<SyntaxNodeOrToken, SyntaxNodeOrToken> AllPairOfIsomorphic(SyntaxNodeOrToken t1, SyntaxNodeOrToken t2)
        {
            IsomorphicPairs pairs = new IsomorphicPairs();
            Dictionary<SyntaxNodeOrToken, SyntaxNodeOrToken> ps = pairs.pairs(t1, t2);

            return ps;
        }
    }
}
