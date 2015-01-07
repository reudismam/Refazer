using System;
using System.Collections.Generic;

namespace Spg.ExampleRefactoring.Data
{
    /// <summary>
    /// Change API example
    /// </summary>
    public class ChangeAPI: ExampleCommand
    {
        /// <summary>
        /// Train set
        /// </summary>
        /// <returns>Example train set</returns>
        public override List<Tuple<String, String>> Train() {
            List<Tuple<String, String>> tuples = new List<Tuple<string, string>>();

                String input01 =
@"static void PrintData(String data2)
  {
    A a = new A();
    a.oldMethod();
  }
";


                String output01 =
@"static void PrintData(String data2)
  {
    B b = new B();
    b.newMethod(data2);
 }
"; ;
                Tuple<String, String> tuple01 = Tuple.Create(input01, output01);
                Console.WriteLine(input01);
                Console.WriteLine(output01);
                tuples.Add(tuple01);

                String input02 =
@"static void PrintData(String data)
  {
    A a = new A();
    a.oldMethod();
    int p = 0;
    Console.WriteLine(data);
  }
";


                String output02 =
@"static void PrintData(String data)
  {
    B b = new B();
    b.newMethod(data);
    int p = 0;
    Console.WriteLine(data);
  }
";
                Tuple<String, String> tuple02 = Tuple.Create(input02, output02);
                Console.WriteLine(input02);
                Console.WriteLine(output02);
                tuples.Add(tuple02);
            return tuples;
        }

        /// <summary>
        /// Test set
        /// </summary>
        /// <returns>Example test set</returns>
        public override Tuple<String, String> Test() {
            String input01 =
@"static void PrintData(String data)
  {
    A a = new A();
    a.newMethod();
    int p = 0;
    int j = 2;
    Console.WriteLine(data);
  }
";

            String output01 =
@"static void PrintData(String data)
  {
    B b = new B();
    b.newMethod(data);
    int p = 0;
    int j = 2;
    Console.WriteLine(data);
  }
";
            Tuple<String, String> test = Tuple.Create(input01, output01);
            return test;
        }
    }
}
