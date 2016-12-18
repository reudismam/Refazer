using System;
using Microsoft.CodeAnalysis;
using ProseFunctions.Substrings.Spg.Bean;
using TreeEdit.Spg.Print;
using TreeEdit.Spg.Script;
using TreeEdit.Spg.TreeEdit.Update;

namespace ProseFunctions.Substrings.Spg.Semantic
{
    public class SemanticEditOperation
    {
        public static Node Insert(Node target, Node ast, int k)
        {
            TreeUpdate update = new TreeUpdate(target.Value);
            var child = ast.Value;
            var insert = new Insert<SyntaxNodeOrToken>(child, target.Value, k);
            update.ProcessEditOperation(insert);
#if DEBUG
            Console.WriteLine("TREE UPDATE!!");
            PrintUtil<SyntaxNodeOrToken>.PrintPretty(update.CurrentTree, "", true);
#endif
            return new Node(update.CurrentTree);
        }

        public static Node InsertBefore(Node target, Node parent, Node ast)
        {
            parent.LeftNode = ast;
            return parent;
        }

        public static Node Update(Node target, Node to) => to;

        public static Node Delete(Node target, Node fromNode)
        {
            TreeUpdate update = new TreeUpdate(target.Value);
            var t1Node = target.Value;

            var updateEdit = new Delete<SyntaxNodeOrToken>(t1Node);
            update.ProcessEditOperation(updateEdit);
#if DEBUG
            Console.WriteLine("TREE UPDATE!!");
            PrintUtil<SyntaxNodeOrToken>.PrintPretty(update.CurrentTree, "", true);
#endif
            return new Node(update.CurrentTree);
        }
    }
}
