using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Spg.TreeEdit.Node;
using TreeEdit.Spg.TreeEdit.Script;

namespace TreeEdit.Spg.TreeEdit.Update
{
    public class TreeUpdate
    {
        /// <summary>
        /// Newest updated tree
        /// </summary>
        public ITreeNode<SyntaxNodeOrToken> CurrentTree { get; set; }

        /// <summary>
        /// Indicate the edit operations that was processed.
        /// </summary>
        public Dictionary<EditOperation, bool> Processed;

        /// <summary>
        /// Edit script
        /// </summary>
        private List<EditOperation> _script;


        /// <summary>
        /// Update the tree following the edit script
        /// </summary>
        /// <param name="script">Edit script</param>
        /// <param name="tree">Tree to be updated</param>
        public void UpdateTree(List<EditOperation> script, SyntaxNodeOrToken tree)
        {
            PreProcessTree(script, tree);

            foreach (var item in script)
            {
                if (!Processed.ContainsKey(item))
                {
                    ProcessScript(item);
                }
            }
        }

        public void PreProcessTree(List<EditOperation> script, SyntaxNodeOrToken tree)
        {
            _script = script;
            CurrentTree = ConverterHelper.ConvertCSharpToTreeNode(tree);
        }


        public void ProcessScript(EditOperation operation)
        {
            foreach (var editOperation in _script)
            {
                ProcessEditOperation(editOperation);
            }
        }

        /// <summary>
        /// Process insert operation
        /// </summary>
        /// <param name="editOperation">Edit operation</param>
        /// <returns>Updated version of current node</returns>
        public void ProcessEditOperation(EditOperation editOperation)
        {

            if (editOperation is Insert)
            {
                var parent = FindNode(editOperation.Parent);
                var treeNode = ConverterHelper.ConvertCSharpToTreeNode(editOperation.T1Node);
                treeNode.Children = new List<ITreeNode<SyntaxNodeOrToken>>();
                parent.AddChild(treeNode, editOperation.K - 1);
            }

            if (editOperation is Script.Update)
            {
                var update = (Script.Update)editOperation;
                var treeNode = ConverterHelper.ConvertCSharpToTreeNode(update.To);
                ReplaceNode(CurrentTree, editOperation.T1Node, treeNode);
            }

            if (editOperation is Move)
            {
                var parent = FindNode(editOperation.Parent);
                RemoveNode(CurrentTree, editOperation.T1Node);

                ITreeNode<SyntaxNodeOrToken> treeNode = ConverterHelper.ConvertCSharpToTreeNode(editOperation.T1Node);
                parent.AddChild(treeNode, editOperation.K - 1);
            }

            if (editOperation is Delete)
            {
                RemoveNode(CurrentTree, editOperation.T1Node);
            }
        }

        private void RemoveNode(ITreeNode<SyntaxNodeOrToken> iTree, SyntaxNodeOrToken oldNode)
        {
            if (!iTree.Children.Any()) return;

            int count = 0;
            bool found = false;
            foreach (var item in iTree.Children)
            {
                if (oldNode.Span.Contains(item.Value.Span) && item.Value.Span.Contains(oldNode.Span))
                {
                    found = true;
                    break;
                }

                RemoveNode(item, oldNode);
                count++;
            }

            if (found)
            {
                iTree.RemoveNode(count);
            }
        }

        private void ReplaceNode(ITreeNode<SyntaxNodeOrToken> iTree, SyntaxNodeOrToken oldNode, ITreeNode<SyntaxNodeOrToken> newNode)
        {
            if (!iTree.Children.Any()) return;

            int count = 0;
            bool found = false;
            foreach (var item in iTree.Children)
            {
                if (oldNode.Span.Contains(item.Value.Span) && item.Value.Span.Contains(oldNode.Span))
                {
                    found = true;
                    break;
                }

                ReplaceNode(item, oldNode, newNode);
                count++;
            }

            if (found)
            {
                iTree.RemoveNode(count);
                iTree.AddChild(newNode, count);
            }
        }

        private ITreeNode<SyntaxNodeOrToken> FindNode(SyntaxNodeOrToken node)
        {
            foreach (var item in CurrentTree.GetDescendantsNodes())
            {
                if (node.IsKind(item.Value.Kind()) && item.Value.Span.Contains(node.Span) && node.Span.Contains(item.Value.Span))
                {
                    return item;
                }
            }
            return null;
        }
    }
}