using System;
using System.Collections.Generic;
using System.Linq;
using TreeEdit.Spg.LogInfo;
using TreeEdit.Spg.Transform;

namespace RefazerManager
{
    class Program
    {
        public static void Main()
        {
            var before = @"C:\Users\SPG-04\Documents\Test\SyntaxTreeExtensionsB.cs";
            var after  = @"C:\Users\SPG-04\Documents\Test\SyntaxTreeExtensionsA.cs";
            var tuple  = Tuple.Create(before, after);
            var examples = new List<Tuple<string, string>>();
            examples.Add(tuple);
            var program = Refazer4CSharp.LearnTransformation(examples);
            Refazer4CSharp.Apply(program, before);
            var transformedDocuments = ASTTransformer.Transform(TransformationsInfo.GetInstance().Transformations);
            var document = transformedDocuments.First().Item2.ToString();
        }
    }
}
