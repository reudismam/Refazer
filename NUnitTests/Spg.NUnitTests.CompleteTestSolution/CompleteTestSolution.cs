using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Spg.ExampleRefactoring.Bean;
using Spg.ExampleRefactoring.Synthesis;
using Spg.ExampleRefactoring.Util;
using Spg.LocationRefactor.Transform;
using NUnit.Framework;
using Spg.ExampleRefactoring.Comparator;
using Spg.ExampleRefactoring.Util;
using Spg.LocationRefactor.Controller;

namespace NUnitTests.Spg.NUnitTests.CompleteTestSolution
{
    /// <summary>
    /// Test for complete systematic editing
    /// </summary>
    [TestFixture]
    public class CompleteTestSolution
    {
        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj0086821()
        {
            List<string> projects = new List<string>();
            projects.Add("CodeAnalysis");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"Roslyn\0086821", @"Roslyn\roslyn12\src\Roslyn.sln", projects);

            List<string> list = new List<string>();
            list.Add("CSharpExtensions.cs");
            bool passTransformation = CompleteTestBase(list, @"Roslyn\0086821");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj00552fc()
        {
            List<string> projects = new List<string>();
            projects.Add("CodeAnalysis");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"Roslyn\00552fc", @"Roslyn\roslyn11\src\Roslyn.sln", projects);

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
            List<string> projects = new List<string>();
            projects.Add("CodeAnalysis");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"Roslyn\673f18e", @"Roslyn\roslyn7\src\Roslyn.sln", projects);

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
            List<string> projects = new List<string>();
            projects.Add("Proj2_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8");
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("2_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8", @"..\..\TestProjects\Projects\CodeAnalysisTest\Proj2_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8.sln", projects);

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
            List<string> projects = new List<string>();
            projects.Add("CodeAnalysis");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"Roslyn\3_673f18e", @"Roslyn\roslyn7\src\Roslyn.sln", projects);

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
            List<string> projects = new List<string>();
            projects.Add("CSharpCompilerSyntaxTest");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"Roslyn\5_673f18e", @"Roslyn\roslyn7\src\Roslyn.sln", projects);

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
            List<string> projects = new List<string>();
            projects.Add("CSharpCompilerSymbolTest");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"Roslyn\6_673f18e", @"Roslyn\roslyn7\src\Roslyn.sln", projects);

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
            List<string> projects = new List<string>();
            projects.Add("CSharpCompilerSymbolTest");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"Roslyn\8_673f18e", @"Roslyn\roslyn7\src\Roslyn.sln", projects);

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
            List<string> projects = new List<string>();
            projects.Add("CSharpCompilerEmitTest");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"Roslyn\9_673f18e", @"Roslyn\roslyn7\src\Roslyn.sln", projects);

            List<string> list = new List<string>();
            list.Add("InternalsVisibleToAndStrongNameTests.cs");
            bool passTransformation = CompleteTestBase(list, @"Roslyn\9_673f18e");

            Assert.IsTrue(passLocation && passTransformation);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj2_8c14644()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpCodeAnalysis");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"Roslyn\2_8c14644", @"Roslyn\roslyn\src\Roslyn.sln", projects);

            List<string> list = new List<string>();
            list.Add("Binder_Expressions.cs");
            list.Add("WhileBinder.cs");
            bool passTransformation = CompleteTestBase(list, @"Roslyn\2_8c14644");

            Assert.IsTrue(passLocation && passTransformation);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj4_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8()
        {
            List<string> projects = new List<string>();
            projects.Add("Proj4_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8");
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("4_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8", @"..\..\TestProjects\Projects\CommandLine\Proj4_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8.sln", projects);

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
            List<string> projects = new List<string>();
            projects.Add("Proj83e4349133d27a8f4dd5a85b69eb4ba00d41e6f8");
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("83e4349133d27a8f4dd5a85b69eb4ba00d41e6f8", @"..\..\TestProjects\Projects\Portable10\Proj83e4349133d27a8f4dd5a85b69eb4ba00d41e6f8.sln", projects);

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
            List<string> projects = new List<string>();
            projects.Add("Proje817dab72dd5199cb5c7f661bc6b289f63ae706b");
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("e817dab72dd5199cb5c7f661bc6b289f63ae706b", @"..\..\TestProjects\Projects\Portable9\Proje817dab72dd5199cb5c7f661bc6b289f63ae706b.sln", projects);

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
            List<string> projects = new List<string>();
            projects.Add("Workspaces");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"Roslyn\cd68d03", @"Roslyn\roslyn7\src\Roslyn.sln", projects);

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
            List<string> projects = new List<string>();
            projects.Add("Workspaces");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"Roslyn\2_cd68d03", @"Roslyn\roslyn7\src\Roslyn.sln", projects);

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
            List<string> projects = new List<string>();
            projects.Add("Workspaces");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"Roslyn\3_cd68d03", @"Roslyn\roslyn7\src\Roslyn.sln", projects);

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
            List<string> projects = new List<string>();
            projects.Add("VBCSCompilerTests");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"Roslyn\f66696e", @"Roslyn\roslyn8\src\Roslyn.sln", projects);

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
            List<string> projects = new List<string>();
            projects.Add("Proj2_4b402939708adf35a7a5e12ffc99dc14cc1f4766");
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("2_4b402939708adf35a7a5e12ffc99dc14cc1f4766", @"..\..\TestProjects\Projects\CSharp2\Proj2_4b402939708adf35a7a5e12ffc99dc14cc1f4766.sln", projects);

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
            List<string> projects = new List<string>();
            projects.Add("Proj4b402939708adf35a7a5e12ffc99dc14cc1f4766");
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("8ecd05880b478e4ca997a4789b976ef73b070546", @"..\..\TestProjects\Projects\Portable7\Proj4b402939708adf35a7a5e12ffc99dc14cc1f4766.sln", projects);

            List<string> list = new List<string>();
            list.Add("EmitExpression.cs");
            bool passTransformation = CompleteTestBase(list, @"8ecd05880b478e4ca997a4789b976ef73b070546");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj04d0604()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpFxCopRulesDiagnosticAnalyzers");
            projects.Add("FxCopRulesDiagnosticAnalyzers");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"Roslyn\04d0604", @"Roslyn\roslyn18\Src\Roslyn.sln", projects);

            List<string> list = new List<string>();
            list.Add("CA1052CSharpCodeFixProvider.cs"); list.Add("CA1001CodeFixProviderBase.cs");
            bool passTransformation = CompleteTestBase(list, @"Roslyn\04d0604");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj318b2b0e476a122ebc033b13d41449ef1c814c1d()
        {
            List<string> projects = new List<string>();
            projects.Add("Proj318b2b0e476a122ebc033b13d41449ef1c814c1d");
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("318b2b0e476a122ebc033b13d41449ef1c814c1d", @"..\..\TestProjects\Projects\Core2\Proj318b2b0e476a122ebc033b13d41449ef1c814c1d.sln", projects);

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
            List<string> projects = new List<string>();
            projects.Add("CSharpCodeAnalysis");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"Roslyn\c96d9ce", @"Roslyn\roslyn4\src\Roslyn.sln", projects);

            List<string> list = new List<string>();
            list.Add("SyntaxFactory.cs");
            bool passTransformation = CompleteTestBase(list, @"Roslyn\c96d9ce");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj1113fd3()
        {
            List<string> projects = new List<string>();
            projects.Add("CodeAnalysis");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"Roslyn\1113fd3", @"Roslyn\roslyn2\src\Roslyn.sln", projects);

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
            List<string> projects = new List<string>();
            projects.Add("Projcc3d32746f60ed5a9f3775ef0ec44424b03d65cf");
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("cc3d32746f60ed5a9f3775ef0ec44424b03d65cf", @"..\..\TestProjects\Projects\Portable2\Portable\Projcc3d32746f60ed5a9f3775ef0ec44424b03d65cf.sln", projects);

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
            List<string> projects = new List<string>();
            projects.Add("Proje7184bd18b5e2de6e1b71ba8f893c6d5e9a7bebd");
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("e7184bd18b5e2de6e1b71ba8f893c6d5e9a7bebd", @"..\..\TestProjects\Projects\Portable3\Portable\Proje7184bd18b5e2de6e1b71ba8f893c6d5e9a7bebd.sln", projects);

            List<string> list = new List<string>();
            list.Add("TaskExtensions.cs");
            bool passTransformation = CompleteTestBase(list, @"e7184bd18b5e2de6e1b71ba8f893c6d5e9a7bebd");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj2_c96d9ce1b2626b464cf2746ca53cb338d7d2ce38()
        {
            List<string> projects = new List<string>();
            projects.Add("Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38");
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("2_c96d9ce1b2626b464cf2746ca53cb338d7d2ce38", @"..\..\TestProjects\Projects\Portable5\Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38.sln", projects);

            List<string> list = new List<string>();
            list.Add("SymbolDisplay.cs");
            bool passTransformation = CompleteTestBase(list, @"2_c96d9ce1b2626b464cf2746ca53cb338d7d2ce38");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj3_c96d9ce()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpCodeAnalysis");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"Roslyn\3_c96d9ce", @"Roslyn\roslyn4\src\Roslyn.sln", projects);

            List<string> list = new List<string>();
            list.Add("ObjectDisplay.cs");
            bool passTransformation = CompleteTestBase(list, @"Roslyn\3_c96d9ce");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj4_c96d9ce1b2626b464cf2746ca53cb338d7d2ce38()
        {
            List<string> projects = new List<string>();
            projects.Add("Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38");
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("4_c96d9ce1b2626b464cf2746ca53cb338d7d2ce38", @"..\..\TestProjects\Projects\Portable5\Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38.sln", projects);

            List<string> list = new List<string>();
            list.Add("ObjectDisplay.cs");
            bool passTransformation = CompleteTestBase(list, @"4_c96d9ce1b2626b464cf2746ca53cb338d7d2ce38");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj5_c96d9ce1b2626b464cf2746ca53cb338d7d2ce38()
        {
            List<string> projects = new List<string>();
            projects.Add("Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38");
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("5_c96d9ce1b2626b464cf2746ca53cb338d7d2ce38", @"..\..\TestProjects\Projects\Portable5\Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38.sln", projects);

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
            List<string> projects = new List<string>();
            projects.Add("Proj49cdaceb2828acc1f50223826d478a00a80a59e2");
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("49cdaceb2828acc1f50223826d478a00a80a59e2", @"..\..\TestProjects\Projects\CSharp\Proj49cdaceb2828acc1f50223826d478a00a80a59e2.sln", projects);

            List<string> list = new List<string>();
            list.Add("MockCSharpCompiler.cs"); list.Add("MockCsi.cs");
            bool passTransformation = CompleteTestBase(list, @"49cdaceb2828acc1f50223826d478a00a80a59e2");

            Assert.IsTrue(passLocation && passTransformation);
        }
        [Test]
        public void Projcfd9b46()
        {
            List<string> projects = new List<string>();
            projects.Add("CodeAnalysis");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"Roslyn\cfd9b46", @"Roslyn\roslyn6\src\Roslyn.sln", projects);

            List<string> list = new List<string>();
            list.Add("MetadataWriter.cs"); 
            bool passTransformation = CompleteTestBase(list, @"Roslyn\cfd9b46");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj2_cfd9b46()
        {
            List<string> projects = new List<string>();
            projects.Add("CodeAnalysis");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"Roslyn\2_cfd9b46", @"Roslyn\roslyn6\src\Roslyn.sln", projects);

            List<string> list = new List<string>();
            list.Add("MetadataWriter.cs");
            bool passTransformation = CompleteTestBase(list, @"Roslyn\2_cfd9b46");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj7c885ca20209ca95cfec1ed5bfaf1d43db06be99()
        {
            List<string> projects = new List<string>();
            projects.Add("Proj7c885ca20209ca95cfec1ed5bfaf1d43db06be99");
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("7c885ca20209ca95cfec1ed5bfaf1d43db06be99", @"..\..\TestProjects\Projects\Diagnostics\Proj7c885ca20209ca95cfec1ed5bfaf1d43db06be99.sln", projects);

            List<string> list = new List<string>();
            list.Add("InternalImplementationOnlyAnalyzer.cs"); list.Add("DiagnosticAnalyzerAttributeAnalyzer.cs");
            bool passTransformation = CompleteTestBase(list, @"7c885ca20209ca95cfec1ed5bfaf1d43db06be99");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj2_7c885ca20209ca95cfec1ed5bfaf1d43db06be99()
        {
            List<string> projects = new List<string>();
            projects.Add("Proj7c885ca20209ca95cfec1ed5bfaf1d43db06be99");
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("2_7c885ca20209ca95cfec1ed5bfaf1d43db06be99", @"..\..\TestProjects\Projects\Diagnostics\Proj7c885ca20209ca95cfec1ed5bfaf1d43db06be99.sln", projects);

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
            List<string> projects = new List<string>();
            projects.Add("Proje28c81243206f1bb26b861ca0162678ce11b538c");
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("e28c81243206f1bb26b861ca0162678ce11b538c", @"..\..\TestProjects\Projects\Core\Proje28c81243206f1bb26b861ca0162678ce11b538c.sln", projects);

            List<string> list = new List<string>();
            list.Add("CA1309CodeFixProviderBase.cs"); list.Add("CA2101CodeFixProviderBase.cs");
            bool passTransformation = CompleteTestBase(list, @"e28c81243206f1bb26b861ca0162678ce11b538c");

            Assert.IsTrue(passLocation && passTransformation);
        }

        //Entity Framework tests
        [Test]
        public void Projd83cdfa88557bdf399a2f52dc79aad9d69bce007Test()
        {
            List<string> projects = new List<string>();
            projects.Add("Projd83cdfa88557bdf399a2f52dc79aad9d69bce007");
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("d83cdfa88557bdf399a2f52dc79aad9d69bce007", @"..\..\TestProjects\Projects\UnitTests\Projd83cdfa88557bdf399a2f52dc79aad9d69bce007.sln", projects);

            List<string> list = new List<string>();
            list.Add("CommitFailureHandlerTests.cs");
            bool passTransformation = CompleteTestBase(list, @"d83cdfa88557bdf399a2f52dc79aad9d69bce007");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj1571862()
        {
            List<string> projects = new List<string>();
            projects.Add("FunctionalTests");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"EntityFramewok\1571862", @"EntityFramework\entityframework4\EntityFramework.sln", projects);

            List<string> list = new List<string>();
            list.Add("DbSqlQueryTests.cs");
            bool passTransformation = CompleteTestBase(list, @"EntityFramewok\1571862");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj2_1571862()
        {
            List<string> projects = new List<string>();
            projects.Add("FunctionalTests");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"EntityFramewok\2_1571862", @"EntityFramework\entityframework4\EntityFramework.sln", projects);

            List<string> list = new List<string>();
            list.Add("SimpleScenariosForLocalDb.cs");
            bool passTransformation = CompleteTestBase(list, @"EntityFramewok\2_1571862");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj4_1571862()
        {
            List<string> projects = new List<string>();
            projects.Add("FunctionalTests");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"EntityFramewok\4_1571862", @"EntityFramework\entityframework4\EntityFramework.sln", projects);

            List<string> list = new List<string>();
            list.Add("CommitFailureTests.cs");
            bool passTransformation = CompleteTestBase(list, @"EntityFramewok\4_1571862");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj5_1571862()
        {
            List<string> projects = new List<string>();
            projects.Add("FunctionalTests");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"EntityFramewok\5_1571862", @"EntityFramework\entityframework4\EntityFramework.sln", projects);

            List<string> list = new List<string>();
            list.Add("CommitFailureTests.cs");
            bool passTransformation = CompleteTestBase(list, @"EntityFramewok\5_1571862");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj6_1571862()
        {
            List<string> projects = new List<string>();
            projects.Add("FunctionalTests.Transitional");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"EntityFramewok\6_1571862", @"EntityFramework\entityframework4\EntityFramework.sln", projects);

            List<string> list = new List<string>();
            list.Add("CommandTreeInterceptionTests.cs");
            list.Add("FunctionalTestBase.cs");
            bool passTransformation = CompleteTestBase(list, @"EntityFramewok\6_1571862");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Projce1e333()
        {
            List<string> projects = new List<string>();
            projects.Add("EntityFramework");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"EntityFramewok\ce1e333", @"EntityFramework\entityframework5\EntityFramework.sln", projects);

            List<string> list = new List<string>();
            list.Add("QueryableExtensions.cs");
            bool passTransformation = CompleteTestBase(list, @"EntityFramewok\ce1e333");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj8b9180b()
        {
            List<string> projects = new List<string>();
            projects.Add("FunctionalTests");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"EntityFramewok\8b9180b", @"EntityFramework\entityframework6\EntityFramework.sln", projects);

            List<string> list = new List<string>();
            list.Add("DatabaseExistsInInitializerTests.cs");
            bool passTransformation = CompleteTestBase(list, @"EntityFramewok\8b9180b");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj2_8b9180b()
        {
            List<string> projects = new List<string>();
            projects.Add("FunctionalTests");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"EntityFramewok\2_8b9180b", @"EntityFramework\entityframework6\EntityFramework.sln", projects);

            List<string> list = new List<string>();
            list.Add("DatabaseExistsInInitializerTests.cs");
            bool passTransformation = CompleteTestBase(list, @"EntityFramewok\2_8b9180b");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj829dec5()
        {
            List<string> projects = new List<string>();
            projects.Add("EntityFramework");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"EntityFramewok\829dec5", @"EntityFramework\entityframework7\EntityFramework.sln", projects);

            List<string> list = new List<string>();
            list.Add("StorageMappingFragmentExtensions.cs");
            list.Add("MetadataMappingHasherVisitorTests.cs");
            bool passTransformation = CompleteTestBase(list, @"EntityFramewok\829dec5");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj326d525()
        {
            List<string> projects = new List<string>();
            projects.Add("FunctionalTests.Transitional");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"EntityFramewok\326d525", @"EntityFramework\entityframework9\EntityFramework.sln", projects);

            List<string> list = new List<string>();
            list.Add("TestSqlAzureExecutionStrategy.cs");
            bool passTransformation = CompleteTestBase(list, @"EntityFramewok\326d525");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj2_a883600()
        {
            List<string> projects = new List<string>();
            projects.Add("CommandLine");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"NuGet\2_a883600", @"NuGet\nuget4\NuGet.sln", projects);

            List<string> list = new List<string>();
            list.Add("ProjectFactory.cs");
            list.Add("InstallWalker.cs");
            bool passTransformation = CompleteTestBase(list, @"NuGet\2_a883600");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj3_a883600()
        {
            List<string> projects = new List<string>();
            projects.Add("Core");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"NuGet\3_a883600", @"NuGet\nuget4\NuGet.sln", projects);

            List<string> list = new List<string>();
            list.Add("PackageRepositoryExtensions.cs");
            bool passTransformation = CompleteTestBase(list, @"NuGet\3_a883600");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj5_a883600()
        {
            List<string> projects = new List<string>();
            projects.Add("Core.Test");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"NuGet\5_a883600", @"NuGet\nuget4\NuGet.sln", projects);

            List<string> list = new List<string>();
            list.Add("PackageWalkerTest.cs");
            bool passTransformation = CompleteTestBase(list, @"NuGet\5_a883600");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj8da9f0e()
        {
            List<string> projects = new List<string>();
            projects.Add("Core.Test");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"NuGet\8da9f0e", @"NuGet\nuget3\NuGet.sln", projects);

            List<string> list = new List<string>();
            list.Add("PackageSourceProviderTest.cs");
            bool passTransformation = CompleteTestBase(list, @"NuGet\8da9f0e");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj2_8da9f0e()
        {
            List<string> projects = new List<string>();
            projects.Add("Core.Test");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"NuGet\2_8da9f0e", @"NuGet\nuget3\NuGet.sln", projects);

            List<string> list = new List<string>();
            list.Add("PackageSourceProviderTest.cs");
            list.Add("VsPackageSourceProviderTest.cs");
            bool passTransformation = CompleteTestBase(list, @"NuGet\2_8da9f0e");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj3_8da9f0e()
        {
            List<string> projects = new List<string>();
            projects.Add("Test.Utility");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"NuGet\3_8da9f0e", @"NuGet\nuget3\NuGet.sln", projects);

            List<string> list = new List<string>();
            list.Add("MockFileSystem.cs");
            bool passTransformation = CompleteTestBase(list, @"NuGet\3_8da9f0e");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proja569c55()
        {
            List<string> projects = new List<string>();
            projects.Add("NuGet.Client");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"NuGet\a569c55", @"NuGet\nuget5\NuGet.sln", projects);

            List<string> list = new List<string>();
            list.Add("AutoDetectSourceRepository.cs");
            bool passTransformation = CompleteTestBase(list, @"NuGet\a569c55");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj2_a569c55()
        {
            List<string> projects = new List<string>();
            projects.Add("NuGet.Client");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"NuGet\2_a569c55", @"NuGet\nuget5\NuGet.sln", projects);

            List<string> list = new List<string>();
            list.Add("V3SourceRepository.cs");
            bool passTransformation = CompleteTestBase(list, @"NuGet\2_a569c55");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj4_8da9f0e()
        {
            List<string> projects = new List<string>();
            projects.Add("VisualStudio.Test");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"NuGet\4_8da9f0e", @"NuGet\nuget3\NuGet.sln", projects);

            List<string> list = new List<string>();
            list.Add("VsPackageSourceProviderTest.cs");
            bool passTransformation = CompleteTestBase(list, @"NuGet\4_8da9f0e");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj5_8da9f0e()
        {
            List<string> projects = new List<string>();
            projects.Add("FunctionalTests.Transitional");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"NuGet\5_8da9f0e",
                @"NuGet\nuget3\NuGet.sln", projects);

            List<string> list = new List<string>();
            list.Add("LocalPackageRepositoryTest.cs");
            bool passTransformation = CompleteTestBase(list, @"NuGet\5_8da9f0e");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Pro2dea84e()
        {
            List<string> projects = new List<string>();
            projects.Add("NuGet.Client.VisualStudio.PowerShell");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"NuGet\2dea84e", @"NuGet\nuget2\NuGet.sln", projects);

            List<string> list = new List<string>();
            list.Add("NuGetPowerShellBaseCommand.cs");
            bool passTransformation = CompleteTestBase(list, @"NuGet\2dea84e");

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

            List<Tuple<string, string>> documents = new List<Tuple<string, string>>();
            foreach (var item in editeds)
            {
                string sourceCodeAfter = FileUtil.ReadFile(expHome + @"commit\" + commit + @"\" + item);
                string pattern = item.ToUpperInvariant();
                foreach (KeyValuePair<string, List<Selection>> entry in dicionarySelection)
                {
                    string classPath = entry.Key;
                    string className = classPath.Substring(classPath.LastIndexOf(@"\") + 1, classPath.Length - (classPath.LastIndexOf(@"\") + 1));

                    Console.WriteLine(className);
                    string sourceCode = FileUtil.ReadFile(entry.Key);
                    bool containsPattern = className.ToUpperInvariant().Equals(pattern);
                    if (containsPattern)
                    {
                        Tuple<string, string> tuple = Tuple.Create(sourceCode, sourceCodeAfter);
                        documents.Add(tuple);
                        controller.FilesOpened[entry.Key] = true;
                    }
                }
                
            }

            controller.DocumentsBeforeAndAfter = documents;
            controller.EditedLocations = dicionarySelection;
            
            controller.Refact();

            bool passTransformation = true;
            foreach (Transformation transformation in controller.SourceTransformations)
            { 
                string classPath = transformation.SourcePath;
                string className = classPath.Substring(classPath.LastIndexOf(@"\") + 1, classPath.Length - (classPath.LastIndexOf(@"\") + 1));
                string classNamePath = expHome + @"commit\" + commit + @"\tool\" + className;
//                FileUtil.WriteToFile(classNamePath, transformation.transformation.Item2);

                Tuple<string, string> example = Tuple.Create(FileUtil.ReadFile(classNamePath), transformation.transformation.Item2);
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

            string transformations = FileUtil.ReadFile("transformed_locations.json");
            FileUtil.WriteToFile(expHome + @"commit\" + commit + @"\" + "transformed_locations.json", transformations);
            FileUtil.DeleteFile("transformed_locations.json");
            return passTransformation;
        }
    }
}






