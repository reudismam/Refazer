﻿using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using TreeElement.Spg.Node;

namespace TreeEdit.Spg.TreeEdit.Mapping
{
    public interface ITreeMapping<T>
    {
        Dictionary<ITreeNode<T>, ITreeNode<T>> Mapping(ITreeNode<T> t1, ITreeNode<T> t2);
    }
}