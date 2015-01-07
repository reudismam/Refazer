using System;
using System.Collections.Generic;

namespace Spg.ExampleRefactoring.Data
{
    /// <summary>
    /// Free command
    /// </summary>
    [Obsolete("Not used anymore", true)]
    public class FreeCommand: ExampleCommand
    {
        /// <summary>
        /// Return the train data set.
        /// </summary>
        /// <returns>List of examples</returns>
        public override List<Tuple<String, String>> Train() {
            Console.WriteLine("WRITE YOUR EXAMPLES BELOW OR # TO GO OUT\nINPUT.");


            String input = Console.ReadLine();;

            List<Tuple<String, String>> tuples = new List<Tuple<string, string>>();

            while (!input.Equals("#")) {
                //option = Console.ReadLine();
                //Console.WriteLine("INPUT");
                //String input = Console.ReadLine();
                Console.WriteLine("OUTPUT");
                String output = Console.ReadLine();
                Tuple<String, String> tuple = Tuple.Create(input, output);
                tuples.Add(tuple);
                Console.WriteLine("NEW EXAMPLE OR # TO GO OUT.\nINPUT");
                input = Console.ReadLine();
            }

            return tuples;
        }

        /// <summary>
        /// Return the test data.
        /// </summary>
        /// <returns>Return a string to be tested.</returns>
        public override Tuple<String, String> Test() {
            Console.WriteLine("WRITE OUR INPUT TEXT OR WRITE # TO GO OUT. \n");
            String str = Console.ReadLine();

            return null;
        }
    }
}
