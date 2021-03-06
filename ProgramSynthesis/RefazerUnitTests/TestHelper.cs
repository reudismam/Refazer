﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Microsoft.CodeAnalysis;
//using Microsoft.CodeAnalysis.CSharp;
//using Microsoft.ProgramSynthesis;
//using Microsoft.ProgramSynthesis.AST;
//using Microsoft.ProgramSynthesis.Specifications;
//using Microsoft.ProgramSynthesis.Utils;
//using RefazerFunctions.Substrings;
//using RefazerFunctions;
//using RefazerFunctions.Spg.Bean;
//using RefazerManager;
//using Spg.LocationRefactor.Location;
//using Spg.LocationRefactor.TextRegion;
//using Spg.LocationRefactor.Transform;
//using TreeElement.Spg.Node;
//using WorkSpaces.Spg.Workspace;

//namespace UnitTests
//{
//    class TestHelper
//    {
//        /// <summary>
//        /// Grammar
//        /// </summary>
//        private readonly Grammar _grammar;
//        /// <summary>
//        /// Execution id
//        /// </summary>
//        private readonly int _execId;
//        /// <summary>
//        /// Regions
//        /// </summary>
//        private readonly List<Region> _regions;
//        /// <summary>
//        /// Transformations
//        /// </summary>
//        private readonly List<CodeLocation> _locations;
//        /// <summary>
//        /// Global transformations
//        /// </summary>
//        private readonly Dictionary<string, List<CodeTransformation>> _globalTransformations;
//        /// <summary>
//        /// Path to experiment
//        /// </summary>
//        private readonly string _expHome;
//        /// <summary>
//        /// Solution path
//        /// </summary>
//        private readonly string _solutionPath;
//        /// <summary>
//        /// Commit Id
//        /// </summary>
//        private readonly string _commit;
//        /// <summary>
//        /// Kinds
//        /// </summary>
//        private readonly List<SyntaxKind> _kinds;
//        /// <summary>
//        /// Total time to execute
//        /// </summary>
//        public long TotalTimeToExecute { get; set; }
//        /// <summary>
//        /// Total time to learn
//        /// </summary>
//        public long TotalTimeToLearn { get; set; }
//        /// <summary>
//        /// Transformed
//        /// </summary>
//        public List<object> Transformed { get; set; }
//        /// <summary>
//        /// Learned program
//        /// </summary>
//        public ProgramNode Program { get; set; }
//        /// <summary>
//        /// Dictionary selection
//        /// </summary>
//        public Dictionary<string, List<Region>> DictionarySelection { get; set; }

//        public TestHelper(Grammar grammar, List<Region> regions, List<CodeLocation> locations, Dictionary<string, List<CodeTransformation>> globalTransformations, string expHome, string solutionPath, string commit, List<SyntaxKind> kinds, int execId)
//        {
//            _grammar = grammar;
//            _regions = regions;
//            _locations = locations;
//            _globalTransformations = globalTransformations;
//            _expHome = expHome;
//            _solutionPath = solutionPath;
//            _commit = commit;
//            _kinds = kinds;
//            _execId = execId;
//            WorkspaceManager.GetInstance().solutionPath = _expHome + _solutionPath;
//        }

//        public void Execute(List<int> examples)
//        {
//            var metadataRegions = examples.Select(index => _regions[index]).ToList();
//            var locationRegions = examples.Select(index => _locations[index]).ToList();

//            DictionarySelection = RegionManager.GetInstance().GroupRegionBySourcePath(metadataRegions);
//            //Examples
//            var examplesInput = new List<SyntaxNodeOrToken>();
//            var examplesOutput = new List<SyntaxNodeOrToken>();

//            SyntaxNodeOrToken inpTree = default(SyntaxNodeOrToken);
//            //building example methods
//            var ioExamples = new Dictionary<State, IEnumerable<object>>();
//            foreach (KeyValuePair<string, List<Region>> entry in DictionarySelection)
//            {
//                string sourceCode = FileUtil.ReadFile(entry.Key);
//                Tuple<string, List<Region>> tu = Transform(sourceCode, _globalTransformations[entry.Key.ToUpperInvariant()], metadataRegions);
//                string sourceCodeAfter = tu.Item1;

//                inpTree = CSharpSyntaxTree.ParseText(sourceCode, path: entry.Key).GetRoot();
//                SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(sourceCodeAfter).GetRoot();

//                var allMethodsInput = GetNodesByType(inpTree, _kinds);
//                var allMethodsOutput = GetNodesByType(outTree, _kinds);
//                var inputMethods = new List<int>();
//                foreach (var region in locationRegions.Where(o => o.SourceClass.ToUpperInvariant().Equals(entry.Key.ToUpperInvariant())))
//                {
//                    for (int index = 0; index < allMethodsInput.Count; index++)
//                    {
//                        var method = allMethodsInput[index];
//                        var tRegion = new Region
//                        {
//                            Start = method.SpanStart,
//                            Length = method.Span.Length
//                        };

//                        if (region.Region.IsInside(tRegion))
//                        {
//                            inputMethods.Add(index);
//                        }
//                    }
//                }

//                //Examples
//                var inpExs = inputMethods.Distinct().Select(index => allMethodsInput[index]).ToList();
//                var outExs = inputMethods.Distinct().Select(index => allMethodsOutput[index]).ToList();

//                examplesInput.AddRange(inpExs);
//                examplesOutput.AddRange(outExs);
//            }

//            examplesInput = RemoveDuplicates(examplesInput);
//            for (int index = 0; index < examplesInput.Count; index++)
//            {
//                var example = examplesInput.ElementAt(index);
//                var treeNode = ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken) example);
//                var inputState = State.Create(_grammar.InputSymbol, new Node(treeNode));
//                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(index) });
//            }

//            //Learn program
//            var spec = DisjunctiveExamplesSpec.From(ioExamples);

//            long millBeforeLearn = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
//            Program = Utils.Learn(_grammar, spec, new RankingScore(_grammar),  new WitnessFunctions(_grammar));
//            long millAfterLearn = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
//            TotalTimeToLearn = millAfterLearn - millBeforeLearn;

//            var methods = new List<SyntaxNodeOrToken>();
//            if (_solutionPath == null)
//            {
//                //Run program
//                methods = GetNodesByType(inpTree, _kinds);
//            }
//            else
//            {
//                string path = _expHome + _solutionPath;
//                var files = WorkspaceManager.GetInstance().GetSourcesFiles(null, path);
//                foreach (var v in files)
//                {
//                    var tree = CSharpSyntaxTree.ParseText(v.Item1, path: v.Item2).GetRoot();
//                    var vnodes = GetNodesByType(tree, _kinds);
//                    methods.AddRange(vnodes);
//                }
//            }

//            Transformed = new List<object>();
//            var dicTrans = new Dictionary<string, List<object>>();

//            long millBeforeExecution = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
//            foreach (var method in methods)
//            {
//                var newInputState = State.Create(_grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
//                object[] output = Program.Invoke(newInputState).ToEnumerable().ToArray();
//                Transformed.AddRange(output);
//                Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));

//                if (output.Any())
//                {
//                    if (dicTrans.ContainsKey(method.SyntaxTree.FilePath.ToUpperInvariant()))
//                    {
//                        dicTrans[method.SyntaxTree.FilePath.ToUpperInvariant()].AddRange(output);
//                    }
//                    else
//                    {
//                        dicTrans[method.SyntaxTree.FilePath.ToUpperInvariant()] = output.ToList();
//                    }
//                }
//            }
//            long millAfterExecution = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
//            TotalTimeToExecute = millAfterExecution - millBeforeExecution;

//            string s = "";
//            foreach (var v in dicTrans)
//            {
//                s += $"{v.Key} \n====================\n";
//                v.Value.ForEach(o => s += $"{o}\n");
//            }

//            Console.WriteLine($"Count: \n {Transformed.Count}");
//            s += $"Count: \n {Transformed.Count}";
//            FileUtil.WriteToFile(_expHome + @"cprose\" + _commit + @"\result"+_execId+".res", s);
//        }

//        public List<ProgramNode> LearnPrograms(List<int> examples)
//        {
//            var metadataRegions = examples.Select(index => _regions[index]).ToList();
//            var locationRegions = examples.Select(index => _locations[index]).ToList();

//            DictionarySelection = RegionManager.GetInstance().GroupRegionBySourcePath(metadataRegions);
//            //Examples
//            var examplesInput = new List<SyntaxNodeOrToken>();
//            var examplesOutput = new List<SyntaxNodeOrToken>();

//            SyntaxNodeOrToken inpTree = default(SyntaxNodeOrToken);
//            //building example methods
//            var ioExamples = new Dictionary<State, IEnumerable<object>>();
//            foreach (KeyValuePair<string, List<Region>> entry in DictionarySelection)
//            {
//                string sourceCode = FileUtil.ReadFile(entry.Key);
//                Tuple<string, List<Region>> tu = Transform(sourceCode, _globalTransformations[entry.Key.ToUpperInvariant()], metadataRegions);
//                string sourceCodeAfter = tu.Item1;

//                inpTree = CSharpSyntaxTree.ParseText(sourceCode, path: entry.Key).GetRoot();
//                SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(sourceCodeAfter).GetRoot();

//                var allMethodsInput = GetNodesByType(inpTree, _kinds);
//                var allMethodsOutput = GetNodesByType(outTree, _kinds);
//                var inputMethods = new List<int>();
//                foreach (var region in locationRegions.Where(o => o.SourceClass.ToUpperInvariant().Equals(entry.Key.ToUpperInvariant())))
//                {
//                    for (int index = 0; index < allMethodsInput.Count; index++)
//                    {
//                        var method = allMethodsInput[index];
//                        var tRegion = new Region
//                        {
//                            Start = method.SpanStart,
//                            Length = method.Span.Length
//                        };

//                        if (region.Region.IsInside(tRegion))
//                        {
//                            inputMethods.Add(index);
//                        }
//                    }
//                }

//                //Examples
//                var inpExs = inputMethods.Distinct().Select(index => allMethodsInput[index]).ToList();
//                var outExs = inputMethods.Distinct().Select(index => allMethodsOutput[index]).ToList();

//                examplesInput.AddRange(inpExs);
//                examplesOutput.AddRange(outExs);
//            }

//            examplesInput = RemoveDuplicates(examplesInput);
//            for (int index = 0; index < examplesInput.Count; index++)
//            {
//                var example = examplesInput.ElementAt(index);
//                var treeNode = ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)example);
//                var inputState = State.Create(_grammar.InputSymbol, new Node(treeNode));
//                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(index) });
//            }

//            //Learn program
//            var spec = DisjunctiveExamplesSpec.From(ioExamples);
//            var programList = Utils.LearnASet(_grammar, spec, new RankingScore(_grammar), new WitnessFunctions(_grammar));
//            return programList;
//        }

//        public void Execute(List<int> examples, ProgramNode program)
//        {
//            Program = program;
//            var metadataRegions = examples.Select(index => _regions[index]).ToList();
//            DictionarySelection = RegionManager.GetInstance().GroupRegionBySourcePath(metadataRegions);

//            SyntaxNodeOrToken inpTree = default(SyntaxNodeOrToken);
//            //building example methods
//            foreach (KeyValuePair<string, List<Region>> entry in DictionarySelection)
//            {
//                string sourceCode = FileUtil.ReadFile(entry.Key);
//                inpTree = CSharpSyntaxTree.ParseText(sourceCode, path: entry.Key).GetRoot();       
//            }

//            var methods = new List<SyntaxNodeOrToken>();
//            if (_solutionPath == null)
//            {
//                //Run program
//                methods = GetNodesByType(inpTree, _kinds);
//            }
//            else
//            {
//                string path = _expHome + _solutionPath;
//                var files = WorkspaceManager.GetInstance().GetSourcesFiles(null, path);
//                foreach (var v in files)
//                {
//                    var tree = CSharpSyntaxTree.ParseText(v.Item1, path: v.Item2).GetRoot();
//                    var vnodes = GetNodesByType(tree, _kinds);
//                    methods.AddRange(vnodes);
//                }
//            }

//            Transformed = new List<object>();
//            var dicTrans = new Dictionary<string, List<object>>();

//            long millBeforeExecution = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
//            foreach (var method in methods)
//            {
//                var newInputState = State.Create(_grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
//                object[] output = Program.Invoke(newInputState).ToEnumerable().ToArray();
//                Transformed.AddRange(output);
//                Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));

//                if (output.Any())
//                {
//                    if (dicTrans.ContainsKey(method.SyntaxTree.FilePath.ToUpperInvariant()))
//                    {
//                        dicTrans[method.SyntaxTree.FilePath.ToUpperInvariant()].AddRange(output);
//                    }
//                    else
//                    {
//                        dicTrans[method.SyntaxTree.FilePath.ToUpperInvariant()] = output.ToList();
//                    }
//                }
//            }
//            long millAfterExecution = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
//            TotalTimeToExecute = millAfterExecution - millBeforeExecution;

//            string s = "";
//            foreach (var v in dicTrans)
//            {
//                s += $"{v.Key} \n====================\n";
//                v.Value.ForEach(o => s += $"{o}\n");
//            }

//            Console.WriteLine($"Count: \n {Transformed.Count}");
//            s += $"Count: \n {Transformed.Count}";
//            FileUtil.WriteToFile(_expHome + @"cprose\" + _commit + @"\result" + _execId + ".res", s);
//        }


//        private static List<SyntaxNodeOrToken> GetNodesByType(SyntaxNodeOrToken outTree, List<SyntaxKind> kinds)
//        {
//            //select nodes of type method declaration
//            var exampleMethodsInput = from inode in outTree.AsNode().DescendantNodesAndSelf()
//                where kinds.Where(inode.IsKind).Any()
//                select inode;
//            //Select two examples
//            var examplesSotInput = exampleMethodsInput.Select(sot => (SyntaxNodeOrToken)sot).ToList();
//            //.GetRange(0, 1);
//            //var examplesInput = examplesSotInput.Select(o => (object)o).ToList();
//            return examplesSotInput;
//        }

//        private static List<SyntaxNodeOrToken> RemoveDuplicates(List<SyntaxNodeOrToken> methods)
//        {
//            var removes = new List<SyntaxNodeOrToken>();
//            for (int i = 0; i < methods.Count; i++)
//            {
//                for (int j = 0; j < methods.Count; j++)
//                {
//                    if (i != j &&
//                        methods[i].SyntaxTree.FilePath.ToUpperInvariant()
//                            .Equals(methods[j].SyntaxTree.FilePath.ToUpperInvariant()) &&
//                        methods[i].Span.Contains(methods[j].Span))
//                    {
//                        removes.Add(methods[j]);
//                    }
//                }
//            }

//            var result = methods.Except(removes).ToList();
//            return result;
//        }

//        public static Tuple<string, List<Region>> Transform(string source, List<CodeTransformation> transformations, List<Region> regions)
//        {
//            List<Region> tRegions = new List<Region>();
//            int nextStart = 0;
//            string sourceCode = source;
//            foreach (CodeTransformation item in transformations)
//            {
//                Tuple<Region, Region> tregion = GetTRegionShift(regions, item);
//                Region region = tregion.Item1;
//                string BeforeAfter = tregion.Item2.Text;

//                int start = nextStart + region.Start;
//                int end = start + region.Length;
//                var sourceCodeUntilStart = sourceCode.Substring(0, start);
//                var sourceCodeAfterSelection = sourceCode.Substring(end);
//                sourceCode = sourceCodeUntilStart + BeforeAfter + sourceCodeAfterSelection;

//                Region tr = new Region();
//                tr.Start = start - 1;
//                tr.Length = tregion.Item2.Length + 2;
//                tr.Text = tregion.Item2.Text;
//                tr.Path = tregion.Item1.Path;
//                tRegions.Add(tr);

//                nextStart += BeforeAfter.Length - region.Length;
//            }
//            Tuple<string, List<Region>> t = Tuple.Create(sourceCode, tRegions);
//            return t;
//            //return sourceCode;
//        }

//        private static Tuple<Region, Region> GetTRegionShift(List<Region> regions, CodeTransformation codeTransformation)
//        {
//            Tuple<Region, Region> t;
//            foreach (Region tr in regions)
//            {
//                if (codeTransformation.Trans.Equals(tr))
//                {
//                    Region region = new Region();
//                    region.Start = tr.Start;
//                    region.Length = tr.Length - 2;
//                    region.Path = tr.Path;
//                    region.Text = tr.Text;
//                    t = Tuple.Create(codeTransformation.Location.Region, region);
//                    return t;
//                }
//            }
//            t = Tuple.Create(codeTransformation.Location.Region, codeTransformation.Location.Region);
//            return t;
//        }
//    }
//}


//Atualizado 13/10

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.AST;
using Microsoft.ProgramSynthesis.Specifications;
using Microsoft.ProgramSynthesis.Utils;
using RefazerFunctions;
using RefazerFunctions.Bean;
using RefazerManager;
using Spg.LocationRefactor.Location;
using TreeElement;
using TreeElement.Spg.Node;
using WorkSpaces.Spg.Workspace;
using Region = RefazerObject.Region.Region;
using System.IO;
using Controller;
using TreeEdit.Spg.Transform;

namespace RefazerUnitTests
{
    class TestHelper
    {
        /// <summary>
        /// Grammar
        /// </summary>
        private readonly Grammar _grammar;
        /// <summary>
        /// Execution id
        /// </summary>
        private readonly string _execId;
        /// <summary>
        /// Regions
        /// </summary>
        private readonly List<Tuple<Region, string, string>> _transformedRegions;
        /// <summary>
        /// Global transformations
        /// </summary>
        private readonly Dictionary<string, List<Tuple<Region, string, string>>> _globalTransformations;
        /// <summary>
        /// Path to experiment
        /// </summary>
        private readonly string _expHome;
        /// <summary>
        /// Solution path
        /// </summary>
        private readonly string _solutionPath;
        /// <summary>
        /// Commit Id
        /// </summary>
        private readonly string _commit;
        /// <summary>
        /// Kinds
        /// </summary>
        private readonly List<SyntaxKind> _kinds;
        /// <summary>
        /// Folder to be edited
        /// </summary>
        private string _fileFolder;

        /// <summary>
        /// Total time to execute
        /// </summary>
        public long TotalTimeToExecute { get; set; }
        /// <summary>
        /// Total time to learn
        /// </summary>
        public long TotalTimeToLearn { get; set; }
        /// <summary>
        /// Transformed
        /// </summary>
        public List<object> Transformed { get; set; }
        /// <summary>
        /// Learned program
        /// </summary>
        public ProgramNode Program { get; set; }
        /// <summary>
        /// Dictionary selection
        /// </summary>
        public Dictionary<string, List<Tuple<Region, string, string>>> DictionarySelection { get; set; }

        public TestHelper(Grammar grammar, List<Tuple<Region, string, string>> transformedRegions, 
            Dictionary<string, List<Tuple<Region, string, string>>> globalTransformations, 
            string expHome, string solutionPath, string commit, 
            List<SyntaxKind> kinds, string fileFolder, string execId)
        {
            _grammar = grammar;
            _transformedRegions = transformedRegions;
            _globalTransformations = globalTransformations;
            _expHome = expHome;
            _solutionPath = solutionPath;
            _commit = commit;
            _kinds = kinds;
            _fileFolder = fileFolder;
            _execId = execId;
            WorkspaceManager.GetInstance().solutionPath = _expHome + _solutionPath;
        }

        /*public List<ProgramNode> LearnPrograms(List<int> examples)
        {
            var metadataRegions = examples.Select(index => _transformedRegions[index]).ToList();
            DictionarySelection = RegionManager.GetInstance().GroupTransformationsBySourcePath(metadataRegions);
            //Examples
            var examplesInput = new List<SyntaxNodeOrToken>();
            var examplesOutput = new List<SyntaxNodeOrToken>();

            SyntaxNodeOrToken inpTree = default(SyntaxNodeOrToken);
            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (KeyValuePair<string, List<Tuple<Region, string, string>>> entry in DictionarySelection)
            {
                string sourceCode = FileUtil.ReadFile(_expHome + entry.Key);
                Tuple<string, List<Region>> tu = Transform(sourceCode, _globalTransformations[entry.Key.ToUpperInvariant()], metadataRegions);
                string sourceCodeAfter = tu.Item1;

                inpTree = CSharpSyntaxTree.ParseText(sourceCode, path: entry.Key).GetRoot();
                SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(sourceCodeAfter).GetRoot();

                var allMethodsInput = GetNodesByType(inpTree, _kinds);
                var allMethodsOutput = GetNodesByType(outTree, _kinds);
                var inputMethods = new List<int>();
                foreach (var region in metadataRegions.Where(o => o.Item1.Path.ToUpperInvariant().Equals(entry.Key.ToUpperInvariant())))
                {
                    for (int index = 0; index < allMethodsInput.Count; index++)
                    {
                        var method = allMethodsInput[index];
                        var tRegion = new Region
                        {
                            Start = method.SpanStart,
                            Length = method.Span.Length
                        };

                        if (region.Item1.IsInside(tRegion))
                        {
                            inputMethods.Add(index);
                        }
                    }
                }
                //Examples
                var inpExs = inputMethods.Distinct().Select(index => allMethodsInput[index]).ToList();
                var outExs = inputMethods.Distinct().Select(index => allMethodsOutput[index]).ToList();

                examplesInput.AddRange(inpExs);
                examplesOutput.AddRange(outExs);
            }

            examplesInput = RemoveDuplicates(examplesInput);
            for (int index = 0; index < examplesInput.Count; index++)
            {
                var example = examplesInput.ElementAt(index);
                var treeNode = ConverterHelper.ConvertCSharpToTreeNode((SyntaxNodeOrToken)example);
                var inputState = State.Create(_grammar.InputSymbol, new Node(treeNode));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(index) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            int numberOfExamples = ioExamples.Count;
            var programList = Utils.LearnASet(_grammar, spec, new RankingScore(_grammar, numberOfExamples), new WitnessFunctions(_grammar));
            return programList;
        }*/

        private static List<SyntaxNodeOrToken> GetNodesByType(SyntaxNodeOrToken outTree, List<SyntaxKind> kinds)
        {
            //select nodes of type method declaration
            var exampleMethodsInput = from inode in outTree.AsNode().DescendantNodesAndSelf()
                                      where kinds.Where(inode.IsKind).Any()
                                      select inode;
            //Select two examples
            var examplesSotInput = exampleMethodsInput.Select(sot => (SyntaxNodeOrToken)sot).ToList();
            //.GetRange(0, 1);
            //var examplesInput = examplesSotInput.Select(o => (object)o).ToList();
            return examplesSotInput;
        }

        private static Tuple<List<SyntaxNodeOrToken>, List<SyntaxNodeOrToken>> RemoveDuplicates(List<SyntaxNodeOrToken> methodsInput, List<SyntaxNodeOrToken> methodsOuput)
        {
            var removesInput = new List<SyntaxNodeOrToken>();
            var removesOuput = new List<SyntaxNodeOrToken>();
            for (int i = 0; i < methodsInput.Count; i++)
            {
                for (int j = 0; j < methodsInput.Count; j++)
                {
                    if (i != j &&
                        methodsInput[i].SyntaxTree.FilePath.ToUpperInvariant()
                            .Equals(methodsInput[j].SyntaxTree.FilePath.ToUpperInvariant()) &&
                        methodsInput[i].Span.Contains(methodsInput[j].Span))
                    {
                        removesInput.Add(methodsInput[j]);
                        removesOuput.Add(methodsOuput[j]);
                    }
                }
            }
            var resultInput = methodsInput.Except(removesInput).ToList();
            var resultOuput = methodsOuput.Except(removesOuput).ToList();
            var result = Tuple.Create(resultInput, resultOuput);
            return result;
        }

        /*private static Tuple<Region, Region> GetTRegionShift(List<Tuple<Region, string, string>> regions, Tuple<Region, string, string> codeTransformation)
        {
            Tuple<Region, Region> t;
            foreach (Tuple<Region, string, string> ba in regions)
            {
                Region tr = ba.Item1;
                if (codeTransformation.Item1.Equals(tr))
                {
                    Region region = new Region();
                    region.Start = tr.Start;
                    region.Length = tr.Length - 2;
                    region.Path = tr.Path;
                    region.Text = ba.Item2;
                    t = Tuple.Create(tr, region);
                    return t;
                }
            }
            t = Tuple.Create(codeTransformation.Item1, codeTransformation.Item1);
            return t;
        }*/

        public void Execute(List<int> examples)
        {
            Program = LearnPrograms(examples).First();
            RunProgram(examples, Program);
        }

        public void Execute(List<int> positiveExamples, List<Region> negativeExamples)
        {
            var programSet = LearnPrograms(positiveExamples, negativeExamples);
            //List<ProgramNode> newProgramList = FilterPrograms(negativeExamples, programSet);
            //Program = newProgramList.First();
            Program = programSet.First();
            RunProgram(positiveExamples, Program);
        }

        private List<ProgramNode> FilterPrograms(List<Region> negativeExamples, List<ProgramNode> programSet)
        {
            var newMetadataRegions = new List<Tuple<Region, string, string>>();
            foreach (var region in negativeExamples)
            {
                var tuple = Tuple.Create<Region, string, string>(region, null, region.Path);
                newMetadataRegions.Add(tuple);
            }
            var inputOutputToTest = buildExamples(newMetadataRegions);
            var newProgramList = new List<ProgramNode>();
            foreach (var program in programSet)
            {
                bool include = true;
                foreach (var example in inputOutputToTest)
                {
                    program.Invoke(example.Key);
                    include = evaluateProgram(negativeExamples);
                    if (!include)
                    {
                        break;
                    }
                }
                if (include)
                {
                    newProgramList.Add(program);
                }
            }

            return newProgramList;
        }

        public bool evaluateProgram(List<Region> negativeExamples)
        {
            var locations = RefazerController.GetInstance().GetLocations();
            foreach (var location in locations)
            {
                Region region = new Region();
                region.Start = location.SpanStart;
                region.Length = location.Span.Length;
                region.Path = location.SyntaxTree.FilePath;
                foreach (var negativeExample in negativeExamples)
                {
                    if (negativeExample.IntersectWith(region) && region.IntersectWith(negativeExample))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public List<ProgramNode> LearnPrograms(List<int> positives, List<Region> negatives)
        {
            var metadataRegions = positives.Select(index => _transformedRegions[index]).ToList();
            var ioExamples = buildExamples(metadataRegions, negatives);
            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            long millBeforeLearn = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            var programs = Utils.LearnASet(_grammar, spec, new RankingScore(_grammar, positives.Count), new WitnessFunctions(_grammar));
            long millAfterLearn = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            TotalTimeToLearn = millAfterLearn - millBeforeLearn;
            return programs;
        }

        public List<ProgramNode> LearnPrograms(List<int> examples)
        {
            var metadataRegions = examples.Select(index => _transformedRegions[index]).ToList();
            var ioExamples = buildExamples(metadataRegions);
            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            long millBeforeLearn = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            var programs = Utils.LearnASet(_grammar, spec, new RankingScore(_grammar, examples.Count), new WitnessFunctions(_grammar));
            long millAfterLearn = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            TotalTimeToLearn = millAfterLearn - millBeforeLearn;
            return programs;
        }

        public Dictionary<State, IEnumerable<object>> buildExamples(
            List<Tuple<Region, string, string>> metadataRegions, List<Region> negatives)
        {
            var metataNegatives = negatives.Select(o => Tuple.Create(o, o.Text, o.Path.ToUpperInvariant())).ToList();
            //Get positive examples
            var examples = buildExamples(metadataRegions);
            //Get negative examples
            var negativeExamples = beforeAfterNode(metataNegatives);
            for (var indexi = 0; indexi < negatives.Count; indexi++)
            {
                var region = negatives.ElementAt(indexi);
                SyntaxNodeOrToken parent = null;
                for (int index = 0; index < negativeExamples.ToList().Count; index++)
                {
                    var example = negativeExamples.ToList().ElementAt(index);
                    var nodeExample = example.Item1;
                    var parentRegion = new Region();
                    parentRegion.Start = nodeExample.SpanStart;
                    parentRegion.Length = nodeExample.Span.Length;
                    parentRegion.Path = nodeExample.SyntaxTree.FilePath.ToUpperInvariant();
                    if (region.IsInside(parentRegion))
                    {
                        parent = nodeExample;
                        break;
                    }
                }
                if (parent != null)
                {
                    var treeNode = ConverterHelper.ConvertCSharpToTreeNode(parent);
                    var node = new Node(treeNode);
                    node.Region = region;
                    node.Kind = Node.ExampleKind.Negative;
                    var inputState = State.Create(_grammar.InputSymbol, node);
                    examples.Add(inputState, new List<object> { region });
                }
                else
                {
                    throw new Exception("Region is not present in the code.");
                }
            }
            return examples;
        }

        public Dictionary<State, IEnumerable<object>> buildExamples(List<Tuple<Region, string, string>> metadataRegions)
        {
            DictionarySelection = RegionManager.GetInstance().GroupTransformationsBySourcePath(metadataRegions);
            //Examples
            var examplesInput = new List<SyntaxNodeOrToken>();
            var examplesOutput = new List<SyntaxNodeOrToken>();
            SyntaxNodeOrToken inpTree = default(SyntaxNodeOrToken);
            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (KeyValuePair<string, List<Tuple<Region, string, string>>> entry in DictionarySelection)
            {
                string sourceCode = FileUtil.ReadFile(_expHome + entry.Key);
                string sourceCodeAfter = ASTTransformer.Transform(sourceCode, metadataRegions);
                inpTree = CSharpSyntaxTree.ParseText(sourceCode, path: entry.Key).GetRoot();
                SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(sourceCodeAfter).GetRoot();
                var allMethodsInput = GetNodesByType(inpTree, _kinds);
                var allMethodsOutput = GetNodesByType(outTree, _kinds);
                var inputMethods = inputEntriesForRegions(inpTree, metadataRegions, entry.Key);
                //Examples
                var inpExs = inputMethods.Distinct().Select(index => allMethodsInput[index]).ToList();
                var outExs = inputMethods.Distinct().Select(index => allMethodsOutput[index]).ToList();

                examplesInput.AddRange(inpExs);
                examplesOutput.AddRange(outExs);
            }
            var removeDup = RemoveDuplicates(examplesInput, examplesOutput);
            examplesInput = removeDup.Item1;
            examplesOutput = removeDup.Item2;
            for (int index = 0; index < examplesInput.Count; index++)
            {
                var example = examplesInput.ElementAt(index);
                var treeNode = ConverterHelper.ConvertCSharpToTreeNode(example);
                var node = new Node(treeNode);
                node.Kind = Node.ExampleKind.Positive;
                var inputState = State.Create(_grammar.InputSymbol, node);
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(index) });
            }
            return ioExamples;
        }

        public List<Tuple<SyntaxNodeOrToken, SyntaxNodeOrToken>> beforeAfterNode(List<Tuple<Region, string, string>> metadataRegions)
        {
            DictionarySelection = RegionManager.GetInstance().GroupTransformationsBySourcePath(metadataRegions);
            //Examples
            var examplesInput = new List<SyntaxNodeOrToken>();
            var examplesOutput = new List<SyntaxNodeOrToken>();
            SyntaxNodeOrToken inpTree = default(SyntaxNodeOrToken);
            //building example methods
            var ioExamples = new List<Tuple<SyntaxNodeOrToken, SyntaxNodeOrToken>>();
            foreach (KeyValuePair<string, List<Tuple<Region, string, string>>> entry in DictionarySelection)
            {
                string sourceCode = FileUtil.ReadFile(_expHome + entry.Key);
                string sourceCodeAfter = ASTTransformer.Transform(sourceCode, metadataRegions);
                inpTree = CSharpSyntaxTree.ParseText(sourceCode, path: entry.Key).GetRoot();
                SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(sourceCodeAfter).GetRoot();
                var allMethodsInput = GetNodesByType(inpTree, _kinds);
                var allMethodsOutput = GetNodesByType(outTree, _kinds);
                var inputMethods = inputEntriesForRegions(inpTree, metadataRegions, entry.Key);
                //Examples
                var inpExs = inputMethods.Distinct().Select(index => allMethodsInput[index]).ToList();
                var outExs = inputMethods.Distinct().Select(index => allMethodsOutput[index]).ToList();

                examplesInput.AddRange(inpExs);
                examplesOutput.AddRange(outExs);
            }
            var removeDup = RemoveDuplicates(examplesInput, examplesOutput);
            examplesInput = removeDup.Item1;
            examplesOutput = removeDup.Item2;
            for (int index = 0; index < examplesInput.Count; index++)
            {
                ioExamples.Add(Tuple.Create(examplesInput.ElementAt(index), examplesOutput.ElementAt(index)));
            }
            return ioExamples;
        }

        public List<int> inputEntriesForRegions(SyntaxNodeOrToken inpTree, List<Tuple<Region, string, string>> metadataRegions, string path)
        {
            var allMethodsInput = GetNodesByType(inpTree, _kinds);
            var inputMethods = new List<int>();
            foreach (var region in metadataRegions.Where(o => o.Item1.Path.ToUpperInvariant().Equals(path.ToUpperInvariant())))
            {
                for (int index = 0; index < allMethodsInput.Count; index++)
                {
                    var method = allMethodsInput[index];
                    var tRegion = new Region
                    {
                        Start = method.SpanStart,
                        Length = method.Span.Length
                    };
                    if (region.Item1.IsInside(tRegion))
                    {
                        inputMethods.Add(index);
                    }
                }
            }
            return inputMethods;
        }

        public void RunProgram(List<int> examples,  ProgramNode program)
        {
            var methods = new List<SyntaxNodeOrToken>();
            if (_solutionPath != null)
            {
                string path = _expHome + _solutionPath;
                var files = WorkspaceManager.GetInstance().GetSourcesFiles(null, path);
                foreach (var v in files)
                {
                    var tree = CSharpSyntaxTree.ParseText(v.Item1, path: v.Item2).GetRoot();
                    var vnodes = GetNodesByType(tree, _kinds);
                    methods.AddRange(vnodes);
                }
            }
            else if (_fileFolder != null)
            {
                string[] files = Directory.GetFiles(_expHome + _fileFolder, "*.cs");
                foreach (var v in files)
                {
                    var text = FileUtil.ReadFile(v);
                    var tree = CSharpSyntaxTree.ParseText(text, path: v).GetRoot();
                    var vnodes = GetNodesByType(tree, _kinds);
                    methods.AddRange(vnodes);
                }
            }
            else
            {
                var metadataRegions = examples.Select(index => _transformedRegions[index]).ToList();
                DictionarySelection = RegionManager.GetInstance().GroupTransformationsBySourcePath(metadataRegions);
                var entry = DictionarySelection.First();
                string sourceCode = FileUtil.ReadFile(_expHome + entry.Key);
                var inpTree = CSharpSyntaxTree.ParseText(sourceCode, path: entry.Key).GetRoot();
                //Run program
                methods = GetNodesByType(inpTree, _kinds);
            }
            Transformed = new List<object>();
            var dicTrans = new Dictionary<string, List<object>>();

            long millBeforeExecution = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            foreach (var method in methods)
            {
                var newInputState = State.Create(_grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
                object[] output = program.Invoke(newInputState).ToEnumerable().ToArray();
                Transformed.AddRange(output);
                Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));

                if (output.Any())
                {
                    if (dicTrans.ContainsKey(method.SyntaxTree.FilePath.ToUpperInvariant()))
                    {
                        dicTrans[method.SyntaxTree.FilePath.ToUpperInvariant()].AddRange(output);
                    }
                    else
                    {
                        dicTrans[method.SyntaxTree.FilePath.ToUpperInvariant()] = output.ToList();
                    }
                }
            }
            long millAfterExecution = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            TotalTimeToExecute = millAfterExecution - millBeforeExecution;

            string s = "";
            foreach (var v in dicTrans)
            {
                s += $"{v.Key} \n====================\n";
                v.Value.ForEach(o => s += $"{o}\n");
            }
            Console.WriteLine($"Count: \n {Transformed.Count}");
            s += $"Count: \n {Transformed.Count}";
            FileUtil.WriteToFile(_expHome + @"cprose\" + _commit + @"\result" + _execId + ".res", s);
        }

        public void Execute(List<int> examples, ProgramNode program)
        {
            Program = program;
            var metadataRegions = examples.Select(index => _transformedRegions[index]).ToList();
            DictionarySelection = RegionManager.GetInstance().GroupTransformationsBySourcePath(metadataRegions);
            //Examples
            var examplesInput = new List<SyntaxNodeOrToken>();
            var examplesOutput = new List<SyntaxNodeOrToken>();

            SyntaxNodeOrToken inpTree = default(SyntaxNodeOrToken);
            //building example methods
            var ioExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (KeyValuePair<string, List<Tuple<Region, string, string>>> entry in DictionarySelection)
            {
                string sourceCode = FileUtil.ReadFile(_expHome + entry.Key);
                string sourceCodeAfter = ASTTransformer.Transform(sourceCode, metadataRegions);
                inpTree = CSharpSyntaxTree.ParseText(sourceCode, path: entry.Key).GetRoot();
                SyntaxNodeOrToken outTree = CSharpSyntaxTree.ParseText(sourceCodeAfter).GetRoot();
                var allMethodsInput = GetNodesByType(inpTree, _kinds);
                var allMethodsOutput = GetNodesByType(outTree, _kinds);
                var inputMethods = new List<int>();
                foreach (var region in metadataRegions.Where(o => o.Item1.Path.ToUpperInvariant().Equals(entry.Key.ToUpperInvariant())))
                {
                    for (int index = 0; index < allMethodsInput.Count; index++)
                    {
                        var method = allMethodsInput[index];
                        var tRegion = new Region
                        {
                            Start = method.SpanStart,
                            Length = method.Span.Length
                        };

                        if (region.Item1.IsInside(tRegion))
                        {
                            inputMethods.Add(index);
                        }
                    }
                }
                //Examples
                var inpExs = inputMethods.Distinct().Select(index => allMethodsInput[index]).ToList();
                var outExs = inputMethods.Distinct().Select(index => allMethodsOutput[index]).ToList();

                examplesInput.AddRange(inpExs);
                examplesOutput.AddRange(outExs);
            }
            var removeDup = RemoveDuplicates(examplesInput, examplesOutput);
            examplesInput = removeDup.Item1;
            examplesOutput = removeDup.Item2;
            for (int index = 0; index < examplesInput.Count; index++)
            {
                var example = examplesInput.ElementAt(index);
                var treeNode = ConverterHelper.ConvertCSharpToTreeNode(example);
                var inputState = State.Create(_grammar.InputSymbol, new Node(treeNode));
                ioExamples.Add(inputState, new List<object> { examplesOutput.ElementAt(index) });
            }

            //Learn program
            var spec = DisjunctiveExamplesSpec.From(ioExamples);
            var methods = new List<SyntaxNodeOrToken>();
            if (_solutionPath != null)
            {
                string path = _expHome + _solutionPath;
                var files = WorkspaceManager.GetInstance().GetSourcesFiles(null, path);
                foreach (var v in files)
                {
                    var tree = CSharpSyntaxTree.ParseText(v.Item1, path: v.Item2).GetRoot();
                    var vnodes = GetNodesByType(tree, _kinds);
                    methods.AddRange(vnodes);
                }
            }
            else if (_fileFolder != null)
            {
                string[] files = Directory.GetFiles(_expHome + _fileFolder, "*.cs");
                foreach (var v in files)
                {
                    var text = FileUtil.ReadFile(v);
                    var tree = CSharpSyntaxTree.ParseText(text, path: v).GetRoot();
                    var vnodes = GetNodesByType(tree, _kinds);
                    methods.AddRange(vnodes);
                }
            }
            else
            {
                //Run program
                methods = GetNodesByType(inpTree, _kinds);
            }
            Transformed = new List<object>();
            var dicTrans = new Dictionary<string, List<object>>();

            long millBeforeExecution = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            foreach (var method in methods)
            {
                var newInputState = State.Create(_grammar.InputSymbol, new Node(ConverterHelper.ConvertCSharpToTreeNode(method)));
                object[] output = Program.Invoke(newInputState).ToEnumerable().ToArray();
                Transformed.AddRange(output);
                Utils.WriteColored(ConsoleColor.DarkCyan, output.DumpCollection(openDelim: "", closeDelim: "", separator: "\n"));

                if (output.Any())
                {
                    if (dicTrans.ContainsKey(method.SyntaxTree.FilePath.ToUpperInvariant()))
                    {
                        dicTrans[method.SyntaxTree.FilePath.ToUpperInvariant()].AddRange(output);
                    }
                    else
                    {
                        dicTrans[method.SyntaxTree.FilePath.ToUpperInvariant()] = output.ToList();
                    }
                }
            }
            long millAfterExecution = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            TotalTimeToExecute = millAfterExecution - millBeforeExecution;

            string s = "";
            foreach (var v in dicTrans)
            {
                s += $"{v.Key} \n====================\n";
                v.Value.ForEach(o => s += $"{o}\n");
            }

            Console.WriteLine($"Count: \n {Transformed.Count}");
            s += $"Count: \n {Transformed.Count}";
            FileUtil.WriteToFile(_expHome + @"cprose\" + _commit + @"\result" + _execId + ".res", s);
        }
    }
}