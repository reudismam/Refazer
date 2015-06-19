using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ExampleRefactoring.Spg.ExampleRefactoring.Bean;
using ExampleRefactoring.Spg.ExampleRefactoring.Util;
using NUnit.Framework;
using Spg.ExampleRefactoring.Util;
using Spg.LocationCodeRefactoring.Controller;
using Spg.LocationRefactor.TextRegion;

namespace NUnitTests.Spg.NUnitTests.LocationTestSolution
{
    [TestFixture]
    public class LocationTestSolution
    {
        ///// <summary>
        ///// Simple API Test for locations
        ///// </summary>
        //[Test]
        //public void SimpleAPIChangeTest()
        //{
        //    bool isValid = LocaleTest(FilePath.SIMPLE_API_CHANGE_INPUT, FilePath.SIMPLE_API_CHANGE_OUTPUT_SELECTION, FilePath.MAIN_CLASS_SIMPLE_API_CHANGE_PATH);
        //    Assert.IsTrue(isValid);
        //}

        ///// <summary>
        ///// Test Introduce parameter on if
        ///// </summary>
        //[Test]
        //public void IntroduceParamOnIf()
        //{
        //    bool isValid = LocaleTest(FilePath.INTRODUCE_PARAM_ON_IF_INPUT, FilePath.INTRODUCE_PARAM_ON_IF_OUTPUT_SELECTION, FilePath.MAIN_CLASS_INTRODUCE_PARAM_ON_IF_PATH);
        //    Assert.IsTrue(isValid);
        //}

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj00552fc()
        {
            List<string> projects = new List<string>();
            projects.Add("CodeAnalysis");
            bool isValid = LocaleTestSolution(@"Roslyn\00552fc", @"Roslyn\roslyn11\src\Roslyn.sln", projects);
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Projb495c9ac44440cda289f09eb40e276c8d8e27ee9()
        {
            List<string> projects = new List<string>();
            projects.Add("Projb495c9ac44440cda289f09eb40e276c8d8e27ee9");
            bool isValid = LocaleTestSolution("b495c9ac44440cda289f09eb40e276c8d8e27ee9", @"..\..\TestProjects\Projects\Portable14\Projb495c9ac44440cda289f09eb40e276c8d8e27ee9.sln", projects);
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj0086821()
        {
            List<string> projects = new List<string>();
            projects.Add("CodeAnalysis");
            bool isValid = LocaleTestSolution(@"Roslyn\0086821", @"Roslyn\roslyn12\src\Roslyn.sln", projects);
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj8c146441b4ecedbf7648e890d33f946f9b206e01()
        {
            List<string> projects = new List<string>();
            projects.Add("Proj8c146441b4ecedbf7648e890d33f946f9b206e01");
            bool isValid = LocaleTestSolution("8c146441b4ecedbf7648e890d33f946f9b206e01", @"..\..\TestProjects\Projects\Portable12\Proj8c146441b4ecedbf7648e890d33f946f9b206e01.sln", projects);
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj2_8c14644()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpCodeAnalysis");
            bool isValid = LocaleTestSolution(@"Roslyn\2_8c14644", @"Roslyn\roslyn\src\Roslyn.sln", projects);
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj673f18e()
        {
            List<string> projects = new List<string>();
            projects.Add("CodeAnalysis");
            bool isValid = LocaleTestSolution(@"Roslyn\673f18e", @"Roslyn\roslyn7\src\Roslyn.sln", projects);
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj2_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8()
        {
            List<string> projects = new List<string>();
            projects.Add("Proj2_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8");
            bool isValid = LocaleTestSolution("2_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8", @"..\..\TestProjects\Projects\CodeAnalysisTest\Proj2_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8.sln", projects);
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj3_673f18e()
        {
            List<string> projects = new List<string>();
            projects.Add("CodeAnalysis");
            bool isValid = LocaleTestSolution(@"Roslyn\3_673f18e", @"Roslyn\roslyn7\src\Roslyn.sln", projects);
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj4_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8()
        {
            List<string> projects = new List<string>();
            projects.Add("Proj4_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8");
            bool isValid = LocaleTestSolution("4_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8", @"..\..\TestProjects\Projects\CommandLine\Proj4_673f18e1f9bbbae8a8bd8333f367c86d935e8eb8.sln", projects);
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj5_673f18e()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpCompilerSyntaxTest");
            bool isValid = LocaleTestSolution(@"Roslyn\5_673f18e", @"Roslyn\roslyn7\src\Roslyn.sln", projects);
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj6_673f18e()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpCompilerSymbolTest");
            bool isValid = LocaleTestSolution(@"Roslyn\6_673f18e", @"Roslyn\roslyn7\src\Roslyn.sln", projects);
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj8_673f18e()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpCompilerSymbolTest");
            bool isValid = LocaleTestSolution(@"Roslyn\8_673f18e", @"Roslyn\roslyn7\src\Roslyn.sln", projects);
            Assert.IsTrue(isValid);
        }

        // <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj9_673f18e()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpCompilerEmitTest");
            bool isValid = LocaleTestSolution(@"Roslyn\9_673f18e", @"Roslyn\roslyn7\src\Roslyn.sln", projects);
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj83e4349133d27a8f4dd5a85b69eb4ba00d41e6f8()
        {
            List<string> projects = new List<string>();
            projects.Add("Proj83e4349133d27a8f4dd5a85b69eb4ba00d41e6f8");
            bool isValid = LocaleTestSolution("83e4349133d27a8f4dd5a85b69eb4ba00d41e6f8", @"..\..\TestProjects\Projects\Portable10\Proj83e4349133d27a8f4dd5a85b69eb4ba00d41e6f8.sln", projects);
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proje817dab72dd5199cb5c7f661bc6b289f63ae706b()
        {
            List<string> projects = new List<string>();
            projects.Add("Proje817dab72dd5199cb5c7f661bc6b289f63ae706b");
            bool isValid = LocaleTestSolution("e817dab72dd5199cb5c7f661bc6b289f63ae706b", @"..\..\TestProjects\Projects\Portable9\Proje817dab72dd5199cb5c7f661bc6b289f63ae706b.sln", projects);
            Assert.IsTrue(isValid);
        }



        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Projcd68d03()
        {
            List<string> projects = new List<string>();
            projects.Add("Workspaces");
            bool isValid = LocaleTestSolution(@"Roslyn\cd68d03", @"Roslyn\roslyn7\src\Roslyn.sln", projects);
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj2_cd68d03()
        {
            List<string> projects = new List<string>();
            projects.Add("Workspaces");
            bool isValid = LocaleTestSolution(@"Roslyn\2_cd68d03", @"Roslyn\roslyn7\src\Roslyn.sln", projects);
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj3_cd68d03()
        {
            List<string> projects = new List<string>();
            projects.Add("Workspaces");
            bool isValid = LocaleTestSolution(@"Roslyn\3_cd68d03", @"Roslyn\roslyn7\src\Roslyn.sln", projects);
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj3_cd68d0323eb97f18c10281847c831f8e361506b9()
        {
            List<string> projects = new List<string>();
            projects.Add("Projcd68d0323eb97f18c10281847c831f8e361506b9");
            bool isValid = LocaleTestSolution("3_cd68d0323eb97f18c10281847c831f8e361506b9", @"..\..\TestProjects\Projects\Portable8\Projcd68d0323eb97f18c10281847c831f8e361506b9.sln", projects);
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Projf66696e()
        {
            List<string> projects = new List<string>();
            projects.Add("VBCSCompilerTests");
            bool isValid = LocaleTestSolution(@"Roslyn\f66696e", @"Roslyn\roslyn8\src\Roslyn.sln", projects);
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// Test Method Call To Identifier transformation
        /// </summary>
        [Test]
        public void Proj4b402939708adf35a7a5e12ffc99dc14cc1f4766()
        {
            List<string> projects = new List<string>();
            projects.Add("Proj2_4b402939708adf35a7a5e12ffc99dc14cc1f4766");
            bool isValid = LocaleTestSolution("2_4b402939708adf35a7a5e12ffc99dc14cc1f4766", @"..\..\TestProjects\Projects\CSharp2\Proj2_4b402939708adf35a7a5e12ffc99dc14cc1f4766.sln", projects);
            Assert.IsTrue(isValid);
        }


        [Test]
        public void Proj8ecd058()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpCodeAnalysis");
            bool isValid = LocaleTestSolution(@"Roslyn\cfd9b46", @"Roslyn\roslyn7\src\Roslyn.sln", projects);
            Assert.IsTrue(isValid);
        }
        

        [Test]
        public void Proj04d0604()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpFxCopRulesDiagnosticAnalyzers");
            projects.Add("FxCopRulesDiagnosticAnalyzers");
            bool isValid = LocaleTestSolution(@"Roslyn\04d0604", @"Roslyn\roslyn18\Src\Roslyn.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj318b2b0e476a122ebc033b13d41449ef1c814c1d()
        {
            List<string> projects = new List<string>();
            projects.Add("Proj318b2b0e476a122ebc033b13d41449ef1c814c1d");
            bool isValid = LocaleTestSolution("318b2b0e476a122ebc033b13d41449ef1c814c1d", @"..\..\TestProjects\Projects\Core2\Proj318b2b0e476a122ebc033b13d41449ef1c814c1d.sln", projects);
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// Test case for parameter to constant value
        /// </summary>
        [Test]
        public void Projc96d9ce()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpCodeAnalysis");
            bool isValid = LocaleTestSolution(@"Roslyn\c96d9ce", @"Roslyn\roslyn4\src\Roslyn.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proje7184bd18b5e2de6e1b71ba8f893c6d5e9a7bebd()
        {
            List<string> projects = new List<string>();
            projects.Add("Proje7184bd18b5e2de6e1b71ba8f893c6d5e9a7bebd");
            bool isValid = LocaleTestSolution("e7184bd18b5e2de6e1b71ba8f893c6d5e9a7bebd", @"..\..\TestProjects\Projects\Portable3\Portable\Proje7184bd18b5e2de6e1b71ba8f893c6d5e9a7bebd.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj1113fd3()
        {
            List<string> projects = new List<string>();
            projects.Add("CodeAnalysis");
            bool isValid = LocaleTestSolution(@"Roslyn\1113fd3", @"Roslyn\roslyn2\src\Roslyn.sln", projects);
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// Change Exception test
        /// </summary>
        [Test]
        public void Projcc3d32746f60ed5a9f3775ef0ec44424b03d65cf()
        {
            List<string> projects = new List<string>();
            projects.Add("Projcc3d32746f60ed5a9f3775ef0ec44424b03d65cf");
            bool isValid = LocaleTestSolution("cc3d32746f60ed5a9f3775ef0ec44424b03d65cf", @"..\..\TestProjects\Projects\Portable2\Portable\Projcc3d32746f60ed5a9f3775ef0ec44424b03d65cf.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj2_c96d9ce1b2626b464cf2746ca53cb338d7d2ce38()
        {
            List<string> projects = new List<string>();
            projects.Add("Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38");
            bool isValid = LocaleTestSolution("2_c96d9ce1b2626b464cf2746ca53cb338d7d2ce38", @"..\..\TestProjects\Projects\Portable5\Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj3_c96d9ce()
        {
            List<string> projects = new List<string>();
            projects.Add("CSharpCodeAnalysis");
            bool isValid = LocaleTestSolution(@"Roslyn\3_c96d9ce", @"Roslyn\roslyn4\src\Roslyn.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj4_c96d9ce1b2626b464cf2746ca53cb338d7d2ce38()
        {
            List<string> projects = new List<string>();
            projects.Add("Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38");
            bool isValid = LocaleTestSolution("4_c96d9ce1b2626b464cf2746ca53cb338d7d2ce38", @"..\..\TestProjects\Projects\Portable5\Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj5_c96d9ce1b2626b464cf2746ca53cb338d7d2ce38()
        {
            List<string> projects = new List<string>();
            projects.Add("Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38");
            bool isValid = LocaleTestSolution("5_c96d9ce1b2626b464cf2746ca53cb338d7d2ce38", @"..\..\TestProjects\Projects\Portable5\Projc96d9ce1b2626b464cf2746ca53cb338d7d2ce38.sln", projects);
            Assert.IsTrue(isValid);
        }



        //[Test]
        //public void ReturnToGetTest()
        //{
        //    bool isValid = LocaleTest(FilePath.RETURN_TO_GET_INPUT, FilePath.RETURN_TO_GET_OUTPUT_SELECTION, FilePath.MAIN_CLASS_RETURN_TO_GET_PATH);
        //    Assert.IsTrue(isValid);
        //}

        [Test]
        public void Proj49cdaceb2828acc1f50223826d478a00a80a59e2()
        {
            List<string> projects = new List<string>();
            projects.Add("Proj49cdaceb2828acc1f50223826d478a00a80a59e2");
            bool isValid = LocaleTestSolution("49cdaceb2828acc1f50223826d478a00a80a59e2", @"..\..\TestProjects\Projects\CSharp\Proj49cdaceb2828acc1f50223826d478a00a80a59e2.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Projcfd9b46()
        {
            List<string> projects = new List<string>();
            projects.Add("CodeAnalysis");
            bool isValid = LocaleTestSolution(@"Roslyn\cfd9b46", @"Roslyn\roslyn6\src\Roslyn.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj2_cfd9b46()
        {
            List<string> projects = new List<string>();
            projects.Add("CodeAnalysis");
            bool isValid = LocaleTestSolution(@"Roslyn\2_cfd9b46", @"Roslyn\roslyn6\src\Roslyn.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj7c885ca20209ca95cfec1ed5bfaf1d43db06be99()
        {
            List<string> projects = new List<string>();
            projects.Add("Proj7c885ca20209ca95cfec1ed5bfaf1d43db06be99");
            bool isValid = LocaleTestSolution("7c885ca20209ca95cfec1ed5bfaf1d43db06be99", @"..\..\TestProjects\Projects\Diagnostics\Proj7c885ca20209ca95cfec1ed5bfaf1d43db06be99.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj2_7c885ca20209ca95cfec1ed5bfaf1d43db06be99()
        {
            List<string> projects = new List<string>();
            projects.Add("Proj7c885ca20209ca95cfec1ed5bfaf1d43db06be99");
            bool isValid = LocaleTestSolution("2_7c885ca20209ca95cfec1ed5bfaf1d43db06be99", @"..\..\TestProjects\Projects\Diagnostics\Proj7c885ca20209ca95cfec1ed5bfaf1d43db06be99.sln", projects);
            Assert.IsTrue(isValid);
        }

        ///// <summary>
        ///// ASTManager to parent test case
        ///// </summary>
        //[Test]
        //public void ASTManagerToParentTest()
        //{
        //    bool isValid = LocationTest.LocaleTest(FilePath.ASTMANAGER_TO_PARENT_INPUT, FilePath.ASTMANAGER_TO_PARENT_OUTPUT_SELECTION, FilePath.MAIN_CLASS_ASTMANAGER_TO_PARENT_PATH);
        //    Assert.IsTrue(isValid);
        //}

        [Test]
        public void Proje28c81243206f1bb26b861ca0162678ce11b538cTest()
        {
            List<string> projects = new List<string>();
            projects.Add("Proje28c81243206f1bb26b861ca0162678ce11b538c");
            bool isValid = LocaleTestSolution("e28c81243206f1bb26b861ca0162678ce11b538c", @"..\..\TestProjects\Projects\Core\Proje28c81243206f1bb26b861ca0162678ce11b538c.sln", projects);
            Assert.IsTrue(isValid);
        }

        //Entity Framework tests
        [Test]
        public void Projd83cdfa88557bdf399a2f52dc79aad9d69bce007Test()
        {
            List<string> projects = new List<string>();
            projects.Add("Projd83cdfa88557bdf399a2f52dc79aad9d69bce007");
            bool isValid = LocaleTestSolution("d83cdfa88557bdf399a2f52dc79aad9d69bce007", @"..\..\TestProjects\Projects\UnitTests\Projd83cdfa88557bdf399a2f52dc79aad9d69bce007.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Projd8e9409b652bc0fb9aa222f0b36a66a8fd7cede6Test()
        {
            List<string> projects = new List<string>();
            projects.Add("Projd8e9409b652bc0fb9aa222f0b36a66a8fd7cede6");
            bool isValid = LocaleTestSolution("d8e9409b652bc0fb9aa222f0b36a66a8fd7cede6", @"..\..\TestProjects\Projects\EntityFramework\Projd8e9409b652bc0fb9aa222f0b36a66a8fd7cede6.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj14623da1e612d16c52b331bbd37ac1294c856658Test()
        {
            List<string> projects = new List<string>();
            projects.Add("Proj14623da1e612d16c52b331bbd37ac1294c856658");
            bool isValid = LocaleTestSolution("14623da1e612d16c52b331bbd37ac1294c856658", @"..\..\TestProjects\Projects\UnitTests2\Proj14623da1e612d16c52b331bbd37ac1294c856658.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj2bae908ad3f457eaee6cb2f7e2982b39580991d4Test()
        {
            List<string> projects = new List<string>();
            projects.Add("Proj2bae908ad3f457eaee6cb2f7e2982b39580991d4");
            bool isValid = LocaleTestSolution("2bae908ad3f457eaee6cb2f7e2982b39580991d4", @"..\..\TestProjects\Projects\EntityFramework3\Proj2bae908ad3f457eaee6cb2f7e2982b39580991d4.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj8d452499b23e250232406fa9c875973a054b17f9Test()
        {
            List<string> projects = new List<string>();
            projects.Add("Proj8d452499b23e250232406fa9c875973a054b17f9");
            bool isValid = LocaleTestSolution("8d452499b23e250232406fa9c875973a054b17f9", @"..\..\TestProjects\Projects\FunctionalTests\Proj8d452499b23e250232406fa9c875973a054b17f9.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj2_8d452499b23e250232406fa9c875973a054b17f9Test()
        {
            List<string> projects = new List<string>();
            projects.Add("Proj2_8d452499b23e250232406fa9c875973a054b17f9");
            bool isValid = LocaleTestSolution("2_8d452499b23e250232406fa9c875973a054b17f9", @"..\..\TestProjects\Projects\EntityFramework4\Proj2_8d452499b23e250232406fa9c875973a054b17f9.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj1571862()
        {
            List<string> projects = new List<string>();
            projects.Add("FunctionalTests");
            bool isValid = LocaleTestSolution(@"EntityFramewok\1571862", @"EntityFramework\entityframework4\EntityFramework.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj2_1571862()
        {
            List<string> projects = new List<string>();
            projects.Add("FunctionalTests");
            bool isValid = LocaleTestSolution(@"EntityFramewok\2_1571862", @"EntityFramework\entityframework4\EntityFramework.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj4_1571862()
        {
            List<string> projects = new List<string>();
            projects.Add("FunctionalTests");
            bool isValid = LocaleTestSolution(@"EntityFramewok\4_1571862", @"EntityFramework\entityframework4\EntityFramework.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj5_1571862()
        {
            List<string> projects = new List<string>();
            projects.Add("FunctionalTests");
            bool isValid = LocaleTestSolution(@"EntityFramewok\5_1571862", @"EntityFramework\entityframework4\EntityFramework.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj6_1571862()
        {
            List<string> projects = new List<string>();
            projects.Add("FunctionalTests.Transitional");
            bool isValid = LocaleTestSolution(@"EntityFramewok\6_1571862", @"EntityFramework\entityframework4\EntityFramework.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Projce1e333()
        {
            List<string> projects = new List<string>();
            projects.Add("EntityFramework");
            bool isValid = LocaleTestSolution(@"EntityFramewok\ce1e333", @"EntityFramework\entityframework5\EntityFramework.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj8b9180b()
        {
            List<string> projects = new List<string>();
            projects.Add("FunctionalTests");
            bool isValid = LocaleTestSolution(@"EntityFramewok\8b9180b", @"EntityFramework\entityframework6\EntityFramework.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj2_8b9180b()
        {
            List<string> projects = new List<string>();
            projects.Add("FunctionalTests");
            bool isValid = LocaleTestSolution(@"EntityFramewok\2_8b9180b", @"EntityFramework\entityframework6\EntityFramework.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj829dec5()
        {
            List<string> projects = new List<string>();
            projects.Add("EntityFramework");
            bool isValid = LocaleTestSolution(@"EntityFramewok\829dec5", @"EntityFramework\entityframework7\EntityFramework.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj326d525()
        {
            List<string> projects = new List<string>();
            projects.Add("FunctionalTests.Transitional");
            bool isValid = LocaleTestSolution(@"EntityFramewok\326d525", @"EntityFramework\entityframework9\EntityFramework.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proja883600()
        {
            List<string> projects = new List<string>();
            projects.Add("Core.Test");
            bool isValid = LocaleTestSolution(@"NuGet\a883600", @"NuGet\nuget4\NuGet.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj2_a883600()
        {
            List<string> projects = new List<string>();
            projects.Add("CommandLine");
            bool isValid = LocaleTestSolution(@"NuGet\2_a883600", @"NuGet\nuget4\NuGet.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj3_a883600()
        {
            List<string> projects = new List<string>();
            projects.Add("Core");
            bool isValid = LocaleTestSolution(@"NuGet\3_a883600", @"NuGet\nuget4\NuGet.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proja569c55()
        {
            List<string> projects = new List<string>();
            projects.Add("NuGet.Client");
            bool isValid = LocaleTestSolution(@"NuGet\a569c55", @"NuGet\nuget5\NuGet.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj2_a569c55()
        {
            List<string> projects = new List<string>();
            projects.Add("NuGet.Client");
            bool isValid = LocaleTestSolution(@"NuGet\2_a569c55", @"NuGet\nuget5\NuGet.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj4_a883600()
        {
            List<string> projects = new List<string>();
            projects.Add("NuGet.Client.VisualStudio.PowerShell");
            bool isValid = LocaleTestSolution(@"NuGet\4_a883600", @"NuGet\nuget4\NuGet.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj5_a883600()
        {
            List<string> projects = new List<string>();
            projects.Add("Core.Test");
            bool isValid = LocaleTestSolution(@"NuGet\5_a883600", @"NuGet\nuget4\NuGet.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj2dea84e()
        {
            List<string> projects = new List<string>();
            projects.Add("NuGet.Client.VisualStudio.PowerShell");
            bool isValid = LocaleTestSolution(@"NuGet\2dea84e", @"NuGet\nuget2\NuGet.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj8da9f0e()
        {
            List<string> projects = new List<string>();
            projects.Add("Core.Test");
            bool isValid = LocaleTestSolution(@"NuGet\8da9f0e", @"NuGet\nuget3\NuGet.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj2_8da9f0e()
        {
            List<string> projects = new List<string>();
            projects.Add("Core.Test");
            bool isValid = LocaleTestSolution(@"NuGet\2_8da9f0e", @"NuGet\nuget3\NuGet.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj3_8da9f0e()
        {
            List<string> projects = new List<string>();
            projects.Add("Test.Utility");
            bool isValid = LocaleTestSolution(@"NuGet\3_8da9f0e", @"NuGet\nuget3\NuGet.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj4_8da9f0e()
        {
            List<string> projects = new List<string>();
            projects.Add("VisualStudio.Test");
            bool isValid = LocaleTestSolution(@"NuGet\4_8da9f0e", @"NuGet\nuget3\NuGet.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj5_8da9f0e()
        {
            List<string> projects = new List<string>();
            projects.Add("FunctionalTests.Transitional");
            bool isValid = LocaleTestSolution(@"NuGet\5_8da9f0e", @"NuGet\nuget3\NuGet.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Projd9f64ea1704f8d99087ffdb646aa39003fc7c50aTest()
        {
            List<string> projects = new List<string>();
            projects.Add("Projd9f64ea1704f8d99087ffdb646aa39003fc7c50a");
            bool isValid = LocaleTestSolution("d9f64ea1704f8d99087ffdb646aa39003fc7c50a", @"..\..\TestProjects\Projects\Core4\Projd9f64ea1704f8d99087ffdb646aa39003fc7c50a.sln", projects);
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Proj6cf11e1d98dcc2c1c441e674af3cfc23fbd44d51Test()
        {
            List<string> projects = new List<string>();
            projects.Add("Proj6cf11e1d98dcc2c1c441e674af3cfc23fbd44d51");
            bool isValid = LocaleTestSolution("2_a569c556805ce4f788b9644c1cc9a578719df3ab", @"..\..\TestProjects\Projects\NuGet.Client2\Proj6cf11e1d98dcc2c1c441e674af3cfc23fbd44d51.sln", projects);
            Assert.IsTrue(isValid);
        }

        /// <summary>
        /// Locale test base method
        /// </summary>
        /// <param name="commit">commit id</param>
        /// <param name="solution">Solution</param>
        /// <param name="project">Project</param>
        /// <returns>True if locale passed</returns>
        public static bool LocaleTestSolution(string commit, string solution, List<string> project)
        {
            long millBefore = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            EditorController.ReInit();
            EditorController controller = EditorController.GetInstance();

            string expHome = Environment.GetEnvironmentVariable("EXP_HOME", EnvironmentVariableTarget.User);

            List<TRegion> selections = JsonUtil<List<TRegion>>.Read(expHome + @"commit\" + commit + @"\input_selection.json");

            controller.SelectedLocations = selections;
            controller.CurrentViewCodeBefore = FileUtil.ReadFile(selections.First().Path);
            string exactPath = Path.GetFullPath(selections.First().Path);

            controller.CurrentViewCodePath = exactPath;
            controller.SetProject(project);
            controller.SetSolution(expHome + solution);
            controller.SelectedLocations = selections;

            controller.Extract();

            controller.RetrieveLocations(controller.CurrentViewCodeBefore);

            if (File.Exists(expHome + @"commit\" + commit + @"\negatives.json"))
            {
                List<int> negatives = JsonUtil<List<int>>.Read(expHome + @"commit\" + commit + @"\negatives.json");
                List<TRegion> negativesRegions = new List<TRegion>();
                List<TRegion> positivesRegions = new List<TRegion>();
                //foreach (var negative in negatives)         
                for (int i = 0; i < controller.Locations.Count; i++)
                {
                    TRegion parent = new TRegion();
                    parent.Text = controller.Locations[i].SourceCode;
                    controller.Locations[i].Region.Parent = parent;
                    if (negatives.Contains(i))
                    {
                        negativesRegions.Add(controller.Locations[i].Region);
                    }
                    else
                    {
                        positivesRegions.Add(controller.Locations[i].Region);
                    }
                }
                //}

                //controller.Extract(controller.SelectedLocations, negativesRegions);
                controller.Extract(positivesRegions, negativesRegions);
            }
            controller.RetrieveLocations(controller.CurrentViewCodeBefore);

            List<Selection> locations = JsonUtil<List<Selection>>.Read(expHome + @"commit\" + commit + @"\found_locations.json");
            if (locations.Count != controller.Locations.Count) return false;

            bool passed = true;
            for (int i = 0; i < locations.Count; i++)
            {
                if (!Path.GetFullPath(locations[i].SourcePath).ToUpperInvariant().Equals(controller.Locations[i].SourceClass.ToUpperInvariant())) { passed = false; break; }

                if (locations[i].Start != controller.Locations[i].Region.Start || locations[i].Length != controller.Locations[i].Region.Length)
                {
                    passed = false;
                    break;
                }
            }
            long millAfer = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            long totalTime = (millAfer - millBefore);
            FileUtil.WriteToFile(expHome + @"commit\" + commit + @"\time.t", totalTime.ToString());
            return passed;
        }
    }
}

