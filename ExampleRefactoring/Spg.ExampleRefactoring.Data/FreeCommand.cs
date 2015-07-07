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
        public override List<Tuple<string, string>> Train() {
            Console.WriteLine("WRITE YOUR EXAMPLES BELOW OR # TO GO OUT\nINPUT.");


            string input = Console.ReadLine();;

            List<Tuple<string, string>> tuples = new List<Tuple<string, string>>();

            while (!input.Equals("#")) {
                //option = Console.ReadLine();
                //Console.WriteLine("INPUT");
                //String input = Console.ReadLine();
                Console.WriteLine("OUTPUT");
                string output = Console.ReadLine();
                Tuple<string, string> tuple = Tuple.Create(input, output);
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
        public override Tuple<string, string> Test() {
            Console.WriteLine("WRITE OUR INPUT TEXT OR WRITE # TO GO OUT. \n");
            string str = Console.ReadLine();

            return null;
        }
    }
}


