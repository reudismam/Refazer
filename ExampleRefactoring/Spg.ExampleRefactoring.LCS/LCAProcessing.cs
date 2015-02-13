using System.Collections.Generic;

public class LCAProcessing<T>
{
    public object _indexLookup { get; set; }
    public object _nodes { get; set; }
    public List<int> _values { get; set; }

    public LCAProcessing(object _indexLookup, object _nodes, List<int> _values)
    {
        // _indexLookup = new Dictionary<LCA<T>.ITreeNode<T>, LCA<T>.LeastCommonAncestorFinder<T>.NodeIndex>(); // n or so
        // _nodes = new List<LCA<T>.ITreeNode<T>>();  // n
        // _values = new List<int>(); // n * 2
        this._indexLookup = _indexLookup;
        this._nodes = _nodes;
        this._values = _values;
    }
}
