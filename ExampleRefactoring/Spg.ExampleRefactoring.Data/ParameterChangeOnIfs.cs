using System;
using System.Collections.Generic;

namespace Spg.ExampleRefactoring.Data
{
    /// <summary>
    /// Add parameter command
    /// </summary>
    public class ParameterChangeOnIfs : ExampleCommand
    {
        /// <summary>
        /// Return the train data set.
        /// </summary>
        /// <returns>List of examples</returns>
        public override List<Tuple<String, String>> Train()
        {
            List<Tuple<String, String>> tuples = new List<Tuple<string, string>>();

            string input01 =
@"static void Method1(string[] args)
  {
    if (type == typeof(int))
            {
                return FormatLiteral((int)obj, useHexadecimalNumbers);
            }
  }
";


            string output01 =
@"static void Method1(string[] args)
  {
    if (type == typeof(int))
            {
                return FormatLiteral((int)obj, options);
            }
  }
";
            Tuple<String, String> tuple01 = Tuple.Create(input01, output01);
            Console.WriteLine(input01);
            Console.WriteLine(output01);
            tuples.Add(tuple01);

            string input02 =
@"static void Method2(string[] args)
  {
    if (type == typeof(string))
            {
                return FormatLiteral((string)obj, quoteStrings);
            }
  }
";


            string output02 =
@"static void Method2(string[] args)
  {
    if (type == typeof(string))
            {
                return FormatLiteral((string)obj, options);
            }
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
            string input01 =
@"static void Method3(string[] args)
  {
    if (type == typeof(char))
            {
                return FormatLiteral((char)obj, quoteStrings);
            }
  }
";

            string output01 =
@"static void Method3(string[] args)
  {
    if (type == typeof(char))
            {
                return FormatLiteral((char)obj, options);
            }
  }
";
            Tuple<String, String> test = Tuple.Create(input01, output01);
            return test;
        }
    }
}

