using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using NUnitTests.Spg.NUnitTests.LocationTestSolution;
using NUnitTests.Spg.NUnitTests.Util;
using Spg.ExampleRefactoring.Bean;
using Spg.ExampleRefactoring.Comparator;
using Spg.ExampleRefactoring.Synthesis;
using Spg.ExampleRefactoring.Util;
using Spg.LocationRefactor.Controller;
using Spg.LocationRefactor.Location;
using Spg.LocationRefactor.TextRegion;
using Spg.LocationRefactor.Transform;

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

            bool passTransformation = CompleteTestBase(@"Roslyn\0086821");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj00552fc()
        {
            List<string> projects = new List<string>();
            projects.Add("CodeAnalysis");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"Roslyn\00552fc", @"Roslyn\roslyn11\src\Roslyn.sln", projects);

            bool passTransformation = CompleteTestBase(@"Roslyn\00552fc");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj8ecd058()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpCodeAnalysis");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"Roslyn\8ecd058", @"Roslyn\roslyn7\src\Roslyn.sln", projects);

            bool passTransformation = CompleteTestBase(@"Roslyn\8ecd058");

            Assert.IsTrue(passLocation && passTransformation);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Projb495c9a()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpCodeAnalysis");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"Roslyn\b495c9a", @"Roslyn\roslyn16\src\Roslyn.sln", projects);

            bool passTransformation = CompleteTestBase(@"Roslyn\b495c9a");

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

            bool passTransformation = CompleteTestBase(@"Roslyn\673f18e");

            Assert.IsTrue(passLocation && passTransformation);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj2_673f18e()
        {
            List<string> projects = new List<string>();
            projects.Add("CodeAnalysisTest");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"Roslyn\2_673f18e", @"Roslyn\roslyn7\src\Roslyn.sln", projects);

            bool passTransformation = CompleteTestBase(@"Roslyn\2_673f18e");

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

            bool passTransformation = CompleteTestBase(@"Roslyn\3_673f18e");

            Assert.IsTrue(passLocation && passTransformation);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj7_673f18e()
        {
            List<string> projects = new List<string>();
            projects.Add("CodeAnalysis");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"Roslyn\7_673f18e", @"Roslyn\roslyn7\src\Roslyn.sln", projects);

            bool passTransformation = CompleteTestBase(@"Roslyn\7_673f18e");

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

            bool passTransformation = CompleteTestBase(@"Roslyn\5_673f18e");

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

            bool passTransformation = CompleteTestBase(@"Roslyn\6_673f18e");

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

            bool passTransformation = CompleteTestBase(@"Roslyn\8_673f18e");

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

            bool passTransformation = CompleteTestBase(@"Roslyn\9_673f18e");

            Assert.IsTrue(passLocation && passTransformation);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj4_673f18e()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpCommandLineTest");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"Roslyn\4_673f18e", @"Roslyn\roslyn7\src\Roslyn.sln", projects);

            bool passTransformation = CompleteTestBase(@"Roslyn\4_673f18e");

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

            bool passTransformation = CompleteTestBase(@"Roslyn\2_8c14644");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj3_8c14644()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpCodeAnalysis");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"Roslyn\3_8c14644", @"Roslyn\roslyn\src\Roslyn.sln", projects);

            bool passTransformation = CompleteTestBase(@"Roslyn\3_8c14644");

            Assert.IsTrue(passLocation && passTransformation);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj83e4349()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpCodeAnalysis");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"Roslyn\83e4349", @"Roslyn\roslyn15\src\Roslyn.sln", projects);

            bool passTransformation = CompleteTestBase(@"Roslyn\83e4349");

            Assert.IsTrue(passLocation && passTransformation);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proje817dab()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpWorkspace");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"Roslyn\e817dab", @"Roslyn\roslyn17\src\Roslyn.sln", projects);

            bool passTransformation = CompleteTestBase(@"Roslyn\e817dab");

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

            bool passTransformation = CompleteTestBase(@"Roslyn\cd68d03");

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

            bool passTransformation = CompleteTestBase(@"Roslyn\2_cd68d03");

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

            bool passTransformation = CompleteTestBase(@"Roslyn\3_cd68d03");

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

            bool passTransformation = CompleteTestBase(@"Roslyn\f66696e");

            Assert.IsTrue(passLocation && passTransformation);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj4b40293()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpCodeAnalysis");
            projects.Add("CSharpCompilerEmitTest");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"Roslyn\4b40293", @"Roslyn\roslyn7\src\Roslyn.sln", projects);

            bool passTransformation = CompleteTestBase(@"Roslyn\4b40293");

            Assert.IsTrue(passLocation && passTransformation);
        }

        ///// <summary>
        ///// Test Method Call To Identifier transformation
        ///// </summary>
        //[Test]
        //public void Proj8ecd05880b478e4ca997a4789b976ef73b070546()
        //{
        //    List<string> projects = new List<string>();
        //    projects.Add("Proj4b402939708adf35a7a5e12ffc99dc14cc1f4766");
        //    bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("8ecd05880b478e4ca997a4789b976ef73b070546", @"..\..\TestProjects\Projects\Portable7\Proj4b402939708adf35a7a5e12ffc99dc14cc1f4766.sln", projects);

        //    bool passTransformation = CompleteTestBase(@"8ecd05880b478e4ca997a4789b976ef73b070546");

        //    Assert.IsTrue(passLocation && passTransformation);
        //}

        [Test]
        public void Proj04d0604()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpFxCopRulesDiagnosticAnalyzers");
            projects.Add("FxCopRulesDiagnosticAnalyzers");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"Roslyn\04d0604", @"Roslyn\roslyn18\Src\Roslyn.sln", projects);

            bool passTransformation = CompleteTestBase(@"Roslyn\04d0604");

            Assert.IsTrue(passLocation && passTransformation);
        }

        //[Test]
        //public void Proj318b2b0e476a122ebc033b13d41449ef1c814c1d()
        //{
        //    List<string> projects = new List<string>();
        //    projects.Add("Proj318b2b0e476a122ebc033b13d41449ef1c814c1d");
        //    bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("318b2b0e476a122ebc033b13d41449ef1c814c1d", @"..\..\TestProjects\Projects\Core2\Proj318b2b0e476a122ebc033b13d41449ef1c814c1d.sln", projects);

        //    List<string> list = new List<string>();
        //    list.Add("DeclarePublicAPIFix.cs");
        //    bool passTransformation = CompleteTestBase(list, @"318b2b0e476a122ebc033b13d41449ef1c814c1d");

        //    Assert.IsTrue(passLocation && passTransformation);

        //}

        /// <summary>
        /// Test case for parameter to constant value
        /// </summary>
        [Test]
        public void Projc96d9ce()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpCodeAnalysis");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"Roslyn\c96d9ce", @"Roslyn\roslyn4\src\Roslyn.sln", projects);

            bool passTransformation = CompleteTestBase(@"Roslyn\c96d9ce");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj1113fd3()
        {
            List<string> projects = new List<string>();
            projects.Add("CodeAnalysis");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"Roslyn\1113fd3", @"Roslyn\roslyn2\src\Roslyn.sln", projects);

            bool passTransformation = CompleteTestBase(@"Roslyn\1113fd3");

            Assert.IsTrue(passLocation && passTransformation);
        }

        ///// <summary>
        ///// Change Exception test
        ///// </summary>
        //[Test]
        //public void Projcc3d32746f60ed5a9f3775ef0ec44424b03d65cf()
        //{
        //    List<string> projects = new List<string>();
        //    projects.Add("Projcc3d32746f60ed5a9f3775ef0ec44424b03d65cf");
        //    bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("cc3d32746f60ed5a9f3775ef0ec44424b03d65cf", @"..\..\TestProjects\Projects\Portable2\Portable\Projcc3d32746f60ed5a9f3775ef0ec44424b03d65cf.sln", projects);

        //    List<string> list = new List<string>();
        //    list.Add("Contract.cs");
        //    bool passTransformation = CompleteTestBase(list, @"cc3d32746f60ed5a9f3775ef0ec44424b03d65cf");

        //    Assert.IsTrue(passLocation && passTransformation);
        //}

        ///// <summary>
        ///// Change parameter on method test
        ///// </summary>
        //[Test]
        //public void Proje7184bd18b5e2de6e1b71ba8f893c6d5e9a7bebd()
        //{
        //    List<string> projects = new List<string>();
        //    projects.Add("Proje7184bd18b5e2de6e1b71ba8f893c6d5e9a7bebd");
        //    bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("e7184bd18b5e2de6e1b71ba8f893c6d5e9a7bebd", @"..\..\TestProjects\Projects\Portable3\Portable\Proje7184bd18b5e2de6e1b71ba8f893c6d5e9a7bebd.sln", projects);

        //    List<string> list = new List<string>();
        //    list.Add("TaskExtensions.cs");
        //    bool passTransformation = CompleteTestBase(list, @"e7184bd18b5e2de6e1b71ba8f893c6d5e9a7bebd");

        //    Assert.IsTrue(passLocation && passTransformation);
        //}

        //[Test]
        //public void Proj2_c96d9ce1b2626b464cf2746ca53cb338d7d2ce38()
        //{
        //    List<string> projects = new List<string>();
        //    projects.Add("Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38");
        //    bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("2_c96d9ce1b2626b464cf2746ca53cb338d7d2ce38", @"..\..\TestProjects\Projects\Portable5\Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38.sln", projects);

        //    List<string> list = new List<string>();
        //    list.Add("SymbolDisplay.cs");
        //    bool passTransformation = CompleteTestBase(list, @"2_c96d9ce1b2626b464cf2746ca53cb338d7d2ce38");

        //    Assert.IsTrue(passLocation && passTransformation);
        //}

        [Test]
        public void Proj3_c96d9ce()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpCodeAnalysis");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"Roslyn\3_c96d9ce", @"Roslyn\roslyn4\src\Roslyn.sln", projects);

            bool passTransformation = CompleteTestBase(@"Roslyn\3_c96d9ce");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj4_c96d9ce()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpCodeAnalysis");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"Roslyn\4_c96d9ce", @"Roslyn\roslyn4\src\Roslyn.sln", projects);

            bool passTransformation = CompleteTestBase(@"Roslyn\4_c96d9ce");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj5_c96d9ce()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpCodeAnalysis");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"Roslyn\5_c96d9ce", @"Roslyn\roslyn4\src\Roslyn.sln", projects);

            bool passTransformation = CompleteTestBase(@"Roslyn\5_c96d9ce");

            Assert.IsTrue(passLocation && passTransformation);
        }


        // [Test]
        // public void ReturnToGetTest()
        // {
        //     bool passLocation = LocationTest.LocaleTest(FilePath.RETURN_TO_GET_INPUT, FilePath.RETURN_TO_GET_OUTPUT_SELECTION, FilePath.MAIN_CLASS_RETURN_TO_GET_PATH);

        //     bool passTransformation = CompleteTestBase(FilePath.MAIN_CLASS_RETURN_TO_GET_AFTER_EDITING,  FilePath.RETURN_TO_GET_EDITION, @"\return_to_get\");

        //     Assert.IsTrue(passLocation && passTransformation);
        //}

        //[Test]
        //public void Proj49cdaceb2828acc1f50223826d478a00a80a59e2()
        //{
        //    List<string> projects = new List<string>();
        //    projects.Add("Proj49cdaceb2828acc1f50223826d478a00a80a59e2");
        //    bool passLocation = LocationTestProject.LocationTestProject.LocaleTest("49cdaceb2828acc1f50223826d478a00a80a59e2", @"..\..\TestProjects\Projects\CSharp\Proj49cdaceb2828acc1f50223826d478a00a80a59e2.sln", projects);

        //    List<string> list = new List<string>();
        //    list.Add("MockCSharpCompiler.cs"); list.Add("MockCsi.cs");
        //    bool passTransformation = CompleteTestBase(list, @"49cdaceb2828acc1f50223826d478a00a80a59e2");

        //    Assert.IsTrue(passLocation && passTransformation);
        //}

        [Test]
        public void Projcfd9b46()
        {
            List<string> projects = new List<string>();
            projects.Add("CodeAnalysis");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"Roslyn\cfd9b46", @"Roslyn\roslyn6\src\Roslyn.sln", projects);

            bool passTransformation = CompleteTestBase(@"Roslyn\cfd9b46");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj2_cfd9b46()
        {
            List<string> projects = new List<string>();
            projects.Add("CodeAnalysis");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"Roslyn\2_cfd9b46", @"Roslyn\roslyn6\src\Roslyn.sln", projects);

            bool passTransformation = CompleteTestBase(@"Roslyn\2_cfd9b46");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj7c885ca()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpCommandLineTest");
            projects.Add("CodeAnalysisTest");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"Roslyn\7c885ca", @"Roslyn\roslyn7\src\Roslyn.sln", projects);

            bool passTransformation = CompleteTestBase(@"Roslyn\7c885ca");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj2_7c885ca()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpFxCopRulesDiagnosticAnalyzers");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"Roslyn\2_7c885ca", @"Roslyn\roslyn14\src\Roslyn.sln", projects);

            bool passTransformation = CompleteTestBase(@"Roslyn\2_7c885ca");

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
        public void Proje28c812()
        {
            List<string> projects = new List<string>();
            projects.Add("CodeAnalysisDiagnosticAnalyzers");
            projects.Add("CSharpFxCopRulesDiagnosticAnalyzers");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"Roslyn\e28c812", @"Roslyn\roslyn7\src\Roslyn.sln", projects);

            bool passTransformation = CompleteTestBase(@"Roslyn\e28c812");

            Assert.IsTrue(passLocation && passTransformation);
        }

        //Entity Framework tests
        [Test]
        public void Projd83cdfa()
        {
            List<string> projects = new List<string>();
            projects.Add("UnitTests");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"EntityFramewok\d83cdfa", @"EntityFramework\entityframework1\EntityFramework.sln", projects);

            bool passTransformation = CompleteTestBase(@"EntityFramewok\d83cdfa");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Projd8e9409()
        {
            List<string> projects = new List<string>();
            projects.Add("EntityFramework");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"EntityFramewok\d8e9409", @"EntityFramework\entityframework11\EntityFramework.sln", projects);

            bool passTransformation = CompleteTestBase(@"EntityFramewok\d8e9409");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Projbc42e49()
        {
            List<string> projects = new List<string>();
            projects.Add("EntityFramework");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"EntityFramewok\bc42e49", @"EntityFramework\entityframework2\EntityFramework.sln", projects);

            bool passTransformation = CompleteTestBase(@"EntityFramewok\bc42e49");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj14623da()
        {
            List<string> projects = new List<string>();
            projects.Add("EntityFramework");
            projects.Add("UnitTests");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"EntityFramewok\14623da", @"EntityFramework\entityframework10\EntityFramework.sln", projects);

            bool passTransformation = CompleteTestBase(@"EntityFramewok\14623da");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj2_14623da()
        {
            List<string> projects = new List<string>();
            projects.Add("EntityFramework");
            projects.Add("UnitTests");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"EntityFramewok\2_14623da", @"EntityFramework\entityframework10\EntityFramework.sln", projects);

            bool passTransformation = CompleteTestBase(@"EntityFramewok\2_14623da");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj3_14623da()
        {
            List<string> projects = new List<string>();
            projects.Add("UnitTests");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"EntityFramewok\3_14623da", @"EntityFramework\entityframework10\EntityFramework.sln", projects);

            bool passTransformation = CompleteTestBase(@"EntityFramewok\3_14623da");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj4_14623da()
        {
            List<string> projects = new List<string>();
            projects.Add("UnitTests");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"EntityFramewok\4_14623da", @"EntityFramework\entityframework10\EntityFramework.sln", projects);

            bool passTransformation = CompleteTestBase(@"EntityFramewok\4_14623da");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj2bae908()
        {
            List<string> projects = new List<string>();
            projects.Add("EntityFramework");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"EntityFramewok\2bae908", @"EntityFramework\entityframework3\EntityFramework.sln", projects);

            bool passTransformation = CompleteTestBase(@"EntityFramewok\2bae908");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj8d45249()
        {
            List<string> projects = new List<string>();
            projects.Add("FunctionalTests");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"EntityFramewok\8d45249", @"EntityFramework\entityframework4\EntityFramework.sln", projects);

            bool passTransformation = CompleteTestBase(@"EntityFramewok\8d45249");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj1571862()
        {
            List<string> projects = new List<string>();
            projects.Add("FunctionalTests");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"EntityFramewok\1571862", @"EntityFramework\entityframework4\EntityFramework.sln", projects);

            bool passTransformation = CompleteTestBase(@"EntityFramewok\1571862");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj2_1571862()
        {
            List<string> projects = new List<string>();
            projects.Add("FunctionalTests");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"EntityFramewok\2_1571862", @"EntityFramework\entityframework4\EntityFramework.sln", projects);

            bool passTransformation = CompleteTestBase(@"EntityFramewok\2_1571862");

            Assert.IsTrue(passLocation && passTransformation);
        }

        //[Test]
        //public void Proj4_1571862()
        //{
        //    List<string> projects = new List<string>();
        //    projects.Add("FunctionalTests");
        //    bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"EntityFramewok\4_1571862", @"EntityFramework\entityframework4\EntityFramework.sln", projects);

        //    bool passTransformation = CompleteTestBase(@"EntityFramewok\4_1571862");

        //    Assert.IsTrue(passLocation && passTransformation);
        //}

        [Test]
        public void Proj5_1571862()
        {
            List<string> projects = new List<string>();
            projects.Add("FunctionalTests");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"EntityFramewok\5_1571862", @"EntityFramework\entityframework4\EntityFramework.sln", projects);

            bool passTransformation = CompleteTestBase(@"EntityFramewok\5_1571862");

            Assert.IsTrue(passLocation && passTransformation);
        }

        //[Test]
        //public void Proj6_1571862()
        //{
        //    List<string> projects = new List<string>();
        //    projects.Add("FunctionalTests.Transitional");
        //    bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"EntityFramewok\6_1571862", @"EntityFramework\entityframework4\EntityFramework.sln", projects);

        //    bool passTransformation = CompleteTestBase(@"EntityFramewok\6_1571862");

        //    Assert.IsTrue(passLocation && passTransformation);
        //}

        [Test]
        public void Projce1e333()
        {
            List<string> projects = new List<string>();
            projects.Add("EntityFramework");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"EntityFramewok\ce1e333", @"EntityFramework\entityframework5\EntityFramework.sln", projects);

            bool passTransformation = CompleteTestBase(@"EntityFramewok\ce1e333");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj8b9180b()
        {
            List<string> projects = new List<string>();
            projects.Add("FunctionalTests");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"EntityFramewok\8b9180b", @"EntityFramework\entityframework6\EntityFramework.sln", projects);

            bool passTransformation = CompleteTestBase(@"EntityFramewok\8b9180b");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj2_8b9180b()
        {
            List<string> projects = new List<string>();
            projects.Add("FunctionalTests");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"EntityFramewok\2_8b9180b", @"EntityFramework\entityframework6\EntityFramework.sln", projects);

            bool passTransformation = CompleteTestBase(@"EntityFramewok\2_8b9180b");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj829dec5()
        {
            List<string> projects = new List<string>();
            projects.Add("EntityFramework");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"EntityFramewok\829dec5", @"EntityFramework\entityframework7\EntityFramework.sln", projects);

            bool passTransformation = CompleteTestBase(@"EntityFramewok\829dec5");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj326d525()
        {
            List<string> projects = new List<string>();
            projects.Add("FunctionalTests.Transitional");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"EntityFramewok\326d525", @"EntityFramework\entityframework9\EntityFramework.sln", projects);

            bool passTransformation = CompleteTestBase(@"EntityFramewok\326d525");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj2_326d525()
        {
            List<string> projects = new List<string>();
            projects.Add("FunctionalTests");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"EntityFramewok\2_326d525", @"EntityFramework\entityframework9\EntityFramework.sln", projects);

            bool passTransformation = CompleteTestBase(@"EntityFramewok\2_326d525");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proja883600()
        {
            List<string> projects = new List<string>();
            projects.Add("Core.Test");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"NuGet\a883600", @"NuGet\nuget4\NuGet.sln", projects);

            bool passTransformation = CompleteTestBase(@"NuGet\a883600");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj2_a883600()
        {
            List<string> projects = new List<string>();
            projects.Add("CommandLine");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"NuGet\2_a883600", @"NuGet\nuget4\NuGet.sln", projects);

            bool passTransformation = CompleteTestBase(@"NuGet\2_a883600");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj3_a883600()
        {
            List<string> projects = new List<string>();
            projects.Add("Core");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"NuGet\3_a883600", @"NuGet\nuget4\NuGet.sln", projects);

            bool passTransformation = CompleteTestBase(@"NuGet\3_a883600");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj5_a883600()
        {
            List<string> projects = new List<string>();
            projects.Add("Core.Test");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"NuGet\5_a883600", @"NuGet\nuget4\NuGet.sln", projects);

            bool passTransformation = CompleteTestBase(@"NuGet\5_a883600");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj8da9f0e()
        {
            List<string> projects = new List<string>();
            projects.Add("Core.Test");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"NuGet\8da9f0e", @"NuGet\nuget3\NuGet.sln", projects);

            bool passTransformation = CompleteTestBase(@"NuGet\8da9f0e");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj2_8da9f0e()
        {
            List<string> projects = new List<string>();
            projects.Add("Core.Test");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"NuGet\2_8da9f0e", @"NuGet\nuget3\NuGet.sln", projects);

            bool passTransformation = CompleteTestBase(@"NuGet\2_8da9f0e");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj3_8da9f0e()
        {
            List<string> projects = new List<string>();
            projects.Add("Test.Utility");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"NuGet\3_8da9f0e", @"NuGet\nuget3\NuGet.sln", projects);

            bool passTransformation = CompleteTestBase(@"NuGet\3_8da9f0e");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proja569c55()
        {
            List<string> projects = new List<string>();
            projects.Add("NuGet.Client");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"NuGet\a569c55", @"NuGet\nuget5\NuGet.sln", projects);

            bool passTransformation = CompleteTestBase(@"NuGet\a569c55");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj2_a569c55()
        {
            List<string> projects = new List<string>();
            projects.Add("NuGet.Client");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"NuGet\2_a569c55", @"NuGet\nuget5\NuGet.sln", projects);

            bool passTransformation = CompleteTestBase(@"NuGet\2_a569c55");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj4_8da9f0e()
        {
            List<string> projects = new List<string>();
            projects.Add("VisualStudio.Test");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"NuGet\4_8da9f0e", @"NuGet\nuget3\NuGet.sln", projects);

            bool passTransformation = CompleteTestBase(@"NuGet\4_8da9f0e");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj5_8da9f0e()
        {
            List<string> projects = new List<string>();
            projects.Add("FunctionalTests.Transitional");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"NuGet\5_8da9f0e",
                @"NuGet\nuget3\NuGet.sln", projects);

            bool passTransformation = CompleteTestBase(@"NuGet\5_8da9f0e");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Projd9f64ea()
        {
            List<string> projects = new List<string>();
            projects.Add("Core");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"NuGet\d9f64ea", @"NuGet\nuget7\NuGet.sln", projects);

            bool passTransformation = CompleteTestBase(@"NuGet\d9f64ea");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj2_d9f64ea()
        {
            List<string> projects = new List<string>();
            projects.Add("Core");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"NuGet\2_d9f64ea", @"NuGet\nuget7\NuGet.sln", projects);

            bool passTransformation = CompleteTestBase(@"NuGet\2_d9f64ea");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj3_d9f64ea()
        {
            List<string> projects = new List<string>();
            projects.Add("Core");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"NuGet\3_d9f64ea", @"NuGet\nuget7\NuGet.sln", projects);

            bool passTransformation = CompleteTestBase(@"NuGet\3_d9f64ea");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Projdfc4e3d()
        {
            List<string> projects = new List<string>();
            projects.Add("NuGet.Client.VisualStudio.PowerShell");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"NuGet\dfc4e3d", @"NuGet\nuget6\NuGet.sln", projects);

            bool passTransformation = CompleteTestBase(@"NuGet\dfc4e3d");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Pro2dea84e()
        {
            List<string> projects = new List<string>();
            projects.Add("NuGet.Client.VisualStudio.PowerShell");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"NuGet\2dea84e", @"NuGet\nuget2\NuGet.sln", projects);

            bool passTransformation = CompleteTestBase(@"NuGet\2dea84e");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj74d4d32()
        {
            List<string> projects = new List<string>();
            projects.Add("NuGet.Client.CommandLine");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"NuGet\74d4d32", @"NuGet\nuget8\NuGet.sln", projects);

            bool passTransformation = CompleteTestBase(@"NuGet\74d4d32");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj7d11ddd()
        {
            List<string> projects = new List<string>();
            projects.Add("NuGet.Client.CommandLine");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"NuGet\7d11ddd", @"NuGet\nuget9\NuGet.sln", projects);

            bool passTransformation = CompleteTestBase(@"NuGet\7d11ddd");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj2_7d11ddd()
        {
            List<string> projects = new List<string>();
            projects.Add("NuGet.Client.CommandLine");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"NuGet\2_7d11ddd", @"NuGet\nuget9\NuGet.sln", projects);

            bool passTransformation = CompleteTestBase(@"NuGet\2_7d11ddd");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Projee953e8()
        {
            List<string> projects = new List<string>();
            projects.Add("NuGet.Client.VisualStudio.UI");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"NuGet\ee953e8", @"NuGet\nuget10\NuGet.sln", projects);

            bool passTransformation = CompleteTestBase(@"NuGet\ee953e8");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj2_ee953e8()
        {
            List<string> projects = new List<string>();
            projects.Add("NuGet.Client.VisualStudio.UI");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"NuGet\2_ee953e8", @"NuGet\nuget10\NuGet.sln", projects);

            bool passTransformation = CompleteTestBase(@"NuGet\2_ee953e8");

            Assert.IsTrue(passLocation && passTransformation);
        }

        [Test]
        public void Proj4ff8771()
        {
            List<string> projects = new List<string>();
            projects.Add("NuGet.Client.VisualStudio.UI");
            bool passLocation = LocationTestSolution.LocationTestSolution.LocaleTestSolution(@"NuGet\4ff8771", @"NuGet\nuget11\NuGet.sln", projects);

            bool passTransformation = CompleteTestBase(@"NuGet\4ff8771");

            Assert.IsTrue(passLocation && passTransformation);
        }

        ///// <summary>
        ///// Complete test
        ///// </summary>
        ///// <param name="editeds">Files edited</param>
        ///// <param name="commit">Commit where the change occurs</param>
        ///// <returns>True if pass test</returns>
        //public static bool CompleteTestBase(string commit)
        //{
        //    long millBefore = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        //    EditorController controller = EditorController.GetInstance();

        //    string expHome = Environment.GetEnvironmentVariable("EXP_HOME", EnvironmentVariableTarget.User);

        //    //remove
        //    List<CodeLocation> controllerLocations = new List<CodeLocation>(controller.Locations);
        //    List<TRegion> selections = JsonUtil<List<TRegion>>.Read(expHome + @"commit\" + commit + @"\metadata\locations_on_commit.json");
        //    List<CodeLocation> locations = TestUtil.GetAllLocationsOnCommit(selections, controller.Locations);
        //    controller.Locations = locations;


        //    Dictionary<string, List<Selection>> dicionarySelection = JsonUtil<Dictionary<string, List<Selection>>>.Read(expHome + @"commit\" + commit + @"\edited_selections.json");
        //    dicionarySelection = TestUtil.FilterLocationsNotPresentOnCommit(dicionarySelection, controllerLocations, locations);
        //    ////remove

        //    var dicionarySelectionFullpath = new Dictionary<string, List<Selection>>();
        //    foreach (var entry in dicionarySelection)
        //    {
        //        dicionarySelectionFullpath.Add(Path.GetFullPath(entry.Key), entry.Value);
        //    }

        //    //            dicionarySelection = dicionarySelectionFullpath;
        //    controller.EditedLocations = dicionarySelectionFullpath;

        //    List<Tuple<string, string>> documents = new List<Tuple<string, string>>();

        //    foreach (KeyValuePair<string, List<Selection>> entry in dicionarySelection)
        //    {
        //        string classPath = entry.Key;
        //        string className = classPath.Substring(classPath.LastIndexOf(@"\") + 1, classPath.Length - (classPath.LastIndexOf(@"\") + 1));

        //        string sourceCodeAfter = FileUtil.ReadFile(expHome + @"commit\" + commit + @"\" + className);
        //        controller.CurrentViewCodeAfter = sourceCodeAfter;

        //        string sourceCode = FileUtil.ReadFile(entry.Key);
        //        Tuple<string, string> tuple = Tuple.Create(sourceCode, sourceCodeAfter);
        //        documents.Add(tuple);
        //        controller.FilesOpened[entry.Key] = true;
        //    }

        //    controller.DocumentsBeforeAndAfter = documents;

        //    controller.Refact();

        //    FileInfo file = new FileInfo(expHome + @"commit\" + commit + @"\metadata\cs\");
        //    file.Directory.Create();

        //    bool passTransformation = true;
        //    foreach (Transformation transformation in controller.SourceTransformations)
        //    {
        //        string classPath = transformation.SourcePath;
        //        string className = classPath.Substring(classPath.LastIndexOf(@"\") + 1, classPath.Length - (classPath.LastIndexOf(@"\") + 1));
        //        //string classNamePath = expHome + @"commit\" + commit + @"\tool\" + className;
        //        string classNamePath = expHome + @"commit\" + commit + @"\metadata\cs\" + className;
        //        FileUtil.WriteToFile(classNamePath, transformation.transformation.Item2);

        //        Tuple<string, string> example = Tuple.Create(FileUtil.ReadFile(classNamePath), transformation.transformation.Item2);
        //        Tuple<ListNode, ListNode> lnode = ASTProgram.Example(example);

        //        NodeComparer comparator = new NodeComparer();
        //        bool isEqual = comparator.SequenceEqual(lnode.Item1, lnode.Item2);
        //        if (!isEqual)
        //        {
        //            passTransformation = false;
        //            break;
        //        }
        //    }
        //    controller.Undo();
        //    long millAfer = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        //    long totalTime = (millAfer - millBefore);
        //    FileUtil.WriteToFile(expHome + @"commit\" + commit + @"\edit.t", totalTime.ToString());

        //    JsonUtil<List<CodeTransformation>>.Write(TransformationsSave(controller.CodeTransformations), "transformed_locations.json");

        //    string transformations = FileUtil.ReadFile("transformed_locations.json");
        //    FileUtil.WriteToFile(expHome + @"commit\" + commit + @"\" + "transformed_locations.json", transformations);
        //    FileUtil.WriteToFile(expHome + @"commit\" + commit + @"\metadata_tool\transformed_locations.json", transformations);
        //    FileUtil.DeleteFile("transformed_locations.json");
        //    return passTransformation;
        //}

        /// <summary>
        /// Complete test
        /// </summary>
        /// <param name="editeds">Files edited</param>
        /// <param name="commit">Commit where the change occurs</param>
        /// <returns>True if pass test</returns>
        public static bool CompleteTestBase(string commit)
        {
            //long millBefore = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            EditorController controller = EditorController.GetInstance();

            string expHome = Environment.GetEnvironmentVariable("EXP_HOME", EnvironmentVariableTarget.User);

            //remove
            List<CodeLocation> controllerLocations = new List<CodeLocation>(controller.Locations);
            List<TRegion> selections = JsonUtil<List<TRegion>>.Read(expHome + @"commit\" + commit + @"\metadata\locations_on_commit.json");
            List<CodeLocation> locations = TestUtil.GetAllLocationsOnCommit(selections, controller.Locations);
            controller.Locations = locations;

            List<CodeTransformation> allTransformations = JsonUtil<List<CodeTransformation>>.Read(expHome + @"commit\" + commit + @"\metadata_tool\transformed_locations.json");

            List<CodeTransformation> transformationsOnCommit = TestUtil.GetAllTransformationsOnCommit(allTransformations, locations);

            JsonUtil<List<CodeTransformation>>.Write(transformationsOnCommit, "transformed_locations.json");

            string transformations = FileUtil.ReadFile("transformed_locations.json");
            FileUtil.WriteToFile(expHome + @"commit\" + commit + @"\" + "transformed_locations2.json", transformations);
            FileUtil.WriteToFile(expHome + @"commit\" + commit + @"\metadata_tool\transformed_locations2.json", transformations);
            FileUtil.DeleteFile("transformed_locations.json");
            return true;
        }

        //private static List<CodeTransformation> TransformationsSave(List<CodeTransformation> CodeTransformations)
        //{

        //    List<CodeTransformation> transformations = new List<CodeTransformation>();
        //    foreach (CodeTransformation codeTransformation in CodeTransformations)
        //    {
        //        CodeLocation location = new CodeLocation();
        //        location.SourceClass = codeTransformation.Location.SourceClass;
        //        location.SourceCode = codeTransformation.Location.SourceCode;
        //        TRegion region = new TRegion();
        //        region.Start = codeTransformation.Location.Region.Start;
        //        region.Length = codeTransformation.Location.Region.Length;
        //        region.Path = codeTransformation.Location.Region.Path;
        //        region.Parent = codeTransformation.Location.Region.Parent;
        //        region.Text = codeTransformation.Location.Region.Text;
        //        location.Region = region;

        //        CodeTransformation transformation = new CodeTransformation(location, codeTransformation.Trans, codeTransformation.Transformation);
        //        transformations.Add(transformation);
        //    }

        //    return transformations;
        //}
    }
}







