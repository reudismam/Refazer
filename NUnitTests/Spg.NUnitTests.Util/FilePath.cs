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
        public static string SIMPLE_API_CHANGE_EDITION = @"json\simple_api_change_edition.json";


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

        public static string MAIN_CLASS_METHOD_TO_PROPERTY_IF_PATH = @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\ExampleProject\ExampleProject\Company.MethodToPropertyOnIf\MethodToPropertyOnIf.cs";
        public static string MAIN_CLASS_METHOD_TO_PROPERTY_IF_AFTER_EDITING = @"MethodToPropertyOnIf.cs";

        public static string METHOD_TO_PROPERTY_IF_INPUT = @"json\method_to_property_if_input.json";
        public static string METHOD_TO_PROPERTY_IF_OUTPUT_SELECTION = @"json\method_to_property_if_output.json";
        public static string METHOD_TO_PROPERTY_IF_EDITION = @"json\method_to_property_if_edition.json";

        public static string MAIN_CLASS_CHANGE_EXCEPTION_PATH = @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\ExampleProject\ExampleProject\Company.ChangeException\ChangeException.cs";
        public static string MAIN_CLASS_CHANGE_EXCEPTION_AFTER_EDITING = @"ChangeException.cs";

        public static string CHANGE_EXCEPTION_INPUT = @"json\change_exception_input.json";
        public static string CHANGE_EXCEPTION_OUTPUT_SELECTION = @"json\change_exception_output.json";
        public static string CHANGE_EXCEPTION_EDITION = @"json\change_exception_edition.json";


        public static string MAIN_CLASS_CHANGE_PARAM_ON_METHOD_PATH = @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\ExampleProject\ExampleProject\Company.ChangeParamOnMethod\ChangeParamOnMethod.cs";
        public static string MAIN_CLASS_CHANGE_PARAM_ON_METHOD_EDITING = @"ChangeParamOnMethod.cs";

        public static string CHANGE_PARAM_ON_METHOD_INPUT = @"json\change_param_on_method_input.json";
        public static string CHANGE_PARAM_ON_METHOD_OUTPUT_SELECTION = @"json\change_param_on_method_output.json";

        public static string MAIN_CLASS_RETURN_TO_GET_PATH = @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\ExampleProject\ExampleProject\Company.ExampleProject.ReturnToGet\ReturnToGet.cs";
        public static string MAIN_CLASS_RETURN_TO_GET_AFTER_EDITING = @"ReturnToGet.cs";

        public static string RETURN_TO_GET_INPUT = @"json\return_to_get_input.json";
        public static string RETURN_TO_GET_OUTPUT_SELECTION = @"json\return_to_get_output.json";

        public static string MAIN_CLASS_ADD_IO_PARAM_PATH = @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\ExampleProject\ExampleProject\Company.ExampleProject.AddIOParam\AddIOParam.cs";
        public static string MAIN_CLASS_ADD_IO_PARAM_AFTER_EDITING = @"AddIOParam.cs";

        public static string ADD_IO_PARAM_INPUT = @"json\add_io_param_input.json";
        public static string ADD_IO_PARAM_OUTPUT_SELECTION = @"json\add_io_param_output.json";

        public static string MAIN_CLASS_ADD_CONTEXT_PARAM_PATH = @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\ExampleProject\ExampleProject\Company.AddContextParam\AddContextParam.cs";
        public static string MAIN_CLASS_ADD_CONTEXT_PARAM_AFTER_EDITING = @"AddContextParam.cs";

        public static string ADD_CONTEXR_PARAM_INPUT = @"json\add_context_param_input.json";
        public static string ADD_CONTEXR_PARAM_OUTPUT_SELECTION = @"json\add_context_param_output.json";
        public static string ADD_CONTEXR_PARAM_EDITION = @"json\add_context_param_edition.json";

        public static string MAIN_CLASS_CHANGE_ANNOTATION_ON_CLASS_PATH = @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\ExampleProject\ExampleProject\Company.ChangeAnnotationOnClass\ChangeAnnotationOnClass.cs";
        public static string MAIN_CLASS_CHANGE_ANNOTATION_ON_CLASS_AFTER_EDITING = @"ChangeAnnotationOnClass.cs";

        public static string CHANGE_ANNOTATION_ON_CLASS_INPUT = @"json\change_annotation_on_class_input.json";
        public static string CHANGE_ANNOTATION_ON_CLASS_OUTPUT_SELECTION = @"json\change_annotation_on_class_output.json";
        public static string CHANGE_ANNOTATION_ON_CLASS_EDITED = @"json\change_annotation_on_class_edition.json";

        public static string MAIN_CLASS_ASTMANAGER_TO_PARENT_PATH = @"C:\Users\SPG\Documents\Visual Studio 2013\Projects\ExampleProject\ExampleProject\IntelliMeta.ASTManagerChange\ASTManagerChange.cs";
        public static string MAIN_CLASS_ASTMANAGER_TO_PARENT_AFTER_EDITING = @"ASTManagerChange.cs";

        public static string ASTMANAGER_TO_PARENT_INPUT = @"json\astmanager_to_parent_input.json";
        public static string ASTMANAGER_TO_PARENT_OUTPUT_SELECTION = @"json\astmanager_to_parent_output.json";
        public static string ASTMANAGER_TO_PARENT_EDITED = @"json\astmanager_to_parent_edition.json";
    }
}
