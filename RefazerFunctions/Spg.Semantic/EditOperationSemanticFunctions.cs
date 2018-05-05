using System;
using Microsoft.CodeAnalysis;
using RefazerFunctions.Bean;
using TreeEdit.Spg.Print;
using TreeEdit.Spg.Script;
using TreeEdit.Spg.TreeEdit.Update;

namespace RefazerFunctions.Spg.Semantic
{
    public class EditOperationSemanticFunctions
    {
        /// <summary>
        /// Insert witness function
        /// </summary>
        /// <param name="parent">Parent node</param>
        /// <param name="ast">Node to be inserted</param>
        /// <param name="k">Position to insert the node in the parent</param>
        public static Node Insert(Node parent, Node ast, int k)
        {
            TreeUpdate update = new TreeUpdate(parent.Value);
            var child = ast.Value;
            var insert = new Insert<SyntaxNodeOrToken>(child, parent.Value, k);
            try
            {
                update.ProcessEditOperation(insert);
            }
            catch (Exception e)
            {
                return null;
            }
#if DEBUG
            Console.WriteLine("TREE UPDATE!!");
            PrintUtil<SyntaxNodeOrToken>.PrintPretty(update.CurrentTree, "", true);
#endif
            return new Node(update.CurrentTree);
        }

        /// <summary>
        /// Insert before
        /// </summary>
        /// <param name="target">Target tree</param>
        /// <param name="parent">Parent of the node</param>
        /// <param name="ast">Sibling</param>
        public static Node InsertBefore(Node target, Node parent, Node ast)
        {
            parent.LeftNode = ast;
            return parent;
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="target">Target tree</param>
        /// <param name="to">Target Node</param>
        public static Node Update(Node target, Node to) => to;

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="target">Target tree</param>
        /// <param name="fromNode">Node to be deleted</param>
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
