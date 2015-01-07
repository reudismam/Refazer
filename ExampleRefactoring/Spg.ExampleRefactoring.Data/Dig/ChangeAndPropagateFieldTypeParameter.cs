using System;
using System.Collections.Generic;

namespace Spg.ExampleRefactoring.Data.Dig
{
    /// <summary>
    /// Covert element to collection test
    /// </summary>
    public class ChangeAndPropagateFieldTypeParameter : ExampleCommand
    {
        /// <summary>
        /// Return the train data set.
        /// </summary>
        /// <returns>List of examples</returns>
        public override List<Tuple<String, String>> Train()
        {
            List<Tuple<String, String>> tuples = new List<Tuple<string, string>>();

            String input01 =
@"class Class1{
    int mileage;

    void updateMileage(int newMiles){
        mileage += newMiles;
    }
}
";

            String output01 =
@"class Class1{
    long mileage;

    void updateMileage(long newMiles){
        mileage += newMiles;
    }
}
";
            Tuple<String, String> tuple01 = Tuple.Create(input01, output01);
            Console.WriteLine(input01);
            Console.WriteLine(output01);
            tuples.Add(tuple01);

            String input02 =
@"class Class2{
    int mileage;

    void updateMiles(int newMiles){
        mileage += newMiles;
        string z = ""4"";
    }
}
";


            String output02 =
@"class Class2{
    long mileage;

    void updateMiles(long newMiles){
        mileage += newMiles;
        string z = ""4"";
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
            String input01 =
@"class Class3{
    int mileage;

    void updateMiles(int newMiles){
        int p = 10;
        int r = 2;
        mileage += newMiles;
        string z = ""4"";
    }
}
";


            String output01 =
@"class Class3{
    long mileage;

    void updateMiles(long newMiles){
        int p = 10;
        int r = 2;
        mileage += newMiles;
        string z = ""4"";
    }
}
";
            Tuple<String, String> test = Tuple.Create(input01, output01);
            return test;
        }
    }
}
