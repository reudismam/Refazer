using System;
using System.Collections.Generic;
using System.Linq;
using TreeEdit.Spg.TreeEdit.Mapping;
using ProseSample.Substrings;

namespace TreeEdit.Spg.Isomorphic
{
    public class IsomorphicPairs<T>
    {
        private Dictionary<TreeNode<T>, string> _dict1;
        private Dictionary<TreeNode<T>, string> _dict2;

        private Dictionary<Tuple<TreeNode<T>, TreeNode<T>>, int> _alg;


        private void AllPairOfIsomorphic(TreeNode<T> t1, TreeNode<T> t2)
        {
            if (!t1.Children.Any())
            {
                _alg.Add(Tuple.Create(t1, t2), -1); //_alg.Add(Tuple.Create(t1, t2), -1); 
                return;
            }

            foreach (var ci in t1.Children)
            {
                var t2Descendants = t2.Children;//SplitToNodes(t2, ci.Label);
                foreach (var cj in t2Descendants)
                {
                    string ciValue = _dict1[ci];
                    string cjValue = _dict2[cj];
                    if (ciValue.Equals(cjValue))
                    {
                        AllPairOfIsomorphic(ci, cj);                      
                    }
                } 
            }
            _alg.Add(Tuple.Create(t1, t2), -1);
        }

        public List<Tuple<TreeNode<T>, TreeNode<T>>> Pairs(TreeNode<T> t1, TreeNode<T> t2)
        {
            var talg = new TreeAlignment<T>();
            _dict1 = talg.Align(t1);
            _dict2 = talg.Align(t2);

            _alg = new Dictionary<Tuple<TreeNode<T>, TreeNode<T>>, int>();

            AllPairOfIsomorphic(t1, t2);

            return _alg.Keys.ToList();
        }
    }
}
