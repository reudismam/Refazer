using System;
using System.Collections.Generic;
using Spg.ExampleRefactoring.AST;
using Spg.LocationRefactor.Predicate;

namespace Spg.ExampleRefactoring.Synthesis
{
    /// <summary>
    /// Switch expression
    /// </summary>
    public class Switch: SynthesizedProgram
    {
        /// <summary>
        /// Gates
        /// </summary>
        public List<Tuple<IPredicate, SynthesizedProgram>> Gates { get; set; }

        /// <summary>
        /// Switch contructor
        /// </summary>
        /// <param name="gates"></param>
        public Switch(List<Tuple<IPredicate, SynthesizedProgram>> gates)
        { 
            this.Gates = gates;
        }


        /// <summary>
        /// Transform string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override ASTTransformation TransformString(ListNode input)
        {
            foreach (Tuple<IPredicate, SynthesizedProgram> item in Gates)
            {
                if (item.Item1.Evaluate(input))
                {
                    return item.Item2.TransformString(input);
                }
            }
            return null;
        }

        public override ListNode TransformInput(ListNode input)
        {
            foreach (Tuple<IPredicate, SynthesizedProgram> item in Gates)
            {
                if (item.Item1.Evaluate(input))
                {
                    return item.Item2.TransformInput(input);
                }
            }
            throw new ArgumentException("The given argument if not valid for this program");
        }

        public override string ToString()
        {
            string s = "Switch(";

            s += "(b1, e1)";

            for (int i = 1; i < Gates.Count; i++)
            {
                s += ", SS" + (i + 1);
            }

            s += ")";

            for (int i = 0; i < Gates.Count; i++)
            {
                s += "\n\tb" + (i + 1) + " = " + Gates[i].Item1
                   + "\n\te" + (i + 1) + " = " + Gates[i].Item2;
            }
            return s;
        }
    }
}
