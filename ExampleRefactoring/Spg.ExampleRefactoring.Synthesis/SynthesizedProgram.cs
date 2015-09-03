using System.Collections.Generic;
using Spg.ExampleRefactoring.AST;
using Spg.ExampleRefactoring.Expression;

namespace Spg.ExampleRefactoring.Synthesis
{
    /// <summary>
    /// Synthesized program
    /// </summary>
    public class SynthesizedProgram
    {
        /// <summary>
        /// Solution expression list
        /// </summary>
        /// <returns>Get or set solution expression list</returns>
        public List<IExpression> Solutions {get; set;}

        /// <summary>
        /// Default constructor
        /// </summary>
        public SynthesizedProgram(){
            this.Solutions = new List<IExpression>();
        }

        /// <summary>
        /// Add a expression solution
        /// </summary>
        /// <param name="solution"></param>
        public void Add(IExpression solution) {
            this.Solutions.Add(solution);
        }

        /// <summary>
        /// Retrieve the string corresponding to the hypothesis passed as parameter.
        /// </summary>
        /// <param name="input">Input syntax node</param>
        /// <returns>AST transformation</returns>
        public virtual ASTTransformation TransformString(ListNode input)
        {
            return UpdateASTManager.UpdateASTTree(input, this);
        }

        /// <summary>
        /// Retrieve the string corresponding to the hypothesis passed as parameter.
        /// </summary>
        /// <param name="input">Input syntax node</param>
        /// <returns>AST transformation</returns>
        public virtual ListNode TransformInput(ListNode input)
        {
            return UpdateASTManager.UpdateInput(input, this);
        }

        /// <summary>
        /// To string method
        /// </summary>
        /// <returns>String representing this instance</returns>
        public override string ToString()
        {
            string s = "";
            foreach(IExpression str in Solutions){
                s += str + "\n\n";
            }

            return s;
        }
    }
}



