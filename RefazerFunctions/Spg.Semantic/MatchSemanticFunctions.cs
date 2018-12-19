using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using RefazerFunctions.Bean;
using RefazerFunctions.Substrings;
using TreeElement.Spg.Node;
using TreeElement.Token;

namespace RefazerFunctions.Spg.Semantic
{
    public class MatchSemanticFunctions
    {   
        public static Pattern C(Label kind, IEnumerable<Pattern> children)
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
        /// <param name="nodeTree">Tree</param>
        /// <returns>Literal</returns>
        public static Pattern Literal(Node nodeTree)
        {
            SyntaxNodeOrToken tree = nodeTree.Value.Value;
            var token = new DynToken(new Label(tree.Kind().ToString()), ConverterHelper.ConvertCSharpToTreeNode(tree));
            var label = new TLabel(tree.Kind());
            var inode = new TreeNode<Token>(token, label);
            var pattern = new Pattern(inode);
            return pattern;
        }

        /// <summary>
        /// Variable
        /// </summary>
        /// <param name="kind">Label</param>
        /// <returns>Variable pattern</returns>
        public static Pattern Variable(Label kind)
        {
            var token = kind.IsLabel(new Label(Token.Expression)) ? new EmptyToken() : new Token(kind, null);
            var inode = new TreeNode<Token>(token, null);
            var pattern = new Pattern(inode);
            return pattern;
        }
    }
}
