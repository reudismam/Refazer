using System;
using System.Collections.Generic;

namespace Spg.ExampleRefactoring.Data
{
    /// <summary>
    /// Add parameter command
    /// </summary>
    public class ParameterChangeOnMethod : ExampleCommand
    {
        /// <summary>
        /// Return the train data set.
        /// </summary>
        /// <returns>List of examples</returns>
        public override List<Tuple<string, string>> Train()
        {
            List<Tuple<string, string>> tuples = new List<Tuple<string, string>>();

            string input01 =
@"public static string FormatLiteral(string value, bool quote)
        {
            return ObjectDisplay.FormatLiteral(value, quote);
        }
";


            string output01 =
@"public static string FormatLiteral(string value, bool quote)
        {
            return ObjectDisplay.FormatLiteral(value, quote ? ObjectDisplayOptions.UseQuotes : ObjectDisplayOptions.None);
        }
";
            Tuple<string, string> tuple01 = Tuple.Create(input01, output01);
            Console.WriteLine(input01);
            Console.WriteLine(output01);
            tuples.Add(tuple01);

            string input02 =
@"public static string FormatLiteral(char c, bool quote)
        {
            return ObjectDisplay.FormatLiteral(c, quote);
        }
";


            string output02 =
@"public static string FormatLiteral(char c, bool quote)
        {
            return ObjectDisplay.FormatLiteral(c, quote ? ObjectDisplayOptions.UseQuotes : ObjectDisplayOptions.None);
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
@"public static string FormatLiteral(double c, bool quote)
        {
            return ObjectDisplay.FormatLiteral(c, quote);
        }
";

            string output01 =
@"public static string FormatLiteral(double c, bool quote)
        {
            return ObjectDisplay.FormatLiteral(c, quote ? ObjectDisplayOptions.UseQuotes : ObjectDisplayOptions.None);
        }
";
            Tuple<string, string> test = Tuple.Create(input01, output01);
            return test;
        }
    }
}


