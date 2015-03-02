﻿using Spg.ExampleRefactoring.Expression;
using Spg.ExampleRefactoring.Synthesis;
using Spg.LocationRefactor.Operator;
using System;
using System.Collections.Generic;
using ExampleRefactoring.Spg.ExampleRefactoring.Expression;
using ExampleRefactoring.Spg.ExampleRefactoring.Setting;
using ExampleRefactoring.Spg.ExampleRefactoring.Synthesis;
using LocationCodeRefactoring.Spg.LocationRefactor.Learn;
using LocationCodeRefactoring.Spg.LocationRefactor.Program;

namespace Spg.LocationRefactor.Learn
{
    /// <summary>
    /// Learn a pair
    /// </summary>
    public class PairLearn:ILearn
    {
        public PairLearn() { 
        }

        /// <summary>
        /// Learn a Pair operator
        /// </summary>
        /// <param name="examples">Examples</param>
        /// <returns>Pair operators</returns>
        public List<Prog> Learn(List<Tuple<ListNode, ListNode>> examples) {
            SynthesizerSetting setting = new SynthesizerSetting(true, 2, false, true);
            ASTProgram program = new ASTProgram(setting, examples);
            //program.boundary = BoundaryManager.GetInstance().boundary;

            List<SynthesizedProgram> synthesizedProgs = program.GenerateStringProgram(examples);

            List<Prog> progs = new List<Prog>();
            foreach(SynthesizedProgram sprog in synthesizedProgs){
                foreach (IExpression solution in sprog.Solutions) {
                    Pair pair = new Pair();
                    Prog prog = new Prog();
                    if (solution is SubStr)
                    {
                        pair.expression = ((SubStr)solution);
                        prog.Ioperator = pair;
                        progs.Add(prog);
                    }
                }
            }
            return progs;
        }

        public List<Prog> Learn(List<Tuple<ListNode, ListNode>> positiveExamples, List<Tuple<ListNode, ListNode>> negativeExamples)
        {
            SynthesizerSetting setting = new SynthesizerSetting(true, 2, false, true);
            ASTProgram program = new ASTProgram(setting, positiveExamples);
            //program.boundary = BoundaryManager.GetInstance().boundary;

            List<SynthesizedProgram> synthesizedProgs = program.GenerateStringProgram(positiveExamples);

            List<Prog> progs = new List<Prog>();
            foreach (SynthesizedProgram sprog in synthesizedProgs)
            {
                foreach (IExpression solution in sprog.Solutions)
                {
                    Pair pair = new Pair();
                    Prog prog = new Prog();
                    if (solution is SubStr)
                    {
                        pair.expression = ((SubStr)solution);
                        prog.Ioperator = pair;
                        progs.Add(prog);
                    }
                }
            }
            return progs;
        }
    }
}
