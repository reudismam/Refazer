using System;
using System.Collections.Generic;

namespace Spg.ExampleRefactoring.Data.Dig
{
    /// <summary>
    /// Covert element to collection test
    /// </summary>
    public class CreateAndInitializeNewField : ExampleCommand
    {
        /// <summary>
        /// Return the train data set.
        /// </summary>
        /// <returns>List of examples</returns>
        public override List<Tuple<String, String>> Train()
        {
            List<Tuple<String, String>> tuples = new List<Tuple<string, string>>();

            string input01 =
@"class Car {
    private List<Valve> valves;
    
    public Car() {
        valves = new ArrayList<>();
    }
    
    void Method(){
    }
}
";


            string output01 =
@"class Car {
    private List<Valve> valves;
    private List<Wheel> wheels;
    
    public Car() {
        valves = new ArrayList<>();
        wheels = new ArrayList<>();
    }
    
    void Method(){
    }
}
";
            Tuple<String, String> tuple01 = Tuple.Create(input01, output01);
            Console.WriteLine(input01);
            Console.WriteLine(output01);
            tuples.Add(tuple01);

            string input02 =
@"class Bus {
    private List<Valve> valves;

    public Bus() {
        valves = new ArrayList<>();
    }
    
    void Method2(){
        string z = ""4"";
    }
}
";


            string output02 =
@"class Bus {
    private List<Valve> valves;
    private List<Wheel> wheels;

    public Bus() {
        valves = new ArrayList<>();
        wheels = new ArrayList<>();
    }
    
    void Method2(){
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
            string input01 =
@"class Truck {
    private List<Valve> valves;

    public Truck() {
        valves = new ArrayList<>();
    }
    
    void Method(){
    }
}
";


            string output01 =
@"class Truck {
    private List<Valve> valves;
    private List<Wheel> wheels;
    
    public Truck() {
        valves = new ArrayList<>();
        wheels = new ArrayList<>();
    }
    
    void Method(){
    }
}
";
            Tuple<String, String> test = Tuple.Create(input01, output01);
            return test;
        }
    }
}

