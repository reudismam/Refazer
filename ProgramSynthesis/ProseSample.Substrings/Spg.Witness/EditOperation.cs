using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Specifications;
using TreeEdit.Spg.Print;
using TreeEdit.Spg.Script;
using TreeEdit.Spg.TreeEdit.Update;
using TreeElement.Spg.Node;

namespace ProseSample.Substrings.Spg.Witness
{
    public class EditOperation
    {
        public static ExampleSpec DeleteString(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            var kExamples = new Dictionary<State, object>();
            foreach (State input in spec.ProvidedInputs)
            {
                var edit = (Edit<SyntaxNodeOrToken>)spec.Examples[input];
                var editOperation = edit.EditOperation;
                if (!(editOperation is Delete<SyntaxNodeOrToken>)) return null;
                kExamples[input] = "Delete";
            }
            return new ExampleSpec(kExamples);
        }

        public static ExampleSpec DeleteFrom(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            foreach (State input in spec.ProvidedInputs)
            {
                var edit = (Edit<SyntaxNodeOrToken>)spec.Examples[input];
                var editOperation = edit.EditOperation;
                if (!(editOperation is Delete<SyntaxNodeOrToken>)) return null;

                return T1Learner(rule, parameter, spec);
            }
            return null;
        }

        public static ExampleSpec UpdateFrom(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            foreach (State input in spec.ProvidedInputs)
            {
                var edit = (Edit<SyntaxNodeOrToken>)spec.Examples[input];
                var editOperation = edit.EditOperation;
                if (!(editOperation is Update<SyntaxNodeOrToken>)) return null;

                return T1Learner(rule, parameter, spec);
            }
            return null;
        }

        public static ExampleSpec T1Learner(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            var kExamples = new Dictionary<State, object>();
            foreach (State input in spec.ProvidedInputs)
            {
                var edit = (Edit<SyntaxNodeOrToken>)spec.Examples[input];
                var editOperation = edit.EditOperation;

                var from = editOperation.T1Node;
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

        public static ExampleSpec UpdateTo(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            var kExamples = new Dictionary<State, object>();
            foreach (State input in spec.ProvidedInputs)
            {
                var edit = (Edit<SyntaxNodeOrToken>)spec.Examples[input];
                var editOperation = edit.EditOperation;
                if (!(editOperation is Update<SyntaxNodeOrToken>)) return null;
                var update = (Update<SyntaxNodeOrToken>)editOperation;
                kExamples[input] = update.To;
            }
            return new ExampleSpec(kExamples);
        }

        public static ExampleSpec MoveFrom(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            foreach (State input in spec.ProvidedInputs)
            {
                var edit = (Edit<SyntaxNodeOrToken>)spec.Examples[input];
                var editOperation = edit.EditOperation;
                if (!(editOperation is Move<SyntaxNodeOrToken>)) return null;

                return ParentLearn(rule, parameter, spec);
            }
            return null;
        }

        public static ExampleSpec MoveTo(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            var kExamples = new Dictionary<State, object>();
            foreach (State input in spec.ProvidedInputs)
            {
                var edit = (Edit<SyntaxNodeOrToken>)spec.Examples[input];
                var editOperation = edit.EditOperation;
                if (!(editOperation is Move<SyntaxNodeOrToken>)) return null;
                var from = editOperation.T1Node;
                var fromNode = TreeUpdate.FindNode(from.SyntaxTree, from.Value);
                fromNode.SyntaxTree = from.SyntaxTree;
                var result = new Node(fromNode);
                kExamples[input] = result;
            }
            return new ExampleSpec(kExamples);
        }

        public static ExampleSpec MoveK(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            var kExamples = new Dictionary<State, object>();
            foreach (State input in spec.ProvidedInputs)
            {
                var edit = (Edit<SyntaxNodeOrToken>)spec.Examples[input];
                var editOperation = edit.EditOperation;
                if (!(editOperation is Move<SyntaxNodeOrToken>)) return null;
                kExamples[input] = editOperation.K;
            }
            return new ExampleSpec(kExamples);
        }

        public static ExampleSpec ParentLearn(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            var kExamples = new Dictionary<State, object>();
            foreach (State input in spec.ProvidedInputs)
            {
                var edit = (Edit<SyntaxNodeOrToken>)spec.Examples[input];
                var editOperation = edit.EditOperation;
                
                var key = editOperation.Parent.SyntaxTree;
                var treeUp = WitnessFunctions.TreeUpdateDictionary[key];
                var parent = editOperation.Parent;
                var result = new Node(parent); //todo resolve bug here. Similar to the move operation
                kExamples[input] = result;

                var previousTree = ConverterHelper.MakeACopy(treeUp.CurrentTree);
                treeUp.ProcessEditOperation(editOperation); //todo Refactor: print the nodes in a separated method.
                WitnessFunctions.CurrentTrees[key] = previousTree;
                Console.WriteLine("PREVIOUS TREE\n\n");
                PrintUtil<SyntaxNodeOrToken>.PrintPretty(previousTree, "", true);
                Console.WriteLine("UPDATED TREE\n\n");
                PrintUtil<SyntaxNodeOrToken>.PrintPretty(treeUp.CurrentTree, "", true);            
            }
            return new ExampleSpec(kExamples);
        }

        public static ExampleSpec InsertFrom(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            foreach (State input in spec.ProvidedInputs)
            {
                var edit = (Edit<SyntaxNodeOrToken>)spec.Examples[input];
                var editOperation = edit.EditOperation;
                if (!(editOperation is Insert<SyntaxNodeOrToken>)) return null;

                return ParentLearn(rule, parameter, spec);
            }
            return null;
        }
        public static ExampleSpec Insertast(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            var kExamples = new Dictionary<State, object>();
            foreach (State input in spec.ProvidedInputs)
            {
                var edit = (Edit<SyntaxNodeOrToken>)spec.Examples[input];
                var editOperation = edit.EditOperation;
                if (!(editOperation is Insert<SyntaxNodeOrToken>)) return null;

                kExamples[input] = editOperation.T1Node;
            }
            return new ExampleSpec(kExamples);
        }

        public static ExampleSpec InsertK(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            var kExamples = new Dictionary<State, object>();
            foreach (State input in spec.ProvidedInputs)
            {
                var edit = (Edit<SyntaxNodeOrToken>)spec.Examples[input];
                var editOperation = edit.EditOperation;
                if (!(editOperation is Insert<SyntaxNodeOrToken>)) return null;

                kExamples[input] = editOperation.K;
            }
            return new ExampleSpec(kExamples);
        }
    }
}
