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
        public override List<Tuple<string, string>> Train() {
            List<Tuple<string, string>> tuples = new List<Tuple<string, string>>();

                string input01 =
@"static void PrintData(String data2)
  {
    A a = new A();
    a.oldMethod();
  }
";


                string output01 =
@"static void PrintData(String data2)
  {
    B b = new B();
    b.newMethod(data2);
 }
"; ;
                Tuple<string, string> tuple01 = Tuple.Create(input01, output01);
                Console.WriteLine(input01);
                Console.WriteLine(output01);
                tuples.Add(tuple01);

                string input02 =
@"static void PrintData(String data)
  {
    A a = new A();
    a.oldMethod();
    int p = 0;
    Console.WriteLine(data);
  }
";


                string output02 =
@"static void PrintData(String data)
  {
    B b = new B();
    b.newMethod(data);
    int p = 0;
    Console.WriteLine(data);
  }
";
                Tuple<string, string> tuple02 = Tuple.Create(input02, output02);
                Console.WriteLine(input02);
                Console.WriteLine(output02);
                tuples.Add(tuple02);
            return tuples;
        }

        /// <summary>
        /// Test set
        /// </summary>
        /// <returns>Example test set</returns>
        public override Tuple<string, string> Test() {
            string input01 =
@"static void PrintData(String data)
  {
    A a = new A();
    a.newMethod();
    int p = 0;
    int j = 2;
    Console.WriteLine(data);
  }
";

            string output01 =
@"static void PrintData(String data)
  {
    B b = new B();
    b.newMethod(data);
    int p = 0;
    int j = 2;
    Console.WriteLine(data);
  }
";
            Tuple<string, string> test = Tuple.Create(input01, output01);
            return test;
        }
    }
}


