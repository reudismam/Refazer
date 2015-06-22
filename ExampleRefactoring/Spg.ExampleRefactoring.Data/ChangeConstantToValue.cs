using System;
using System.Collections.Generic;

namespace Spg.ExampleRefactoring.Data
{
    /// <summary>
    /// Change constant to value refactor
    /// </summary>
    public class ChangeConstantToValue : ExampleCommand
    {
        /// <summary>
        /// Return the train data set.
        /// </summary>
        /// <returns>List of examples</returns>
        public override List<Tuple<String, String>> Train() {
            List<Tuple<String, String>> tuples = new List<Tuple<string, string>>();

                string input01 =
@"public object[] Method(object type)
        {
            g.setLineWidth(LW);
        }
";

                string output01 =
@"public object[] Method(object type)
        {
            g.setLineWidth(0);
        }
";
            Tuple<String, String> tuple01 = Tuple.Create(input01, output01);
                Console.WriteLine(input01);
                Console.WriteLine(output01);

            string input02 =
@"public object[] Method2(object type)
        {
                string z = ""4"";
                g.setLineWidth(LW);
        }
";

            string output02 =
@"public object[] Method2(object type)
        {
                string z = ""4"";
                g.setLineWidth(0);
        }
";
            Tuple<String, String> tuple02 = Tuple.Create(input02, output02);
            Console.WriteLine(input02);
            Console.WriteLine(output02);

            tuples.Add(tuple01);
            tuples.Add(tuple02);
            return tuples;
        }

        /// <summary>
        /// Return the test data.
        /// </summary>
        /// <returns>Return a string to be tested.</returns>
        public override Tuple<String, String> Test() {
            string input01 =
@"public object[] Method3(object type)
        {
                g.setLineWidth(LW);
        }
";

            string output01 =
@"public object[] Method3(object type)
        {
                g.setLineWidth(0);
        }
";
            Tuple<String, String> test = Tuple.Create(input01, output01);
            return test;
        }
    }
}

