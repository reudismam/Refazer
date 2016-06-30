﻿using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Specifications;
using TreeEdit.Spg.Print;
using TreeEdit.Spg.Script;
using TreeElement.Spg.Node;

namespace ProseSample.Substrings.Spg.Witness.Target
{
    public abstract class LearnTargetTemplate
    {
        public ExampleSpec NodeLearner(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            var kExamples = new Dictionary<State, object>();
            foreach (State input in spec.ProvidedInputs)
            {
                var edit = (Edit<SyntaxNodeOrToken>)spec.Examples[input];
                var editOperation = edit.EditOperation;

                var from = Target(edit);
                var result = new Node(from);
                kExamples[input] = result;

                var key = editOperation.T1Node.SyntaxTree;
                var treeUp = WitnessFunctions.TreeUpdateDictionary[key];

                var previousTree = ConverterHelper.MakeACopy(treeUp.CurrentTree);
                treeUp.ProcessEditOperation(editOperation);
                WitnessFunctions.CurrentTrees[key] = previousTree;

                Console.WriteLine("PREVIOUS TREE\n\n");
                PrintUtil<SyntaxNodeOrToken>.PrintPretty(previousTree, "", true);
                Console.WriteLine("UPDATED TREE\n\n");
                PrintUtil<SyntaxNodeOrToken>.PrintPretty(treeUp.CurrentTree, "", true);
            }

            return new ExampleSpec(kExamples);
        }

        protected abstract ITreeNode<SyntaxNodeOrToken> Target(Edit<SyntaxNodeOrToken> edit);
    }
}
