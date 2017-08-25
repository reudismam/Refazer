using System;
using System.Collections.Generic;
using ProseManager;

namespace RefazerManager
{
    class Program
    {
        public static void Main()
        {
            var before = @"C:\Users\SPG-04\Documents\Test\QueryableExtensionsB.cs";
            var after =  @"C:\Users\SPG-04\Documents\Test\QueryableExtensionsA.cs";
            var tuple = Tuple.Create(before, after);
            var examples = new List<Tuple<string, string>>();
            examples.Add(tuple);
            var program = Refazer4CSharp.LearnTransformation(examples);
            var result = Refazer4CSharp.Apply(program, before);
        }


    }
}
