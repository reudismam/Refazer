//using System;
//using System.Collections.Generic;
//using Microsoft.CodeAnalysis;

//namespace Spg.TreeEdit.Node
//{
//    class ConverterHelper
//    {

//        /// <summary>
//        /// Convert a syntax tree to a TreeNode
//        /// </summary>
//        /// <param name="st">Syntax tree root</param>
//        /// <returns>TreeNode</returns>
//        private TreeNode<SyntaxNodeOrToken> _ConvertToTreeNode(SyntaxNodeOrToken st)
//        {
//            if (st.AsNode() == null st == 0)
//            {
//                return new TreeNode<SyntaxNodeOrToken>(st);
//            }

//            List<TreeNode<SyntaxNodeOrToken>> children = new List<TreeNode<SyntaxNodeOrToken>>();
//            foreach (SyntaxNodeOrToken sot in st.ChildNodesAndTokens())
//            {
//                TreeNode<SyntaxNodeOrToken> node = _ConvertToTreeNode(sot);
//                children.Add(node);
//            }

//           TreeNode<SyntaxNodeOrToken> tree = new TreeNode<SyntaxNodeOrToken>(st, children.ToArray());
//            return tree;
//        }
//    }
//}
