using System;
using System.Collections.Generic;
using System.Linq;
using TreeEdit.Spg.TreeEdit.Mapping;
using Tutor.Spg.Node;

namespace TreeEdit.Spg.Isomorphic
{
    public class IsomorphicPairs<T>
    {
        private Dictionary<ITreeNode<T>, string> _dict1;
        private Dictionary<ITreeNode<T>, string> _dict2;

        private Dictionary<Tuple<ITreeNode<T>, ITreeNode<T>>, int> _alg;


        private void AllPairOfIsomorphic(ITreeNode<T> t1, ITreeNode<T> t2)
        {
            if (_dict1[t1].Equals(_dict2[t2]))
            {
                _alg.Add(Tuple.Create(t1, t2), -1);
            }

            foreach (var ci in t1.DescendantNodes())
            {
                var t2Descendants = SplitToNodes(t2, ci.Label);
                foreach (var cj in t2Descendants)
                {
                    string ciValue = _dict1[ci];
                    string cjValue = _dict2[cj];
                    if (ciValue.Equals(cjValue))
                    {
                        _alg.Add(Tuple.Create(ci, cj), -1);
                    }
                }
            }
        }

        /// <summary>
        /// Splits the source node in the elements of type kind.
        /// </summary>
        /// <param name="node">Source node</param>
        /// <param name="label">Syntax kind</param>
        /// <returns></returns>
        private static List<ITreeNode<T>> SplitToNodes(ITreeNode<T> node, TLabel label)
        {
            var descendantNodes = node.DescendantNodesAndSelf();
            var kinds = from k in descendantNodes
                        where k.IsLabel(label)
                        select k;

            return kinds.ToList();
        }

        public List<Tuple<ITreeNode<T>, ITreeNode<T>>> Pairs(ITreeNode<T> t1, ITreeNode<T> t2)
        {
            var talg = new TreeAlignment<T>();
            _dict1 = talg.Align(t1);
            _dict2 = talg.Align(t2);

            _alg = new Dictionary<Tuple<ITreeNode<T>, ITreeNode<T>>, int>();

            AllPairOfIsomorphic(t1, t2);

            return _alg.Keys.ToList();
        }
    }
}
