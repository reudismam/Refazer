using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Spg.TreeEdit.Node;
using TreeEdit.Spg.Script;
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
        public Dictionary<EditOperation<SyntaxNodeOrToken>, bool> Processed;

        public TreeUpdate(SyntaxNodeOrToken tree)
        {
            CurrentTree = ConverterHelper.ConvertCSharpToTreeNode(tree);
        }

        public TreeUpdate(){}

        public void PreProcessTree(List<EditOperation<SyntaxNodeOrToken>> script, SyntaxNodeOrToken tree)
        {
            CurrentTree = ConverterHelper.ConvertCSharpToTreeNode(tree);
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
                var treeNode = ConverterHelper.ConvertCSharpToTreeNode(editOperation.T1Node.Value);

                treeNode.Children = new List<ITreeNode<SyntaxNodeOrToken>>();
                parent.AddChild(treeNode, editOperation.K - 1);
                treeNode.Parent = parent;
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
                RemoveNode(CurrentTree, editOperation.T1Node.Value);

                ITreeNode<SyntaxNodeOrToken> treeNode = ConverterHelper.ConvertCSharpToTreeNode(editOperation.T1Node.Value);
                parent.AddChild(treeNode, editOperation.K - 1);
                treeNode.Parent = parent;
            }

            if (editOperation is Delete<SyntaxNodeOrToken>)
            {
                RemoveNode(CurrentTree, editOperation.T1Node.Value);
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

        public static ITreeNode<SyntaxNodeOrToken> FindNode(ITreeNode<SyntaxNodeOrToken> tree,  SyntaxNodeOrToken node)
        {
            return tree.DescendantNodesAndSelf().FirstOrDefault(item => node.IsKind(item.Value.Kind()) 
                                                                        && item.Value.Span.Contains(node.Span) 
                                                                        && node.Span.Contains(item.Value.Span));
        }
    }
}