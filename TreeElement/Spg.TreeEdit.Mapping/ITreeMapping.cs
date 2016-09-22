using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using ProseSample.Substrings;

namespace TreeEdit.Spg.TreeEdit.Mapping
{
    public interface ITreeMapping<T>
    {
        Dictionary<TreeNode<T>, TreeNode<T>> Mapping(TreeNode<T> t1, TreeNode<T> t2);
    }
}
