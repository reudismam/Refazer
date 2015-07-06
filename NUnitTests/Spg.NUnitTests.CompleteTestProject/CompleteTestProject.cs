using System;
using System.Collections.Generic;
using System.Linq;
using Spg.ExampleRefactoring.Bean;
using Spg.ExampleRefactoring.RegularExpression;
using Spg.ExampleRefactoring.Synthesis;
using Spg.ExampleRefactoring.Util;
using Spg.LocationRefactor.Controller;
using Spg.LocationRefactor.Transformation;
using NUnit.Framework;
using Spg.ExampleRefactoring.Comparator;
using Spg.ExampleRefactoring.Util;
using Spg.NUnitTests.Util;
using NUnitTests.Spg.NUnitTests.LocationTestProject;
using System.IO;

namespace NUnitTests.Spg.NUnitTests.CompleteTestProject
{
    /// <summary>
    /// Test for complete systematic editing
    /// </summary>
    [TestFixture]
    public class CompleteTestProject
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

        [Test]
        public void Proj00552fc2287f820ae9d42fd259aa6c07c2c5a805()
        {
            List<string> projects = new List<string>();
            projects.Add("Proj00552fc2287f820ae9d42fd259aa6c07c2c5a805");
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("00552fc2287f820ae9d42fd259aa6c07c2c5a805", @"..\..\TestProjects\Projects\Portable6\Proj00552fc2287f820ae9d42fd259aa6c07c2c5a805.sln", projects);

            List<string> list = new List<string>();
            list.Add("LanguageParser.cs");
            bool passTransformation = CompleteTestBase(list, @"00552fc2287f820ae9d42fd259aa6c07c2c5a805");

            Assert.IsTrue(passLocation && passTransformation);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj673f18e1f9bbbae8a8bd8333f367c86d935e8eb8()
        {
            List<string> projects = new List<string>();
            projects.Add("Proj673f18e1f9bbbae8a8bd8333f367c86d935e8eb8");
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("673f18e1f9bbbae8a8bd8333f367c86d935e8eb8", @"..\..\TestProjects\Projects\Portable11\Proj673f18e1f9bbbae8a8bd8333f367c86d935e8eb8.sln", projects);

            List<string> list = new List<string>();
            list.Add("DiagnosticFormatter.cs");
            bool passTransformation = CompleteTestBase(list, @"673f18e1f9bbbae8a8bd8333f367c86d935e8eb8");

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
        public void Proj3_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8()
        {
            List<string> projects = new List<string>();
            projects.Add("Proj2_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8");
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("3_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8", @"..\..\TestProjects\Projects\CodeAnalysisTest\Proj2_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8.sln", projects);

            List<string> list = new List<string>();
            list.Add("CommonCommandLineParserTests.cs");
            bool passTransformation = CompleteTestBase(list, @"3_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8");

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
        public void Proj2_cd68d0323eb97f18c10281847c831f8e361506b9()
        {
            List<string> projects = new List<string>();
            projects.Add("Projcd68d0323eb97f18c10281847c831f8e361506b9");
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("2_cd68d0323eb97f18c10281847c831f8e361506b9", @"..\..\TestProjects\Projects\Portable8\Projcd68d0323eb97f18c10281847c831f8e361506b9.sln", projects);

            List<string> list = new List<string>();
            list.Add("Project.cs");
            bool passTransformation = CompleteTestBase(list, @"2_cd68d0323eb97f18c10281847c831f8e361506b9");

            Assert.IsTrue(passLocation && passTransformation);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj3_cd68d0323eb97f18c10281847c831f8e361506b9()
        {
            List<string> projects = new List<string>();
            projects.Add("Projcd68d0323eb97f18c10281847c831f8e361506b9");
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("3_cd68d0323eb97f18c10281847c831f8e361506b9", @"..\..\TestProjects\Projects\Portable8\Projcd68d0323eb97f18c10281847c831f8e361506b9.sln", projects);

            List<string> list = new List<string>();
            list.Add("Project.cs");
            bool passTransformation = CompleteTestBase(list, @"3_cd68d0323eb97f18c10281847c831f8e361506b9");

            Assert.IsTrue(passLocation && passTransformation);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Projf66696e70c90ce7fa1476d53cc84cc18e438d19b()
        {
            List<string> projects = new List<string>();
            projects.Add("Proje4d141a3c5f51ce4021d105e2b330564e02069fc");
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("f66696e70c90ce7fa1476d53cc84cc18e438d19b", @"..\..\TestProjects\Projects\VBCSCompilerTests\Proje4d141a3c5f51ce4021d105e2b330564e02069fc.sln", projects);

            List<string> list = new List<string>();
            list.Add("CompilerServerApiTest.cs");
            bool passTransformation = CompleteTestBase(list, @"f66696e70c90ce7fa1476d53cc84cc18e438d19b");

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
        public void Proj04d060498bc0c30403bb05872e396052d826d082()
        {
            List<string> projects = new List<string>();
            projects.Add("Proj04d060498bc0c30403bb05872e396052d826d082");
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("04d060498bc0c30403bb05872e396052d826d082", @"..\..\TestProjects\Projects\Diagnostics2\Proj04d060498bc0c30403bb05872e396052d826d082.sln", projects);

            List<string> list = new List<string>();
            list.Add("ApplyDiagnosticAnalyzerAttributeFix.cs"); list.Add("CA1052CSharpCodeFixProvider.cs");
            bool passTransformation = CompleteTestBase(list, @"04d060498bc0c30403bb05872e396052d826d082");

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
        public void Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38()
        {
            List<string> projects = new List<string>();
            projects.Add("Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38");
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("c96d9ce1b2626b464cf2746ca53cb338d7d2ce38", @"..\..\TestProjects\Projects\Portable5\Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38.sln", projects);

            List<string> list = new List<string>();
            list.Add("SyntaxFactory.cs");
            bool passTransformation = CompleteTestBase(list, @"c96d9ce1b2626b464cf2746ca53cb338d7d2ce38");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj1113fd3db14fd23fc081e90f27f4ddafad7b244d()
        {
            List<string> projects = new List<string>();
            projects.Add("Proj1113fd3db14fd23fc081e90f27f4ddafad7b244d");
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("1113fd3db14fd23fc081e90f27f4ddafad7b244d", @"..\..\TestProjects\Projects\Portable\Proj1113fd3db14fd23fc081e90f27f4ddafad7b244d.sln", projects);

            List<string> list = new List<string>();
            list.Add("MetadataWriter.cs");
            bool passTransformation = CompleteTestBase(list, @"1113fd3db14fd23fc081e90f27f4ddafad7b244d");

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
        public void Proj3_c96d9ce1b2626b464cf2746ca53cb338d7d2ce38()
        {
            List<string> projects = new List<string>();
            projects.Add("Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38");
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("3_c96d9ce1b2626b464cf2746ca53cb338d7d2ce38", @"..\..\TestProjects\Projects\Portable5\Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38.sln", projects);

            List<string> list = new List<string>();
            list.Add("ObjectDisplay.cs");
            bool passTransformation = CompleteTestBase(list, @"3_c96d9ce1b2626b464cf2746ca53cb338d7d2ce38");

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
        public void Projcfd9b464dbb07c8b183d89a403a8bc877b3e929d()
        {
            List<string> projects = new List<string>();
            projects.Add("Projcfd9b464dbb07c8b183d89a403a8bc877b3e929d");
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("cfd9b464dbb07c8b183d89a403a8bc877b3e929d", @"..\..\TestProjects\Projects\Portable4\Portable\Projcfd9b464dbb07c8b183d89a403a8bc877b3e929d.sln", projects);

            List<string> list = new List<string>();
            list.Add("MetadataWriter.cs"); 
            bool passTransformation = CompleteTestBase(list, @"cfd9b464dbb07c8b183d89a403a8bc877b3e929d");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj2_cfd9b464dbb07c8b183d89a403a8bc877b3e929d()
        {
            List<string> projects = new List<string>();
            projects.Add("Projcfd9b464dbb07c8b183d89a403a8bc877b3e929d");
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("2-cfd9b464dbb07c8b183d89a403a8bc877b3e929d", @"..\..\TestProjects\Projects\Portable4\Portable\Projcfd9b464dbb07c8b183d89a403a8bc877b3e929d.sln", projects);

            List<string> list = new List<string>();
            list.Add("MetadataWriter.cs");
            bool passTransformation = CompleteTestBase(list, @"2-cfd9b464dbb07c8b183d89a403a8bc877b3e929d");

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
        public void Proj1571862b0479b8f1e1976abceac0bca9f3cdd2d7Test()
        {
            List<string> projects = new List<string>();
            projects.Add("Proj1571862b0479b8f1e1976abceac0bca9f3cdd2d7");
            bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("1571862b0479b8f1e1976abceac0bca9f3cdd2d7", @"..\..\TestProjects\Projects\FunctionalTests2\FunctionalTests\Proj1571862b0479b8f1e1976abceac0bca9f3cdd2d7.sln", projects);

            List<string> list = new List<string>();
            list.Add("DbSqlQueryTests.cs");
            bool passTransformation = CompleteTestBase(list, @"1571862b0479b8f1e1976abceac0bca9f3cdd2d7");

            Assert.IsTrue(passLocation && passTransformation);
        }

        /// <summary>
        /// Complete test
        /// </summary>
        /// <param name="mainClassAfterEditing">Main class after edit</param>
        /// <param name="complement">Complement information</param>
        /// <returns></returns>
        public static bool CompleteTestBase(List<string> editeds, string commit)
        {
            long millBefore = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            EditorController controller = EditorController.GetInstance();
            controller.CurrentViewCodeAfter = FileUtil.ReadFile(@"..\..\TestProjects\commits\" + commit + @"\" + editeds.First());

            var dicionarySelection = JsonUtil<Dictionary<string, List<Selection>>>.Read(@"..\..\TestProjects\commits\" + commit + @"\edited_selections.json");

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
                string sourceCodeAfter = FileUtil.ReadFile(@"..\..\TestProjects\commits\" + commit + @"\" + item);
                string pattern = System.Text.RegularExpressions.Regex.Escape(item);
                foreach (var entry in dicionarySelection)
                {
                    string sourceCode = FileUtil.ReadFile(entry.Key);
                    bool containsPattern = System.Text.RegularExpressions.Regex.IsMatch(entry.Key, pattern);
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

                Tuple<string, string> example = Tuple.Create(FileUtil.ReadFile(@"..\..\TestProjects\commits\" + commit + @"\tool\" + className), transformation.transformation.Item2);
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
            FileUtil.WriteToFile(@"..\..\TestProjects\commits\" + commit + @"\edit.t", totalTime.ToString());
            return passTransformation;
        }

        private static string GetClassPath()
        {
            return null;
        }
    }
}





