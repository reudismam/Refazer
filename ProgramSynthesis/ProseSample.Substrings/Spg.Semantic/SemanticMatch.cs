using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using TreeElement.Spg.Node;

namespace ProseSample.Substrings.Spg.Semantic
{
    public class SemanticMatch
    {   
        public static Pattern C(SyntaxKind kind, IEnumerable<Pattern> children)
        {
            var pchildren = children.Select(child => child.Tree).ToList();

            var token = new Token(kind);
            var inode = new TreeNode<Token>(token, null, pchildren);
            var pattern = new Pattern(inode);
            return pattern;
        }    

        /// <summary>
        /// Literal
        /// </summary>
        /// <param name="tree">Value</param>
        /// <returns>Literal</returns>
        public static Pattern Literal(SyntaxNodeOrToken tree)
        {
            var token = new DynToken(tree.Kind(), tree);
            var label = new TLabel(tree.Kind());
            var inode = new TreeNode<Token>(token, label);
            var pattern = new Pattern(inode);
            return pattern;
        }

        /// <summary>
        /// Searches a node with with kind and occurrence
        /// </summary>
        /// <param name="kind">Kind</param>
        /// <returns>Search result</returns>
        public static Pattern Variable(SyntaxKind kind)
        {
            var token = new Token(kind);
            var inode = new TreeNode<Token>(token, null);
            var pattern = new Pattern(inode);
            return pattern;
        }
    }
}
