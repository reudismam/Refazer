using System;
using System.Collections.Generic;

namespace Spg.ExampleRefactoring.Data
{
    /// <summary>
    /// Refactoring 02
    /// </summary>
    public class Refact03: ExampleCommand
    {
        /// <summary>
        /// Return the train data set.
        /// </summary>
        /// <returns>List of examples</returns>
        public override List<Tuple<string, string>> Train() {
            List<Tuple<string, string>> tuples = new List<Tuple<string, string>>();

                string input01 =
@"public IActionBars getActionBars()
        {
            if (fContainer == null)
            {
                return Utilities.findActionBars(fComposite);
            }
            return fContainer.getActionBars();
        }
";

                string output01 =
@"public IActionBars getActionBars()
        {
            IActionBars actionBars = fContainer.getActionBars();
            if (actionBars == null && !fContainerProvided)
            {
                return Utilities.findActionBars(fComposite);
            }
            return actionBars;
        }
";
                Tuple<string, string> tuple01 = Tuple.Create(input01, output01);
                Console.WriteLine(input01);
                Console.WriteLine(output01);
                tuples.Add(tuple01);

           /* String input02 =
@"public IActionBars getActionBars()
        {
            if (fContainer == null)
            {
                return Utilities.findActionBars(fComposite);
            }
            return fContainer.getActionBars();
        }
";


            String output02 =
@"public object[] Method(object type)
        {
            IServiceLocator serviceLocator = fContainer.getServiceLocator();
            if (serviceLocator == null && !fContainerProvided) {
            {
                return Utilities.findActionBars(fComposite);
            }
            return serviceLocator;
        }
";*/

            return tuples;
        }

        /// <summary>
        /// Return the test data.
        /// </summary>
        /// <returns>Return a string to be tested.</returns>
        public override Tuple<string, string> Test() {
            string input01 =
@"public IActionBars getActionBars()
        {
            if (fContainer == null)
            {
                return Utilities.findActionBars(fComposite);
            }
            return fContainer.getActionBars();
        }
";

            string output01 =
@"public IActionBars getActionBars()
        {
            if (fContainer == null)
            {
                return Utilities.findActionBars(fComposite);
            }
            return fContainer.getActionBars();
        }
";
            Tuple<string, string> test = Tuple.Create(input01, output01);
            return test;
        }
    }
}


