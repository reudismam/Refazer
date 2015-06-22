using System;
using System.Collections.Generic;

namespace Spg.ExampleRefactoring.Data
{
    /// <summary>
    /// Refactoring 01
    /// </summary>
    public class Refact01: ExampleCommand
    {
        /// <summary>
        /// Return the train data set.
        /// </summary>
        /// <returns>List of examples</returns>
        public override List<Tuple<String, String>> Train() {
            List<Tuple<String, String>> tuples = new List<Tuple<string, string>>();

                string input01 =
@"IEnumerator enumerable = new List().GetEnumerator();
            List<object> configs = new List();

            while (enumerable.MoveNext())
            {
                config = (bool)enumerable.Current;

                if (config is bool)
                {
                    configs.Add(config);
                }
            }

            return configs.ToArray(configs);
";


                string output01 =
@"public object[] Method(object type)
        {
            IEnumerator enumerable = new List().GetEnumerator();
            List<object> configs = new List();
            object config = null;

            while (enumerable.MoveNext())
            {
                if (config == true)
                {
                    bool = false;
                }

                if (config is bool)
                {
                    configs.Add(config);
                }
            }

            return configs.ToArray(configs);
        }
";
                Tuple<String, String> tuple01 = Tuple.Create(input01, output01);
                Console.WriteLine(input01);
                Console.WriteLine(output01);
                tuples.Add(tuple01);

            string input02 =
@"public object[] Method(object type)
        {
            while (enumerable.MoveNext())
            {
                object cfg = enumerable.Current;
            }
            return configs.ToArray(configs);
        }
";


            string output02 =
@"public object[] Method(object type)
        {
            object cfg = null;
            while (enumerable.MoveNext())
            {
                cfg = enumerable.Currect;
            }
            return configs.ToArray(configs);
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
        public override Tuple<String, String> Test() {
            string input01 =
@"public object[] Method(object type)
        {
            while (enumerable.MoveNext())
            {
                object a = enumerable.Current;
            }
            return configs.ToArray(configs);
        }
";

            string output01 =
@"public object[] Method(object type)
        {
            while (enumerable.MoveNext())
            {
                object a = enumerable.Current;
            }
            return configs.ToArray(configs);
        }
";
            Tuple<String, String> test = Tuple.Create(input01, output01);
            return test;
        }
    }
}

