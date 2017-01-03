using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.AST;
using Microsoft.ProgramSynthesis.Compiler;
using Microsoft.ProgramSynthesis.Extraction.Text.Semantics;
using Microsoft.ProgramSynthesis.Learning;
using Microsoft.ProgramSynthesis.Learning.Logging;
using Microsoft.ProgramSynthesis.Learning.Strategies;
using Microsoft.ProgramSynthesis.Specifications;
using Microsoft.ProgramSynthesis.Utils;

namespace ProseFunctions
{
    public static class Utils
    {
        public static Grammar LoadGrammar(string grammarFile, params string[] prerequisiteGrammars)
        {
            foreach (string prerequisite in prerequisiteGrammars)
            {
                var buildResult = DSLCompiler.Compile(prerequisite, $"{prerequisite}.xml");
                if (buildResult.HasErrors)
                {
                    WriteColored(ConsoleColor.Magenta, buildResult.TraceDiagnostics);
                    return null;
                }
            }

            var compilationResult = DSLCompiler.LoadGrammarFromFile(grammarFile);
            if (compilationResult.HasErrors)
            {
                WriteColored(ConsoleColor.Magenta, compilationResult.TraceDiagnostics);
                return null;
            }
            if (compilationResult.Diagnostics.Count > 0)
            {
                WriteColored(ConsoleColor.Yellow, compilationResult.TraceDiagnostics);
            }
            return compilationResult.Value;
        }

        public static ProgramNode Learn(Grammar grammar, Spec spec)
        {
            var deductivecf = new DeductiveSynthesis.Config
            {
                PrereqProgramsThreshold = k => 1,
            };

            var engine = new SynthesisEngine(grammar, new SynthesisEngine.Config
            {
                UseThreads = false,
                LogListener = new LogListener(),
                //Strategies = new [] {typeof(DeductiveSynthesis.Config)},
            });

            var consistentPrograms = engine.LearnGrammar(spec);
            const ulong a = 10;
            var topK = consistentPrograms.Size < 20000 ? consistentPrograms.RealizedPrograms.ToList().ToList() : consistentPrograms.TopK("Score", 5).ToList();
            
            var b =  (ulong) topK.Count;
            topK = topK.OrderByDescending(o => o["Score"]).ToList().GetRange(0, (int) Math.Min(a, b)).ToList();
            //topK = consistentPrograms.RealizedPrograms.ToList();
            topK = topK.OrderByDescending(o => o["Score"]).ToList();
            var programs = "";
            List<ProgramNode> validated = new List<ProgramNode>();
            foreach (ProgramNode p in topK)
            {
                var scorep = p["Score"];
                programs += $"Score[{scorep}] " + p + "\n\n";          
                ////////Console.WriteLine($"Score[{scorep}] " + p + "\n");
                if (ValidateProgram(p, spec))
                {
                    validated.Add(p);
                }
            }

            File.WriteAllText(@"C:\Users\SPG-04\Desktop\programs.txt", programs);
            ProgramNode bestProgram = validated.First();
            string stringprogram = bestProgram.ToString();
            var score = bestProgram["Score"];
            WriteColored(ConsoleColor.Cyan, $"[score = {score:F3}] {bestProgram}");
            return bestProgram;
        }

        private static bool ValidateProgram(ProgramNode program, Spec spec)
        {
            foreach (var state in spec.ProvidedInputs)
            {
                //try
                //{
                    object[] output = program.Invoke(state).ToEnumerable().ToArray();
                //}
                //catch (Exception e)
                //{
                //    return false;
                //}
            }
            return true;
        }

        #region Auxiliary methods

        public static void WriteColored(ConsoleColor color, object obj)
            => WriteColored(color, () => Console.WriteLine(obj));

        public static void WriteColored(ConsoleColor color, Action write)
        {
            ConsoleColor currentColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            write();
            Console.ForegroundColor = currentColor;
        }

        private static readonly Regex ExampleRegex = new Regex(@"(?<=\{)[^}]+(?=\})", RegexOptions.Compiled);

        public static List<StringRegion> LoadBenchmark(string filename, out StringRegion document)
        {
            string content = File.ReadAllText(filename);
            Match[] examples = ExampleRegex.Matches(content).Cast<Match>().ToArray();
            document = StringRegion.Create(content.Replace("}", "").Replace("{", ""));
            var result = new List<StringRegion>();
            for (int i = 0, shift = -1; i < examples.Length; i++, shift -= 2)
            {
                int start = shift + examples[i].Index;
                int end = start + examples[i].Length;
                result.Add(document.Slice((uint) start, (uint) end));
            }
            return result;
        }

        #endregion Auxiliary methods
    }
}
