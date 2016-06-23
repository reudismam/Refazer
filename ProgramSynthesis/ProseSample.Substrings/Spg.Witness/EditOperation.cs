using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Specifications;
using TreeEdit.Spg.Script;

namespace ProseSample.Substrings.Spg.Witness
{
    public class EditOperation
    {
        public static ExampleSpec DeleteFrom(GrammarRule rule, int parameter, ExampleSpec spec)
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

        public static ExampleSpec MoveTo(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            var kExamples = new Dictionary<State, object>();
            foreach (State input in spec.ProvidedInputs)
            {
                var edit = (Edit<SyntaxNodeOrToken>)spec.Examples[input];
                var editOperation = edit.EditOperation;
                if (!(editOperation is Move<SyntaxNodeOrToken>)) return null;

                var from = editOperation.T1Node;
                var result = new Node(from);

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

        public static ExampleSpec InsertAST(GrammarRule rule, int parameter, ExampleSpec spec)
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
