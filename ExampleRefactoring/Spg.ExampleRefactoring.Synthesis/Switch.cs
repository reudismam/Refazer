using System;
using System.Collections.Generic;
using Spg.ExampleRefactoring.AST;
using Spg.ExampleRefactoring.Synthesis;
using Spg.LocationRefactor.Predicate;

namespace ExampleRefactoring.Spg.ExampleRefactoring.Synthesis
{
    internal  class Switch: SynthesizedProgram
    {
        public List<Tuple<IPredicate, SynthesizedProgram>> Gates = new List<Tuple<IPredicate, SynthesizedProgram>>();

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
    }
}
