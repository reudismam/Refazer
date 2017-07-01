using System.Collections.Generic;
using TreeElement.Spg.Node;

namespace TreeElement.Spg.TreeEdit.Mapping
{
    public interface ITreeMapping<T>
    {
        Dictionary<TreeNode<T>, TreeNode<T>> Mapping(TreeNode<T> t1, TreeNode<T> t2);
    }
}
