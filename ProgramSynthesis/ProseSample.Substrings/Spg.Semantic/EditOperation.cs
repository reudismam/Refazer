using System;
using Microsoft.CodeAnalysis;
using TreeEdit.Spg.Print;
using TreeEdit.Spg.Script;
using TreeEdit.Spg.TreeEdit.Update;
using TreeElement.Spg.Node;

namespace ProseSample.Substrings.Spg.Semantic
{
    public class EditOperation
    {
        /// <summary>
        /// Insert the ast node as in the k position of the node in the matching result 
        /// </summary>
        /// <param name="node">Input data</param>
        /// <param name="k">Position in witch the node will be inserted.</param>
        /// <param name="mresult">Matching result</param>
        /// <param name="ast">Node that will be insert</param>
        /// <returns>New node with the ast node inserted as the k child</returns>
        public static SyntaxNodeOrToken Insert(SyntaxNodeOrToken node, /*Pattern mresult,*/ Node ast, int k)
        {
            //TreeUpdate update = Semantics.TreeUpdateDictionary[node];

            //var parent = ConverterHelper.ConvertCSharpToTreeNode(mresult.Match.Item1.Value);
            ////var child = ConverterHelper.ConvertCSharpToTreeNode(ast);
            //var child = ast.Value;

            //var insert = new Insert<SyntaxNodeOrToken>(child, parent, k);
            //update.ProcessEditOperation(insert);

            //Console.WriteLine("TREE UPDATE!!");
            //PrintUtil<SyntaxNodeOrToken>.PrintPretty(update.CurrentTree, "", true);
            //return update.CurrentTree.Value;
            return null;
        }

        /// <summary>
        /// Move the from node such that it is the k child of the node
        /// </summary>
        /// <param name="node">Source node</param>
        /// <param name="k">Child index</param>
        /// <param name="parent">Parent</param>
        /// <param name="from">Moved node</param>
        /// <returns></returns>
        public static SyntaxNodeOrToken Move(SyntaxNodeOrToken node, /*Pattern parent,*/ Pattern from, int k)
        {
            //TreeUpdate update = Semantics.TreeUpdateDictionary[node];

            //var parentNode = ConverterHelper.ConvertCSharpToTreeNode(parent.Match.Item1.Value);
            //var child = ConverterHelper.ConvertCSharpToTreeNode(from.Match.Item1.Value);

            //var move = new Move<SyntaxNodeOrToken>(child, parentNode, k);
            //update.ProcessEditOperation(move);

            //Console.WriteLine("TREE UPDATE!!");
            //PrintUtil<SyntaxNodeOrToken>.PrintPretty(update.CurrentTree, "", true);
            //return update.CurrentTree.Value;
            return null;
        }

        public static SyntaxNodeOrToken Update(SyntaxNodeOrToken node, /*Pattern from,*/ Node to)
        {
            //TreeUpdate update = Semantics.TreeUpdateDictionary[node];

            //var fromTreeNode = ConverterHelper.ConvertCSharpToTreeNode(from.Match.Item1.Value);
            //var toTreeNode = to.Value;//ConverterHelper.ConvertCSharpToTreeNode(to);

            //var updateEdit = new Update<SyntaxNodeOrToken>(fromTreeNode, toTreeNode, null);
            //update.ProcessEditOperation(updateEdit);

            //Console.WriteLine("TREE UPDATE!!");
            //PrintUtil<SyntaxNodeOrToken>.PrintPretty(update.CurrentTree, "", true);
            //return update.CurrentTree.Value;
            return null;
        }

        public static SyntaxNodeOrToken Delete(SyntaxNodeOrToken node,  Pattern delete)
        {
            //TreeUpdate update = Semantics.TreeUpdateDictionary[node];

            //var t1Node = ConverterHelper.ConvertCSharpToTreeNode(delete.Match.Item1.Value);

            //var updateEdit = new Delete<SyntaxNodeOrToken>(t1Node);
            //update.ProcessEditOperation(updateEdit);

            //Console.WriteLine("TREE UPDATE!!");
            //PrintUtil<SyntaxNodeOrToken>.PrintPretty(update.CurrentTree, "", true);
            //return update.CurrentTree.Value;
            return null;
        }
    }
}
