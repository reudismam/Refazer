using System.Collections.Generic;
using Spg.TreeEdit.Node;
using TreeEdit.Spg.TreeEdit.Mapping;

namespace TreeEdit.Spg.TreeEdit.Isomorphic
{
    public class IsomorphicManager<T>
    {

        public static bool IsIsomorphic(ITreeNode<T> t1, ITreeNode<T> t2)
        {
            return AhuTreeIsomorphism(t1, t2);
        }

        public static bool AhuTreeIsomorphism(ITreeNode<T> t1, ITreeNode<T> t2)
        {
            var talg = new TreeAlignment<T>();
            Dictionary<ITreeNode<T>, string> dict1 = talg.align(t1);
            Dictionary<ITreeNode<T>, string> dict2 = talg.align(t2);

            if (dict1[t1].Equals(dict2[t2]))
            {
                return true;
            }

            return false;
        }

        public static Dictionary<ITreeNode<T>, ITreeNode<T>> AllPairOfIsomorphic(ITreeNode<T> t1, ITreeNode<T> t2)
        {
            var pairs = new IsomorphicPairs<T>();
            Dictionary<ITreeNode<T>, ITreeNode<T>> ps = pairs.Pairs(t1, t2);

            return ps;
        }
    }
}
