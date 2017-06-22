/*using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.ProgramSynthesis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RefazerManager;
using Spg.ExampleRefactoring.Util;
using Spg.LocationRefactor.Location;
using Spg.LocationRefactor.TextRegion;
using Spg.LocationRefactor.Transform;
using Taramon.Exceller;

namespace UnitTests
{
    [TestClass]
    public class TestSuite
    {
        [TestMethod]
        public void E1()
        {
            var isCorrect = CompleteTestBase(@"EntityFramewok\1");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void E2()
        {
            var isCorrect = CompleteTestBase(@"EntityFramewok\3");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void E3()
        {
            var isCorrect = CompleteTestBase(@"EntityFramewok\4", solutionPath: @"EntityFramework\entityframework10\EntityFramework.sln");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void E4()
        {
            var isCorrect = CompleteTestBase(@"EntityFramewok\5");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void E5()
        {
            var isCorrect = CompleteTestBase(@"EntityFramewok\6");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void E6()
        {
            var isCorrect = CompleteTestBase(@"EntityFramewok\7");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void E7()
        {
            var isCorrect = CompleteTestBase(@"EntityFramewok\10");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void E8()
        {
            var isCorrect = CompleteTestBase(@"EntityFramewok\11", solutionPath: @"EntityFramework\entityframework7\EntityFramework.sln");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void E9()
        {
            var isCorrect = CompleteTestBase(@"EntityFramewok\12");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void E10()
        {
            var isCorrect = CompleteTestBase(@"EntityFramewok\13");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void E11()
        {
            var isCorrect = CompleteTestBase(@"EntityFramewok\14", solutionPath: @"EntityFramework\entityframework2\EntityFramework.sln");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void E12()
        {
            var isCorrect = CompleteTestBase(@"EntityFramewok\15");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void N13()
        {
            var isCorrect = CompleteTestBase(@"NuGet\16",
                kinds: new List<SyntaxKind> { SyntaxKind.PropertyDeclaration });
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void N14()
        {
            var isCorrect = CompleteTestBase(@"NuGet\17", solutionPath: @"NuGet\nuget3\NuGet.sln");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void N15()
        {
            var isCorrect = CompleteTestBase(@"NuGet\18");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void N16()
        {
            var isCorrect = CompleteTestBase(@"NuGet\19", solutionPath: @"NuGet\nuget4\NuGet.sln");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void N17()
        {
            var isCorrect = CompleteTestBase(@"NuGet\20");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void N18()
        {
            var isCorrect = CompleteTestBase(@"NuGet\21", solutionPath: @"NuGet\nuget2\NuGet.sln");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void N19()
        {
            var isCorrect = CompleteTestBase(@"NuGet\22");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void N20()
        {
            var isCorrect = CompleteTestBase(@"NuGet\23");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void N21()
        {
            var isCorrect = CompleteTestBase(@"Nuget\24", solutionPath: @"NuGet\nuget7\NuGet.sln");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void N22()
        {
            var isCorrect = CompleteTestBase(@"NuGet\25");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void N23()
        {
            var isCorrect = CompleteTestBase(@"NuGet\26");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void N24()
        {
            var isCorrect = CompleteTestBase(@"NuGet\27");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void N25()
        {
            var isCorrect = CompleteTestBase(@"NuGet\28");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void N26()
        {
            var isCorrect = CompleteTestBase(@"NuGet\29");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void N27()
        {
            var isCorrect = CompleteTestBase(@"NuGet\30");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void N28()
        {
            var isCorrect = CompleteTestBase(@"NuGet\31", solutionPath: @"NuGet\nuget6\NuGet.sln",
                kinds: new List<SyntaxKind> { SyntaxKind.AttributeList });
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void N29()
        {
            var isCorrect = CompleteTestBase(@"NuGet\32", solutionPath: @"NuGet\nuget10\NuGet.sln");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R30()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\33");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R31()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\34", solutionPath: @"Roslyn\roslyn7\src\Roslyn.sln", kinds: new List<SyntaxKind> { SyntaxKind.ClassDeclaration });
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R32()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\35", solutionPath: @"Roslyn\roslyn18\Src\Roslyn.sln", kinds: new List<SyntaxKind> { SyntaxKind.MethodDeclaration, SyntaxKind.PropertyDeclaration });
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R33()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\36");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R34()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\37");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R35()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\38", solutionPath: @"Roslyn\roslyn\src\Roslyn.sln");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R36()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\39");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R37()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\40");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R38()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\41");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R39()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\42");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R40()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\43");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R41()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\44", solutionPath: @"Roslyn\roslyn7\src\Roslyn.sln");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R42()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\45");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R43()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\46");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R44()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\47");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R45()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\48");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R46()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\49");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R47()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\50");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R48()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\51");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R49()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\52");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R50()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\53", kinds: new List<SyntaxKind> { SyntaxKind.ConstructorDeclaration });
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R51()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\54");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R52()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\55");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R53()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\56");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R54()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\57", solutionPath: @"Roslyn\roslyn7\src\Roslyn.sln",
                kinds: new List<SyntaxKind> { SyntaxKind.UsingDirective });
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R55()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\58");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R56()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\59");
            Assert.IsTrue(isCorrect);
        }

        static string GetTestDataFolder(string testDataLocation)
        {
            string startupPath = System.AppDomain.CurrentDomain.BaseDirectory;
            var pathItems = startupPath.Split(Path.DirectorySeparatorChar);
            string projectPath = String.Join(Path.DirectorySeparatorChar.ToString(),
                pathItems.Take(pathItems.Length - 4));
            string result = projectPath + testDataLocation;
            return result;
        }

        public static string GetBasePath()
        {
            string path = GetTestDataFolder(@"\");
            return path;
        }

        public static Grammar GetGrammar()
        {
            string path = GetBasePath();
            var grammar = Utils.LoadGrammar(path + @"ProgramSynthesis\grammar\Transformation.grammar");
            return grammar;
        }

        ///// <summary>
        ///// Complete test
        ///// </summary>
        ///// <param name="commit">Commit where the change occurs</param>
        ///// <returns>True if pass test</returns>
        //public static bool CompleteTestBase(string commit, string solutionPath = null, List<SyntaxKind> kinds = null)
        //{
        //    if (kinds == null)
        //    {
        //        kinds = new List<SyntaxKind> { SyntaxKind.MethodDeclaration, SyntaxKind.ConstructorDeclaration };
        //    }

        //    //Load grammar
        //    var grammar = GetGrammar();

        //    string expHome = Environment.GetEnvironmentVariable("EXP_HOME", EnvironmentVariableTarget.User);
        //    var codeTransformations = JsonUtil<List<CodeTransformation>>.Read(expHome + @"cprose\" + commit + @"\metadata\transformed_locations.json");

        //    List<TRegion> regions = codeTransformations.Select(entry => entry.Trans).ToList();
        //    var locations = codeTransformations.Select(entry => entry.Location).ToList();
        //    var globalTransformations = RegionManager.GetInstance().GroupTransformationsBySourcePath(codeTransformations);

        //    //Random number generator with a seed.
        //    int seed = 86028157;
        //    //int seed = 104395301;
        //    //int seed = 122949829;
        //    //int seed = 141650963;
        //    //int seed = 160481219;
        //    Random random = new Random(seed);
        //    var randomList = Enumerable.Range(0, locations.Count).OrderBy(o => random.Next()).ToList();
        //    var examples = randomList.GetRange(0, 2);

        //    //Execution
        //    TestHelper helper;
        //    string scriptsizes;
        //    while (true)
        //    {
        //        examples.Sort();
        //        helper = new TestHelper(grammar, regions, locations, globalTransformations, expHome, solutionPath, commit, kinds, seed);
        //        helper.Execute(examples);

        //        string codeFragments = "codefragments";
        //        var fragments = GetDataAndSaveToFile(commit, expHome, seed, codeFragments);
        //        var regionsFrags = ConvertFragmentsToRegions(fragments);
        //        JsonUtil<List<TRegion>>.Write(regionsFrags, expHome + @"cprose\" + commit + @"\metadata\transformed_locationsAll" + seed + ".json");

        //        string scriptsize = "scriptsize";
        //        scriptsizes = GetDataAndSaveToFile(commit, expHome, seed, scriptsize);

        //        string beforeafter = "beforeafter";
        //        beforeafter = GetDataAndSaveToFile(commit, expHome, seed, beforeafter);

        //        string programs = "programs";
        //        programs = GetDataAndSaveToFile(commit, expHome, seed, programs);

        //        var foundLocations = GetEditedLocations(fragments, locations);
        //        var firstProblematicLocation = GetFirstNotFound(foundLocations, locations, randomList);

        //        if (firstProblematicLocation == -1)
        //        {
        //            //Generate meta-data for BaselineBeforeAfterList on commit.
        //            var foundList = GetEditionInLocations(fragments, locations);
        //            JsonUtil<List<TRegion>>.Write(foundList, expHome + @"cprose\" + commit + @"\metadata_tool\transformed_locations"+ seed +".json");

        //            var beforeafterList = GetBeforeAfterData(beforeafter, locations);
        //            JsonUtil<List<Tuple<TRegion, string, string>>>.Write(beforeafterList, expHome + @"cprose\" + commit + @"\metadata_tool\before_after_locations" + seed + ".json");
        //            var beforeafterListAll = ConvertBeforeAfterToRegions(beforeafter);
        //            JsonUtil<List<Tuple<TRegion, string, string>>>.Write(beforeafterListAll, expHome + @"cprose\" + commit + @"\metadata_tool\before_after_locationsAll" + seed + ".json");
        //            //Comparing edited locations with baseline
        //            var baselineBeforeAfterList = JsonUtil<List<Tuple<TRegion, string, string>>>.Read(expHome + @"cprose\" + commit + @"\metadata\before_after_locations" + seed + ".json");
        //            var firstIncorrect = GetFirstIncorrect(beforeafterList, baselineBeforeAfterList, randomList, locations);
        //            if (firstIncorrect == -1)
        //            {
        //                GenerateDiffBeforeAfter(beforeafter, commit);
        //                break;
        //            }

        //            firstProblematicLocation = firstIncorrect;
        //        }
        //        if (examples.Contains(firstProblematicLocation))
        //        {
        //            GenerateDiffBeforeAfter(beforeafter, commit);
        //            throw new Exception("A transformation could not be learned using this examples.");
        //        }
        //        examples.Add(firstProblematicLocation);
        //    }

        //    var sizes = scriptsizes.Split(new[] { "\n" }, StringSplitOptions.None).Select(o => Int32.Parse(o));
        //    var mean = sizes.Average();
        //    //Execution end

        //    long totalTimeToLearn = helper.TotalTimeToLearn;
        //    long totalTimeToExecute = helper.TotalTimeToLearn;
        //    var transformed = helper.Transformed;
        //    var program = helper.Program;
        //    var dictionarySelection = helper.DictionarySelection;

        //    Log(commit, 
        //        totalTimeToLearn + totalTimeToExecute, 
        //        examples.Count,
        //        regions.Count,
        //        transformed.Count,
        //        dictionarySelection.Count,
        //        program.ToString(),
        //        totalTimeToLearn,
        //        totalTimeToExecute,
        //        mean);
        //    return true;
        //}

        /// <summary>
        /// Complete test
        /// </summary>
        /// <param name="commit">Commit where the change occurs</param>
        /// <returns>True if pass test</returns>
        public static bool CompleteTestBase(string commit, string solutionPath = null, List<SyntaxKind> kinds = null)
        {
            if (kinds == null)
            {
                kinds = new List<SyntaxKind> { SyntaxKind.MethodDeclaration, SyntaxKind.ConstructorDeclaration };
            }

            //Load grammar
            var grammar = GetGrammar();

            string expHome = Environment.GetEnvironmentVariable("EXP_HOME", EnvironmentVariableTarget.User);
            var codeTransformations = JsonUtil<List<CodeTransformation>>.Read(expHome + @"cprose\" + commit + @"\metadata\transformed_locations.json");

            List<TRegion> regions = codeTransformations.Select(entry => entry.Trans).ToList();
            var locations = codeTransformations.Select(entry => entry.Location).ToList();
            var globalTransformations = RegionManager.GetInstance().GroupTransformationsBySourcePath(codeTransformations);

            //Random number generator with a seed.
            int seed = 86028157;
            Random random = new Random(seed);
            var randomList = Enumerable.Range(0, locations.Count).OrderBy(o => random.Next()).ToList();
            var examples = randomList.GetRange(0, 2);

            //Execution
            TestHelper helper;
            examples.Sort();
            helper = new TestHelper(grammar, regions, locations, globalTransformations, expHome, solutionPath, commit, kinds, seed);
            var allPrograms = helper.LearnPrograms(examples);

            for (var i = 1; i <= allPrograms.Count; i++)
            {
                var p = allPrograms[i - 1];
                helper.Execute(examples, p);

                string codeFragments = "codefragments";
                var fragments = GetDataAndSaveToFile(commit, expHome, seed, codeFragments);
                var regionsFrags = ConvertFragmentsToRegions(fragments);
                JsonUtil<List<TRegion>>.Write(regionsFrags,
                    expHome + @"cprose\" + commit + @"\metadata\transformed_locationsAll" + seed + ".json");

                string beforeafter = "beforeafter";
                beforeafter = GetDataAndSaveToFile(commit, expHome, seed, beforeafter);

                var foundLocations = GetEditedLocations(fragments, locations);
                var firstProblematicLocation = GetFirstNotFound(foundLocations, locations, randomList);

                if (firstProblematicLocation == -1)
                {
                    //Generate meta-data for BaselineBeforeAfterList on commit.
                    var foundList = GetEditionInLocations(fragments, locations);
                    JsonUtil<List<TRegion>>.Write(foundList,
                        expHome + @"cprose\" + commit + @"\metadata_tool\transformed_locations" + seed + ".json");

                    var beforeafterList = GetBeforeAfterData(beforeafter, locations);
                    JsonUtil<List<Tuple<TRegion, string, string>>>.Write(beforeafterList,
                        expHome + @"cprose\" + commit + @"\metadata_tool\before_after_locations" + seed + ".json");
                    var beforeafterListAll = ConvertBeforeAfterToRegions(beforeafter);
                    JsonUtil<List<Tuple<TRegion, string, string>>>.Write(beforeafterListAll,
                        expHome + @"cprose\" + commit + @"\metadata_tool\before_after_locationsAll" + seed + ".json");
                    //Comparing edited locations with baseline
                    var baselineBeforeAfterList =
                        JsonUtil<List<Tuple<TRegion, string, string>>>.Read(expHome + @"cprose\" + commit +
                                                                            @"\metadata\before_after_locations" +
                                                                            seed + ".json");
                    var firstIncorrect = GetFirstIncorrect(beforeafterList, baselineBeforeAfterList, randomList,
                        locations);
                    if (firstIncorrect == -1)
                    {
                        LogProgram(commit, i, p.ToString(), true);
                    }
                    else
                    {
                        LogProgram(commit, i, p.ToString(), false);
                    }
                }
                else
                {
                    LogProgram(commit, i, p.ToString(), false);
                }
            }
            return true;
        }

        private static int GetFirstIncorrect(List<Tuple<TRegion, string, string>> toolBeforeAfterList, List<Tuple<TRegion, string, string>> baselineBeforeAfterList, List<int> randomList, List<CodeLocation> locations)
        {
            //computing list of locations that do not follow the baseline.
            var notFoundList = baselineBeforeAfterList.Where(o => !toolBeforeAfterList.Contains(o)).ToList();
            if (!notFoundList.Any()) return -1;
            //Computing example locations that do not follow the baseline.
            var incorrectLocationsList = locations.Where(o => notFoundList.Any(e => o.Region.IntersectWith(e.Item1)));
            //Computing the index of the locations that do not follow the baseline.
            var notFoundIndexList = incorrectLocationsList.Select(o => locations.IndexOf(o));
            //Getting the index of the location in the random list.
            var index = randomList.FindIndex(o => notFoundIndexList.Contains(o));
            var firstNotFound = randomList[index];
            return firstNotFound;
        }

        private static List<Tuple<TRegion, string, string>> GetBeforeAfterData(string beforeafter, List<CodeLocation> locations)
        {
            var regions = ConvertBeforeAfterToRegions(beforeafter);
            var dictionary = CreateDictionaryMatch(locations, regions.Select(o => o.Item1).ToList());

            var foundList = new List<TRegion>();
            dictionary.Values.ForEach(o => foundList.AddRange(o));
            foundList = foundList.Distinct().ToList();

            var regionsFound = regions.Where(o => foundList.Contains(o.Item1)).ToList();
            return regionsFound;
        }

        private static void GenerateDiffBeforeAfter(string beforeafter, string commit)
        {
            var regions = ConvertBeforeAfterToRegions(beforeafter);
            var groups = regions.GroupBy(o => o.Item3);
            var dic = groups.ToDictionary(group => group.Key, group => group.ToList());

            string expHome = Environment.GetEnvironmentVariable("EXP_HOME", EnvironmentVariableTarget.User);
            var transmedList = Transform(dic);
            string output = "";
            string errors = "";
            var pathoutput = Path.Combine(expHome, @"cprose\", commit + @"\metadata\diff.df");
            foreach (var ba in transmedList)
            {
                var className = ba.Item3.Split(@"\".ToCharArray()).Last();
                var pathb = Path.Combine(expHome, @"cprose\", commit + @"\metadata_tool\B" + className);
                var patha = Path.Combine(expHome, @"cprose\", commit + @"\metadata_tool\A" + className);
                FileUtil.WriteToFile(pathb, ba.Item1);
                FileUtil.WriteToFile(patha, ba.Item2);

                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.FileName = "cmd.exe";
                startInfo.Arguments = $"/C diff {pathb} {patha} -U5";
                process.StartInfo = startInfo;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.Start();

                output += process.StandardOutput.ReadToEnd();
                errors += process.StandardError.ReadToEnd();
            }

            if (!output.IsEmpty())
            {
                FileUtil.WriteToFile(pathoutput, output);
            }
            else if (!errors.IsEmpty())
            {
                FileUtil.WriteToFile(pathoutput, errors);
            }
            else
            {
                FileUtil.WriteToFile(pathoutput, "Occurs an error while running process.");
            }
        }

        public static List<Tuple<string, string, string>> Transform(Dictionary<string, List<Tuple<TRegion, string, string>>> transformations)
        {
            List<Tuple<string, string, string>> tRegions = new List<Tuple<string, string, string>>();
            foreach (var item in transformations)
            {
                int nextStart = 0;
                var filePath = item.Value.First().Item3;
                var source = File.ReadAllText(filePath);
                string sourceCode = source;
                foreach (var tregion in item.Value)
                {
                    TRegion region = tregion.Item1;
                    string transformation = tregion.Item2;

                    int start = nextStart + region.Start;
                    int end = start + region.Length;
                    var sourceCodeUntilStart = sourceCode.Substring(0, start);
                    var sourceCodeAfterSelection = sourceCode.Substring(end);
                    sourceCode = sourceCodeUntilStart + transformation + sourceCodeAfterSelection;

                    nextStart += transformation.Length - region.Length;
                }
                tRegions.Add(Tuple.Create(source, sourceCode, filePath));
            }
            return tRegions;
        }

        private static List<Tuple<TRegion, string, string>> ConvertBeforeAfterToRegions(string beforeafter)
        {
            var regions = new List<Tuple<TRegion, string, string>>();
            var enumerator = beforeafter.Split(new[] { "EndLine" }, StringSplitOptions.None).GetEnumerator();
            while (enumerator.MoveNext())
            {
                if ((enumerator.Current + "").Equals("")) break;

                int start = Int32.Parse(enumerator.Current + "");
                enumerator.MoveNext();
                int length = Int32.Parse(enumerator.Current + "");
                enumerator.MoveNext();
                string content = enumerator.Current + "";
                enumerator.MoveNext();
                string after = enumerator.Current + "";
                enumerator.MoveNext();
                string path = enumerator.Current + "";

                var region = new TRegion();
                region.Start = start;
                region.Length = length;
                region.Text = content;
                region.Path = path;

                regions.Add(Tuple.Create(region, after, path));
            }
            return regions;
        }

        private static string GetDataAndSaveToFile(string commit, string expHome, int seed, string fileName)
        {
            string file = expHome + fileName + ".txt";
            var fragments = FileUtil.ReadFile(file);
            FileUtil.WriteToFile(expHome + @"cprose\" + commit + @"\" + fileName + seed + ".res", fragments);
            File.Delete(file);
            return fragments;
        }

        private static int GetFirstNotFound(List<CodeLocation> foundLocations, List<CodeLocation> locations, List<int> result)
        {
            var notFoundList = locations.Where(o => !foundLocations.Contains(o)).ToList();
            if (!notFoundList.Any()) return -1;

            var notFoundIndexList = notFoundList.Select(o => locations.IndexOf(o));
            var index = result.FindIndex(o => notFoundIndexList.Contains(o));
            var firstNotFound = result[index];
            return firstNotFound;
        }

        private static List<CodeLocation> GetEditedLocations(string fragments, List<CodeLocation> locations)
        {
            var regions = ConvertFragmentsToRegions(fragments);
            var dictionary = CreateDictionaryMatch(locations, regions);

            var found = dictionary.Where(o => o.Value.Any()).Select(o => o.Key).ToList();
            return found;
        }

        private static List<TRegion> GetEditionInLocations(string fragments, List<CodeLocation> locations)
        {
            var regions = ConvertFragmentsToRegions(fragments);
            var dictionary = CreateDictionaryMatch(locations, regions);

            var foundList = new List<TRegion>();
            dictionary.Values.ForEach(o => foundList.AddRange(o));
            foundList = foundList.Distinct().ToList();
            return foundList;
        }

        private static Dictionary<CodeLocation, List<TRegion>> CreateDictionaryMatch(List<CodeLocation> locations, List<TRegion> regions)
        {
            var dictionary = new Dictionary<CodeLocation, List<TRegion>>();
            foreach (var v in locations)
            {
                var matches = regions.Where(o => v.Region.IntersectWith(o));
                if (!dictionary.ContainsKey(v))
                {
                    dictionary[v] = new List<TRegion>();
                }
                dictionary[v].AddRange(matches);
            }
            return dictionary;
        }

        private static List<TRegion> ConvertFragmentsToRegions(string fragments)
        {
            var regions = new List<TRegion>();
            var enumerator = fragments.Split(new[] { "EndLine" }, StringSplitOptions.None).GetEnumerator();
            while (enumerator.MoveNext())
            {
                if ((enumerator.Current + "").Equals("")) break;

                int start = Int32.Parse(enumerator.Current + "");
                enumerator.MoveNext();
                int length = Int32.Parse(enumerator.Current + "");
                enumerator.MoveNext();
                string content = enumerator.Current + "";
                enumerator.MoveNext();
                string path = enumerator.Current + "";

                var region = new TRegion();
                region.Start = start;
                region.Length = length;
                region.Text = content;
                region.Path = path;

                regions.Add(region);
            }
            return regions;
        }

        public static void Log(string commit, double time, int exTransformations, int locations, int acTrasnformation,
            int documents, string program, double timeToLearnEdit, double timeToTransformEdit, double mean)
        {
            string commitFirstLetter = commit.ElementAt(0).ToString();
            string commitId = commit.Substring(commit.IndexOf(@"\") + 1);

            commit = /*commitFirstLetter + "-" +*/ /*commitId;

            string path = TestUtil.LogPath;
            using (ExcelManager em = new ExcelManager())
            {

                em.Open(path);

                int empty;
                for (int i = 1; ; i++)
                {
                    if (em.GetValue("A" + i, Category.Formatted).ToString().Equals("") ||
                        em.GetValue("A" + i, Category.Formatted).ToString().Equals(commit))
                    {
                        empty = i;
                        break;
                    }
                }

                em.SetValue("A" + empty, commit);
                em.SetValue("B" + empty, time / 1000);
                em.SetValue("C" + empty, exTransformations);
                em.SetValue("D" + empty, locations);
                em.SetValue("E" + empty, acTrasnformation);
                em.SetValue("F" + empty, documents);
                em.SetValue("G" + empty, program);
                em.SetValue("H" + empty, timeToLearnEdit / 1000);
                em.SetValue("I" + empty, timeToTransformEdit / 1000);
                em.SetValue("J" + empty, mean);
                em.Save();
            }
        }


        public static void LogProgram(string commit, int programIndex, string program, bool status)
        {
            string commitFirstLetter = commit.ElementAt(0).ToString();
            string commitId = commit.Substring(commit.IndexOf(@"\") + 1);

            commit = commitId;

            string path = TestUtil.LogProgramStatus;
            using (ExcelManager em = new ExcelManager())
            {
                em.Open(path);

                int empty;
                for (int i = 1; ; i++)
                {
                    if (em.GetValue("A" + i, Category.Formatted).ToString().Equals("") ||
                        em.GetValue("A" + i, Category.Formatted).ToString().Equals(commit) && em.GetValue("B" + i, Category.Formatted).ToString().Equals(programIndex.ToString()))
                    {
                        empty = i;
                        break;
                    }
                }

                em.SetValue("A" + empty, commit);
                em.SetValue("B" + empty, programIndex);
                em.SetValue("C" + empty, program);
                em.SetValue("D" + empty, status);
                em.Save();
            }
        }
    }
}

*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.ProgramSynthesis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RefazerManager;
using Spg.ExampleRefactoring.Util;
using Spg.LocationRefactor.Location;
using Spg.LocationRefactor.TextRegion;
using Spg.LocationRefactor.Transform;
using Taramon.Exceller;
using UnitTests;

namespace RefazerUnitTests
{
    [TestClass]
    public class TestSuite
    {
        [TestMethod]
        public void E1()
        {
            var isCorrect = CompleteTestBase(@"EntityFramewok\1");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void E2()
        {
            var isCorrect = CompleteTestBase(@"EntityFramewok\2");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void E3()
        {
            var isCorrect = CompleteTestBase(@"EntityFramewok\3", solutionPath: @"EntityFramework\entityframework10\EntityFramework.sln");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void E4()
        {
            var isCorrect = CompleteTestBase(@"EntityFramewok\4");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void E5()
        {
            var isCorrect = CompleteTestBase(@"EntityFramewok\5");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void E6()
        {
            var isCorrect = CompleteTestBase(@"EntityFramewok\6");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void E7()
        {
            var isCorrect = CompleteTestBase(@"EntityFramewok\7");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void E8()
        {
            var isCorrect = CompleteTestBase(@"EntityFramewok\8", solutionPath: @"EntityFramework\entityframework7\EntityFramework.sln");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void E9()
        {
            var isCorrect = CompleteTestBase(@"EntityFramewok\9");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void E10()
        {
            var isCorrect = CompleteTestBase(@"EntityFramewok\10");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void E11()
        {
            var isCorrect = CompleteTestBase(@"EntityFramewok\11", solutionPath: @"EntityFramework\entityframework2\EntityFramework.sln");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void E12()
        {
            var isCorrect = CompleteTestBase(@"EntityFramewok\12");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void N13()
        {
            var isCorrect = CompleteTestBase(@"NuGet\13", kinds: new List<SyntaxKind> { SyntaxKind.PropertyDeclaration });
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void N14()
        {
            var isCorrect = CompleteTestBase(@"NuGet\14", solutionPath: @"NuGet\nuget3\NuGet.sln");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void N15()
        {
            var isCorrect = CompleteTestBase(@"NuGet\15");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void N16()
        {
            var isCorrect = CompleteTestBase(@"NuGet\16", solutionPath: @"NuGet\nuget4\NuGet.sln");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void N17()
        {
            var isCorrect = CompleteTestBase(@"NuGet\17");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void N18()
        {
            var isCorrect = CompleteTestBase(@"NuGet\18", solutionPath: @"NuGet\nuget2\NuGet.sln");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void N19()
        {
            var isCorrect = CompleteTestBase(@"NuGet\19");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void N20()
        {
            var isCorrect = CompleteTestBase(@"NuGet\20");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void N21()
        {
            var isCorrect = CompleteTestBase(@"Nuget\21", solutionPath: @"NuGet\nuget7\NuGet.sln");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void N22()
        {
            var isCorrect = CompleteTestBase(@"NuGet\22");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void N23()
        {
            var isCorrect = CompleteTestBase(@"NuGet\23");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void N24()
        {
            var isCorrect = CompleteTestBase(@"NuGet\24");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void N25()
        {
            var isCorrect = CompleteTestBase(@"NuGet\25");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void N26()
        {
            var isCorrect = CompleteTestBase(@"NuGet\26");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void N27()
        {
            var isCorrect = CompleteTestBase(@"NuGet\27");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void N28()
        {
            var isCorrect = CompleteTestBase(@"NuGet\28", solutionPath: @"NuGet\nuget6\NuGet.sln",
                kinds: new List<SyntaxKind> { SyntaxKind.AttributeList });
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void N29()
        {
            var isCorrect = CompleteTestBase(@"NuGet\29", solutionPath: @"NuGet\nuget10\NuGet.sln");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R30()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\30");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R31()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\31", solutionPath: @"Roslyn\roslyn7\src\Roslyn.sln", kinds: new List<SyntaxKind> { SyntaxKind.ClassDeclaration });
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R32()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\32", solutionPath: @"Roslyn\roslyn18\Src\Roslyn.sln", kinds: new List<SyntaxKind> { SyntaxKind.MethodDeclaration, SyntaxKind.PropertyDeclaration });
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R33()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\33");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R34()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\34");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R35()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\35", solutionPath: @"Roslyn\roslyn\src\Roslyn.sln");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R36()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\36");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R37()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\37");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R38()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\38");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R39()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\39");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R40()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\40");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R41()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\41", solutionPath: @"Roslyn\roslyn7\src\Roslyn.sln");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R42()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\42");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R43()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\43");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R44()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\44");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R45()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\45");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R46()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\46");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R47()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\47");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R48()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\48");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R49()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\49");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R50()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\50", kinds: new List<SyntaxKind> { SyntaxKind.ConstructorDeclaration });
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R51()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\51");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R52()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\52");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R53()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\53");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R54()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\54", solutionPath: @"Roslyn\roslyn7\src\Roslyn.sln",
                kinds: new List<SyntaxKind> { SyntaxKind.UsingDirective });
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R55()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\55");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R56()
        {
            var isCorrect = CompleteTestBase(@"Roslyn\56");
            Assert.IsTrue(isCorrect);
        }

        static string GetTestDataFolder(string testDataLocation)
        {
            string startupPath = System.AppDomain.CurrentDomain.BaseDirectory;
            var pathItems = startupPath.Split(Path.DirectorySeparatorChar);
            string projectPath = String.Join(Path.DirectorySeparatorChar.ToString(),
                pathItems.Take(pathItems.Length - 4));
            string result = projectPath + testDataLocation;
            return result;
        }

        public static string GetBasePath()
        {
            string path = GetTestDataFolder(@"\");
            return path;
        }

        public static Grammar GetGrammar()
        {
            string path = GetBasePath();
            var grammar = Utils.LoadGrammar(path + @"ProgramSynthesis\grammar\Transformation.grammar");
            return grammar;
        }

        /// <summary>
        /// Complete test
        /// </summary>
        /// <param name="commit">Commit where the change occurs</param>
        /// <returns>True if pass test</returns>
        public static bool CompleteTestBase(string commit, string solutionPath = null, List<SyntaxKind> kinds = null)
        {
            if (kinds == null)
            {
                kinds = new List<SyntaxKind> { SyntaxKind.MethodDeclaration, SyntaxKind.ConstructorDeclaration };
            }

            //Load grammar
            var grammar = GetGrammar();

            string expHome = Environment.GetEnvironmentVariable("EXP_HOME", EnvironmentVariableTarget.User);
            var codeTransformations = JsonUtil<List<CodeTransformation>>.Read(expHome + @"cprose\" + commit + @"\metadata\transformed_locations.json");

            List<TRegion> regions = codeTransformations.Select(entry => entry.Trans).ToList();
            var locations = codeTransformations.Select(entry => entry.Location).ToList();
            var globalTransformations = RegionManager.GetInstance().GroupTransformationsBySourcePath(codeTransformations);

            //Random number generator with a seed.
            int seed = 86028157;
            //int seed = 104395301;
            //int seed = 122949829;
            //int seed = 141650963;
            //int seed = 160481219;
            Random random = new Random(seed);
            var randomList = Enumerable.Range(0, locations.Count).OrderBy(o => random.Next()).ToList();
            var examples = randomList.GetRange(0, 2);  

            //Execution
            TestHelper helper;
            string scriptsizes;
            while (true)
            {
                examples.Sort();
                helper = new TestHelper(grammar, regions, locations, globalTransformations, expHome, solutionPath, commit, kinds, seed);
                helper.Execute(examples);

                string codeFragments = "codefragments";
                var fragments = GetDataAndSaveToFile(commit, expHome, seed, codeFragments);
                var regionsFrags = ConvertFragmentsToRegions(fragments);
                JsonUtil<List<TRegion>>.Write(regionsFrags, expHome + @"cprose\" + commit + @"\metadata\transformed_locationsAll" + seed + ".json");

                string scriptsize = "scriptsize";
                scriptsizes = GetDataAndSaveToFile(commit, expHome, seed, scriptsize);

                string beforeafter = "beforeafter";
                beforeafter = GetDataAndSaveToFile(commit, expHome, seed, beforeafter);

                string programs = "programs";
                programs = GetDataAndSaveToFile(commit, expHome, seed, programs);

                var foundLocations = GetEditedLocations(fragments, locations);
                var firstProblematicLocation = GetFirstNotFound(foundLocations, locations, randomList);

                if (firstProblematicLocation == -1)
                {
                    //Generate meta-data for BaselineBeforeAfterList on commit.
                    var foundList = GetEditionInLocations(fragments, locations);
                    JsonUtil<List<TRegion>>.Write(foundList, expHome + @"cprose\" + commit + @"\metadata_tool\transformed_locations" + seed + ".json");

                    var beforeafterList = GetBeforeAfterData(beforeafter, locations);
                    JsonUtil<List<Tuple<TRegion, string, string>>>.Write(beforeafterList, expHome + @"cprose\" + commit + @"\metadata_tool\before_after_locations" + seed + ".json");
                    var beforeafterListAll = ConvertBeforeAfterToRegions(beforeafter);
                    JsonUtil<List<Tuple<TRegion, string, string>>>.Write(beforeafterListAll, expHome + @"cprose\" + commit + @"\metadata_tool\before_after_locationsAll" + seed + ".json");
                    //Comparing edited locations with baseline
                    var baselineBeforeAfterList = JsonUtil<List<Tuple<TRegion, string, string>>>.Read(expHome + @"cprose\" + commit + @"\metadata\before_after_locations" + seed + ".json");
                    var firstIncorrect = GetFirstIncorrect(beforeafterList, baselineBeforeAfterList, randomList, locations);
                    if (firstIncorrect == -1)
                    {
                        GenerateDiffBeforeAfter(beforeafter, commit);
                        break;
                    }

                    firstProblematicLocation = firstIncorrect;
                }
                if (examples.Contains(firstProblematicLocation))
                {
                    GenerateDiffBeforeAfter(beforeafter, commit);
                    throw new Exception("A transformation could not be learned using this examples.");
                }
                examples.Add(firstProblematicLocation);
            }

            var sizes = scriptsizes.Split(new[] { "\n" }, StringSplitOptions.None).Select(o => Int32.Parse(o));
            var mean = sizes.Average();
            //Execution end

            long totalTimeToLearn = helper.TotalTimeToLearn;
            long totalTimeToExecute = helper.TotalTimeToLearn;
            var transformed = helper.Transformed;
            var program = helper.Program;
            var dictionarySelection = helper.DictionarySelection;

            Log(commit,
                totalTimeToLearn + totalTimeToExecute,
                examples.Count,
                regions.Count,
                transformed.Count,
                dictionarySelection.Count,
                program.ToString(),
                totalTimeToLearn,
                totalTimeToExecute,
                mean);
            return true;
        }

        private static int GetFirstIncorrect(List<Tuple<TRegion, string, string>> toolBeforeAfterList, List<Tuple<TRegion, string, string>> baselineBeforeAfterList, List<int> randomList, List<CodeLocation> locations)
        {
            //computing list of locations that do not follow the baseline.
            var notFoundList = baselineBeforeAfterList.Where(o => !toolBeforeAfterList.Contains(o)).ToList();
            if (!notFoundList.Any()) return -1;
            //Computing example locations that do not follow the baseline.
            var incorrectLocationsList = locations.Where(o => notFoundList.Any(e => o.Region.IntersectWith(e.Item1)));
            //Computing the index of the locations that do not follow the baseline.
            var notFoundIndexList = incorrectLocationsList.Select(o => locations.IndexOf(o));
            //Getting the index of the location in the random list.
            var index = randomList.FindIndex(o => notFoundIndexList.Contains(o));
            var firstNotFound = randomList[index];
            return firstNotFound;
        }

        private static List<Tuple<TRegion, string, string>> GetBeforeAfterData(string beforeafter, List<CodeLocation> locations)
        {
            var regions = ConvertBeforeAfterToRegions(beforeafter);
            var dictionary = CreateDictionaryMatch(locations, regions.Select(o => o.Item1).ToList());

            var foundList = new List<TRegion>();
            dictionary.Values.ForEach(o => foundList.AddRange(o));
            foundList = foundList.Distinct().ToList();

            var regionsFound = regions.Where(o => foundList.Contains(o.Item1)).ToList();
            return regionsFound;
        }

        private static void GenerateDiffBeforeAfter(string beforeafter, string commit)
        {
            var regions = ConvertBeforeAfterToRegions(beforeafter);
            var groups = regions.GroupBy(o => o.Item3);
            var dic = groups.ToDictionary(group => group.Key, group => group.ToList());

            string expHome = Environment.GetEnvironmentVariable("EXP_HOME", EnvironmentVariableTarget.User);
            var transmedList = Transform(dic);
            string output = "";
            string errors = "";
            var pathoutput = Path.Combine(expHome, @"cprose\", commit + @"\metadata\diff.df");
            foreach (var ba in transmedList)
            {
                var className = ba.Item3.Split(@"\".ToCharArray()).Last();
                var pathb = Path.Combine(expHome, @"cprose\", commit + @"\metadata_tool\B" + className);
                var patha = Path.Combine(expHome, @"cprose\", commit + @"\metadata_tool\A" + className);
                FileUtil.WriteToFile(pathb, ba.Item1);
                FileUtil.WriteToFile(patha, ba.Item2);

                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.FileName = "cmd.exe";
                startInfo.Arguments = $"/C diff {pathb} {patha} -U5";
                process.StartInfo = startInfo;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.Start();

                output += process.StandardOutput.ReadToEnd();
                errors += process.StandardError.ReadToEnd();
            }

            if (!output.IsEmpty())
            {
                FileUtil.WriteToFile(pathoutput, output);
            }
            else if (!errors.IsEmpty())
            {
                FileUtil.WriteToFile(pathoutput, errors);
            }
            else
            {
                FileUtil.WriteToFile(pathoutput, "Occurs an error while running process.");
            }
        }

        public static List<Tuple<string, string, string>> Transform(Dictionary<string, List<Tuple<TRegion, string, string>>> transformations)
        {
            List<Tuple<string, string, string>> tRegions = new List<Tuple<string, string, string>>();
            foreach (var item in transformations)
            {
                int nextStart = 0;
                var filePath = item.Value.First().Item3;
                var source = FileUtil.ReadFile(filePath);
                string sourceCode = source;
                foreach (var tregion in item.Value)
                {
                    TRegion region = tregion.Item1;
                    string transformation = tregion.Item2;

                    int start = nextStart + region.Start;
                    int end = start + region.Length;
                    var sourceCodeUntilStart = sourceCode.Substring(0, start);
                    var sourceCodeAfterSelection = sourceCode.Substring(end);
                    sourceCode = sourceCodeUntilStart + transformation + sourceCodeAfterSelection;

                    nextStart += transformation.Length - region.Length;
                }
                tRegions.Add(Tuple.Create(source, sourceCode, filePath));
            }
            return tRegions;
        }

        private static List<Tuple<TRegion, string, string>> ConvertBeforeAfterToRegions(string beforeafter)
        {
            var regions = new List<Tuple<TRegion, string, string>>();
            var enumerator = beforeafter.Split(new[] { "EndLine" }, StringSplitOptions.None).GetEnumerator();
            while (enumerator.MoveNext())
            {
                if ((enumerator.Current + "").Equals("")) break;

                int start = Int32.Parse(enumerator.Current + "");
                enumerator.MoveNext();
                int length = Int32.Parse(enumerator.Current + "");
                enumerator.MoveNext();
                string content = enumerator.Current + "";
                enumerator.MoveNext();
                string after = enumerator.Current + "";
                enumerator.MoveNext();
                string path = enumerator.Current + "";

                var region = new TRegion();
                region.Start = start;
                region.Length = length;
                region.Text = content;
                region.Path = path;

                regions.Add(Tuple.Create(region, after, path));
            }
            return regions;
        }

        private static string GetDataAndSaveToFile(string commit, string expHome, int seed, string fileName)
        {
            string file = expHome + fileName + ".txt";
            var fragments = FileUtil.ReadFile(file);
            FileUtil.WriteToFile(expHome + @"cprose\" + commit + @"\" + fileName + seed + ".res", fragments);
            File.Delete(file);
            return fragments;
        }

        private static int GetFirstNotFound(List<CodeLocation> foundLocations, List<CodeLocation> locations, List<int> result)
        {
            var notFoundList = locations.Where(o => !foundLocations.Contains(o)).ToList();
            if (!notFoundList.Any()) return -1;

            var notFoundIndexList = notFoundList.Select(o => locations.IndexOf(o));
            var index = result.FindIndex(o => notFoundIndexList.Contains(o));
            var firstNotFound = result[index];
            return firstNotFound;
        }

        private static List<CodeLocation> GetEditedLocations(string fragments, List<CodeLocation> locations)
        {
            var regions = ConvertFragmentsToRegions(fragments);
            var dictionary = CreateDictionaryMatch(locations, regions);

            var found = dictionary.Where(o => o.Value.Any()).Select(o => o.Key).ToList();
            return found;
        }

        private static List<TRegion> GetEditionInLocations(string fragments, List<CodeLocation> locations)
        {
            var regions = ConvertFragmentsToRegions(fragments);
            var dictionary = CreateDictionaryMatch(locations, regions);

            var foundList = new List<TRegion>();
            dictionary.Values.ForEach(o => foundList.AddRange(o));
            foundList = foundList.Distinct().ToList();
            return foundList;
        }

        private static Dictionary<CodeLocation, List<TRegion>> CreateDictionaryMatch(List<CodeLocation> locations, List<TRegion> regions)
        {
            var dictionary = new Dictionary<CodeLocation, List<TRegion>>();
            foreach (var v in locations)
            {
                var matches = regions.Where(o => v.Region.IntersectWith(o));
                if (!dictionary.ContainsKey(v))
                {
                    dictionary[v] = new List<TRegion>();
                }
                dictionary[v].AddRange(matches);
            }
            return dictionary;
        }

        private static List<TRegion> ConvertFragmentsToRegions(string fragments)
        {
            var regions = new List<TRegion>();
            var enumerator = fragments.Split(new[] { "EndLine" }, StringSplitOptions.None).GetEnumerator();
            while (enumerator.MoveNext())
            {
                if ((enumerator.Current + "").Equals("")) break;

                int start = Int32.Parse(enumerator.Current + "");
                enumerator.MoveNext();
                int length = Int32.Parse(enumerator.Current + "");
                enumerator.MoveNext();
                string content = enumerator.Current + "";
                enumerator.MoveNext();
                string path = enumerator.Current + "";

                var region = new TRegion();
                region.Start = start;
                region.Length = length;
                region.Text = content;
                region.Path = path;

                regions.Add(region);
            }
            return regions;
        }

        public static void Log(string commit, double time, int exTransformations, int locations, int acTrasnformation,
            int documents, string program, double timeToLearnEdit, double timeToTransformEdit, double mean)
        {
            string commitFirstLetter = commit.ElementAt(0).ToString();
            string commitId = commit.Substring(commit.IndexOf(@"\") + 1);

            commit = /*commitFirstLetter + "-" +*/ commitId;

            string path = TestUtil.LogPath;
            using (ExcelManager em = new ExcelManager())
            {

                em.Open(path);

                int empty;
                for (int i = 1; ; i++)
                {
                    if (em.GetValue("A" + i, Category.Formatted).ToString().Equals("") ||
                        em.GetValue("A" + i, Category.Formatted).ToString().Equals(commit))
                    {
                        empty = i;
                        break;
                    }
                }

                em.SetValue("A" + empty, commit);
                em.SetValue("B" + empty, time / 1000);
                em.SetValue("C" + empty, exTransformations);
                em.SetValue("D" + empty, locations);
                em.SetValue("E" + empty, acTrasnformation);
                em.SetValue("F" + empty, documents);
                em.SetValue("G" + empty, program);
                em.SetValue("H" + empty, timeToLearnEdit / 1000);
                em.SetValue("I" + empty, timeToTransformEdit / 1000);
                em.SetValue("J" + empty, mean);
                em.Save();
            }
        }


        public static void LogProgram(string commit, int programIndex, string program, bool status)
        {
            string commitFirstLetter = commit.ElementAt(0).ToString();
            string commitId = commit.Substring(commit.IndexOf(@"\") + 1);

            commit = commitId;

            string path = TestUtil.LogProgramStatus;
            using (ExcelManager em = new ExcelManager())
            {
                em.Open(path);

                int empty;
                for (int i = 1; ; i++)
                {
                    if (em.GetValue("A" + i, Category.Formatted).ToString().Equals("") ||
                        em.GetValue("A" + i, Category.Formatted).ToString().Equals(commit) && em.GetValue("B" + i, Category.Formatted).ToString().Equals(programIndex.ToString()))
                    {
                        empty = i;
                        break;
                    }
                }

                em.SetValue("A" + empty, commit);
                em.SetValue("B" + empty, programIndex);
                em.SetValue("C" + empty, program);
                em.SetValue("D" + empty, status);
                em.Save();
            }
        }
    }
}