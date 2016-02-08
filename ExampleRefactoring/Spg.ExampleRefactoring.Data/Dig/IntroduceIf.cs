using System;
using System.Collections.Generic;

namespace Spg.ExampleRefactoring.Data.Dig
{
    /// <summary>
    /// Introduce If command
    /// </summary>
    public class IntroduceIf : ExampleCommand
    {
        /// <summary>
        /// Return the train data set.
        /// </summary>
        /// <returns>List of examples</returns>
        public override List<Tuple<string, string>> Train()
        {
            List<Tuple<string, string>> tuples = new List<Tuple<string, string>>();

            string input01 =
@"double computeDistance(Point p1, Point p2)
{
    return -1;
}
";


            string output01 =
@"double computeDistance(Point p1, Point p2)
{
    if (p1.equals(p2)) return 0;
    return -1;
}
";
            Tuple<string, string> tuple01 = Tuple.Create(input01, output01);
            Console.WriteLine(input01);
            Console.WriteLine(output01);
            tuples.Add(tuple01);

            string input02 =
@"float computeDirection(Point o, Point p)
  {
    int p = 0;
    Console.WriteLine(data);
    return -1;
  }
";


            string output02 =
@"float computeDirection(Point o, Point p)
  {
    if (o.equals(p)) return 0;
    int p = 0;
    Console.WriteLine(data);
    return -1;
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
@"float computeDirection2(Point z, Point p)
  {
    int p = 0;
    int j = 2;
    Console.WriteLine(data);
    return -1;
 }
";

            string output01 =
@"float computeDirection2(Point z, Point p)
  {
    if (z.equals(p)) return 0;
    int p = 0;
    int j = 2;
    Console.WriteLine(data);
    return -1;
 }
";
            Tuple<string, string> test = Tuple.Create(input01, output01);
            return test;
        }
    }
}


