using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Specifications;
using TreeEdit.Spg.Print;
using TreeEdit.Spg.Script;
using TreeElement.Spg.Node;

namespace ProseSample.Substrings.Spg.Witness
{
    public class EditOperation
    {
        public static DisjunctiveExamplesSpec DeleteFrom(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var kExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var matches = new List<object>();
                foreach (Edit<SyntaxNodeOrToken> edit in spec.DisjunctiveExamples[input])
                {
                    var editOperation = edit.EditOperation;
                    if (!(editOperation is Delete<SyntaxNodeOrToken>)) return null;

                    var from = editOperation.T1Node;
                    var result = new MatchResult(Tuple.Create(from, new Bindings(new List<SyntaxNodeOrToken> { from.Value })));
                    matches.Add(result);

                    var key = input[rule.Body[0]];
                    var treeUp = WitnessFunctions.TreeUpdateDictionary[key];
                    var previousTree = ConverterHelper.MakeACopy(treeUp.CurrentTree);
                    treeUp.ProcessEditOperation(editOperation);
                    WitnessFunctions.CurrentTrees[key] = previousTree;

                    Console.WriteLine("PREVIOUS TREE\n\n");
                    PrintUtil<SyntaxNodeOrToken>.PrintPretty(previousTree, "", true);
                    Console.WriteLine("UPDATED TREE\n\n");
                    PrintUtil<SyntaxNodeOrToken>.PrintPretty(treeUp.CurrentTree, "", true);
                }

                kExamples[input] = matches;
            }

            return DisjunctiveExamplesSpec.From(kExamples);
        }


        public static DisjunctiveExamplesSpec UpdateFrom(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var kExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var matches = new List<object>();
                foreach (Edit<SyntaxNodeOrToken> edit in spec.DisjunctiveExamples[input])
                {
                    var editOperation = edit.EditOperation;
                    if (!(editOperation is Update<SyntaxNodeOrToken>)) return null;

                    var from = editOperation.T1Node;
                    var result = new MatchResult(Tuple.Create(from, new Bindings(new List<SyntaxNodeOrToken> { from.Value })));
                    matches.Add(result);

                    var key = input[rule.Body[0]];
                    var treeUp = WitnessFunctions.TreeUpdateDictionary[key];

                    var previousTree = ConverterHelper.MakeACopy(treeUp.CurrentTree);
                    treeUp.ProcessEditOperation(editOperation);
                    WitnessFunctions.CurrentTrees[key] = previousTree;
                }

                kExamples[input] = matches;
            }

            return DisjunctiveExamplesSpec.From(kExamples);
        }

        public static DisjunctiveExamplesSpec UpdateTo(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, ExampleSpec fromBinding)
        {
            var kExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var matches = new List<object>();
                foreach (Edit<SyntaxNodeOrToken> edit in spec.DisjunctiveExamples[input])
                {
                    var editOperation = edit.EditOperation;
                    if (!(editOperation is Update<SyntaxNodeOrToken>)) return null;

                    var update = (Update<SyntaxNodeOrToken>)editOperation;
                    matches.Add(update.To);
                }
                kExamples[input] = matches;
            }
            return DisjunctiveExamplesSpec.From(kExamples);
        }

        public static DisjunctiveExamplesSpec MoveFrom(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var kExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var matches = new List<object>();
                foreach (Edit<SyntaxNodeOrToken> edit in spec.DisjunctiveExamples[input])
                {
                    var editOperation = edit.EditOperation;
                    if (!(editOperation is Move<SyntaxNodeOrToken>)) return null;

                    var key = input[rule.Body[0]];
                    var treeUp = WitnessFunctions.TreeUpdateDictionary[key];

                    var move = (Move<SyntaxNodeOrToken>)editOperation;
                    var parent = move.Parent;
                    var result = new MatchResult(Tuple.Create(parent, new Bindings(new List<SyntaxNodeOrToken> { parent.Value })));

                    matches.Add(result);

                    var previousTree = ConverterHelper.MakeACopy(treeUp.CurrentTree);
                    treeUp.ProcessEditOperation(editOperation);
                    WitnessFunctions.CurrentTrees[key] = previousTree;
                }

                kExamples[input] = matches;
            }

            return DisjunctiveExamplesSpec.From(kExamples);
        }

        public static DisjunctiveExamplesSpec MoveTo(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var kExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var matches = new List<object>();
                foreach (Edit<SyntaxNodeOrToken> edit in spec.DisjunctiveExamples[input])
                {
                    var editOperation = edit.EditOperation;
                    if (!(editOperation is Move<SyntaxNodeOrToken>)) return null;

                    var from = editOperation.T1Node;
                    var result = new MatchResult(Tuple.Create(from, new Bindings(new List<SyntaxNodeOrToken> { from.Value })));
                    matches.Add(result);
                }

                kExamples[input] = matches;
            }

            return DisjunctiveExamplesSpec.From(kExamples);
        }

        public static DisjunctiveExamplesSpec MoveK(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, ExampleSpec kBinding, ExampleSpec parentBinding)
        {
            var kExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var matches = new List<object>();
                foreach (Edit<SyntaxNodeOrToken> edit in spec.DisjunctiveExamples[input])
                {
                    var editOperation = edit.EditOperation;
                    if (!(editOperation is Move<SyntaxNodeOrToken>)) return null;

                    matches.Add(editOperation.K);
                }
                kExamples[input] = matches;
            }
            return DisjunctiveExamplesSpec.From(kExamples);
        }

        public static DisjunctiveExamplesSpec InsertParent(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec)
        {
            var kExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var matches = new List<object>();
                foreach (Edit<SyntaxNodeOrToken> edit in spec.DisjunctiveExamples[input])
                {
                    var editOperation = edit.EditOperation;
                    if (!(editOperation is Insert<SyntaxNodeOrToken>)) return null;

                    var key = input[rule.Body[0]];
                    var treeUp = WitnessFunctions.TreeUpdateDictionary[key];

                    var parent = editOperation.Parent;
                    var result = new MatchResult(Tuple.Create(parent, new Bindings(new List<SyntaxNodeOrToken> { parent.Value })));
                    matches.Add(result);

                    var previousTree = ConverterHelper.MakeACopy(treeUp.CurrentTree);
                    treeUp.ProcessEditOperation(editOperation);
                    WitnessFunctions.CurrentTrees[key] = previousTree;
                    Console.WriteLine("PREVIOUS TREE\n\n");
                    PrintUtil<SyntaxNodeOrToken>.PrintPretty(previousTree, "", true);
                    Console.WriteLine("UPDATED TREE\n\n");
                    PrintUtil<SyntaxNodeOrToken>.PrintPretty(treeUp.CurrentTree, "", true);
                }

                kExamples[input] = matches;
            }

            return DisjunctiveExamplesSpec.From(kExamples);
        }

        public static DisjunctiveExamplesSpec InsertAST(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, ExampleSpec kBinding)
        {
            var kExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var matches = new List<object>();
                foreach (Edit<SyntaxNodeOrToken> edit in spec.DisjunctiveExamples[input])
                {
                    var editOperation = edit.EditOperation;
                    if (!(editOperation is Insert<SyntaxNodeOrToken>)) return null;

                    editOperation.T1Node.Children = new List<ITreeNode<SyntaxNodeOrToken>>();
                    matches.Add(editOperation.T1Node);
                }

                kExamples[input] = matches;
            }

            return DisjunctiveExamplesSpec.From(kExamples);
        }

        public static DisjunctiveExamplesSpec InsertK(GrammarRule rule, int parameter, DisjunctiveExamplesSpec spec, ExampleSpec kBinding, ExampleSpec parentBinding)
        {
            var kExamples = new Dictionary<State, IEnumerable<object>>();
            foreach (State input in spec.ProvidedInputs)
            {
                var matches = new List<object>();
                foreach (Edit<SyntaxNodeOrToken> edit in spec.DisjunctiveExamples[input])
                {
                    var editOperation = edit.EditOperation;
                    if (!(editOperation is Insert<SyntaxNodeOrToken>)) return null;

                    matches.Add(editOperation.K);
                }
                kExamples[input] = matches;
            }
            return DisjunctiveExamplesSpec.From(kExamples);
        }
    }
}
