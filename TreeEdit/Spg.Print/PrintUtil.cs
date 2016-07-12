using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TreeElement.Spg.Node;

namespace TreeEdit.Spg.Print
{
    public class PrintUtil<T>
    {
        private static StreamWriter _prettyPrint;

        private static Dictionary<Tuple<ITreeNode<T>, ITreeNode<T>>, int> _dic;

        private static int _currentIndex;

        public StreamWriter PrettyPrint
        {
            get { return _prettyPrint; }
            set { _prettyPrint = value;}
        } 

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


        private static void PrettyPrintString(ITreeNode<T> tree, string indent, bool last)
        {
            _prettyPrint.Write(indent);
            if (last)
            {
                _prettyPrint.Write("\\-");
                indent += "  ";
            }
            else
            {
                _prettyPrint.Write("|-");
                indent += "| ";
            }
            if (!tree.Children.Any())
            {
                _prettyPrint.WriteLine(tree.Label + "(\"" + tree + "\")");
            }
            else
            {
                _prettyPrint.WriteLine(tree.Label + "");
            }

            for (int i = 0; i < tree.Children.Count; i++)
                PrettyPrintString(tree.Children[i], indent, i == tree.Children.Count - 1);
        }


        private static void PrettyPrintKey(ITreeNode<T> tree, string indent, bool last, Dictionary<ITreeNode<T>, ITreeNode<T>> M)
        {
            Tuple<ITreeNode<T>, ITreeNode<T>> t = null;

            if (M.ContainsKey(tree))
            {
                t = Tuple.Create(tree, M[tree]);
            }

            int index ;
            if (t != null)
            {
                if (!_dic.ContainsKey(t))
                {
                    _dic.Add(t, _currentIndex++);
                }

                index = _dic[t];
            }
            else
            {
                index = -1;
            }

            _prettyPrint.Write(indent);
            if (last)
            {
                _prettyPrint.Write("\\-");
                indent += "  ";
            }
            else
            {
                _prettyPrint.Write("|-");
                indent += "| ";
            }
            if (!tree.Children.Any())
            {
                _prettyPrint.WriteLine(tree.Label + "(\"" + tree + "\")" + "[" + index +"]");
            }
            else
            {
                _prettyPrint.WriteLine(tree.Label + "[" + index + "]");
            }

            for (int i = 0; i < tree.Children.Count; i++)
                PrettyPrintKey(tree.Children[i], indent, i == tree.Children.Count - 1, M);
        }

        private static void PrettyPrintValue(ITreeNode<T> tree, string indent, bool last, Dictionary<ITreeNode<T>, ITreeNode<T>> M)
        {
            List<Tuple<ITreeNode<T>, ITreeNode<T>>> listT = new List<Tuple<ITreeNode<T>, ITreeNode<T>>>();

            if (M.ContainsValue(tree))
            {
                var key = M.ToList().FindAll(o => o.Value.Equals(tree));
                listT.AddRange(key.Select(mat => Tuple.Create(mat.Key, mat.Value)));
            }

            List<int> indices = new List<int>();
            if (listT.Any())
            {
                foreach (var mat in listT)
                {
                    indices.Add(_dic[mat]);
                }
            }
            else
            {
                indices.Add(-1);
            }

            string index = indices.GetRange(0, indices.Count - 1).Aggregate("", (current, s) => current + (s + ", "));
            index += indices.Last();

            _prettyPrint.Write(indent);
            if (last)
            {
                _prettyPrint.Write("\\-");
                indent += "  ";
            }
            else
            {
                _prettyPrint.Write("|-");
                indent += "| ";
            }
            if (!tree.Children.Any())
            {
                _prettyPrint.WriteLine(tree.Label + "(\"" + tree + "\")" + "[" + index + "]");
            }
            else
            {
                _prettyPrint.WriteLine(tree.Label + "[" + index + "]");
            }

            for (int i = 0; i < tree.Children.Count; i++)
                PrettyPrintValue(tree.Children[i], indent, i == tree.Children.Count - 1, M);
        }

        public static string PrettyPrintString(ITreeNode<T> tree, string path = "out.txt")
        {
            _prettyPrint = new StreamWriter(path);
            PrettyPrintString(tree, "", true);
            _prettyPrint.Close();
            string result = File.ReadAllText(path);
            return result;
        }

        public static Tuple<string, string> PrettyPrintString(ITreeNode<T> t1, ITreeNode<T> t2, Dictionary<ITreeNode<T>, ITreeNode<T>> M, string path = "out.txt")
        {
            _currentIndex = 0;
            _dic = new Dictionary<Tuple<ITreeNode<T>, ITreeNode<T>>, int>();
            string startupPath = System.AppDomain.CurrentDomain.BaseDirectory;
            path = startupPath + path;
            _prettyPrint = new StreamWriter(path);
            PrettyPrintKey(t1, "", true, M);
            _prettyPrint.Close();

            string t1result = File.ReadAllText(path);

            _prettyPrint = new StreamWriter(path);
            PrettyPrintValue(t2, "", true, M);
            _prettyPrint.Close();

            string t2result = File.ReadAllText(path);

            return null;
        }
    }
}