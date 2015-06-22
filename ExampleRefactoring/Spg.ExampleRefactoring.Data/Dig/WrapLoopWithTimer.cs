using System;
using System.Collections.Generic;

namespace Spg.ExampleRefactoring.Data.Dig
{
    /// <summary>
    /// Covert element to collection test
    /// </summary>
    public class WrapLoopWithTimer : ExampleCommand
    {
        /// <summary>
        /// Return the train data set.
        /// </summary>
        /// <returns>List of examples</returns>
        public override List<Tuple<String, String>> Train()
        {
            List<Tuple<String, String>> tuples = new List<Tuple<string, string>>();

            string input01 =
@"void Start(){
    for(i = 0; i < 1000; i++)
        if (isPrime(i)) println(i);
}
";


            string output01 =
@"void Start(){
    long start = System.currentTimeinMillis();
    for(i = 0; i < 1000; i++)
        if (isPrime(i)) println(i);
    long end = System.currentTimeinMillis();
    long totalTime = end – start;
}
";
            Tuple<String, String> tuple01 = Tuple.Create(input01, output01);
            Console.WriteLine(input01);
            Console.WriteLine(output01);
            tuples.Add(tuple01);

            string input02 =
@"void Initialize(){
    for(j = 0; j < 1000; j++)
        if (isEven(j)) println(j);
}
";


            string output02 =
@"void Initialize(){
    long start = System.currentTimeinMillis();
    for(j = 0; j < 1000; j++)
        if (isEven(j)) println(j);
    long end = System.currentTimeinMillis();
    long totalTime = end – start;
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
@"void Initialize(){
    for(k = 0; k < 1000; k++)
        if (isOdd(k)) println(k);
}
";
            
            string output01 =
@"void Initialize(){
    long start = System.currentTimeinMillis();
    for(k = 0; k < 1000; k++)
        if (isOdd(k)) println(k);
    long end = System.currentTimeinMillis();
    long totalTime = end – start;
}
";
            Tuple<String, String> test = Tuple.Create(input01, output01);
            return test;
        }
    }
}

