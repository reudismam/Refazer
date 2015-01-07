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
        public override List<Tuple<String, String>> Train() {
            List<Tuple<String, String>> tuples = new List<Tuple<string, string>>();

                String input01 =
@"public IActionBars getActionBars()
        {
            if (fContainer == null)
            {
                return Utilities.findActionBars(fComposite);
            }
            return fContainer.getActionBars();
        }
";

                String output01 =
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
                Tuple<String, String> tuple01 = Tuple.Create(input01, output01);
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
        public override Tuple<String, String> Test() {
            String input01 =
@"public IActionBars getActionBars()
        {
            if (fContainer == null)
            {
                return Utilities.findActionBars(fComposite);
            }
            return fContainer.getActionBars();
        }
";

            String output01 =
@"public IActionBars getActionBars()
        {
            if (fContainer == null)
            {
                return Utilities.findActionBars(fComposite);
            }
            return fContainer.getActionBars();
        }
";
            Tuple<String, String> test = Tuple.Create(input01, output01);
            return test;
        }
    }
}
