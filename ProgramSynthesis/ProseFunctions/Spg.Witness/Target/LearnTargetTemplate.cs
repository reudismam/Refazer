using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Specifications;
using ProseFunctions.Spg.Bean;
using ProseFunctions.Substrings;
using TreeEdit.Spg.Print;
using TreeEdit.Spg.Script;
using TreeElement.Spg.Node;

namespace ProseFunctions.Spg.Witness.Target
{
    public abstract class LearnTargetTemplate
    {
        /// <summary>
        /// Learn the node witness function
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">parameter</param>
        /// <param name="spec">specification</param>
        public ExampleSpec NodeLearner(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            var kExamples = new Dictionary<State, object>();
            foreach (State input in spec.ProvidedInputs)
            {
                var edit = (Edit<SyntaxNodeOrToken>)spec.Examples[input];
                var editOperation = edit.EditOperation;

                if (ProcessEditOperation(edit))
                {
                    var key = editOperation.T1Node.SyntaxTree;
                    var treeUp = WitnessFunctions.TreeUpdateDictionary[key];

                    var previousTree = ConverterHelper.MakeACopy(treeUp.CurrentTree);
                    treeUp.ProcessEditOperation(editOperation);
                    WitnessFunctions.CurrentTrees[key] = previousTree;
#if DEBUG
                    Console.WriteLine("PREVIOUS TREE\n\n");
                    PrintUtil<SyntaxNodeOrToken>.PrintPretty(previousTree, "", true);
                    Console.WriteLine("UPDATED TREE\n\n");
                    PrintUtil<SyntaxNodeOrToken>.PrintPretty(treeUp.CurrentTree, "", true);
#endif
                }

                var from = Target(edit);
                var result = EditOperation.GetNode(from);
                if (result == null)
                {
                    var inputTree = (Node)input[rule.Grammar.InputSymbol];
                   result = EditOperation.GetNode(inputTree.Value, from);
                }

                kExamples[input] = result;
            }
            return new ExampleSpec(kExamples);
        }

        protected abstract TreeNode<SyntaxNodeOrToken> Target(Edit<SyntaxNodeOrToken> edit);

        protected abstract bool ProcessEditOperation(Edit<SyntaxNodeOrToken> edit);
    }
}
