using System;
using System.Collections.Generic;

namespace Spg.ExampleRefactoring.Data.Dig
{
    /// <summary>
    /// Covert element to collection test
    /// </summary>
    public class CopyFieldInitializer : ExampleCommand
    {
        /// <summary>
        /// Return the train data set.
        /// </summary>
        /// <returns>List of examples</returns>
        public override List<Tuple<string, string>> Train()
        {
            List<Tuple<string, string>> tuples = new List<Tuple<string, string>>();

            string input01 =
@"class Cars {
    List<Car> compacts;
}
";

            string output01 =
@"class Cars {
    List<Car> compacts = new ArrayList<>();
}
";
            Tuple<string, string> tuple01 = Tuple.Create(input01, output01);
            Console.WriteLine(input01);
            Console.WriteLine(output01);
            tuples.Add(tuple01);

            string input02 =
@"class Sedans {
    List<Sedan> sedans;
}
";


            string output02 =
@"class Sedans {
    List<Sedan> sedans = new ArrayList<>();
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
@"class Buses {
    List<Bus> bus;
}
";


            string output01 =
@"class Buses {
    List<Bus> bus = new ArrayList<>();
}
";
            Tuple<string, string> test = Tuple.Create(input01, output01);
            return test;
        }
    }
}


