using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using ProseFunctions.Spg.Bean;
using ProseFunctions.Substrings;
using TreeElement.Spg.Node;

namespace ProseFunctions.Substrings.Spg.Semantic
{
    public class SemanticMatch
    {   
        public static Pattern C(SyntaxKind kind, IEnumerable<Pattern> children)
        {
            var pchildren = children.Select(child => child.Tree).ToList();
            var token = (kind == SyntaxKind.EmptyStatement) ? new EmptyToken() : new Token(kind, null);
            var inode = new TreeNode<Token>(token, null, pchildren);
            var pattern = new Pattern(inode);
            return pattern;
        }    

        /// <summary>
        /// Literal
        /// </summary>
        /// <param name="tree">Tree</param>
        /// <returns>Literal</returns>
        public static Pattern Literal(SyntaxNodeOrToken tree)
        {
            var token = new DynToken(tree.Kind(), ConverterHelper.ConvertCSharpToTreeNode(tree));
            var label = new TLabel(tree.Kind());
            var inode = new TreeNode<Token>(token, label);
            var pattern = new Pattern(inode);
            return pattern;
        }

        /// <summary>
        /// Variable
        /// </summary>
        /// <param name="kind">Kind</param>
        /// <returns>Variable pattern</returns>
        public static Pattern Variable(SyntaxKind kind)
        {
            var token = (kind == SyntaxKind.EmptyStatement) ? new EmptyToken() : new Token(kind, null);
            var inode = new TreeNode<Token>(token, null);
            var pattern = new Pattern(inode);
            return pattern;
        }

        /// <summary>
        /// Variable
        /// </summary>
        /// <param name="kind">Kind</param>
        /// <returns>Variable pattern</returns>
        public static Pattern Leaf(SyntaxKind kind)
        {
            var token = new LeafToken(kind, null);
            var inode = new TreeNode<Token>(token, null);
            var pattern = new Pattern(inode);
            return pattern;
        }
    }
}
