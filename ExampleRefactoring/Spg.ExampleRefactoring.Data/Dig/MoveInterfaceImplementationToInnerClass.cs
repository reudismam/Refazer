using System;
using System.Collections.Generic;

namespace Spg.ExampleRefactoring.Data.Dig
{
    /// <summary>
    /// Covert element to collection test
    /// </summary>
    public class MoveInterfaceImplementationToInnerClass : ExampleCommand
    {
        /// <summary>
        /// Return the train data set.
        /// </summary>
        /// <returns>List of examples</returns>
        public override List<Tuple<string, string>> Train()
        {
            List<Tuple<string, string>> tuples = new List<Tuple<string, string>>();

            string input01 =
@"class FolderNode implements SelectionListener{
    public void selected() {
    }
}
";


            string output01 =
@"class FolderNode {
    class SelectionBehaviour
        implements SelectionListener{
    public void selected() {
    }
    }
}
";
            Tuple<string, string> tuple01 = Tuple.Create(input01, output01);
            Console.WriteLine(input01);
            Console.WriteLine(output01);
            tuples.Add(tuple01);

            string input02 =
@"class FolderToken implements SelectionListener{
    public void selected() {
        string z = ""4"";
    }
}
";


            string output02 =
@"class FolderToken {
    class SelectionBehaviour
        implements SelectionListener{
    public void selected() {
        string z = ""4"";
    }
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
@"class FolderSyntax implements SelectionListener{
    public void selected() {
        string z = ""4"";
        int p = 10;
    }
}
";
            
            string output01 =
@"class FolderSyntax {
    class SelectionBehaviour
        implements SelectionListener{
    public void selected() {
        string z = ""4"";
        int p = 10;
    }
    }
}
";
            Tuple<string, string> test = Tuple.Create(input01, output01);
            return test;
        }
    }
}


