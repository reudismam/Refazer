using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spg.ExampleRefactoring.Data;

namespace ExampleRefactoring.Spg.ExampleRefactoring.Data
{
    public class AddLangParam : ExampleCommand
    {
        /// <summary>
        /// Return the train data set.
        /// </summary>
        /// <returns>List of examples</returns>
        public override List<Tuple<String, String>> Train()
        {
            List<Tuple<String, String>> tuples = new List<Tuple<string, string>>();

            String input03 =
@"static void PrintData(String data)
  {
   int exitCode = new MockCSharpCompiler(null, folder.Path, new[] {String.Format(""path,{ 0},private"", longE)}).Run(outWriter);
  }
";


            String output03 =
@"static void PrintData(String data)
  {
   int exitCode = new MockCSharpCompiler(null, folder.Path, new[] {String.Format(""path,{ 0},private"", longE), ""preferreduilang:en""}).Run(outWriter);
  }
";
            Tuple<String, String> tuple03 = Tuple.Create(input03, output03);
            Console.WriteLine(input03);
            Console.WriteLine(output03);
            tuples.Add(tuple03);


            String input01 =
@"static void PrintData(String data)
  {
   int exitCode = new MockCSharpCompiler(null, folder.Path, new[] { source, ""t:library"", ""recurse:."", ""out:abc.dll"" }).Run(outWriter);
  }
";


            String output01 =
@"static void PrintData(String data)
  {
   int exitCode = new MockCSharpCompiler(null, folder.Path, new[] { source, ""preferreduilang:en"", ""t:library"", ""recurse:."", ""out:abc.dll"" }).Run(outWriter);
  }
";
            Tuple<String, String> tuple01 = Tuple.Create(input01, output01);
            Console.WriteLine(input01);
            Console.WriteLine(output01);
            tuples.Add(tuple01);

            String input02 =
@"static void PrintData(String data)
  {
    int exitCode = new MockCSharpCompiler(null, new[] { ""nologo"", ""t:library"", ""recurse:.""}).Run(outWriter);
 }
";


            String output02 =
@"static void PrintData(String data)
  {
    int exitCode = new MockCSharpCompiler(null, new[] { ""nologo"", ""preferreduilang:en"", ""t:library"", ""recurse:.""}).Run(outWriter);
 }
";
            Tuple<String, String> tuple02 = Tuple.Create(input02, output02);
            Console.WriteLine(input02);
            Console.WriteLine(output02);
            tuples.Add(tuple02);
            //}
            
            return tuples;
        }

        /// <summary>
        /// Return the test data.
        /// </summary>
        /// <returns>Return a string to be tested.</returns>
        public override Tuple<String, String> Test()
        {
            String input01 =
@"static void PrintData(String data)
  {
   int exitCode = new MockCSharpCompiler(null, folder.Path, new[] { ""nologo"", ""t:library"", ""recurse:."", ""out:abc.dll"" }).Run(outWriter);
  }
";


            String output01 =
@"static void PrintData(String data)
  {
   int exitCode = new MockCSharpCompiler(null, folder.Path, new[] { ""nologo"", ""preferreduilang:en"", ""t:library"", ""recurse:."", ""out:abc.dll"" }).Run(outWriter);
  }
";
            Tuple<String, String> test = Tuple.Create(input01, output01);
            return test;
        }
    }
}

