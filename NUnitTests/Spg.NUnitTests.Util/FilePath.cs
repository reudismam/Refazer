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

        public static string MAIN_CLASS_SIMPLE_API_CHANGE_PATH = @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\ExampleProject\ExampleProject\Company.ExampleProject.API\ChangeAPI.cs";
        public static string MAIN_CLASS_SIMPLE_API_CHANGE_AFTER_EDITING = @"ChangeAPI.cs";

        public static string SIMPLE_API_CHANGE_INPUT = @"json\simple_api_change_input.json";
        public static string SIMPLE_API_CHANGE_OUTPUT_SELECTION = @"json\simple_api_change_output.json";


        public static string MAIN_CLASS_INTRODUCE_PARAM_ON_IF_PATH = @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\ExampleProject\ExampleProject\Company.ParameterChangeIf.API\ParameterChangeOnIf.cs";
        public static string MAIN_CLASS_INTRODUCE_PARAM_ON_IF_AFTER_EDITING = @"ParameterChangeOnIf.cs";

        public static string INTRODUCE_PARAM_ON_IF_INPUT = @"json\param_on_if_input.json";
        public static string INTRODUCE_PARAM_ON_IF_OUTPUT_SELECTION = @"json\param_on_if_output.json";

        public static string MAIN_CLASS_METHOD_CALL_TO_IDENTIFIER_PATH = @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\ExampleProject\ExampleProject\Company.ExampleProject.MethodCallToIdentifier\LanguageParser.cs";
        public static string MAIN_CLASS_METHOD_CALL_TO_IDENTIFIER_PATH_AFTER_EDITING = @"LanguageParser.cs";

        public static string METHOD_CALL_TO_IDENTIFIER_INPUT = @"json\method_call_to_identifier_input.json";
        public static string METHOD_CALL_TO_IDENTIFIER_OUTPUT_SELECTION = @"json\method_call_to_identifier_output.json";

        public static string MAIN_CLASS_PARAMETER_TO_CONSTANT_VALUE_PATH = @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\ExampleProject\ExampleProject\Company.ExampleProject.Boolean ParameterToConstantValue\ParameterToConstantValue.cs";
        public static string MAIN_CLASS_PARAMETER_TO_CONSTANT_VALUE_AFTER_EDITING = @"ParameterToConstantValue.cs";

        public static string PARAMETER_TO_CONSTANT_VALUE_INPUT = @"json\boolean_parameter_to_constant_value_input.json";
        public static string PARAMETER_TO_CONSTANT_VALUE_OUTPUT_SELECTION = @"json\boolean_parameter_to_constant_value_output.json";

        public static string MAIN_CLASS_PARAMETER_CHANGE_ON_METHOD_PATH = @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\ExampleProject\ExampleProject\Company.ExampleProject.ParameterChangeOnMethod\SymbolDisplay.cs";
        public static string MAIN_CLASS_PARAMETER_CHANGE_ON_METHOD_AFTER_EDITING = @"SymbolDisplay.cs";

        public static string PARAMETER_CHANGE_ON_METHOD_INPUT = @"json\parameter_change_on_method_input.json";
        public static string PARAMETER_CHANGE_ON_METHOD_OUTPUT_SELECTION = @"json\parameter_change_on_method_output.json";
    }
}
