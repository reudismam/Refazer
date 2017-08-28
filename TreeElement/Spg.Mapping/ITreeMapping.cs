using System.Collections.Generic;
using TreeElement.Spg.Node;

namespace TreeElement.Spg.Mapping
{
    public interface ITreeMapping<T>
    {
        /// <summary>
        /// Identifies the mapping between nodes from two trees.
        /// </summary>
        /// <param name="t1">First tree</param>
        /// <param name="t2">Second tree</param>
        Dictionary<TreeNode<T>, TreeNode<T>> Mapping(TreeNode<T> t1, TreeNode<T> t2);
    }
}
