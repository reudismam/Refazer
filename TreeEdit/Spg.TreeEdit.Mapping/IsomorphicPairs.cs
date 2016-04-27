using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spg.TreeEdit.Node;

namespace TreeEdit.Spg.TreeEdit.Mapping
{
    public class IsomorphicPairs<T>
    {
        Dictionary<ITreeNode<T>, string> _dict1;
        Dictionary<ITreeNode<T>, string> _dict2;

        Dictionary<ITreeNode<T>, ITreeNode<T>> _alg;


        private void AllPairOfIsomorphic(ITreeNode<T> t1, ITreeNode<T> t2)
        {
           
            if (_dict1[t1].Equals(_dict2[t2]))
            {
                _alg.Add(t1, t2);
            }

            foreach (var ci in t1.Children)
            {
                foreach (var cj in t2.Children)
                {
                    if (_dict1[ci].Equals(_dict2[cj]))
                    {
                        AllPairOfIsomorphic(ci, cj);
                    }
                }
            }
        }

        public Dictionary<ITreeNode<T>, ITreeNode<T>> Pairs(ITreeNode<T> t1, ITreeNode<T> t2)
        {
            var talg = new TreeAlignment<T>();
            _dict1 = talg.align(t1);
            _dict2 = talg.align(t2);

            _alg = new Dictionary<ITreeNode<T>, ITreeNode<T>>();

            AllPairOfIsomorphic(t1, t2);
            return _alg;
        }
    }
}
