﻿using ProseSample.Substrings;
using TreeElement.Spg.Node;

namespace ProseSample.Substrings
{
    public class Pattern
    {
        public TreeNode<Token> Tree;

        public string K { get; set; }

        public Pattern(TreeNode<Token> tree, string k)
        {
            Tree = tree;
            K = k;
        }

        public Pattern(TreeNode<Token> tree)
        {
            Tree = tree;
        }

        public override string ToString()
        {
            return $"Pattern({Tree}, {K})";
        }
    }
}
