using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Specifications;
using ProseFunctions.Spg.Bean;
using ProseFunctions.Spg.Witness.Target;
using ProseFunctions.Substrings;
using TreeEdit.Spg.Script;
using TreeEdit.Spg.TreeEdit.Update;
using TreeElement.Spg.Node;

namespace ProseFunctions.Spg.Witness
{
    public class EditOperation
    {
        /// <summary>
        /// Learn the specification for the T1 node.
        /// </summary>
        /// <typeparam name="T">Type of the operation</typeparam>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">parameter</param>
        /// <param name="spec">Specification</param>
        public static ExampleSpec T1Learner<T>(GrammarRule rule, ExampleSpec spec)
        {
            foreach (State input in spec.ProvidedInputs)
            {
                var edit = (Edit<SyntaxNodeOrToken>)spec.Examples[input];
                var editOperation = edit.EditOperation;
                if (!(editOperation is T)) return null;
                return new T1TargetLearner().NodeLearner(rule, spec);
            }
            return null;
        }

        /// <summary>
        /// Learn the to parameter for the Update operator.
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="spec">Specification</param>
        public static ExampleSpec UpdateTo(GrammarRule rule, ExampleSpec spec)
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

        /// <summary>
        /// Learn the parameter k
        /// </summary>
        /// <typeparam name="T">Type of edit operation</typeparam>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="spec">Specification</param>
        public static ExampleSpec LearnK<T>(GrammarRule rule, ExampleSpec spec)
        {
            var kExamples = new Dictionary<State, object>();
            foreach (State input in spec.ProvidedInputs)
            {
                var edit = (Edit<SyntaxNodeOrToken>)spec.Examples[input];
                var editOperation = edit.EditOperation;
                if (!(editOperation is T)) return null;
                kExamples[input] = editOperation.K;
            }
            return new ExampleSpec(kExamples);
        }

        /// <summary>
        /// Witness function to learn the specification of the parameter ast of the operator Insert.
        /// </summary>
        /// <param name="rule">Grammar rule</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="spec">Specification</param>
        public static ExampleSpec Insertast(GrammarRule rule, ExampleSpec spec)
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

        public static ExampleSpec InsertBeforeParentLearner(GrammarRule rule, ExampleSpec spec)
        {
            var kExamples = new Dictionary<State, object>();
            foreach (State input in spec.ProvidedInputs)
            {
                //input tree
                var inputTree = (Node)input[rule.Grammar.InputSymbol];
                //edit operation
                var edit = (Edit<SyntaxNodeOrToken>)spec.Examples[input];
                var editOperation = edit.EditOperation;
                if (!(editOperation is Insert<SyntaxNodeOrToken>)) return null;
                //Current tree
                var treeUp = new TreeUpdate(inputTree.Value);
                //Compute the after node
                var beforeAfter = Transformation.ConfigContextBeforeAfterNode(edit, ConverterHelper.MakeACopy(treeUp.CurrentTree));
                var from = beforeAfter.Item2;
                if (from == null) return null;

                from.SyntaxTree = editOperation.T1Node.SyntaxTree;
                var result = EditOperation.GetNode(from);
                if (result == null) return null;
                kExamples[input] = result;
            }
            return new ExampleSpec(kExamples);
        }

        public static TreeNode<SyntaxNodeOrToken> GetNode(TreeNode<SyntaxNodeOrToken> searchedNode)
        {
            TreeNode<SyntaxNodeOrToken> currentTree = null;// WitnessFunctions.GetCurrentTree(searchedNode.SyntaxTree);
            var targetNode = TreeUpdate.FindNode(currentTree, searchedNode.Value);
            if (targetNode == null) return null;
            var targetNodeHeight = targetNode;
            targetNodeHeight.SyntaxTree = searchedNode.SyntaxTree;
            targetNodeHeight.Parent = targetNode.Parent;
            return targetNodeHeight;
        }

        public static TreeNode<SyntaxNodeOrToken> GetNode(TreeNode<SyntaxNodeOrToken> currentTree, TreeNode<SyntaxNodeOrToken> searchedNode)
        {
            var targetNode = TreeUpdate.FindNode(currentTree, searchedNode.Value);
            var targetNodeHeight = TreeManager<SyntaxNodeOrToken>.GetNodeAtHeight(targetNode, 3);
            targetNodeHeight.SyntaxTree = searchedNode.SyntaxTree;
            targetNodeHeight.Parent = targetNode.Parent;
            return targetNodeHeight;
        }
    }
}
