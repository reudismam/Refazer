using System.Collections.Generic;
using System.Linq;
using ProseSample.Substrings;
using TreeElement.Spg.Node;

namespace TreeEdit.Spg.ConnectedComponents
{
    public class TemplateConnectedComponents<T>
    {
        private Dictionary<TreeNode<T>, bool> _visited;
        private List<TreeNode<T>> _nodes;

        public void DepthFirstSearch(TreeNode<T> tree)
        {
            _visited[tree] = true;

            foreach (var child in tree.Children)
            {
                if (_visited.ContainsKey(child)) continue;

                foreach (var node in _nodes)
                {
                    if (child.Equals(node))
                    {
                        DepthFirstSearch(node);
                    }
                }
            }
        }

        public List<TreeNode<T>> ConnectedNodes(List<TreeNode<T>> nodes)
        {
            _visited = new Dictionary<TreeNode<T>, bool>();
            _nodes = nodes;

            var ccs = new List<TreeNode<T>>();
            foreach (var node in nodes)
            {
                if (!_visited.ContainsKey(node))
                {
                    DepthFirstSearch(node);
                    ccs.Add(node);
                }
            }

            return ccs.Select(ReconstructTree).ToList();
        }

        //private TreeNode<T> result = new TreeNode<T>();
        public TreeNode<T> ReconstructTree(TreeNode<T> node)
        {
            var children = new List<TreeNode<T>>();
            for (int i = 0; i < node.Children.Count; i++)
            {
                var child = node.Children[i];
                if (_visited.ContainsKey(child))
                {
                    children.Add(child);
                    ReconstructTree(child);
                }
            }

            node.Children = new List<TreeNode<T>>();
            for (int i = 0; i < children.Count; i++)
            {
                var child = children[i];
                node.AddChild(child, i);
            }
            return node;
        }
    }
}
