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
        public override List<Tuple<string, string>> Train()
        {
            List<Tuple<string, string>> tuples = new List<Tuple<string, string>>();

            string input01 =
@"void start(Car car){
    car.start();
}
";


            string output01 =
@"void start(List<Car> list){
    for(Car car in cars)
        car.start();
}";
            Tuple<string, string> tuple01 = Tuple.Create(input01, output01);
            Console.WriteLine(input01);
            Console.WriteLine(output01);
            tuples.Add(tuple01);

            string input02 =
@"void start(Book book){
    book.start();
}
";


            string output02 =
@"void start(List<Book> list){
    for(Book book in books)
        book.start();
}";
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
@"void start(Bus bus){
    bus.start();
}
";

            string output01 =
@"void start(List<Bus> list){
    for(Bus bus in list)
        bus.start();
}";
            Tuple<string, string> test = Tuple.Create(input01, output01);
            return test;
        }
    }
}


