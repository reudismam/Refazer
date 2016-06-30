using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Specifications;
using ProseSample.Substrings.Spg.Witness.Target;
using TreeEdit.Spg.Script;
using TreeEdit.Spg.TreeEdit.Update;

namespace ProseSample.Substrings.Spg.Witness
{
    public class EditOperation
    {
        public static ExampleSpec DeleteFrom(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            foreach (State input in spec.ProvidedInputs)
            {
                var edit = (Edit<SyntaxNodeOrToken>)spec.Examples[input];
                var editOperation = edit.EditOperation;
                if (!(editOperation is Delete<SyntaxNodeOrToken>)) return null;

                return new T1TargetLearner().NodeLearner(rule, parameter, spec);
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

                return new T1TargetLearner().NodeLearner(rule, parameter, spec);
            }
            return null;
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

                return new ParentTargetLearner().NodeLearner(rule, parameter, spec);
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

        public static ExampleSpec InsertFrom(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            foreach (State input in spec.ProvidedInputs)
            {
                var edit = (Edit<SyntaxNodeOrToken>)spec.Examples[input];
                var editOperation = edit.EditOperation;
                if (!(editOperation is Insert<SyntaxNodeOrToken>)) return null;

                return new ParentTargetLearner().NodeLearner(rule, parameter, spec);
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
