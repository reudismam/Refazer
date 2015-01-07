using System;
using System.Collections.Generic;

namespace Spg.ExampleRefactoring.Data.Dig
{
    /// <summary>
    /// Covert element to collection test
    /// </summary>
    public class ConvertElementToCollection : ExampleCommand
    {
        /// <summary>
        /// Return the train data set.
        /// </summary>
        /// <returns>List of examples</returns>
        public override List<Tuple<String, String>> Train()
        {
            List<Tuple<String, String>> tuples = new List<Tuple<string, string>>();

            String input01 =
@"void start(Car car){
    car.start();
}
";


            String output01 =
@"void start(List<Car> list){
    for(Car car in list)
        car.start();
}";
            Tuple<String, String> tuple01 = Tuple.Create(input01, output01);
            Console.WriteLine(input01);
            Console.WriteLine(output01);
            tuples.Add(tuple01);

            String input02 =
@"void start(Book book){
    book.start();
}
";


            String output02 =
@"void start(List<Book> list){
    for(Book book in list)
        book.start();
}";
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
@"void start(Bus bus){
    bus.start();
}
";

            String output01 =
@"void start(List<Bus> list){
    for(Bus bus in list)
        bus.start();
}";
            Tuple<String, String> test = Tuple.Create(input01, output01);
            return test;
        }
    }
}
