using System.Collections.Generic;
using System.Linq;
using ProseSample.Substrings;
using TreeElement.Spg.Node;

namespace TreeEdit.Spg.TreeEdit.Mapping
{
    class TreeAlignment<T>
    {
        Dictionary<TreeNode<T>, string> _dict;

        private void KnuthAssignCanonicalName(TreeNode<T> root)
        {
            if (root == null) return;

            if (!_dict.ContainsKey(root)) _dict.Add(root, "");

            if (!root.Children.Any())
            {
                _dict[root] = "1" + root + "0";
                return;
            }

            foreach (var child in root.Children)
            {
                KnuthAssignCanonicalName(child);
            }

            List<string> children = new List<string>();

            foreach (var child in root.Children)
            {
                children.Add(_dict[child]);
            }

            children = children.OrderBy(o => o).ToList();

            string label = root.Label + "1";
            foreach (var child in children)
            {
                label += child;
            }
            label += "0" + root.Label;

            _dict[root] = label;
        }

        public Dictionary<TreeNode<T>, string> Align(TreeNode<T> t)
        {
            _dict = new Dictionary<TreeNode<T>, string>();
            KnuthAssignCanonicalName(t);
            return _dict;
        }
    }
}
