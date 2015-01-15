using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spg.NUnitTests.Util
{
    /// <summary>
    /// Constants to be used in tests
    /// </summary>
    public abstract class FilePath
    {
        /// <summary>
        /// Solution path
        /// </summary>
        public static string SOLUTION_PATH = @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\ExampleProject\ExampleProject.sln";
        public static string MAIN_CLASS_PATH = @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\ExampleProject\ExampleProject\ChangeAPI.cs";
        public static string MAIN_CLASS_AFTER_EDITING = "ChangeAPI.cs";

        public static string SIMPLE_API_CHANGE_INPUT = "simple_api_change_input.json";
        public static string SIMPLE_API_CHANGE_OUTPUT_SELECTION = "simple_api_change_output.json";
    }
}
