using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Spg.TreeEdit.Node;
using Tutor.Spg.TreeEdit.Node;

namespace TreeEdit.Spg.TreeEdit.Mapping
{
    public class IsomorphicPairs<T>
    {
        Dictionary<ITreeNode<T>, string> _dict1;
        Dictionary<ITreeNode<T>, string> _dict2;

        List<Tuple<ITreeNode<T>, ITreeNode<T>>> _alg;


        private void AllPairOfIsomorphic(ITreeNode<T> t1, ITreeNode<T> t2)
        {      
            if (_dict1[t1].Equals(_dict2[t2]))
            {
                _alg.Add(Tuple.Create(t1, t2));
            }

            foreach (var ci in t1.DescendantNodes())
            {
                var t2Descendants = SplitToNodes(t2, ci.Label);
                foreach (var cj in t2Descendants)
                {
                    string ciValue = _dict1[ci];
                    string cjValue = _dict2[cj];
                    if(ciValue.Equals(cjValue))
                    {
                        var tuple = _alg.Find(o => o.Item1.Equals(ci) && o.Item2.Equals(cj));
                        if (tuple == null)
                        {
                            _alg.Add(Tuple.Create(ci, cj));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Splits the source node in the elements of type kind.
        /// </summary>
        /// <param name="node">Source node</param>
        /// <param name="kind">Syntax kind</param>
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

            _alg = new List<Tuple<ITreeNode<T>, ITreeNode<T>>>();

            AllPairOfIsomorphic(t1, t2);
            return _alg;
        }
    }
}
