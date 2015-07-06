using System;
using Spg.ExampleRefactoring.Data;
using Spg.ExampleRefactoring.Data.Dig;

namespace Spg.ExampleRefactoring.Data
{
    /// <summary>
    /// Example manager
    /// </summary>
    public static class ExampleManager
    {

        /// <summary>
        /// Command based on option
        /// </summary>
        /// <param name="option">Option</param>
        /// <returns>Command</returns>
        public static ExampleCommand GetCommand(string option) {
            ExampleCommand command = null;

            switch (option)
            {
                case "1":
                    command = new DeletePrint();
                    break;
                case "2":
                    command = new AddParameter();
                    break;
                case "3":
                    command = new ChangeAPISimple();
                    break;
                case "4":
                    command = new ChangeAPI();
                    break;
                case "5":
                    command = new ExtractCode();
                    break;
                case "6":
                    command = new AddAnnotation();
                    break;
                case "7":
                    command = new ChangeConstantToValue();
                    break;
                case "8":
                    command = new Refact03();
                    break;
                case "9":
                    command = new ParameterChangeOnIfs();
                    break;
                case "10":
                    command = new MethodCallToIdentifier();
                    break;
                case "11":
                    command = new ParameterChangeOnMethod();
                    break;
                case "12":
                    command = new ChangeStringValueToConstant();
                    break;
                case "13":
                    command = new IntroduceIf();
                    break;
                case "14":
                    command = new ConvertElementToCollection();
                    break;
                case "15":
                    command = new AddLoopCollector();
                    break;
                case "16":
                    command = new WrapLoopWithTimer();
                    break;
                case "17":
                    command = new CopyFieldInitializer();
                    break;
                case "18":
                    command = new CreateAndInitializeNewField();
                    break;
                case "19":
                    command = new MoveInterfaceImplementationToInnerClass();
                    break;
                case "20":
                    command = new ChangeAndPropagateFieldType();
                    break;
                case "21":
                    command = new ChangeAndPropagateFieldTypeParameter();
                    break;
                case "22":
                    command = new ChangeClassVisibility();
                    break;
                case "23":
                    command = new AddLangParam();
                    break;
            }
            return command;
        }

    }
}


