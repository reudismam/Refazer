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
        public override List<Tuple<string, string>> Train()
        {
            List<Tuple<string, string>> tuples = new List<Tuple<string, string>>();

            string input01 =
@"public class ClassA
  {
        public MethodA()
        {
            int p = 0;
            string z = ""4"";
        }
  }
";


            string output01 =
@"internal class ClassA
  {
        public MethodA()
        {
            int p = 0;
            string z = ""4"";
        }
  }
";
            Tuple<string, string> tuple01 = Tuple.Create(input01, output01);
            Console.WriteLine(input01);
            Console.WriteLine(output01);
            tuples.Add(tuple01);

            string input02 =
@"public class ClassB
  {
  }
";


            string output02 =
@"internal class ClassB
  {
  }
";
            Tuple<string, string> tuple02 = Tuple.Create(input02, output02);
            Console.WriteLine(input02);
            Console.WriteLine(output02);
            tuples.Add(tuple02);
            return tuples;
        }

        /// <summary>
        /// Return the test data.
        /// </summary>
        /// <returns>Return a string to be tested.</returns>
        public override Tuple<string, string> Test()
        {
            string input01 =
@"public class ClassC
  {
        public MethodC()
        {
            int p = 0;
        }
  }
";

            string output01 =
@"internal class ClassC
  {
        public MethodC()
        {
            int p = 0;
        }
  }
";
            Tuple<string, string> test = Tuple.Create(input01, output01);
            return test;
        }
    }
}


