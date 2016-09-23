﻿using System;
using System.Collections.Generic;
using TreeEdit.Spg.TreeEdit.Mapping;
using ProseSample.Substrings;
using TreeElement.Spg.Node;

namespace TreeEdit.Spg.Isomorphic
{
    public class IsomorphicManager<T>
    {

        public static bool IsIsomorphic(TreeNode<T> t1, TreeNode<T> t2)
        {
            return AhuTreeIsomorphism(t1, t2);
        }

        public static TreeNode<T> FindIsomorphicSubTree(TreeNode<T> tree, TreeNode<T> subtree)
        {
            if (IsIsomorphic(tree, subtree)) return tree;

            foreach (var child in tree.Children)
            {
                var result = FindIsomorphicSubTree(child, subtree);
                if (result != null) return result;
            }

            return null;
        }

        public static bool AhuTreeIsomorphism(TreeNode<T> t1, TreeNode<T> t2)
        {
            var talg = new TreeAlignment<T>();
            Dictionary<TreeNode<T>, string> dict1 = talg.Align(t1);
            Dictionary<TreeNode<T>, string> dict2 = talg.Align(t2);

            if (dict1[t1].Equals(dict2[t2]))
            {
                return true;
            }

            return false;
        }

        public static List<Tuple<TreeNode<T>, TreeNode<T>>> AllPairOfIsomorphic(TreeNode<T> t1, TreeNode<T> t2)
        {
            var pairs = new IsomorphicPairs<T>();
            var ps = pairs.Pairs(t1, t2);

            return ps;
        }
    }
}
