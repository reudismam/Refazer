using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeEdit.Spg.TreeEdit.Mapping
{
    class TreeAlignment
    {
        Dictionary<SyntaxNodeOrToken, string> dict;

        public TreeAlignment()
        {
        }

        private void KnuthAssignCanonicalName(SyntaxNodeOrToken t1)
        {
            SyntaxNode root = t1.AsNode();
            if (root == null) return;
            if (!dict.ContainsKey(root)) dict.Add(root, "");

            if (!root.ChildNodes().Any())
            {
                dict[root] = "1" + root.ToString() + "0";
                return;
            }
            else
            {
                foreach (var child in root.ChildNodes())
                {
                    KnuthAssignCanonicalName(child);
                }
            }

            List<string> children = new List<string>();
            foreach (var child in root.ChildNodes())
            {
                children.Add(dict[child]);
            }

            children = children.OrderBy(o => o).ToList();

            string label = "1" + root.Kind();
            foreach (var child in children)
            {
                label += child;
            }
            label += "0" + root.Kind();

            dict[root] = label;
        }

        public Dictionary<SyntaxNodeOrToken, string> align(SyntaxNodeOrToken t)
        {
            dict = new Dictionary<SyntaxNodeOrToken, string>();
            KnuthAssignCanonicalName(t);
            return dict;
        }
    }
}
