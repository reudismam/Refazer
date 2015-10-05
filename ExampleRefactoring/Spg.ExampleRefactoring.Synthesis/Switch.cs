using System;
using System.Collections.Generic;
using Spg.ExampleRefactoring.AST;
using Spg.LocationRefactor.Predicate;

namespace Spg.ExampleRefactoring.Synthesis
{
    public class Switch: SynthesizedProgram
    {
        public List<Tuple<IPredicate, SynthesizedProgram>> Gates;

        public Switch(List<Tuple<IPredicate, SynthesizedProgram>> gates)
        { 
            this.Gates = gates;
        }

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
    }
}
