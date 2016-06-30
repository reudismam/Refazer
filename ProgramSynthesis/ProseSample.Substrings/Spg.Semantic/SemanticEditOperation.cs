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
        public static Node Insert(Pattern node, Node ast, int k)
        {
            //TreeUpdate update = new TreeUpdate(node.Value.SyntaxTree);
            //var parent = node.Value;
            //var child = ast.Value;
            //var insert = new Insert<SyntaxNodeOrToken>(child, parent, k);
            //update.ProcessEditOperation(insert);

            //Console.WriteLine("TREE UPDATE!!");
            //PrintUtil<SyntaxNodeOrToken>.PrintPretty(update.CurrentTree, "", true);
            //return new Node(update.CurrentTree);
            return null;
        }

        public static Node Move(Pattern node, Pattern pattern, int k)
        {
            //TreeUpdate update = new TreeUpdate(node.Value.SyntaxTree);
            //var parent = node.Value;
            //var matches = Matches(node.Value.SyntaxTree, pattern);

            //if (matches.Count > 1) throw new Exception("More than one match.");

            //var child = matches.First();
            //var move = new Move<SyntaxNodeOrToken>(child, parent, k);
            //update.ProcessEditOperation(move);
            //Console.WriteLine("TREE UPDATE!!");
            //PrintUtil<SyntaxNodeOrToken>.PrintPretty(update.CurrentTree, "", true);
            //return new Node(update.CurrentTree);
            return null;
        }

        public static Node Update(Pattern node, Node to)
        {
            //TreeUpdate update = new TreeUpdate(node.Value.SyntaxTree);
            //var parent = node.Value;
            //var toTreeNode = to.Value;

            //var updateEdit = new Update<SyntaxNodeOrToken>(parent, toTreeNode, null);
            //update.ProcessEditOperation(updateEdit);

            //Console.WriteLine("TREE UPDATE!!");
            //PrintUtil<SyntaxNodeOrToken>.PrintPretty(update.CurrentTree, "", true);
            //return new Node(update.CurrentTree);
            return null;
        }

        public static Node Delete(Pattern node)
        {
            //TreeUpdate update = new TreeUpdate(node.Value.SyntaxTree);
            //var t1Node = node.Value;
            //var updateEdit = new Delete<SyntaxNodeOrToken>(t1Node);
            //update.ProcessEditOperation(updateEdit);

            //Console.WriteLine("TREE UPDATE!!");
            //PrintUtil<SyntaxNodeOrToken>.PrintPretty(update.CurrentTree, "", true);
            //return new Node(update.CurrentTree);
            return null;
        }

        public static List<ITreeNode<SyntaxNodeOrToken>> Matches(ITreeNode<SyntaxNodeOrToken> node, Pattern pattern)
        {
            TreeTraversal<SyntaxNodeOrToken> tree = new TreeTraversal<SyntaxNodeOrToken>();
            var nodes = tree.PostOrderTraversal(node);

            return nodes.Where(v => Semantics.IsValue(v, pattern.Tree)).ToList();
        }
    }
}
