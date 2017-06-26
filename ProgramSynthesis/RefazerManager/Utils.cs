using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.AST;
using Microsoft.ProgramSynthesis.Compiler;
using Microsoft.ProgramSynthesis.Learning;
using Microsoft.ProgramSynthesis.Learning.Logging;
using Microsoft.ProgramSynthesis.Learning.Strategies;
using Microsoft.ProgramSynthesis.Specifications;

namespace RefazerManager
{
    public static class Utils
    {
        public static Grammar LoadGrammar(string grammarFile, params string[] prerequisiteGrammars)
        {
            foreach (string prerequisite in prerequisiteGrammars)
            {
                var options = new CompilerOptions { InputGrammar = prerequisite };
                var buildResult = DSLCompiler.Compile(options);
                if (!buildResult.HasErrors) continue;
                WriteColored(ConsoleColor.Magenta, buildResult.TraceDiagnostics);
                return null;
            }

            var compilationResult = DSLCompiler.ParseGrammarFromFile(grammarFile);
            if (compilationResult.HasErrors)
            {
                WriteColored(ConsoleColor.Magenta, compilationResult.TraceDiagnostics);
                WriteColored(ConsoleColor.Magenta, String.Join("\n", compilationResult.Diagnostics));
                return null;
            }
            if (compilationResult.Diagnostics.Count > 0)
            {
                WriteColored(ConsoleColor.Yellow, compilationResult.TraceDiagnostics);
            }

            return compilationResult.Value;
        }

        /// <summary>
        /// Learns one program transformations
        /// </summary>
        /// <param name="grammar">Grammar</param>
        /// <param name="spec">Example-based specification</param>
        /// <param name="scorer">Ranking functions</param>
        /// <param name="witnessFunctions">Back-propagation functions</param>
        public static ProgramNode Learn(Grammar grammar, Spec spec,
                                         Feature<double> scorer, DomainLearningLogic witnessFunctions)
        {
            var engine = new SynthesisEngine(grammar, new SynthesisEngine.Config
            {
                Strategies = new ISynthesisStrategy[]
                {
                    new EnumerativeSynthesis(),
                    new DeductiveSynthesis(witnessFunctions)
                },
                UseThreads = false,
                LogListener = new LogListener(),
            });

            var consistentPrograms = engine.LearnGrammar(spec);
            const ulong a = 100;
            var topK = consistentPrograms.Size < 201 ? consistentPrograms.RealizedPrograms.ToList() : consistentPrograms.TopK(scorer, 5).ToList();
            var b =  (ulong) topK.Count;
            topK = topK.OrderByDescending(o => o.GetFeatureValue(scorer)).ToList().GetRange(0, (int) Math.Min(a, b)).ToList();
            var programs = "";
            List<ProgramNode> validated = new List<ProgramNode>();
            foreach (ProgramNode p in topK)
            {
                var scorep = p.GetFeatureValue(scorer);
                programs += $"Score[{scorep}] " + p + "\n\n";          
                validated.Add(p);
            }

            string expHome = Environment.GetEnvironmentVariable("EXP_HOME", EnvironmentVariableTarget.User);
            string file = expHome + "programs.txt";
            File.WriteAllText(file, programs);

            ProgramNode bestProgram = validated.First();
            string stringprogram = bestProgram.ToString();
            var score = bestProgram.GetFeatureValue(scorer);
            WriteColored(ConsoleColor.Cyan, $"[score = {score:F3}] {stringprogram}");
            return bestProgram;
        }

        /// <summary>
        /// Learns a set of program transformations
        /// </summary>
        /// <param name="grammar">Grammar</param>
        /// <param name="spec">Example-based specification</param>
        /// <param name="scorer">Ranking functions</param>
        /// <param name="witnessFunctions">Back-propagation functions</param>
        public static List<ProgramNode> LearnASet(Grammar grammar, Spec spec,
                                         Feature<double> scorer, DomainLearningLogic witnessFunctions)
        {
            var engine = new SynthesisEngine(grammar, new SynthesisEngine.Config
            {
                Strategies = new ISynthesisStrategy[]
                {
                    new EnumerativeSynthesis(),
                    new DeductiveSynthesis(witnessFunctions)
                },
                UseThreads = false,
                LogListener = new LogListener(),
            });

            var consistentPrograms = engine.LearnGrammar(spec);
            const ulong a = 201;
            var topK = consistentPrograms.Size < 201 ? consistentPrograms.RealizedPrograms.ToList() : consistentPrograms.TopK(scorer, 5).ToList();
            var b = (ulong)topK.Count;
            topK = topK.OrderByDescending(o => o.GetFeatureValue(scorer)).ToList().GetRange(0, (int)Math.Min(a, b)).ToList();
            var programs = "";
            var validated = new List<ProgramNode>();
            foreach (var p in topK)
            {
                var scorep = p.GetFeatureValue(scorer);
                programs += $"Score[{scorep}] " + p + "\n\n";
                validated.Add(p);
            }

            var expHome = Environment.GetEnvironmentVariable("EXP_HOME", EnvironmentVariableTarget.User);
            var file = expHome + "programs.txt";
            File.WriteAllText(file, programs);

            return validated;
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


        #endregion Auxiliary methods
    }
}
