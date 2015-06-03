using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ExampleRefactoring.Spg.ExampleRefactoring.Bean;
using ExampleRefactoring.Spg.ExampleRefactoring.Synthesis;
using ExampleRefactoring.Spg.ExampleRefactoring.Util;
using LocationCodeRefactoring.Spg.LocationRefactor.Transformation;
using NUnit.Framework;
using Spg.ExampleRefactoring.Comparator;
using Spg.ExampleRefactoring.Util;
using Spg.LocationCodeRefactoring.Controller;

namespace NUnitTests.Spg.NUnitTests.CompleteTestSolution
{
    /// <summary>
    /// Test for complete systematic editing
    /// </summary>
    [TestFixture]
    public class CompleteTestSolution
    {
        // /// <summary>
        // /// Simple API Test for locations
        // /// </summary>
        // [Test]
        // public void SimpleAPIChangeTest()
        // {
        //     bool passLocation = LocationTest.LocaleTest(FilePath.SIMPLE_API_CHANGE_INPUT, FilePath.SIMPLE_API_CHANGE_OUTPUT_SELECTION, FilePath.MAIN_CLASS_SIMPLE_API_CHANGE_PATH);

        //     bool passTransformation = CompleteTestBase(FilePath.MAIN_CLASS_SIMPLE_API_CHANGE_AFTER_EDITING, FilePath.SIMPLE_API_CHANGE_EDITION, @"\change_api\");

        //     Assert.IsTrue(passLocation && passTransformation);
        // }

        // /// <summary>
        // /// Parameter test on if
        // /// </summary>
        // [Test]
        // public void IntroduceParamOnIf()
        // {
        //     bool passLocation = LocationTest.LocaleTest(FilePath.INTRODUCE_PARAM_ON_IF_INPUT, FilePath.INTRODUCE_PARAM_ON_IF_OUTPUT_SELECTION, FilePath.MAIN_CLASS_INTRODUCE_PARAM_ON_IF_PATH);

        //     bool passTransformation = CompleteTestBase(FilePath.MAIN_CLASS_INTRODUCE_PARAM_ON_IF_AFTER_EDITING, FilePath.INTRODUCE_PARAM_ON_IF_EDITION, @"\parameter_change_on_if\");

        //     Assert.IsTrue(passLocation && passTransformation);
        // }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj0086821()
        {
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"Roslyn\0086821", @"Roslyn\roslyn12\src\Roslyn.sln", "CodeAnalysis");

            List<string> list = new List<string>();
            list.Add("CSharpExtensions.cs");
            bool passTransformation = CompleteTestBase(list, @"Roslyn\0086821");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj00552fc()
        {
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"Roslyn\00552fc", @"Roslyn\roslyn11\src\Roslyn.sln", "CodeAnalysis");

            List<string> list = new List<string>();
            list.Add("LanguageParser.cs");
            bool passTransformation = CompleteTestBase(list, @"Roslyn\00552fc");

            Assert.IsTrue(passLocation && passTransformation);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj673f18e()
        {
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"Roslyn\673f18e", @"Roslyn\roslyn7\src\Roslyn.sln", "CodeAnalysis");

            List<string> list = new List<string>();
            list.Add("DiagnosticFormatter.cs");
            bool passTransformation = CompleteTestBase(list, @"Roslyn\673f18e");

            Assert.IsTrue(passLocation && passTransformation);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj2_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8()
        {
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("2_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8", @"..\..\TestProjects\Projects\CodeAnalysisTest\Proj2_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8.sln", "Proj2_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8");

            List<string> list = new List<string>();
            list.Add("CommonCommandLineParserTests.cs");
            bool passTransformation = CompleteTestBase(list, @"2_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8");

            Assert.IsTrue(passLocation && passTransformation);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj3_673f18e()
        {
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"Roslyn\3_673f18e", @"Roslyn\roslyn7\src\Roslyn.sln", "CodeAnalysis");

            List<string> list = new List<string>();
            list.Add("CommonCommandLineParserTests.cs");
            bool passTransformation = CompleteTestBase(list, @"Roslyn\3_673f18e");

            Assert.IsTrue(passLocation && passTransformation);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj5_673f18e()
        {
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"Roslyn\5_673f18e", @"Roslyn\roslyn7\src\Roslyn.sln", "CSharpCompilerSyntaxTest");

            List<string> list = new List<string>();
            list.Add("ParsingErrorRecoveryTests.cs");
            bool passTransformation = CompleteTestBase(list, @"Roslyn\5_673f18e");

            Assert.IsTrue(passLocation && passTransformation);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj6_673f18e()
        {
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"Roslyn\6_673f18e", @"Roslyn\roslyn7\src\Roslyn.sln", "CSharpCompilerSymbolTest");

            List<string> list = new List<string>();
            list.Add("BaseClassTests.cs");
            bool passTransformation = CompleteTestBase(list, @"Roslyn\6_673f18e");

            Assert.IsTrue(passLocation && passTransformation);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj8_673f18e()
        {
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"Roslyn\8_673f18e", @"Roslyn\roslyn7\src\Roslyn.sln", "CSharpCompilerSymbolTest");

            List<string> list = new List<string>();
            list.Add("CSharpCompilationOptionsTests.cs");
            bool passTransformation = CompleteTestBase(list, @"Roslyn\8_673f18e");

            Assert.IsTrue(passLocation && passTransformation);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj9_673f18e()
        {
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"Roslyn\9_673f18e", @"Roslyn\roslyn7\src\Roslyn.sln", "CSharpCompilerEmitTest");

            List<string> list = new List<string>();
            list.Add("InternalsVisibleToAndStrongNameTests.cs");
            bool passTransformation = CompleteTestBase(list, @"Roslyn\9_673f18e");

            Assert.IsTrue(passLocation && passTransformation);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj4_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8()
        {
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("4_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8", @"..\..\TestProjects\Projects\CommandLine\Proj4_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8.sln", "Proj4_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8");

            List<string> list = new List<string>();
            list.Add("CommandLineTests.cs");
            bool passTransformation = CompleteTestBase(list, @"4_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8");

            Assert.IsTrue(passLocation && passTransformation);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj83e4349133d27a8f4dd5a85b69eb4ba00d41e6f8()
        {
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("83e4349133d27a8f4dd5a85b69eb4ba00d41e6f8", @"..\..\TestProjects\Projects\Portable10\Proj83e4349133d27a8f4dd5a85b69eb4ba00d41e6f8.sln", "Proj83e4349133d27a8f4dd5a85b69eb4ba00d41e6f8");

            List<string> list = new List<string>();
            list.Add("UserDefinedImplicitConversions.cs");
            bool passTransformation = CompleteTestBase(list, @"83e4349133d27a8f4dd5a85b69eb4ba00d41e6f8");

            Assert.IsTrue(passLocation && passTransformation);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proje817dab72dd5199cb5c7f661bc6b289f63ae706b()
        {
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("e817dab72dd5199cb5c7f661bc6b289f63ae706b", @"..\..\TestProjects\Projects\Portable9\Proje817dab72dd5199cb5c7f661bc6b289f63ae706b.sln", "Proje817dab72dd5199cb5c7f661bc6b289f63ae706b");

            List<string> list = new List<string>();
            list.Add("TokenBasedFormattingRule.cs");
            bool passTransformation = CompleteTestBase(list, @"e817dab72dd5199cb5c7f661bc6b289f63ae706b");

            Assert.IsTrue(passLocation && passTransformation);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Projcd68d03()
        {
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"Roslyn\cd68d03", @"Roslyn\roslyn7\src\Roslyn.sln", "Workspaces");

            List<string> list = new List<string>();
            list.Add("Project.cs");
            bool passTransformation = CompleteTestBase(list, @"Roslyn\cd68d03");

            Assert.IsTrue(passLocation && passTransformation);
        }


        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj2_cd68d03()
        {
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"Roslyn\2_cd68d03", @"Roslyn\roslyn7\src\Roslyn.sln", "Workspaces");

            List<string> list = new List<string>();
            list.Add("Project.cs");
            bool passTransformation = CompleteTestBase(list, @"Roslyn\2_cd68d03");

            Assert.IsTrue(passLocation && passTransformation);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj3_cd68d03()
        {
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"Roslyn\3_cd68d03", @"Roslyn\roslyn7\src\Roslyn.sln", "Workspaces");

            List<string> list = new List<string>();
            list.Add("Project.cs");
            bool passTransformation = CompleteTestBase(list, @"Roslyn\3_cd68d03");

            Assert.IsTrue(passLocation && passTransformation);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Projf66696e()
        {
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"Roslyn\f66696e", @"Roslyn\roslyn8\src\Roslyn.sln", "VBCSCompilerTests");

            List<string> list = new List<string>();
            list.Add("CompilerServerApiTest.cs");
            bool passTransformation = CompleteTestBase(list, @"Roslyn\f66696e");

            Assert.IsTrue(passLocation && passTransformation);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj4b402939708adf35a7a5e12ffc99dc14cc1f4766()
        {
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("2_4b402939708adf35a7a5e12ffc99dc14cc1f4766", @"..\..\TestProjects\Projects\CSharp2\Proj2_4b402939708adf35a7a5e12ffc99dc14cc1f4766.sln", "Proj2_4b402939708adf35a7a5e12ffc99dc14cc1f4766");

            List<string> list = new List<string>();
            list.Add("AssemblySymbol.cs"); list.Add("InternalsVisibleToAndStrongNameTests.cs");
            bool passTransformation = CompleteTestBase(list, @"2_4b402939708adf35a7a5e12ffc99dc14cc1f4766");

            Assert.IsTrue(passLocation && passTransformation);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj8ecd05880b478e4ca997a4789b976ef73b070546()
        {
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("8ecd05880b478e4ca997a4789b976ef73b070546", @"..\..\TestProjects\Projects\Portable7\Proj4b402939708adf35a7a5e12ffc99dc14cc1f4766.sln", "Proj4b402939708adf35a7a5e12ffc99dc14cc1f4766");

            List<string> list = new List<string>();
            list.Add("EmitExpression.cs");
            bool passTransformation = CompleteTestBase(list, @"8ecd05880b478e4ca997a4789b976ef73b070546");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj04d060498bc0c30403bb05872e396052d826d082()
        {
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("04d060498bc0c30403bb05872e396052d826d082", @"..\..\TestProjects\Projects\Diagnostics2\Proj04d060498bc0c30403bb05872e396052d826d082.sln", "Proj04d060498bc0c30403bb05872e396052d826d082");

            List<string> list = new List<string>();
            list.Add("ApplyDiagnosticAnalyzerAttributeFix.cs"); list.Add("CA1052CSharpCodeFixProvider.cs");
            bool passTransformation = CompleteTestBase(list, @"04d060498bc0c30403bb05872e396052d826d082");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj318b2b0e476a122ebc033b13d41449ef1c814c1d()
        {
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("318b2b0e476a122ebc033b13d41449ef1c814c1d", @"..\..\TestProjects\Projects\Core2\Proj318b2b0e476a122ebc033b13d41449ef1c814c1d.sln", "Proj318b2b0e476a122ebc033b13d41449ef1c814c1d");

            List<string> list = new List<string>();
            list.Add("DeclarePublicAPIFix.cs");
            bool passTransformation = CompleteTestBase(list, @"318b2b0e476a122ebc033b13d41449ef1c814c1d");

            Assert.IsTrue(passLocation && passTransformation);

        }

        /// <summary>
        /// Test case for parameter to constant value
        /// </summary>
        [Test]
        public void Projc96d9ce()
        {
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"Roslyn\c96d9ce", @"Roslyn\roslyn4\src\Roslyn.sln", "CSharpCodeAnalysis");

            List<string> list = new List<string>();
            list.Add("SyntaxFactory.cs");
            bool passTransformation = CompleteTestBase(list, @"Roslyn\c96d9ce");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj1113fd3()
        {
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"Roslyn\1113fd3", @"Roslyn\roslyn2\src\Roslyn.sln", "CodeAnalysis");

            List<string> list = new List<string>();
            list.Add("MetadataWriter.cs");
            bool passTransformation = CompleteTestBase(list, @"Roslyn\1113fd3");

            Assert.IsTrue(passLocation && passTransformation);
        }

        /// <summary>
        /// Change Exception test
        /// </summary>
        [Test]
        public void Projcc3d32746f60ed5a9f3775ef0ec44424b03d65cf()
        {
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("cc3d32746f60ed5a9f3775ef0ec44424b03d65cf", @"..\..\TestProjects\Projects\Portable2\Portable\Projcc3d32746f60ed5a9f3775ef0ec44424b03d65cf.sln", "Projcc3d32746f60ed5a9f3775ef0ec44424b03d65cf");

            List<string> list = new List<string>();
            list.Add("Contract.cs");
            bool passTransformation = CompleteTestBase(list, @"cc3d32746f60ed5a9f3775ef0ec44424b03d65cf");

            Assert.IsTrue(passLocation && passTransformation);
        }

        /// <summary>
        /// Change parameter on method test
        /// </summary>
        [Test]
        public void Proje7184bd18b5e2de6e1b71ba8f893c6d5e9a7bebd()
        {
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("e7184bd18b5e2de6e1b71ba8f893c6d5e9a7bebd", @"..\..\TestProjects\Projects\Portable3\Portable\Proje7184bd18b5e2de6e1b71ba8f893c6d5e9a7bebd.sln", "Proje7184bd18b5e2de6e1b71ba8f893c6d5e9a7bebd");

            List<string> list = new List<string>();
            list.Add("TaskExtensions.cs");
            bool passTransformation = CompleteTestBase(list, @"e7184bd18b5e2de6e1b71ba8f893c6d5e9a7bebd");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj2_c96d9ce1b2626b464cf2746ca53cb338d7d2ce38()
        {
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("2_c96d9ce1b2626b464cf2746ca53cb338d7d2ce38", @"..\..\TestProjects\Projects\Portable5\Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38.sln", "Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38");

            List<string> list = new List<string>();
            list.Add("SymbolDisplay.cs");
            bool passTransformation = CompleteTestBase(list, @"2_c96d9ce1b2626b464cf2746ca53cb338d7d2ce38");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj3_c96d9ce()
        {
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"Roslyn\3_c96d9ce", @"Roslyn\roslyn4\src\Roslyn.sln", "CSharpCodeAnalysis");

            List<string> list = new List<string>();
            list.Add("ObjectDisplay.cs");
            bool passTransformation = CompleteTestBase(list, @"Roslyn\3_c96d9ce");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj4_c96d9ce1b2626b464cf2746ca53cb338d7d2ce38()
        {
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("4_c96d9ce1b2626b464cf2746ca53cb338d7d2ce38", @"..\..\TestProjects\Projects\Portable5\Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38.sln", "Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38");

            List<string> list = new List<string>();
            list.Add("ObjectDisplay.cs");
            bool passTransformation = CompleteTestBase(list, @"4_c96d9ce1b2626b464cf2746ca53cb338d7d2ce38");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj5_c96d9ce1b2626b464cf2746ca53cb338d7d2ce38()
        {
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("5_c96d9ce1b2626b464cf2746ca53cb338d7d2ce38", @"..\..\TestProjects\Projects\Portable5\Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38.sln", "Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38");

            List<string> list = new List<string>();
            list.Add("ObjectDisplay.cs");
            bool passTransformation = CompleteTestBase(list, @"5_c96d9ce1b2626b464cf2746ca53cb338d7d2ce38");

            Assert.IsTrue(passLocation && passTransformation);
        }


        // [Test]
        // public void ReturnToGetTest()
        // {
        //     bool passLocation = LocationTest.LocaleTest(FilePath.RETURN_TO_GET_INPUT, FilePath.RETURN_TO_GET_OUTPUT_SELECTION, FilePath.MAIN_CLASS_RETURN_TO_GET_PATH);

        //     bool passTransformation = CompleteTestBase(FilePath.MAIN_CLASS_RETURN_TO_GET_AFTER_EDITING,  FilePath.RETURN_TO_GET_EDITION, @"\return_to_get\");

        //     Assert.IsTrue(passLocation && passTransformation);
        //}

        [Test]
        public void Proj49cdaceb2828acc1f50223826d478a00a80a59e2()
        {
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("49cdaceb2828acc1f50223826d478a00a80a59e2", @"..\..\TestProjects\Projects\CSharp\Proj49cdaceb2828acc1f50223826d478a00a80a59e2.sln", "Proj49cdaceb2828acc1f50223826d478a00a80a59e2");

            List<string> list = new List<string>();
            list.Add("MockCSharpCompiler.cs"); list.Add("MockCsi.cs");
            bool passTransformation = CompleteTestBase(list, @"49cdaceb2828acc1f50223826d478a00a80a59e2");

            Assert.IsTrue(passLocation && passTransformation);
        }
        [Test]
        public void Projcfd9b46()
        {
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"Roslyn\cfd9b46", @"Roslyn\roslyn6\src\Roslyn.sln", "CodeAnalysis");

            List<string> list = new List<string>();
            list.Add("MetadataWriter.cs"); 
            bool passTransformation = CompleteTestBase(list, @"Roslyn\cfd9b46");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj2_cfd9b46()
        {
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"Roslyn\2_cfd9b46", @"Roslyn\roslyn6\src\Roslyn.sln", "CodeAnalysis");

            List<string> list = new List<string>();
            list.Add("MetadataWriter.cs");
            bool passTransformation = CompleteTestBase(list, @"Roslyn\2_cfd9b46");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj7c885ca20209ca95cfec1ed5bfaf1d43db06be99()
        {
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("7c885ca20209ca95cfec1ed5bfaf1d43db06be99", @"..\..\TestProjects\Projects\Diagnostics\Proj7c885ca20209ca95cfec1ed5bfaf1d43db06be99.sln", "Proj7c885ca20209ca95cfec1ed5bfaf1d43db06be99");

            List<string> list = new List<string>();
            list.Add("InternalImplementationOnlyAnalyzer.cs"); list.Add("DiagnosticAnalyzerAttributeAnalyzer.cs");
            bool passTransformation = CompleteTestBase(list, @"7c885ca20209ca95cfec1ed5bfaf1d43db06be99");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj2_7c885ca20209ca95cfec1ed5bfaf1d43db06be99()
        {
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("2_7c885ca20209ca95cfec1ed5bfaf1d43db06be99", @"..\..\TestProjects\Projects\Diagnostics\Proj7c885ca20209ca95cfec1ed5bfaf1d43db06be99.sln", "Proj7c885ca20209ca95cfec1ed5bfaf1d43db06be99");

            List<string> list = new List<string>();
            list.Add("CA1001CSharpCodeFixProvider.cs"); list.Add("CA1008CSharpCodeFixProvider.cs");
            bool passTransformation = CompleteTestBase(list, @"2_7c885ca20209ca95cfec1ed5bfaf1d43db06be99");

            Assert.IsTrue(passLocation && passTransformation);
        }

        // [Test]
        // public void ASTManagerToParentTest()
        // {

        //     bool passLocation = LocationTest.LocaleTest(FilePath.ASTMANAGER_TO_PARENT_INPUT, FilePath.ASTMANAGER_TO_PARENT_OUTPUT_SELECTION, FilePath.MAIN_CLASS_ASTMANAGER_TO_PARENT_PATH);

        //     bool passTransformation = CompleteTestBase(FilePath.MAIN_CLASS_ASTMANAGER_TO_PARENT_AFTER_EDITING, FilePath.ASTMANAGER_TO_PARENT_EDITED, @"\astmanager_to_parent\");

        //     Assert.IsTrue(passLocation && passTransformation);
        // }

        [Test]
        public void Proje28c81243206f1bb26b861ca0162678ce11b538cTest()
        {
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("e28c81243206f1bb26b861ca0162678ce11b538c", @"..\..\TestProjects\Projects\Core\Proje28c81243206f1bb26b861ca0162678ce11b538c.sln", "Proje28c81243206f1bb26b861ca0162678ce11b538c");

            List<string> list = new List<string>();
            list.Add("CA1309CodeFixProviderBase.cs"); list.Add("CA2101CodeFixProviderBase.cs");
            bool passTransformation = CompleteTestBase(list, @"e28c81243206f1bb26b861ca0162678ce11b538c");

            Assert.IsTrue(passLocation && passTransformation);
        }

        //Entity Framework tests
        [Test]
        public void Projd83cdfa88557bdf399a2f52dc79aad9d69bce007Test()
        {
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("d83cdfa88557bdf399a2f52dc79aad9d69bce007", @"..\..\TestProjects\Projects\UnitTests\Projd83cdfa88557bdf399a2f52dc79aad9d69bce007.sln", "Projd83cdfa88557bdf399a2f52dc79aad9d69bce007");

            List<string> list = new List<string>();
            list.Add("CommitFailureHandlerTests.cs");
            bool passTransformation = CompleteTestBase(list, @"d83cdfa88557bdf399a2f52dc79aad9d69bce007");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj1571862()
        {
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"EntityFramewok\1571862", @"EntityFramework\entityframework4\EntityFramework.sln", "FunctionalTests");

            List<string> list = new List<string>();
            list.Add("DbSqlQueryTests.cs");
            bool passTransformation = CompleteTestBase(list, @"EntityFramewok\1571862");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj2_1571862()
        {
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"EntityFramewok\2_1571862", @"EntityFramework\entityframework4\EntityFramework.sln", "FunctionalTests");

            List<string> list = new List<string>();
            list.Add("SimpleScenariosForLocalDb.cs");
            bool passTransformation = CompleteTestBase(list, @"EntityFramewok\2_1571862");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj4_1571862()
        {
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"EntityFramewok\4_1571862", @"EntityFramework\entityframework4\EntityFramework.sln", "FunctionalTests");

            List<string> list = new List<string>();
            list.Add("CommitFailureTests.cs");
            bool passTransformation = CompleteTestBase(list, @"EntityFramewok\4_1571862");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj5_1571862()
        {
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"EntityFramewok\5_1571862", @"EntityFramework\entityframework4\EntityFramework.sln", "FunctionalTests");

            List<string> list = new List<string>();
            list.Add("CommitFailureTests.cs");
            bool passTransformation = CompleteTestBase(list, @"EntityFramewok\5_1571862");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Projce1e333()
        {
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"EntityFramewok\ce1e333", @"EntityFramework\entityframework5\EntityFramework.sln", "EntityFramework");

            List<string> list = new List<string>();
            list.Add("QueryableExtensions.cs");
            bool passTransformation = CompleteTestBase(list, @"EntityFramewok\ce1e333");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj8b9180b()
        {
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"EntityFramewok\8b9180b", @"EntityFramework\entityframework6\EntityFramework.sln", "FunctionalTests");

            List<string> list = new List<string>();
            list.Add("DatabaseExistsInInitializerTests.cs");
            bool passTransformation = CompleteTestBase(list, @"EntityFramewok\8b9180b");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj2_8b9180b()
        {
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"EntityFramewok\2_8b9180b", @"EntityFramework\entityframework6\EntityFramework.sln", "FunctionalTests");

            List<string> list = new List<string>();
            list.Add("DatabaseExistsInInitializerTests.cs");
            bool passTransformation = CompleteTestBase(list, @"EntityFramewok\2_8b9180b");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj326d525()
        {
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"EntityFramewok\326d525", @"EntityFramework\entityframework9\EntityFramework.sln", "FunctionalTests.Transitional");

            List<string> list = new List<string>();
            list.Add("TestSqlAzureExecutionStrategy.cs");
            bool passTransformation = CompleteTestBase(list, @"EntityFramewok\326d525");

            Assert.IsTrue(passLocation && passTransformation);
        }

        /// <summary>
        /// Complete test
        /// </summary>
        /// <param name="editeds">Files edited</param>
        /// <param name="commit">Commit where the change occurs</param>
        /// <returns>True if pass test</returns>
        public static bool CompleteTestBase(List<string> editeds, string commit)
        {
            long millBefore = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            EditorController controller = EditorController.GetInstance();

            string expHome = Environment.GetEnvironmentVariable("EXP_HOME", EnvironmentVariableTarget.User);
            controller.CurrentViewCodeAfter = FileUtil.ReadFile(expHome + @"commit\" + commit + @"\" + editeds.First());

            var dicionarySelection = JsonUtil<Dictionary<string, List<Selection>>>.Read(expHome + @"commit\" + commit + @"\edited_selections.json");

            var dicionarySelectionFullpath = new Dictionary<string, List<Selection>>();
            foreach (var entry in dicionarySelection)
            {
                dicionarySelectionFullpath.Add(Path.GetFullPath(entry.Key), entry.Value);
            }

            dicionarySelection = dicionarySelectionFullpath;


            //controller.RetrieveLocations(controller.CurrentViewCodeAfter);
            //var locationsToEdit = controller.Locations;

            //List<Selection> selections = new List<Selection>();
            //foreach (var location in locationsToEdit)
            //{
            //    Selection selection = new Selection(location.Region.Start - 1, location.Region.Length + 2,
            //        dicionarySelection.Keys.First(), null);
            //    selections.Add(selection);

            //}
            //Dictionary<string, List<Selection>> d = new Dictionary<string, List<Selection>>();
            //d.Add(dicionarySelection.Keys.First(), selections);
            //JsonUtil<Dictionary<string, List<Selection>>>.Write(d, "selections.json");

            //Tuple<string, string> beforeAfter = Tuple.Create(controller.CurrentViewCodeBefore, controller.CurrentViewCodeAfter);
            List<Tuple<string, string>> documents = new List<Tuple<string, string>>();
            foreach (var item in editeds)
            {
                string sourceCodeAfter = FileUtil.ReadFile(expHome + @"commit\" + commit + @"\" + item);
                string pattern = Regex.Escape(item.ToUpperInvariant());
                foreach (var entry in dicionarySelection)
                {
                    string sourceCode = FileUtil.ReadFile(entry.Key);
                    bool containsPattern = Regex.IsMatch(entry.Key, pattern);
                    if (containsPattern)
                    {
                        Tuple<string, string> tuple = Tuple.Create(sourceCode, sourceCodeAfter);
                        documents.Add(tuple);
                        controller.FilesOpened[entry.Key] = true;
                    }
                }
                
            }

            //documents.Add(beforeAfter);
            controller.DocumentsBeforeAndAfter = documents;
            controller.EditedLocations = dicionarySelection;
            
            controller.Refact();

            bool passTransformation = true;
            foreach (Transformation transformation in controller.SourceTransformations)
            { 
                string classPath = transformation.SourcePath;
                string className = classPath.Substring(classPath.LastIndexOf(@"\") + 1, classPath.Length - (classPath.LastIndexOf(@"\") + 1));
                //className = "files" + complement + className;
                //string source = FileUtil.ReadFile(classPath);
                //FileUtil.WriteToFile(@"..\..\TestProjects\commits\" + commit + @"\tool\" + className, transformation.transformation.Item2);

                Tuple<string, string> example = Tuple.Create(FileUtil.ReadFile(expHome + @"commit\" + commit + @"\tool\" + className), transformation.transformation.Item2);
                Tuple<ListNode, ListNode> lnode = ASTProgram.Example(example);

                NodeComparer comparator = new NodeComparer();
                bool isEqual = comparator.SequenceEqual(lnode.Item1, lnode.Item2);
                if (!isEqual)
                {
                    passTransformation = false;
                    break;
                }
            }
            controller.Undo();
            long millAfer = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            long totalTime = (millAfer - millBefore);
            FileUtil.WriteToFile(expHome + @"commit\" + commit + @"\edit.t", totalTime.ToString());
            return passTransformation;
        }
    }
}

