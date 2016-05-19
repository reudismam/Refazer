using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using Tutor.Spg.Node;

namespace TreeEdit.Spg.TreeEdit.Mapping
{
    public interface ITreeMapping<T>
    {
        Dictionary<ITreeNode<T>, ITreeNode<T>> Mapping(ITreeNode<T> t1, ITreeNode<T> t2);
    }
}
