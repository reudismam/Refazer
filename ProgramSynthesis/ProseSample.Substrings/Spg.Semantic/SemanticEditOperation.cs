using System;
using Microsoft.CodeAnalysis;
using TreeEdit.Spg.Print;
using TreeEdit.Spg.Script;
using TreeEdit.Spg.TreeEdit.Update;
using TreeElement.Spg.Node;
using System.Collections.Generic;
using System.Linq;
using TreeElement;

namespace ProseSample.Substrings.Spg.Semantic
{
    public class SemanticEditOperation
    {
        public static Node Insert(Node target, Node parent, Node ast, int k)
        {
            TreeUpdate update = new TreeUpdate(target.Value);
            var child = ast.Value;
            var insert = new Insert<SyntaxNodeOrToken>(child, parent.Value, k);
            update.ProcessEditOperation(insert);
#if DEBUG
            Console.WriteLine("TREE UPDATE!!");
            PrintUtil<SyntaxNodeOrToken>.PrintPretty(update.CurrentTree, "", true);
#endif
            return new Node(update.CurrentTree);
        }

        public static Node Move(Node target, Node parent, Node moved, int k)
        {
            TreeUpdate update = new TreeUpdate(target.Value);
            var child = moved.Value;
            var move = new Move<SyntaxNodeOrToken>(child, parent.Value, k);
            update.ProcessEditOperation(move);
#if DEBUG
            Console.WriteLine("TREE UPDATE!!");
            PrintUtil<SyntaxNodeOrToken>.PrintPretty(update.CurrentTree, "", true);
#endif
            return new Node(update.CurrentTree);
        }

        public static Node Update(Node target, Node fromNode, Node to)
        {
            TreeUpdate update = new TreeUpdate(target.Value);
            var toTreeNode = to.Value;
            var updateEdit = new Update<SyntaxNodeOrToken>(fromNode.Value, toTreeNode, null);
            update.ProcessEditOperation(updateEdit);
#if DEBUG
            Console.WriteLine("TREE UPDATE!!");
            PrintUtil<SyntaxNodeOrToken>.PrintPretty(update.CurrentTree, "", true);
#endif
            return new Node(update.CurrentTree);
        }

        public static Node Delete(Node target, Node fromNode)
        {
            TreeUpdate update = new TreeUpdate(target.Value);
            var t1Node = fromNode.Value;

            var updateEdit = new Delete<SyntaxNodeOrToken>(t1Node);
            update.ProcessEditOperation(updateEdit);
#if DEBUG
            Console.WriteLine("TREE UPDATE!!");
            PrintUtil<SyntaxNodeOrToken>.PrintPretty(update.CurrentTree, "", true);
#endif
            return new Node(update.CurrentTree);
        }

        public static List<ITreeNode<SyntaxNodeOrToken>> Matches(ITreeNode<SyntaxNodeOrToken> node, Pattern pattern)
        {
            TreeTraversal<SyntaxNodeOrToken> tree = new TreeTraversal<SyntaxNodeOrToken>();
            var nodes = tree.PostOrderTraversal(node);
            return nodes.Where(v => Semantics.IsValue(v, pattern.Tree)).ToList();
        }
    }
}
