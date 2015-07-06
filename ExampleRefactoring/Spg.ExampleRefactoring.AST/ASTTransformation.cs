using System;
using Microsoft.CodeAnalysis;

namespace Spg.ExampleRefactoring.AST
{
    /// <summary>
    /// Abstract syntax tree transformation
    /// </summary>
    public class ASTTransformation
    {
        /// <summary>
        /// Transformation
        /// </summary>
        /// <returns>Get or set transformation</returns>
        public String Transformation { get; set; }
        /// <summary>
        /// Syntax tree
        /// </summary>
        /// <returns>Get or set syntax tree</returns>
        public SyntaxTree Tree { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="transformation">Transformation</param>
        /// <param name="tree">Syntax tree</param>
        public ASTTransformation(string transformation, SyntaxTree tree) {
            if (transformation == null) throw new ArgumentNullException("transformation");
            if (tree == null) throw new ArgumentNullException("tree");
            this.Transformation = transformation;
            this.Tree = tree;
        }
    }
}


