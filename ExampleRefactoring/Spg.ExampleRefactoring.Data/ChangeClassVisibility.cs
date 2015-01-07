using System;
using System.Collections.Generic;

namespace Spg.ExampleRefactoring.Data
{
    /// <summary>
    /// Change class visibility
    /// </summary>
    public class ChangeClassVisibility : ExampleCommand
    {
        /// <summary>
        /// Return the train data set.
        /// </summary>
        /// <returns>List of examples</returns>
        public override List<Tuple<String, String>> Train()
        {
            List<Tuple<String, String>> tuples = new List<Tuple<string, string>>();

            String input01 =
@"public class ClassA
  {
        public MethodA()
        {
            int p = 0;
            string z = ""4"";
        }
  }
";


            String output01 =
@"internal class ClassA
  {
        public MethodA()
        {
            int p = 0;
            string z = ""4"";
        }
  }
";
            Tuple<String, String> tuple01 = Tuple.Create(input01, output01);
            Console.WriteLine(input01);
            Console.WriteLine(output01);
            tuples.Add(tuple01);

            String input02 =
@"public class ClassB
  {
  }
";


            String output02 =
@"internal class ClassB
  {
  }
";
            Tuple<String, String> tuple02 = Tuple.Create(input02, output02);
            Console.WriteLine(input02);
            Console.WriteLine(output02);
            tuples.Add(tuple02);
            return tuples;
        }

        /// <summary>
        /// Return the test data.
        /// </summary>
        /// <returns>Return a string to be tested.</returns>
        public override Tuple<String, String> Test()
        {
            String input01 =
@"public class ClassC
  {
        public MethodC()
        {
            int p = 0;
        }
  }
";

            String output01 =
@"internal class ClassC
  {
        public MethodC()
        {
            int p = 0;
        }
  }
";
            Tuple<String, String> test = Tuple.Create(input01, output01);
            return test;
        }
    }
}
