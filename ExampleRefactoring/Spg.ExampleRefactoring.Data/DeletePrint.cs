using System;
using System.Collections.Generic;

namespace Spg.ExampleRefactoring.Data
{
    /// <summary>
    /// Delete print example
    /// </summary>
    public class DeletePrint: ExampleCommand
    {
        /// <summary>
        /// Return the train data set.
        /// </summary>
        /// <returns>List of examples</returns>
        public override List<Tuple<string, string>> Train() {
            List<Tuple<string, string>> tuples = new List<Tuple<string, string>>();

                string input01 =
@"static void Main1(string[] args)
  {
     int j = 2;
     Console.WriteLine(""Hello, World!"");
     int i = 0;
  }
";


                string output01 =
@"static void Main1(string[] args)
  {
     int j = 2;
     int i = 0;
  }
";
                Tuple<string, string> tuple01 = Tuple.Create(input01, output01);
                Console.WriteLine(input01);
                Console.WriteLine(output01);
                tuples.Add(tuple01);

                string input02 =
@"static void Main1(string[] args)
{
    int j = 2;
    int i = 0;
    string z = ""4""; 
    Console.WriteLine(""Hello, Earth!"");
}";
            

                string output02 =
@"static void Main1(string[] args)
{
    int j = 2;
    int i = 0;
    string z = ""4""; 
}";
                Tuple<string, string> tuple02 = Tuple.Create(input02, output02);
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
        public override Tuple<string, string> Test() {
            string input01 =
@"static void Main1(string[] args)
{
    int j = 2;
    Console.WriteLine(""Hello, World!"");
    int i = 0;
    int b = 2;
    Console.WriteLine(""Hello, World (For the second time.)"");
}";

            string output01 =
@"static void Main1(string[] args)
{
    int j = 2;
    int i = 0;
    int b = 2;
    Console.WriteLine(""Hello, World (For the second time.)"");
}";
            Tuple<string, string> test = Tuple.Create(input01, output01);
            return test;
        }
    }
}


