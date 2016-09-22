using System.Collections.Generic;
using System.Linq;
using ProseSample.Substrings;

namespace TreeEdit.Spg.ConnectedComponents
{
    public class TemplateConnectedComponents<T>
    {
        private Dictionary<ITreeNode<T>, bool> _visited;
        private List<ITreeNode<T>> _nodes;

        public void DepthFirstSearch(ITreeNode<T> tree)
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

        public List<ITreeNode<T>> ConnectedNodes(List<ITreeNode<T>> nodes)
        {
            _visited = new Dictionary<ITreeNode<T>, bool>();
            _nodes = nodes;

            var ccs = new List<ITreeNode<T>>();
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

        //private ITreeNode<T> result = new ITreeNode<T>();
        public ITreeNode<T> ReconstructTree(ITreeNode<T> node)
        {
            var children = new List<ITreeNode<T>>();
            for (int i = 0; i < node.Children.Count; i++)
            {
                var child = node.Children[i];
                if (_visited.ContainsKey(child))
                {
                    children.Add(child);
                    ReconstructTree(child);
                }
            }

            node.Children = new List<ITreeNode<T>>();
            for (int i = 0; i < children.Count; i++)
            {
                var child = children[i];
                node.AddChild(child, i);
            }
            return node;
        }
    }
}
