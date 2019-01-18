using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.ProgramSynthesis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RefazerFunctions.Spg.Config;
using RefazerManager;
using RefazerObject.Constants;
using RefazerObject.Location;
using RefazerObject.Region;
using Spg.LocationRefactor.Location;
using Taramon.Exceller;
using TreeEdit.Spg.Log;
using TreeEdit.Spg.LogInfo;
using TreeEdit.Spg.Transform;
using TreeElement;
using System.Windows.Forms;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using CsvHelper;

namespace RefazerUnitTests
{
    [TestClass]
    public class TestSuite
    {
        [TestMethod]
        public void E1()
        {
            var isCorrect = CompleteTestBase(@"E1\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void E2()
        {
            var isCorrect = CompleteTestBase(@"E2\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void E3()
        {
            var isCorrect = CompleteTestBase(@"E3\", fileFolder: @"E3\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void E4()
        {
            var isCorrect = CompleteTestBase(@"E4\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void E5()
        {
            var isCorrect = CompleteTestBase(@"E5\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void E6()
        {
            var isCorrect = CompleteTestBase(@"E6\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void E7()
        {
            var isCorrect = CompleteTestBase(@"E7\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void E8()
        {
            var isCorrect = CompleteTestBase(@"E8\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void E9()
        {
            var isCorrect = CompleteTestBase(@"E9\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void E10()
        {
            var isCorrect = CompleteTestBase(@"E10\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void E11()
        {
            var isCorrect = CompleteTestBase(@"E11\", fileFolder: @"E11\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void E12()
        {
            var isCorrect = CompleteTestBase(@"E12\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void N13()
        {
            var isCorrect = CompleteTestBase(@"N13\", kinds: new List<SyntaxKind> { SyntaxKind.PropertyDeclaration });
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void N14()
        {
            var isCorrect = CompleteTestBase(@"N14\", fileFolder: @"N14\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void N15()
        {
            var isCorrect = CompleteTestBase(@"N15\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void N16()
        {
            var isCorrect = CompleteTestBase(@"N16\", fileFolder: @"N16\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void N17()
        {
            var isCorrect = CompleteTestBase(@"N17\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void N18()
        {
            var isCorrect = CompleteTestBase(@"N18\", fileFolder: @"N18\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void N19()
        {
            var isCorrect = CompleteTestBase(@"N19\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void N20()
        {
            var isCorrect = CompleteTestBase(@"N20\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void N21()
        {
            var isCorrect = CompleteTestBase(@"N21\", fileFolder: @"N21\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void N22()
        {
            var isCorrect = CompleteTestBase(@"N22\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void N23()
        {
            var isCorrect = CompleteTestBase(@"N23\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void N24()
        {
            var isCorrect = CompleteTestBase(@"N24\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void N25()
        {
            var isCorrect = CompleteTestBase(@"N25\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void N26()
        {
            var isCorrect = CompleteTestBase(@"N26\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void N27()
        {
            var isCorrect = CompleteTestBase(@"N27\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void N28()
        {
            var isCorrect = CompleteTestBase(@"N28\", fileFolder: @"N28\",
                kinds: new List<SyntaxKind> { SyntaxKind.AttributeList });
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void N29()
        {
            var isCorrect = CompleteTestBase(@"N29\", fileFolder: @"N29\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R30()
        {
            var isCorrect = CompleteTestBase(@"R30\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R31()
        {
            var isCorrect = CompleteTestBase(@"R31\", fileFolder: @"R31\", kinds: new List<SyntaxKind> { SyntaxKind.ClassDeclaration });
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R32()
        {
            var isCorrect = CompleteTestBase(@"R32\", fileFolder: @"R32\", kinds: new List<SyntaxKind> { SyntaxKind.MethodDeclaration, SyntaxKind.PropertyDeclaration });
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R33()
        {
            var isCorrect = CompleteTestBase(@"R33\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R34()
        {
            var isCorrect = CompleteTestBase(@"R34\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R35()
        {
            var isCorrect = CompleteTestBase(@"R35\", fileFolder: @"R35\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R36()
        {
            var isCorrect = CompleteTestBase(@"R36\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R37()
        {
            var isCorrect = CompleteTestBase(@"R37\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R38()
        {
            var isCorrect = CompleteTestBase(@"R38\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R39()
        {
            var isCorrect = CompleteTestBase(@"R39\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R40()
        {
            var isCorrect = CompleteTestBase(@"R40\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R41()
        {
            var isCorrect = CompleteTestBase(@"R41\", fileFolder: @"R41\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R42()
        {
            var isCorrect = CompleteTestBase(@"R42\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R43()
        {
            var isCorrect = CompleteTestBase(@"R43\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R44()
        {
            var isCorrect = CompleteTestBase(@"R44\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R45()
        {
            var isCorrect = CompleteTestBase(@"R45\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R46()
        {
            var isCorrect = CompleteTestBase(@"R46\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R47()
        {
            var isCorrect = CompleteTestBase(@"R47\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R48()
        {
            var isCorrect = CompleteTestBase(@"R48\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R49()
        {
            var isCorrect = CompleteTestBase(@"R49\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R50()
        {
            var isCorrect = CompleteTestBase(@"R50\", kinds: new List<SyntaxKind> { SyntaxKind.ConstructorDeclaration });
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R51()
        {
            var isCorrect = CompleteTestBase(@"R51\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R52()
        {
            var isCorrect = CompleteTestBase(@"R52\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R53()
        {
            var isCorrect = CompleteTestBase(@"R53\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R54()
        {
            var isCorrect = CompleteTestBase(@"R54\", fileFolder: @"R54\",
                kinds: new List<SyntaxKind> { SyntaxKind.UsingDirective });
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R55()
        {
            var isCorrect = CompleteTestBase(@"R55\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void R56()
        {
            var isCorrect = CompleteTestBase(@"R56\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void NJ025()
        {
            var isCorrect = CompleteTestBase(@"NJ025\", kinds: new List<SyntaxKind> { SyntaxKind.PropertyDeclaration });
            Assert.IsTrue(isCorrect);

        }

        [TestMethod]
        public void NJ023()
        {
            var isCorrect = CompleteTestBase(@"NJ023\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void NJ059()
        {

            var isCorrect = CompleteTestBase(@"NJ059\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void NJ224()
        {

            var isCorrect = CompleteTestBase(@"NJ224\");
            Assert.IsTrue(isCorrect);
        }


        [TestMethod]
        public void NJ225()
        {

            var isCorrect = CompleteTestBase(@"NJ225\");
            Assert.IsTrue(isCorrect);
        }


        [TestMethod]
        public void NJ234()
        {

            var isCorrect = CompleteTestBase(@"NJ234\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void NJ236()
        {
            var isCorrect = CompleteTestBase(@"NJ236\");
            Assert.IsTrue(isCorrect);
        }


        [TestMethod]
        public void NJ241()
        {

            var isCorrect = CompleteTestBase(@"NJ241\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void NJ242()
        {
            var isCorrect = CompleteTestBase(@"NJ242\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void NJ844()
        {
            var isCorrect = CompleteTestBase(@"NJ844\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void NJ1428()
        {
            var isCorrect = CompleteTestBase(@"NJ1428\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void NJ1479()
        {
            var isCorrect = CompleteTestBase(@"NJ1479\", fileFolder: @"NJ1479\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void NJ1491()
        {
            var isCorrect = CompleteTestBase(@"NJ1491\", fileFolder: @"NJ1491\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void S002()
        {
            var isCorrect = CompleteTestBase(@"S002\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void S007()
        {
            var isCorrect = CompleteTestBase(@"S007\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void S043()
        {
            var isCorrect = CompleteTestBase(@"S043\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void S058()
        {
            var isCorrect = CompleteTestBase(@"S058\");
            Assert.IsTrue(isCorrect);
        }


        [TestMethod]
        public void S044()
        {
            var isCorrect = CompleteTestBase(@"S044\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void S224()
        {
            var isCorrect = CompleteTestBase(@"S224\");
            Assert.IsTrue(isCorrect);
        }
        [TestMethod]
        public void S236()
        {
            var isCorrect = CompleteTestBase(@"S236\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void S431()
        {
            var isCorrect = CompleteTestBase(@"S431\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void S564()
        {
            var isCorrect = CompleteTestBase(@"S564\", fileFolder: @"S564\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void S571()
        {
            var isCorrect = CompleteTestBase(@"S571\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void S583()
        {
            var isCorrect = CompleteTestBase(@"S583\", fileFolder: @"S583\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void S592()
        {
            var isCorrect = CompleteTestBase(@"S592\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void S761()
        {
            var isCorrect = CompleteTestBase(@"S761\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void S863()
        {
            var isCorrect = CompleteTestBase(@"S863\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void S897()
        {
            var isCorrect = CompleteTestBase(@"S897\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void S1021()
        {
            var isCorrect = CompleteTestBase(@"S1021\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void S1088()
        {
            var isCorrect = CompleteTestBase(@"S1088\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void S1549()
        {
            var isCorrect = CompleteTestBase(@"S1549\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void S1580()
        {
            var isCorrect = CompleteTestBase(@"S1580\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void S1810()
        {
            var isCorrect = CompleteTestBase(@"S1810\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void S2076()
        {
            var isCorrect = CompleteTestBase(@"S2076\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void S3778()
        {
            var isCorrect = CompleteTestBase(@"S3778\");
            Assert.IsTrue(isCorrect);
        }

        [TestMethod]
        public void S3791()
        {
            var isCorrect = CompleteTestBase(@"S3791\");
            Assert.IsTrue(isCorrect);
        }

        public static List<Region> GetTransformedLocations(string expHome)
        {
            var regionsFragments = CodeFragmentsInfo.GetInstance().Locations.Select(o => new Region { Start = o.Span.Start, Length = o.Span.Length, Text = o.ToString(), Path = o.SyntaxTree.FilePath }).ToList();
            var regionsFrags = new List<Region>();
            foreach (var region in regionsFragments)
            {
                if (region.Path.ToUpperInvariant().Contains(expHome.ToUpperInvariant()))
                {
                    var regionPathUpper = region.Path.ToUpperInvariant();
                    var index = expHome.Length;
                    var substring = regionPathUpper.Substring(index, regionPathUpper.Length - index);
                    region.Path = substring;
                    regionsFrags.Add(region);
                }
                else
                {
                    regionsFrags.Add(region);
                }
            }
            return regionsFrags;
        }

        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        private static void GeneratedDiffEdits(string commit, List<Tuple<SyntaxNodeOrToken, SyntaxNodeOrToken>> transformedList)
        {
            string expHome = RefazerObject.Environment.Environment.ExpHome();
            string output = "";
            string errors = "";
            var pathoutput = expHome + TestConstants.MetadataFolder + "\\" + commit + TestConstants.DiffFolder;
            foreach (var ba in transformedList)
            {
                var className = ba.Item1.SyntaxTree.FilePath.Split(@"\".ToCharArray()).Last();
                var pathb = expHome + TestConstants.MetadataFolder + "\\" + commit + TestConstants.BeforeFile + className;
                var patha = expHome + TestConstants.MetadataFolder + "\\" + commit + TestConstants.AfterFile + className;
                FileUtil.WriteToFile(pathb, ba.Item1.ToString());
                FileUtil.WriteToFile(patha, ba.Item2.ToString());

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

        private static int GetFirstIncorrect(List<Tuple<Region, string, string>> toolBeforeAfterList, List<Tuple<Region, string, string>> baselineBeforeAfterList, List<int> randomList, List<Region> locations, List<Region> notTransformed)
        {
            //computing list of locations that do not follow the baseline.
            var notFoundList = baselineBeforeAfterList.Where(o => !toolBeforeAfterList.Contains(o)).ToList();
            var notTranformedCodeLocations = notTransformed.Select(o => Tuple.Create(o, o.Path, o.Text)).ToList();
            notFoundList.AddRange(notTranformedCodeLocations);
            if (!notFoundList.Any()) return -1;
            //Computing example locations that do not follow the baseline.
            var incorrectLocationsList = locations.Where(o => notFoundList.Any(e => o.IntersectWith(e.Item1))).ToList();
            //Computing the index of the locations that do not follow the baseline.
            var notFoundIndexList = incorrectLocationsList.Select(o => locations.IndexOf(o)).ToList();
            //Getting the index of the location in the random list.
            var index = randomList.FindIndex(o => notFoundIndexList.Contains(o));
            if (index == -1) return -1;
            var firstNotFound = randomList[index];
            return firstNotFound;
        }

        private static List<Tuple<Region, string, string>> GetBeforeAfterData(List<Tuple<Region, string, string>> regions, List<Region> locations)
        {
            var dictionary = CreateDictionaryMatch(locations, regions.Select(o => o.Item1).ToList());
            var foundList = new List<Region>();
            dictionary.Values.ForEach(o => foundList.AddRange(o));
            foundList = foundList.Distinct().ToList();
            var regionsFound = regions.Where(o => foundList.Contains(o.Item1)).ToList();
            return regionsFound;
        }

        public static List<Tuple<string, string, string>> Transform(Dictionary<string, List<Tuple<Region, string, string>>> transformations)
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
                    Region region = tregion.Item1;
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

        private static string GetDataAndSaveToFile(string commit, string expHome, string seed, string fileName)
        {
            string file = expHome + fileName + ".txt";
            var fragments = FileUtil.ReadFile(file);
            var pathToOutput = expHome + TestConstants.MetadataFolder + "\\" + commit + "\\" + fileName + seed + ".res";
            FileUtil.WriteToFile(pathToOutput, fragments);
            File.Delete(file);
            return fragments;
        }

        private static int GetFirstNotFound(List<Region> foundLocations, List<Region> locations, List<int> result)
        {
            var notFoundList = locations.Where(o => !foundLocations.Contains(o)).ToList();
            if (!notFoundList.Any()) return -1;

            var notFoundIndexList = notFoundList.Select(o => locations.IndexOf(o));
            var index = result.FindIndex(o => notFoundIndexList.Contains(o));
            var firstNotFound = result[index];
            return firstNotFound;
        }

        private static List<Region> GetEditedLocations(List<Region> regions, List<Region> locations)
        {
            var dictionary = CreateDictionaryMatch(locations, regions);
            var found = dictionary.Where(o => o.Value.Any()).Select(o => o.Key).ToList();
            return found;
        }

        private static List<Region> GetEditionInLocations(List<Region> regions, List<Region> locations)
        {
            var dictionary = CreateDictionaryMatch(locations, regions);

            var foundList = new List<Region>();
            dictionary.Values.ForEach(o => foundList.AddRange(o));
            foundList = foundList.Distinct().ToList();
            return foundList;
        }

        private static Dictionary<Region, List<Region>> CreateDictionaryMatch(List<Region> locations, List<Region> regions)
        {
            var dictionary = new Dictionary<Region, List<Region>>();
            foreach (var v in locations)
            {
                var matches = regions.Where(o => v.Equals(o)).ToList();
                if (!dictionary.ContainsKey(v))
                {
                    dictionary[v] = new List<Region>();
                }
                dictionary[v].AddRange(matches);
            }
            return dictionary;
        }

        public static void Log(string commit, double time, int exTransformations, int locations, int acTrasnformation,
            int documents, string program, double timeToLearnEdit, double timeToTransformEdit, double mean, string gl)
        {
            string path = LogData.LogPath();
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
                em.SetValue("K" + empty, gl);
                em.Save();
            }
        }

        /*public static void Log(string commit, double time, int exTransformations, int locations, int acTrasnformation,
            int documents, string program, double timeToLearnEdit, double timeToTransformEdit, double mean)
        {
            string path = LogData.LogPath();
            XSSFWorkbook hssfwb;
            using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.ReadWrite))
            {
                hssfwb = new XSSFWorkbook(file);
            
            ISheet sheet = hssfwb.GetSheetAt(0);
            int empty = sheet.LastRowNum;
            for (int row = 0; row <= sheet.LastRowNum; row++)
            {
                if (sheet.GetRow(row).GetCell(0) == null || //null is when the row only contains empty cells
                    sheet.GetRow(row).GetCell(0).Equals(commit))  
                {
                    empty = row;
                    break;
                
                }
            }
           
            var srow = sheet.CreateRow(empty + 1);
            MessageBox.Show("aqui: " + srow + commit);
            
            MessageBox.Show(srow.ToString());
            for (int col = 0; col < 10; col++)
            {
                if (srow.GetCell(col) == null)
                {
                    srow.CreateCell(col);
                    }
                }
            srow.GetCell(0).SetCellValue(commit);
            srow.GetCell(1).SetCellValue(time / 1000);
            srow.GetCell(2).SetCellValue(exTransformations);
            srow.GetCell(3).SetCellValue(locations);
            srow.GetCell(4).SetCellValue(acTrasnformation);
            srow.GetCell(5).SetCellValue(documents);
            srow.GetCell(6).SetCellValue(program);
            srow.GetCell(7).SetCellValue(timeToLearnEdit / 1000);
            srow.GetCell(8).SetCellValue(timeToTransformEdit / 1000);
            srow.GetCell(9).SetCellValue(mean);
            hssfwb.Write(file);
            file.Close();
            }
        }*/

        /*public static void Log(string commit, double time, int exTransformations, int locations, int acTrasnformation,
            int documents, string program, double timeToLearnEdit, double timeToTransformEdit, double mean)
        {
            string filename = LogData.LogPath();
            TextReader textReader = File.OpenText(filename);
            var csv = new CsvReader(textReader);
            var records = csv.GetRecords<Record>().ToList();
            textReader.Close();

            Record record = new Record();
            
            record.Commit = commit;
            record.Time = time / 1000;
            record.Examples = exTransformations;
            record.Locations = locations;
            record.AcTransformation = acTrasnformation;
            record.Documents = documents;
            record.Program = program;
            record.TimeToLearnEdit = timeToLearnEdit / 1000;
            record.TimeToTranformEdit = timeToTransformEdit / 1000;
            record.Mean = mean;
            records.Add(record);

            TextWriter textWriter = File.CreateText(filename);
            var csvWriter = new CsvWriter(textWriter);
            csvWriter.WriteRecords(records);
            textWriter.Close();
        }*/

        public static void LogProgram(string commit, int programIndex, string program, bool status)
        {
            //  string commitFirstLetter = commit.ElementAt(0).ToString();
            //  string commitId = commit.Substring(commit.IndexOf(@"\") + 1);

            //commit = commitId;

            //string commitFirstLetter = commit.ElementAt(0).ToString();
            //string commitId = commit.Substring(commit.IndexOf(@"\") + 1);

            //commit = commitFirstLetter + "" + commitId;
            //  commit = commit.Substring(0, commit.Length - 1);

            string path = LogData.LogPath();
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
        ///// Gets the domain specific grammar
        ///// </summary>
        //public static Grammar GetGrammar()
        //{
        //    string path = FileUtil.GetBasePath() + @"\ProgramSynthesis\grammar\Transformation.grammar";
        //    var grammar = Utils.LoadGrammar(path);
        //    return grammar;
        //}

        
        // Here starts the Log version for Number of Examples

        /// <summary>
        /// Complete test
        /// </summary>
        /// <param name="commit">Commit where the change occurs</param>
        /// <param name="solutionPath">Path to the solution.</param>
        /// <param name="kinds">Kinds that will be transformed.</param>
        /// <returns>True if pass test</returns>
        public static bool CompleteTestBase(string commit, string solutionPath = null, List<SyntaxKind> kinds = null,
            string fileFolder = null)
        {
            string expHome = RefazerObject.Environment.Environment.ExpHome();
            if (expHome.IsEmpty()) throw new Exception("Environment variable for the experiment not defined");
            if (kinds == null)
            {
                kinds = new List<SyntaxKind> { SyntaxKind.MethodDeclaration, SyntaxKind.ConstructorDeclaration };
            }
            //int seed = 86028157;
            int seed = 3;
            var execId = "ranking";
            //Load grammar
            var grammar = GetGrammar();
            var baselineBeforeAfterList = JsonUtil<List<Tuple<Region, string, string>>>.Read(expHome + TestConstants.MetadataFolder + "\\" + commit + TestConstants.BeforeAfterLocations + execId + ".json");
            var locations = baselineBeforeAfterList.Select(O => O.Item1).ToList();
            var globalTransformations = RegionManager.GetInstance().GroupTransformationsBySourcePath(baselineBeforeAfterList);
            //Random number generator with a seed.
            Random random = new Random(seed);
            var randomList = Enumerable.Range(0, locations.Count).OrderBy(o => random.Next()).ToList();
            var positiveExamples = randomList.GetRange(0, Math.Min(1, locations.Count));//Aqui minimo 2
            //var positiveExamples = new int [] {3, 21 }.ToList(); //para adicionar o exemplos 1 e 2 explicitamente
            var negativeExamples = new List<Region>();
            var addPositive = false;
            var includeNegExamples = true;
            var incrementalExamples = new List<int>();
            bool INCREMENTAL_EXAMPLES = false;
            TestHelper helper = null;
            double mean = -1.0;
            while (true)
            {
                CodeFragmentsInfo.GetInstance().Init();
                TransformationInfos.GetInstance().Init();
                if (INCREMENTAL_EXAMPLES)
                {
                    if (incrementalExamples.Count >= locations.Count)
                    {
                        break;
                    }
                    incrementalExamples.Add(randomList[incrementalExamples.Count]);
                    positiveExamples = new List<int>(incrementalExamples);
                }
                positiveExamples.Sort();
                helper = new TestHelper(grammar, baselineBeforeAfterList, globalTransformations,
                    expHome, solutionPath, commit, kinds, fileFolder, execId);
                helper.Execute(positiveExamples, negativeExamples);
                var regionsFrags = GetTransformedLocations(expHome);
                string transformedPath = expHome + TestConstants.MetadataFolder + "\\" + commit + TestConstants.TransformedLocationsAll + execId + ".json";
                JsonUtil<List<Region>>.Write(regionsFrags, transformedPath);
                
                if (SynthesisConfig.GetInstance().CreateLog)
                {
                    var scriptsizes = GetDataAndSaveToFile(commit, expHome, execId, Constants.ScriptSize);
                    var sizes = scriptsizes.Split(new[] { "\n" }, StringSplitOptions.None).Select(int.Parse);
                    mean = sizes.Average();
                }
                var beforeafter = TestUtil.GetBeforeAfterList(expHome);
                var toolBeforeAfterList = GetBeforeAfterData(beforeafter, locations);
                GenerateMetadata(regionsFrags, locations, expHome, commit, execId, beforeafter, toolBeforeAfterList);
                GetDataAndSaveToFile(commit, expHome, execId, Constants.Programs);
                var foundLocations = GetEditedLocations(regionsFrags, locations);
                var firstProblematicLocation = GetFirstNotFound(foundLocations, locations, randomList);
                if (firstProblematicLocation == -1)
                {
                    //Generate meta-data for BaselineBeforeAfterList on commit.
                    var foundList = GetEditionInLocations(regionsFrags, locations);
                    JsonUtil<List<Region>>.Write(foundList, expHome + TestConstants.MetadataFolder + "\\" + commit + TestConstants.TransformedLocationsTool + execId + ".json");
                    //var toolBeforeAfterList = GetBeforeAfterData(beforeafter, locations);
                    //GenerateMetadata(regionsFrags, locations, expHome, commit, execId, befo6reafter, toolBeforeAfterList);
                    //Comparing edited locations with baseline
                    var firstIncorrect = GetFirstTransformedIncorrectly(locations, baselineBeforeAfterList, toolBeforeAfterList, foundList, randomList);
                    //coment these lines to add negatives and positives.
              //      addPositive = true;
              //      includeNegExamples = false;
                    //end of comment
                    if (firstIncorrect == -1)
                    {
                        bool moreThanNeeded = (beforeafter.Count > locations.Count);
                        if (moreThanNeeded)
                        {
                            if (includeNegExamples)
                            {
                                var identifiedLocations = beforeafter.Select(o => o.Item1).ToList();
                                var program = helper.Program.ToString();
                                AddNegativeExample(includeNegExamples, identifiedLocations, locations, negativeExamples);
                                continue;
                            }
                            else
                            {
                                var firstMissing = getFirstMissingExample(positiveExamples, randomList);
                                if (firstMissing != -1)
                                {
                                    AddExample(positiveExamples, locations, negativeExamples, beforeafter, firstMissing, addPositive);
                                    addPositive = !addPositive;
                                    continue;
                                }
                                else
                                {
                                    Log(helper, commit, positiveExamples, negativeExamples, baselineBeforeAfterList, mean, "GL");
                                    throw new Exception("Good Luck!");
                                }
                            }
                        }
                        else
                        {
                            TryToTransform(commit);
                            break;
                        }
                    }
                    firstProblematicLocation = firstIncorrect;
                }
                IsProblematicLocationInExamples(commit, positiveExamples, firstProblematicLocation);
                positiveExamples.Add(firstProblematicLocation);
            }
            Log(helper, commit, positiveExamples, negativeExamples, baselineBeforeAfterList, mean);
            return true;
        }
        
        private static int GetFirstTransformedIncorrectly(List<Region> locations, List<Tuple<Region, string, string>> baselineBeforeAfterList,
            List<Tuple<Region, string, string>> toolBeforeAfterList, List<Region> foundList, List<int> randomList)
        {
            var baselineBeforeAfter = CreateBaselineBeforeAfter(baselineBeforeAfterList);
            var notTransformed = foundList.Except(toolBeforeAfterList.Select(o => o.Item1)).ToList();
            var firstIncorrect = GetFirstIncorrect(toolBeforeAfterList, baselineBeforeAfter, randomList, locations, notTransformed);
            return firstIncorrect;
        }

        private static void GenerateMetadata(List<Region> regionsFrags, List<Region> locations, string expHome, string commit, string execId,
            List<Tuple<Region, string, string>> beforeafter, List<Tuple<Region, string, string>> toolBeforeAfterList)
        {
            JsonUtil<List<Tuple<Region, string, string>>>.Write(toolBeforeAfterList, expHome + TestConstants.MetadataFolder + "\\" + commit + TestConstants.BeforeAfterLocationsTool + execId + ".json");
            JsonUtil<List<Tuple<Region, string, string>>>.Write(beforeafter, expHome + TestConstants.MetadataFolder + "\\" + commit + TestConstants.BeforeAfterLocationsAll + execId + ".json");
        }

        private static void AddNegativeExample(bool includeNegExamples, List<Region> foundList, List<Region> locations, List<Region> negativeExamples)
        {
            var mustNotBeSelected = foundList.Except(locations).ToList();
            var missingNegative = mustNotBeSelected.Except(negativeExamples).ToList();
            if (!missingNegative.IsEmpty())
            {
                negativeExamples.Add(missingNegative.First());
            }
        }

        private static void AddExample(List<int> positiveExamples, List<Region> locations, List<Region> negativeExamples,
            List<Tuple<Region, string, string>> beforeafter, int firstMissing, bool addPositive)
        {
            if (addPositive)
            {
                positiveExamples.Add(firstMissing);
            }
            else
            {
                var mustNotBeSelected = beforeafter.Select(o => o.Item1).Except(locations).ToList();
                var missingNegative = mustNotBeSelected.Except(negativeExamples).ToList();
                negativeExamples.Add(missingNegative.First());
            }
        }

        private static void TryToTransform(string commit)
        {
            try
            {
                var transformedDocuments = ASTTransformer.Transform(TransformationInfos.GetInstance().Transformations);
                GeneratedDiffEdits(commit, transformedDocuments);
            }
            catch (Exception)
            {
                // ignored
                //throw new Exception("Errors in transforming the locations.");
            }
        }

        private static void IsProblematicLocationInExamples(string commit, List<int> positiveExamples, int firstProblematicLocation)
        {
            if (positiveExamples.Contains(firstProblematicLocation))
            {
                try
                {
                    var transformedDocuments = ASTTransformer.Transform(TransformationInfos.GetInstance().Transformations);
                    GeneratedDiffEdits(commit, transformedDocuments);
                }
                catch (Exception)
                {
                    // ignored
                    //throw new Exception("Errors in transforming the locations.");
                }
                throw new Exception("A transformation could not be learned using this examples.");
            }
        }

        private static List<Tuple<Region, string, string>> CreateBaselineBeforeAfter(List<Tuple<Region, string, string>> baselineBeforeAfterList)
        {
            var baselineBeforeAfter = new List<Tuple<Region, string, string>>();
            foreach (var baseline in baselineBeforeAfterList)
            {
                var region = baseline.Item1;
                region.Path = region.Path.ToUpperInvariant();
                baselineBeforeAfter.Add(Tuple.Create(region, baseline.Item2, baseline.Item3.ToUpperInvariant()));
            }
            return baselineBeforeAfter;
        }

        /// <summary>
        /// Generates the log
        /// </summary>
        /// <param name="helper">Helper class</param>
        /// <param name="commit">commit</param>
        /// <param name="positiveExamples">positive examples</param>
        /// <param name="negativeExamples">negative examples</param>
        /// <param name="baselineBeforeAfterList">baseline list</param>
        /// <param name="mean">mean</param>
        private static void Log(TestHelper helper, string commit, List<int> positiveExamples,
            List<Region> negativeExamples, List<Tuple<Region, string, string>> baselineBeforeAfterList, double mean, string gl = "")
        {
            //end of execution 
            long totalTimeToLearn = helper.TotalTimeToLearn;
            long totalTimeToExecute = helper.TotalTimeToLearn;
            var transformed = helper.Transformed;
            var program = helper.Program;
            var dictionarySelection = helper.DictionarySelection;
            Log(commit,
                 totalTimeToLearn + totalTimeToExecute,
                 positiveExamples.Count + negativeExamples.Count,
                 baselineBeforeAfterList.Count,
                 transformed.Count,
                 dictionarySelection.Count,
                 program.ToString(),
                 totalTimeToLearn,
                 totalTimeToExecute,
                 mean, gl);
        }

        private static int getFirstMissingExample(List<int> examples, List<int> randomList)
        {
            foreach (int x in randomList)
            {
                if (!examples.Contains(x))
                {
                    return x;
                }
            }
            return -1;
        }

        //Here ends the Log verion 



        ////Here starts the LogProgram version for Labels
        ///// <summary>
        ///// Complete test
        ///// </summary>
        ///// <param name="commit">Commit where the change occurs</param>
        ///// <param name="solutionPath">Path to the solution.</param>
        ///// <param name="kinds">Kinds that will be transformed.</param>
        ///// <returns>True if pass test</returns>
        //public static bool CompleteTestBase(string commit, string solutionPath = null, List<SyntaxKind> kinds = null,
        //    string fileFolder = null)
        //{
        //    string expHome = RefazerObject.Environment.Environment.ExpHome();
        //    if (expHome.IsEmpty()) throw new Exception("Environment variable for the experiment not defined");

        //    if (kinds == null)
        //    {
        //        kinds = new List<SyntaxKind> { SyntaxKind.MethodDeclaration, SyntaxKind.ConstructorDeclaration };
        //    }
        //    int seed = 86028157;
        //    var execId = "ranking";
        //    //Load grammar
        //    var grammar = GetGrammar();
        //    var baselineBeforeAfterList = JsonUtil<List<Tuple<Region, string, string>>>.Read(expHome + TestConstants.MetadataFolder + "\\" + commit + TestConstants.BeforeAfterLocations + execId + ".json");
        //    var locations = baselineBeforeAfterList.Select(O => O.Item1).ToList();
        //    var globalTransformations = RegionManager.GetInstance().GroupTransformationsBySourcePath(baselineBeforeAfterList);
        //    Random random = new Random(seed);
        //    var randomList = Enumerable.Range(0, locations.Count).OrderBy(o => random.Next()).ToList();
        //    bool atLeastOneCorrect = false;
        //    var negativeExamples = new List<Region>();
        //    //for (int exampleIndex = 1; exampleIndex <= locations.Count; exampleIndex++) // inicia com 1 exemplo originalmente
        //    //{
        //    var exampleIndex = 1;
        //    // MessageBox.Show(exampleIndex + "");
        //    var examples = randomList.GetRange(0, exampleIndex);
        //    //Execution
        //    TestHelper helper;
        //    examples.Sort();
        //    helper = new TestHelper(grammar, baselineBeforeAfterList, globalTransformations,
        //                expHome, solutionPath, commit, kinds, fileFolder, execId);
        //    helper.Execute(examples);
        //    var beforeafter = TestUtil.GetBeforeAfterList(expHome);
        //    var identifiedLocations = beforeafter.Select(o => o.Item1).ToList();
        //    AddNegativeExample(true, identifiedLocations, locations, negativeExamples);
        //    var allPrograms = helper.LearnPrograms(examples, negativeExamples);
        //    for (var i = 1; i <= Math.Min(10, allPrograms.Count); i++)
        //    {
        //        var p = allPrograms[i - 1];
        //        CodeFragmentsInfo.GetInstance().Locations.Clear();
        //        TransformationInfos.GetInstance().Transformations.Clear();
        //        helper.Execute(examples, p);

        //        var regionsFrags = GetTransformedLocations(expHome);
        //        string transformedPath = expHome + TestConstants.MetadataFolder + "\\" + commit + TestConstants.TransformedLocationsAll + execId + ".json";
        //        JsonUtil<List<Region>>.Write(regionsFrags, transformedPath);
        //        beforeafter = TestUtil.GetBeforeAfterList(expHome);
        //        //  GetDataAndSaveToFile(commit, expHome, execId, Constants.Programs);
        //        var foundLocations = GetEditedLocations(regionsFrags, locations);
        //        var firstProblematicLocation = GetFirstNotFound(foundLocations, locations, randomList);
        //        if (firstProblematicLocation == -1)
        //        {
        //            //Generate meta-data for BaselineBeforeAfterList on commit.
        //            var foundList = GetEditionInLocations(regionsFrags, locations);
        //            JsonUtil<List<Region>>.Write(foundList, expHome + TestConstants.MetadataFolder + "\\" + commit + TestConstants.TransformedLocationsTool + execId + ".json");
        //            var beforeafterList = GetBeforeAfterData(beforeafter, locations);
        //            JsonUtil<List<Tuple<Region, string, string>>>.Write(beforeafterList, expHome + TestConstants.MetadataFolder + "\\" + commit + TestConstants.BeforeAfterLocationsTool + execId + ".json");
        //            JsonUtil<List<Tuple<Region, string, string>>>.Write(beforeafter, expHome + TestConstants.MetadataFolder + "\\" + commit + TestConstants.BeforeAfterLocationsAll + execId + ".json");
        //            //Comparing edited locations with baseline
        //            var baselineBeforeAfter = new List<Tuple<Region, string, string>>();
        //            foreach (var baseline in baselineBeforeAfterList)
        //            {
        //                var region = baseline.Item1;
        //                region.Path = region.Path.ToUpperInvariant();
        //                baselineBeforeAfter.Add(Tuple.Create(region, baseline.Item2, baseline.Item3.ToUpperInvariant()));
        //            }
        //            var notTransformed = foundList.Except(beforeafterList.Select(o => o.Item1)).ToList();
        //            var firstIncorrect = GetFirstIncorrect(beforeafterList, baselineBeforeAfter, randomList, locations, notTransformed);
        //            if (firstIncorrect == -1)
        //            {
        //                if (beforeafterList.Count > locations.Count)
        //                {
        //                    LogProgram(commit, i, p.ToString(), false);
        //                }
        //                else
        //                {
        //                    atLeastOneCorrect = true;
        //                    LogProgram(commit, i, p.ToString(), true);
        //                }
        //            }
        //            else
        //            {
        //                LogProgram(commit, i, p.ToString(), false);
        //            }
        //        }
        //        else
        //        {
        //            LogProgram(commit, i, p.ToString(), false);
        //        }
        //    }
        //    if (atLeastOneCorrect)
        //    {
        //        //break;
        //    }
        //    //}
        //    return true;
        //}
        ////  Here ends the LogProgram version for Label

        /*
    //Here ends the Log verion 
    //Here starts the LogProgram version
    /// <summary>
    /// Complete test
    /// </summary>
    /// <param name="commit">Commit where the change occurs</param>
    /// <param name="solutionPath">Path to the solution.</param>
    /// <param name="kinds">Kinds that will be transformed.</param>
    /// <returns>True if pass test</returns>
    public static bool CompleteTestBase(string commit, string solutionPath = null, List<SyntaxKind> kinds = null,
        string fileFolder = null)
    {
        string expHome = RefazerObject.Environment.Environment.ExpHome();
        if (expHome.IsEmpty()) throw new Exception("Environment variable for the experiment not defined");

        if (kinds == null)
        {
            kinds = new List<SyntaxKind> { SyntaxKind.MethodDeclaration, SyntaxKind.ConstructorDeclaration };
        }
        int seed = 86028157;
        var execId = "ranking";
        //Load grammar
        var grammar = GetGrammar();
        var baselineBeforeAfterList = JsonUtil<List<Tuple<Region, string, string>>>.Read(expHome + TestConstants.MetadataFolder + "\\" + commit + TestConstants.BeforeAfterLocations + execId + ".json");
        var locations = baselineBeforeAfterList.Select(O => O.Item1).ToList();
        var globalTransformations = RegionManager.GetInstance().GroupTransformationsBySourcePath(baselineBeforeAfterList);
        Random random = new Random(seed);
        var randomList = Enumerable.Range(0, locations.Count).OrderBy(o => random.Next()).ToList();
        bool atLeastOneCorrect = false;
        for (int exampleIndex = 1; exampleIndex <= locations.Count; exampleIndex++) // inicia com 1 exemplo originalmente
        {
        // MessageBox.Show(exampleIndex + "");
        var examples = randomList.GetRange(0, exampleIndex);
        //Execution
        TestHelper helper;
        examples.Sort();
        helper = new TestHelper(grammar, baselineBeforeAfterList, globalTransformations,
                    expHome, solutionPath, commit, kinds, fileFolder, execId);
        var allPrograms = helper.LearnPrograms(examples);
        for (var i = 1; i <= allPrograms.Count; i++)
        {
            var p = allPrograms[i - 1];
            CodeFragmentsInfo.GetInstance().Locations.Clear();
            TransformationInfos.GetInstance().Transformations.Clear();
            helper.Execute(examples, p);

            var regionsFrags = GetTransformedLocations(expHome);
            string transformedPath = expHome + TestConstants.MetadataFolder + "\\" + commit + TestConstants.TransformedLocationsAll + execId + ".json";
            JsonUtil<List<Region>>.Write(regionsFrags, transformedPath);
            var beforeafter = TestUtil.GetBeforeAfterList(expHome);
            //  GetDataAndSaveToFile(commit, expHome, execId, Constants.Programs);
            var foundLocations = GetEditedLocations(regionsFrags, locations);
            var firstProblematicLocation = GetFirstNotFound(foundLocations, locations, randomList);
            if (firstProblematicLocation == -1)
            {
                //Generate meta-data for BaselineBeforeAfterList on commit.
                var foundList = GetEditionInLocations(regionsFrags, locations);
                JsonUtil<List<Region>>.Write(foundList, expHome + TestConstants.MetadataFolder + "\\" + commit + TestConstants.TransformedLocationsTool + execId + ".json");
                var beforeafterList = GetBeforeAfterData(beforeafter, locations);
                JsonUtil<List<Tuple<Region, string, string>>>.Write(beforeafterList, expHome + TestConstants.MetadataFolder + "\\" + commit + TestConstants.BeforeAfterLocationsTool + execId + ".json");
                JsonUtil<List<Tuple<Region, string, string>>>.Write(beforeafter, expHome + TestConstants.MetadataFolder + "\\" + commit + TestConstants.BeforeAfterLocationsAll + execId + ".json");
                //Comparing edited locations with baseline
                var baselineBeforeAfter = new List<Tuple<Region, string, string>>();
                foreach (var baseline in baselineBeforeAfterList)
                {
                    var region = baseline.Item1;
                    region.Path = region.Path.ToUpperInvariant();
                    baselineBeforeAfter.Add(Tuple.Create(region, baseline.Item2, baseline.Item3.ToUpperInvariant()));
                }
                var notTransformed = foundList.Except(beforeafterList.Select(o => o.Item1)).ToList();
                var firstIncorrect = GetFirstIncorrect(beforeafterList, baselineBeforeAfter, randomList, locations, notTransformed);
                if (firstIncorrect == -1)
                {
                    if (beforeafterList.Count > locations.Count)
                    {
                        LogProgram(commit, i, p.ToString(), false);
                    }
                    else
                    {
                        atLeastOneCorrect = true;
                        LogProgram(commit, i, p.ToString(), true);
                    }
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
        if (atLeastOneCorrect)
        {
           break;
        }
      }
        return true;
    }
    ////  Here ends the LogProgram version
    */

        private static int GetFirstIncorrect(List<Tuple<Region, string, string>> toolBeforeAfterList, List<Tuple<Region, string, string>> baselineBeforeAfterList, List<int> randomList, List<CodeLocation> locations)
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

        private static List<Tuple<Region, string, string>> GetBeforeAfterData(string beforeafter, List<CodeLocation> locations)
        {
            var regions = ConvertBeforeAfterToRegions(beforeafter);
            var dictionary = CreateDictionaryMatch(locations, regions.Select(o => o.Item1).ToList());

            var foundList = new List<Region>();
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

        private static List<Tuple<Region, string, string>> ConvertBeforeAfterToRegions(string beforeafter)
        {
            var regions = new List<Tuple<Region, string, string>>();
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

                var region = new Region();
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

        private static List<Region> GetEditionInLocations(string fragments, List<CodeLocation> locations)
        {
            var regions = ConvertFragmentsToRegions(fragments);
            var dictionary = CreateDictionaryMatch(locations, regions);

            var foundList = new List<Region>();
            dictionary.Values.ForEach(o => foundList.AddRange(o));
            foundList = foundList.Distinct().ToList();
            return foundList;
        }

        private static Dictionary<CodeLocation, List<Region>> CreateDictionaryMatch(List<CodeLocation> locations, List<Region> regions)
        {
            var dictionary = new Dictionary<CodeLocation, List<Region>>();
            foreach (var v in locations)
            {
                var matches = regions.Where(o => v.Region.IntersectWith(o));
                if (!dictionary.ContainsKey(v))
                {
                    dictionary[v] = new List<Region>();
                }
                dictionary[v].AddRange(matches);
            }
            return dictionary;
        }

        private static List<Region> ConvertFragmentsToRegions(string fragments)
        {
            var regions = new List<Region>();
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

                var region = new Region();
                region.Start = start;
                region.Length = length;
                region.Text = content;
                region.Path = path;

                regions.Add(region);
            }
            return regions;
        }

        //public static void LogProgram(string commit, int programIndex, string program, bool status)
        //{
        //    string commitFirstLetter = commit.ElementAt(0).ToString();
        //    string commitId = commit.Substring(commit.IndexOf(@"\") + 1);

        //    commit = commitId;

        //    string path = TestUtil.LogProgramStatus;
        //    using (ExcelManager em = new ExcelManager())
        //    {
        //        em.Open(path);

        //        int empty;
        //        for (int i = 1; ; i++)
        //        {
        //            if (em.GetValue("A" + i, Category.Formatted).ToString().Equals("") ||
        //                em.GetValue("A" + i, Category.Formatted).ToString().Equals(commit) && em.GetValue("B" + i, Category.Formatted).ToString().Equals(programIndex.ToString()))
        //            {
        //                empty = i;
        //                break;
        //            }
        //        }

        //        em.SetValue("A" + empty, commit);
        //        em.SetValue("B" + empty, programIndex);
        //        em.SetValue("C" + empty, program);
        //        em.SetValue("D" + empty, status);
        //        em.Save();
        //    }
        //}
    }
}