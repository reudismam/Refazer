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
        public String transformation { get; set; }
        /// <summary>
        /// Syntax tree
        /// </summary>
        /// <returns>Get or set syntax tree</returns>
        public SyntaxTree tree { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="transformation">Transformation</param>
        /// <param name="tree">Syntax tree</param>
        public ASTTransformation(String transformation, SyntaxTree tree) {
            if (transformation == null || tree == null)
            {
                throw new Exception("Transformation or tree cannot be true.");
            }
            this.transformation = transformation;
            this.tree = tree;
        }
    }
}
