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
    public class EditOperation
    {
        public static Node Insert(Node node, Node ast, int k)
        {
            TreeUpdate update = new TreeUpdate(node.SyntaxTree);
            var parent = node.Value;
            var child = ast.Value;
            var insert = new Insert<SyntaxNodeOrToken>(child, parent, k);
            update.ProcessEditOperation(insert);

            Console.WriteLine("TREE UPDATE!!");
            PrintUtil<SyntaxNodeOrToken>.PrintPretty(update.CurrentTree, "", true);
            return new Node(update.CurrentTree, node.SyntaxTree);
        }

        public static Node Move(Node node, Pattern pattern, int k)
        {
            TreeUpdate update = new TreeUpdate(node.SyntaxTree);
            var parent = node.Value;
            var matches = Matches(node.SyntaxTree, pattern);

            if (matches.Count > 1) throw new Exception("More than one match.");

            var child = matches.First();
            var move = new Move<SyntaxNodeOrToken>(child, parent, k);
            update.ProcessEditOperation(move);
            Console.WriteLine("TREE UPDATE!!");
            PrintUtil<SyntaxNodeOrToken>.PrintPretty(update.CurrentTree, "", true);
            return new Node(update.CurrentTree, node.SyntaxTree);
        }

        public static Node Update(Node node, Node to)
        {
            TreeUpdate update = new TreeUpdate(node.SyntaxTree);
            var parent = node.Value;
            var toTreeNode = to.Value;

            var updateEdit = new Update<SyntaxNodeOrToken>(parent, toTreeNode, null);
            update.ProcessEditOperation(updateEdit);

            Console.WriteLine("TREE UPDATE!!");
            PrintUtil<SyntaxNodeOrToken>.PrintPretty(update.CurrentTree, "", true);
            return new Node(update.CurrentTree, node.SyntaxTree);
        }

        public static Node Delete(Node node)
        {
            TreeUpdate update = new TreeUpdate(node.SyntaxTree);
            var t1Node = node.Value;
            var updateEdit = new Delete<SyntaxNodeOrToken>(t1Node);
            update.ProcessEditOperation(updateEdit);

            Console.WriteLine("TREE UPDATE!!");
            PrintUtil<SyntaxNodeOrToken>.PrintPretty(update.CurrentTree, "", true);
            return new Node(update.CurrentTree, node.SyntaxTree);
        }

        private static List<ITreeNode<SyntaxNodeOrToken>> Matches(ITreeNode<SyntaxNodeOrToken> node, Pattern pattern)
        {
            TreeTraversal<SyntaxNodeOrToken> tree = new TreeTraversal<SyntaxNodeOrToken>();
            var nodes = tree.PostOrderTraversal(node);

            return nodes.Where(v => Semantics.IsValue(v, pattern.Tree)).ToList();
        }
    }
}
