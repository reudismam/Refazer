using System;
using System.Collections.Generic;
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
        public List<IExpression> solutions {get; set;}

        /// <summary>
        /// Default constructor
        /// </summary>
        public SynthesizedProgram(){
            this.solutions = new List<IExpression>();
        }

        /// <summary>
        /// Add a expression solution
        /// </summary>
        /// <param name="solution"></param>
        public void Add(IExpression solution) {
            this.solutions.Add(solution);
        }

        /// <summary>
        /// To string method
        /// </summary>
        /// <returns>String representing this instance</returns>
        public override string ToString()
        {
            String s = "";
            foreach(IExpression str in solutions){
                s += str + "\n\n";
            }

            return s;
        }
    }
}
