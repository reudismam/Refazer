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
        public override List<Tuple<String, String>> Train()
        {
            List<Tuple<String, String>> tuples = new List<Tuple<string, string>>();

            String input01 =
@"class Cars {
    List<Car> compacts;
}
";

            String output01 =
@"class Cars {
    List<Car> compacts = new ArrayList<>();
}
";
            Tuple<String, String> tuple01 = Tuple.Create(input01, output01);
            Console.WriteLine(input01);
            Console.WriteLine(output01);
            tuples.Add(tuple01);

            String input02 =
@"class Sedans {
    List<Sedan> sedans;
}
";


            String output02 =
@"class Sedans {
    List<Sedan> sedans = new ArrayList<>();
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
@"class Buses {
    List<Bus> bus;
}
";


            String output01 =
@"class Buses {
    List<Bus> bus = new ArrayList<>();
}
";
            Tuple<String, String> test = Tuple.Create(input01, output01);
            return test;
        }
    }
}
