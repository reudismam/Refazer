using System;
using System.Collections.Generic;
using Spg.ExampleRefactoring.Data;

namespace Spg.ExampleRefactoring.Data
{
    /// <summary>
    /// Add language parameter test
    /// </summary>
    public class AddLangParam : ExampleCommand
    {
        /// <summary>
        /// Return the train data set.
        /// </summary>
        /// <returns>List of examples</returns>
        public override List<Tuple<string, string>> Train()
        {
            List<Tuple<string, string>> tuples = new List<Tuple<string, string>>();

            string input03 =
@"static void PrintData(String data)
  {
   int exitCode = new MockCSharpCompiler(null, folder.Path, new[] {String.Format(""path,{ 0},private"", longE)}).Run(outWriter);
  }
";


            string output03 =
@"static void PrintData(String data)
  {
   int exitCode = new MockCSharpCompiler(null, folder.Path, new[] {String.Format(""path,{ 0},private"", longE), ""preferreduilang:en""}).Run(outWriter);
  }
";
            Tuple<string, string> tuple03 = Tuple.Create(input03, output03);
            Console.WriteLine(input03);
            Console.WriteLine(output03);
            tuples.Add(tuple03);


            string input01 =
@"static void PrintData(String data)
  {
   int exitCode = new MockCSharpCompiler(null, folder.Path, new[] { source, ""t:library"", ""recurse:."", ""out:abc.dll"" }).Run(outWriter);
  }
";


            string output01 =
@"static void PrintData(String data)
  {
   int exitCode = new MockCSharpCompiler(null, folder.Path, new[] { source, ""preferreduilang:en"", ""t:library"", ""recurse:."", ""out:abc.dll"" }).Run(outWriter);
  }
";
            Tuple<string, string> tuple01 = Tuple.Create(input01, output01);
            Console.WriteLine(input01);
            Console.WriteLine(output01);
            tuples.Add(tuple01);

            string input02 =
@"static void PrintData(String data)
  {
    int exitCode = new MockCSharpCompiler(null, new[] { ""nologo"", ""t:library"", ""recurse:.""}).Run(outWriter);
 }
";


            string output02 =
@"static void PrintData(String data)
  {
    int exitCode = new MockCSharpCompiler(null, new[] { ""nologo"", ""preferreduilang:en"", ""t:library"", ""recurse:.""}).Run(outWriter);
 }
";
            Tuple<string, string> tuple02 = Tuple.Create(input02, output02);
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
        public override Tuple<string, string> Test()
        {
            string input01 =
@"static void PrintData(String data)
  {
   int exitCode = new MockCSharpCompiler(null, folder.Path, new[] { ""nologo"", ""t:library"", ""recurse:."", ""out:abc.dll"" }).Run(outWriter);
  }
";


            string output01 =
@"static void PrintData(String data)
  {
   int exitCode = new MockCSharpCompiler(null, folder.Path, new[] { ""nologo"", ""preferreduilang:en"", ""t:library"", ""recurse:."", ""out:abc.dll"" }).Run(outWriter);
  }
";
            Tuple<string, string> test = Tuple.Create(input01, output01);
            return test;
        }
    }
}


