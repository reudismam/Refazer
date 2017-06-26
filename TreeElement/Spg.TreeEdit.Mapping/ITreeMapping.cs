using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using RefazerFunctions.Substrings;
using TreeElement.Spg.Node;

namespace TreeEdit.Spg.TreeEdit.Mapping
{
    public interface ITreeMapping<T>
    {
        Dictionary<TreeNode<T>, TreeNode<T>> Mapping(TreeNode<T> t1, TreeNode<T> t2);
    }
}
