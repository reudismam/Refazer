using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Specifications;
using ProseSample.Substrings.Spg.Witness.Target;
using TreeEdit.Spg.Script;
using TreeEdit.Spg.TreeEdit.Update;
using TreeElement.Spg.Node;

namespace ProseSample.Substrings.Spg.Witness
{
    public class EditOperation
    {
        public static ExampleSpec T1Learner<T>(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            foreach (State input in spec.ProvidedInputs)
            {
                var edit = (Edit<SyntaxNodeOrToken>)spec.Examples[input];
                var editOperation = edit.EditOperation;
                if (!(editOperation is T)) return null;

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

        public static ExampleSpec LearnK<T>(GrammarRule rule, int parameter, ExampleSpec spec)
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

        public static ExampleSpec ParentLearner<T>(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            foreach (State input in spec.ProvidedInputs)
            {
                var edit = (Edit<SyntaxNodeOrToken>)spec.Examples[input];
                var editOperation = edit.EditOperation;
                if (!(editOperation is T)) return null;

                return new ParentTargetLearner().NodeLearner(rule, parameter, spec);
            }
            return null;
        }

        public static ExampleSpec InsertParentLearner(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            foreach (State input in spec.ProvidedInputs)
            {
                var edit = (Edit<SyntaxNodeOrToken>)spec.Examples[input];
                var editOperation = edit.EditOperation;
                if (!(editOperation is Insert<SyntaxNodeOrToken>)) return null;
                var inpTree = (Node)input[rule.Body[0]];
                var currentTree = WitnessFunctions.GetCurrentTree(inpTree.Value);
                if (!NodeContains(currentTree, edit.EditOperation.Parent)) return null;

                return new ParentTargetLearner().NodeLearner(rule, parameter, spec);
            }
            return null;
        }

        private static bool NodeContains(ITreeNode<SyntaxNodeOrToken> node, ITreeNode<SyntaxNodeOrToken> lookfor)
        {
            var no = TreeUpdate.FindNode(node, lookfor.Value);
            return no != null;
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

        public static ExampleSpec InsertBeforeParentLearner(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            var kExamples = new Dictionary<State, object>();
            foreach (State input in spec.ProvidedInputs)
            {
                //edit opration
                var edit = (Edit<SyntaxNodeOrToken>)spec.Examples[input];
                var editOperation = edit.EditOperation;
                if (!(editOperation is Insert<SyntaxNodeOrToken>)) return null;

                //Current tree
                var key = editOperation.T1Node.SyntaxTree;
                var treeUp = WitnessFunctions.TreeUpdateDictionary[key];

                //Compute after node
                var from = GetAfterNode(treeUp.CurrentTree, editOperation.T1Node);
                if (from == null) return null;

                //Get nodes with a predefined depth
                from.SyntaxTree = editOperation.T1Node.SyntaxTree;
                var result = EditOperation.GetNode(from);
                kExamples[input] = result;
            }
            return new ExampleSpec(kExamples);
        }

        private static ITreeNode<SyntaxNodeOrToken> GetAfterNode(ITreeNode<SyntaxNodeOrToken> currentTree, ITreeNode<SyntaxNodeOrToken> t1Node)
        {
            var node = TreeUpdate.FindNode(currentTree, t1Node.Value);
            var parent = node.Parent;
            for (int i = 0; i < parent.Children.Count; i++)
            {
                var child = parent.Children[i];
                if (child.Equals(node))
                {
                    if (i + 1 < parent.Children.Count)
                    {
                        return parent.Children[i + 1];
                    }
                }
            }
            return null;
        }

        public static ExampleSpec InsertBeforeast(GrammarRule rule, int parameter, ExampleSpec spec)
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

        public static Node GetNode(ITreeNode<SyntaxNodeOrToken> searchedNode)
        {
            var currentTree = WitnessFunctions.GetCurrentTree(searchedNode.SyntaxTree);
            var targetNode = TreeUpdate.FindNode(currentTree, searchedNode.Value);
            var targetNodeHeight = TreeManager<SyntaxNodeOrToken>.GetNodeAtHeight(targetNode, 2);

            targetNodeHeight.SyntaxTree = searchedNode.SyntaxTree;
            targetNodeHeight.Parent = targetNode.Parent;
            return new Node(targetNodeHeight);
        }
    }
}
