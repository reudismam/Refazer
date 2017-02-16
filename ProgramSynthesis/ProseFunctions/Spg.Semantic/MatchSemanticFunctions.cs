using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using ProseFunctions.Spg.Bean;
using ProseFunctions.Substrings;
using TreeElement.Spg.Node;

namespace ProseFunctions.Spg.Semantic
{
    public class MatchSemanticFunctions
    {   
        public static Pattern C(string kind, IEnumerable<Pattern> children)
        {
            var pchildren = children.Select(child => child.Tree).ToList();
            var token = kind.Equals(Token.Expression) ? new EmptyToken() : new Token(kind, null);
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
            var token = new DynToken(tree.Kind().ToString(), ConverterHelper.ConvertCSharpToTreeNode(tree));
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
        public static Pattern Variable(string kind)
        {
            var token = kind.Equals(Token.Expression) ? new EmptyToken() : new Token(kind, null);
            var inode = new TreeNode<Token>(token, null);
            var pattern = new Pattern(inode);
            return pattern;
        }

        /// <summary>
        /// Variable
        /// </summary>
        /// <param name="kind">Kind</param>
        /// <returns>Variable pattern</returns>
        public static Pattern Leaf(string kind)
        {
            var token = new LeafToken(kind, null);
            var inode = new TreeNode<Token>(token, null);
            var pattern = new Pattern(inode);
            return pattern;
        }
    }
}
