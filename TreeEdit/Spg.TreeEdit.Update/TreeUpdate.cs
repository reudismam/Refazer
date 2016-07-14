using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using TreeEdit.Spg.Script;
using TreeElement.Spg.Node;

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
        public Dictionary<EditOperation<SyntaxNodeOrToken>, bool> Processed;

        public TreeUpdate(SyntaxNodeOrToken tree)
        {
            CurrentTree = ConverterHelper.ConvertCSharpToTreeNode(tree);
        }

        public TreeUpdate(ITreeNode<SyntaxNodeOrToken> tree)
        {
            CurrentTree = tree;
        }

        /// <summary>
        /// Process insert operation
        /// </summary>
        /// <param name="editOperation">Edit operation</param>
        /// <returns>Updated version of current node</returns>
        public void ProcessEditOperation(EditOperation<SyntaxNodeOrToken> editOperation)
        {

            if (editOperation is Insert<SyntaxNodeOrToken>)
            {
                var parent = FindNode(CurrentTree, editOperation.Parent.Value);
                if (parent == null) parent = CurrentTree;
                var treeNode = editOperation.T1Node;

                parent.AddChild(treeNode, editOperation.K - 1);
                treeNode.Parent = parent;
            }

            if (editOperation is InsertBefore<SyntaxNodeOrToken>)
            {
                var before = FindNode(CurrentTree, editOperation.Parent.Value);
                int i;
                for (i = 0; i < before.Parent.Children.Count; i++)
                {
                    var child = before.Parent.Children[i];
                    if (child.Equals(before)) break;
                }
                var parent = FindNode(CurrentTree, before.Parent.Value);
                parent.AddChild(editOperation.T1Node, i);
            }

            if (editOperation is Update<SyntaxNodeOrToken>)
            {
                var update = (Update<SyntaxNodeOrToken>)editOperation;
                var treeNode = ConverterHelper.ConvertCSharpToTreeNode(update.To.Value);
                ReplaceNode(CurrentTree, editOperation.T1Node.Value, treeNode);
            }

            if (editOperation is Move<SyntaxNodeOrToken>)
            {
                var parent = FindNode(CurrentTree, editOperation.Parent.Value);
                if (parent == null) parent = CurrentTree;
                CurrentTree = RemoveNode(CurrentTree, editOperation.T1Node);

                ITreeNode<SyntaxNodeOrToken> treeNode = editOperation.T1Node;
                parent.AddChild(treeNode, editOperation.K - 1);
                treeNode.Parent = parent;
            }

            if (editOperation is Delete<SyntaxNodeOrToken>)
            {
                CurrentTree = RemoveNode(CurrentTree, editOperation.T1Node);             
            }
        }

        private ITreeNode<SyntaxNodeOrToken> RemoveNode(ITreeNode<SyntaxNodeOrToken> iTree, ITreeNode<SyntaxNodeOrToken> oldNode)
        {
            if (!iTree.Children.Any()) return iTree;

            if (IsEquals(iTree, oldNode))
            {
                iTree = new TreeNode<SyntaxNodeOrToken>(null, null);
                return iTree;
            }

            int count = 0;
            bool found = false;
            foreach (var item in iTree.Children)
            {
                if (oldNode.Value.Span.Contains(item.Value.Span) && item.Value.Span.Contains(oldNode.Value.Span) && oldNode.Value.IsKind(item.Value.Kind()))
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
                return iTree;
            }
            return iTree;
        }

        private bool IsEquals(ITreeNode<SyntaxNodeOrToken> iTree, ITreeNode<SyntaxNodeOrToken> oldNode)
        {
            if (!iTree.Value.IsKind(oldNode.Value.Kind()))
            {
                return false;
            }

            if (iTree.Children.Count != oldNode.Children.Count)
            {
                return false;
            }

            for (int i = 0; i < iTree.Children.Count; i++)
            {
                var equals = IsEquals(iTree.Children.ElementAt(i), oldNode.Children.ElementAt(i));
                if (!equals) return false;
            }
            return true;
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

        public static ITreeNode<SyntaxNodeOrToken> FindNode(ITreeNode<SyntaxNodeOrToken> tree,  SyntaxNodeOrToken node)
        {
            return tree.DescendantNodesAndSelf().FirstOrDefault(item => node.IsKind(item.Value.Kind()) 
                                                                        && item.Value.Span.Contains(node.Span) 
                                                                        && node.Span.Contains(item.Value.Span));
        }
    }
}