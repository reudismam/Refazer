using System;
using System.Collections.Generic;

namespace Spg.ExampleRefactoring.Data
{
    /// <summary>
    /// Extract code example
    /// </summary>
    public class ExtractCode : ExampleCommand
    {
        /// <summary>
        /// Return the train data set.
        /// </summary>
        /// <returns>List of examples</returns>
        public override List<Tuple<String, String>> Train() {
            List<Tuple<String, String>> tuples = new List<Tuple<string, string>>();

            string input01 =
@"static void Method1(string data1)
        {
            A a = new A();
            a.aMethod();
        }";


                string output01 =
@"A a = new A();
  a.aMethod();
";
                Tuple<String, String> tuple01 = Tuple.Create(input01, output01);
                Console.WriteLine(input01);
                Console.WriteLine(output01);
                tuples.Add(tuple01);

                string input02 =
@"static void Method2(string data2)
        {
            A a = new A();
            a.aMethod();
            int p = 0;
            Console.WriteLine(data2);
        }";


                string output02 =
@"A a = new A();
  a.aMethod();
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
        public override Tuple<String, String> Test() {
            string input01 =
@"static void Method4(string data4)
        {
            A a = new A();
            a.aMethod();
            int p = 0;
            int j = 2;
            Console.WriteLine(data4);
        }
";

            string output01 =
@"
            A a = new A();
            a.aMethod();        
";
            Tuple<string, string> tuple = Tuple.Create(input01, output01);
            return tuple;
        }
    }
}

