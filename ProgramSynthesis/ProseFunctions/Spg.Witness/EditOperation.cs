using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Specifications;
using ProseFunctions.Substrings.Spg.Bean;
using ProseFunctions.Substrings.Spg.Witness.Target;
using TreeEdit.Spg.Script;
using TreeEdit.Spg.TreeEdit.Update;
using TreeElement.Spg.Node;

namespace ProseFunctions.Substrings.Spg.Witness
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

        //public static ExampleSpec ParentLearner<T>(GrammarRule rule, int parameter, ExampleSpec spec)
        //{
        //    foreach (State input in spec.ProvidedInputs)
        //    {
        //        var edit = (Edit<SyntaxNodeOrToken>)spec.Examples[input];
        //        var editOperation = edit.EditOperation;
        //        if (!(editOperation is T)) return null;

        //        return new ParentTargetLearner().NodeLearner(rule, parameter, spec);
        //    }
        //    return null;
        //}

        //public static ExampleSpec InsertParentLearner(GrammarRule rule, int parameter, ExampleSpec spec)
        //{
        //    foreach (State input in spec.ProvidedInputs)
        //    {
        //        var edit = (Edit<SyntaxNodeOrToken>)spec.Examples[input];
        //        var editOperation = edit.EditOperation;
        //        if (!(editOperation is Insert<SyntaxNodeOrToken>)) return null;

        //        return new ParentTargetLearner().NodeLearner(rule, parameter, spec);
        //    }
        //    return null;
        //}

        public static ExampleSpec Insertast(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            var kExamples = new Dictionary<State, object>();
            foreach (State input in spec.ProvidedInputs)
            {
                var edit = (Edit<SyntaxNodeOrToken>)spec.Examples[input];
                var editOperation = edit.EditOperation;
                if (!(editOperation is Insert<SyntaxNodeOrToken>)) return null;

                //if (!ConnectedComponentMannager<SyntaxNodeOrToken>.IsValidBlock(editOperation.Parent)) return null;
                kExamples[input] = editOperation.T1Node;
            }
            return new ExampleSpec(kExamples);
        }

        public static ExampleSpec InsertBeforeast(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            var kExamples = new Dictionary<State, object>();
            foreach (State input in spec.ProvidedInputs)
            {
                //input tree
                var inputTree = (Node)input[rule.Grammar.InputSymbol];

                //edit opration
                var edit = (Edit<SyntaxNodeOrToken>)spec.Examples[input];
                var editOperation = edit.EditOperation;
                if (!(editOperation is Insert<SyntaxNodeOrToken>)) return null;

                //Current tree
                //var key = editOperation.T1Node.SyntaxTree;
                //var treeUp = WitnessFunctions.TreeUpdateDictionary[key];
                var treeUp = new TreeUpdate(inputTree.Value);
                treeUp.ProcessEditOperation(editOperation);

                //Compute after node
                var from = GetAfterNode(treeUp.CurrentTree, editOperation.T1Node);
                if (from == null) return null;

                ////Get nodes with a predefined depth
                //from.SyntaxTree = editOperation.T1Node.SyntaxTree;
                //var result = EditOperation.GetNode(from);
                kExamples[input] = editOperation.T1Node;
            }
            return new ExampleSpec(kExamples);
        }

        public static ExampleSpec InsertBeforeParentLearner(GrammarRule rule, int parameter, ExampleSpec spec)
        {
            var kExamples = new Dictionary<State, object>();
            foreach (State input in spec.ProvidedInputs)
            {
                //input tree
                var inputTree = (Node)input[rule.Grammar.InputSymbol];

                //edit opration
                var edit = (Edit<SyntaxNodeOrToken>)spec.Examples[input];
                var editOperation = edit.EditOperation;
                if (!(editOperation is Insert<SyntaxNodeOrToken>)) return null;

                //Current tree
                //var key = editOperation.T1Node.SyntaxTree;
                //var treeUp = WitnessFunctions.TreeUpdateDictionary[key];
                var treeUp = new TreeUpdate(inputTree.Value);
                //treeUp.ProcessEditOperation(editOperation);

                //Compute after node
                //var from = GetAfterNode(treeUp.CurrentTree, editOperation.T1Node);
                var beforeAfter = Transformation.ConfigContextBeforeAfterNode(edit, ConverterHelper.MakeACopy(treeUp.CurrentTree));
                //if (beforeAfter.Item2 == null) return null;

                //from.SyntaxTree = editOperation.T1Node.SyntaxTree;
                //var result = EditOperation.GetNode(from);
                var from = beforeAfter.Item2;
                if (from == null) return null;

                from.SyntaxTree = editOperation.T1Node.SyntaxTree;
                var result = EditOperation.GetNode(from);
                //if (result == null)
                //{
                //    result = EditOperation.GetNode(inputTree.Value, from);
                //}

                if (result == null) return null;
                ////Get nodes with a predefined depth
                //from.SyntaxTree = editOperation.T1Node.SyntaxTree;
                //var result = EditOperation.GetNode(from);
                kExamples[input] = result;
            }
            return new ExampleSpec(kExamples);
        }

        //public static ExampleSpec InsertBeforeParentLearner(GrammarRule rule, int parameter, ExampleSpec spec)
        //{
        //    var kExamples = new Dictionary<State, object>();
        //    foreach (State input in spec.ProvidedInputs)
        //    {
        //        //input tree
        //        var inputTree = (Node)input[rule.Grammar.InputSymbol];

        //        //edit opration
        //        var edit = (Edit<SyntaxNodeOrToken>)spec.Examples[input];
        //        var editOperation = edit.EditOperation;
        //        if (!(editOperation is Insert<SyntaxNodeOrToken>)) return null;

        //        //Current tree
        //        //var key = editOperation.T1Node.SyntaxTree;
        //        //var treeUp = WitnessFunctions.TreeUpdateDictionary[key];
        //        var treeUp = new TreeUpdate(inputTree.Value);
        //        treeUp.ProcessEditOperation(editOperation);

        //        //Compute after node
        //        var from = GetAfterNode(treeUp.CurrentTree, editOperation.T1Node);
        //        if (from == null) return null;

        //        from.SyntaxTree = editOperation.T1Node.SyntaxTree;
        //        var result = EditOperation.GetNode(from);
        //        if (result == null)
        //        {
        //            result = EditOperation.GetNode(inputTree.Value, from);
        //        }

        //        if (result == null) return null;
        //        ////Get nodes with a predefined depth
        //        //from.SyntaxTree = editOperation.T1Node.SyntaxTree;
        //        //var result = EditOperation.GetNode(from);
        //        kExamples[input] = result;
        //    }
        //    return new ExampleSpec(kExamples);
        //}

        //public static ExampleSpec InsertBeforeParentLearner(GrammarRule rule, int parameter, ExampleSpec spec)
        //{
        //    var kExamples = new Dictionary<State, object>();
        //    foreach (State input in spec.ProvidedInputs)
        //    {
        //        //edit opration
        //        var edit = (Edit<SyntaxNodeOrToken>)spec.Examples[input];
        //        var editOperation = edit.EditOperation;
        //        if (!(editOperation is Insert<SyntaxNodeOrToken>)) return null;

        //        //Current tree
        //        var key = editOperation.T1Node.SyntaxTree;
        //        var treeUp = WitnessFunctions.TreeUpdateDictionary[key];

        //        //Compute after node
        //        var from = GetAfterNode(treeUp.CurrentTree, editOperation.T1Node);
        //        if (from == null) return null;

        //        //Get nodes with a predefined depth
        //        from.SyntaxTree = editOperation.T1Node.SyntaxTree;
        //        var result = EditOperation.GetNode(from);
        //        kExamples[input] = result;
        //    }
        //    return new ExampleSpec(kExamples);
        //}

        private static TreeNode<SyntaxNodeOrToken> GetAfterNode(TreeNode<SyntaxNodeOrToken> currentTree, TreeNode<SyntaxNodeOrToken> t1Node)
        {
            var node = TreeUpdate.FindNode(currentTree, t1Node.Value);
            if (node == null) return null;
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

        //public static ExampleSpec InsertBeforeast(GrammarRule rule, int parameter, ExampleSpec spec)
        //{
        //    var kExamples = new Dictionary<State, object>();
        //    foreach (State input in spec.ProvidedInputs)
        //    {
        //        var edit = (Edit<SyntaxNodeOrToken>)spec.Examples[input];
        //        var editOperation = edit.EditOperation;
        //        if (!(editOperation is Insert<SyntaxNodeOrToken>)) return null;

        //        kExamples[input] = editOperation.T1Node;
        //    }
        //    return new ExampleSpec(kExamples);
        //}

        public static TreeNode<SyntaxNodeOrToken> GetNode(TreeNode<SyntaxNodeOrToken> searchedNode)
        {
            var currentTree = WitnessFunctions.GetCurrentTree(searchedNode.SyntaxTree);
            var targetNode = TreeUpdate.FindNode(currentTree, searchedNode.Value);

            if (targetNode == null) return null;

            var targetNodeHeight = targetNode;
            //var targetNodeHeight = TreeManager<SyntaxNodeOrToken>.GetNodeAtHeight(targetNode, 3);

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
