using System;
using System.Linq;
using Spg.TreeEdit.Node;

namespace TreeEdit.Spg.Print
{
    public class PrintUtil<T>
    {
        public static void PrintPretty(ITreeNode<T> tree, string indent, bool last)
        {
            Console.Write(indent);
            if (last)
            {
                Console.Write("\\-");
                indent += "  ";
            }
            else
            {
                Console.Write("|-");
                indent += "| ";
            }
            if (!tree.Children.Any())
            {
                Console.WriteLine(tree.Label + "(\"" + tree + "\")");
            }
            else
            {
                Console.WriteLine(tree.Label);
            }

            for (int i = 0; i < tree.Children.Count; i++)
                PrintPretty(tree.Children[i], indent, i == tree.Children.Count - 1);
        }
    }
}