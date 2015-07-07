using System;
using System.Collections.Generic;

namespace Spg.ExampleRefactoring.Data.Dig
{
    /// <summary>
    /// Covert element to collection test
    /// </summary>
    public class AddLoopCollector : ExampleCommand
    {
        /// <summary>
        /// Return the train data set.
        /// </summary>
        /// <returns>List of examples</returns>
        public override List<Tuple<string, string>> Train()
        {
            List<Tuple<string, string>> tuples = new List<Tuple<string, string>>();

            string input01 =
@"void Start(){
    foreach(Task t in tasks){
        t.Execute();
    }
}
";


            string output01 =
@"void Start(){
    Set<TaskResult> results = new HashSet<>();
    foreach(Task t in tasks){
        t.Execute();
        results.Add(t.getResult());
    }
}
";
            Tuple<string, string> tuple01 = Tuple.Create(input01, output01);
            Console.WriteLine(input01);
            Console.WriteLine(output01);
            tuples.Add(tuple01);

            string input02 =
@"void Start2(){
    foreach(Command c in commands){
        c.Execute();
    }
}
";


            string output02 =
@"void Start2(){
    Set<TaskResult> results = new HashSet<>();
    foreach(Command c in commands){
        c.Execute();
        results.Add(c.getResult());
    }
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
@"void Start(){
    foreach(TaskResult p in tasks){
        p.Execute();
    }
}
";
            
            string output01 =
@"void Start(){
    Set<TaskResult> results = new HashSet<>();
    foreach(TaskResult p in tasks){
        p.Execute();
        results.Add(p.getResult());
    }
}
";
            Tuple<string, string> test = Tuple.Create(input01, output01);
            return test;
        }
    }
}


