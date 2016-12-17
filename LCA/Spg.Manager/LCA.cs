using System;
using System.Collections.Generic;

namespace LCA.Spg.Manager
{
    /// <summary>
    /// Least common ancestor
    /// </summary>
    /// <typeparam name="T">Type of data</typeparam>
    public class LCA<T>
    {
        /// <summary>
        /// Compute the Least Common Ancestor
        /// </summary>
        /// <param name="id">Unique id</param>
        /// <param name="rootNode">Root node</param>
        /// <param name="x">First syntax node</param>
        /// <param name="y">Second syntax node</param>
        /// <returns>The least common ancestor of node n1 and n2.</returns>
        public T LeastCommonAncestor(string id, ITreeNode<T> rootNode, ITreeNode<T> x, ITreeNode<T> y)
        {
            if (x.Equals(y)) return x.Value;

            LeastCommonAncestorFinder<T> finder = LeastCommonAncestorFinder<T>.GetInstance(id, rootNode);
            ITreeNode<T> result = finder.FindCommonParent(x, y);
            return result.Value;
        }

        /// <summary>
        /// TreeNode class
        /// </summary>
        /// <typeparam name="T">Node type</typeparam>
        public class TreeNode<T> : ITreeNode<T>
        {
            private readonly ITreeNode<T>[] _children;

            /// <summary>
            /// Initializes a new instance of the <see cref="TreeNode&lt;T&gt;"/> class.
            /// </summary>
            /// <param name="value">The value.</param>
            /// <param name="children">The children.</param>
            public TreeNode(T value, params ITreeNode<T>[] children)
            {
                Value = value;
                _children = children ?? new ITreeNode<T>[0];
            }

            /// <summary>
            /// Gets or sets the value.
            /// </summary>
            /// <value>The value.</value>
            public T Value { get; set; }

            /// <summary>
            /// Gets the children.
            /// </summary>
            /// <value>The children.</value>
            IEnumerable<ITreeNode<T>> ITreeNode<T>.Children => _children;

            /// <summary>
            /// String representation of this object
            /// </summary>
            /// <returns>String representation of this object</returns>
            public override string ToString()
            {
                return Value.ToString();
            }

            /// <summary>
            /// Equals
            /// </summary>
            /// <param name="obj">Another object</param>
            /// <returns>True if objects are equals</returns>
            public override bool Equals(object obj)
            {
                if (!(obj is TreeNode<T>))
                {
                    return false;
                }
                TreeNode<T> compare = obj as TreeNode<T>;
                return this.Value.Equals(compare.Value);
            }

            /// <summary>
            /// Hash code
            /// </summary>
            /// <returns>Hash code</returns>
            public override int GetHashCode()
            {
                return ToString().GetHashCode();
            }

        }

        /// <summary>
        /// Helps find the least common ancestor in a graph 
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        public class LeastCommonAncestorFinder<T>
        {
            private ITreeNode<T> _rootNode;
            private Dictionary<ITreeNode<T>, NodeIndex> _indexLookup = new Dictionary<ITreeNode<T>, NodeIndex>(); // n or so
            private List<ITreeNode<T>> _nodes = new List<ITreeNode<T>>();  // n
            private List<int> _values = new List<int>(); // n * 2

            readonly Dictionary<object, LCAProcessing<T>>  _preprocessing = new Dictionary<object, LCAProcessing<T>>();
            private static LeastCommonAncestorFinder<T> _instance;
 

            /// <summary>
            /// Initializes a new instance of the <see cref="LeastCommonAncestorFinder&lt;T&gt;"/> class.
            /// </summary>
            private LeastCommonAncestorFinder()
            {
            }

            /// <summary>
            /// Initiate a new instance
            /// </summary>
            public static void Init()
            {
                _instance = null;
            }

            /// <summary>
            /// Singleton instance of LCA
            /// </summary>
            /// <param name="obj">Object to be compared</param>
            /// <param name="rootNode">Node tree</param>
            /// <returns>A singleton instance of LeastCommonAncestorFinder</returns>
            public static LeastCommonAncestorFinder<T> GetInstance(object obj, ITreeNode<T> rootNode)
            {
                if (_instance == null)
                {
                    _instance = new LeastCommonAncestorFinder<T>();
                }
                _instance.Init(obj, rootNode);

                return _instance;
            }

            private void Init(object obj, ITreeNode<T> rootNode)
            {
                if (obj == null) throw new ArgumentNullException(nameof(obj));
                if (rootNode == null) throw new ArgumentNullException(nameof(rootNode));

                _rootNode = rootNode;
                LCAProcessing<T> value;
                if (!_preprocessing.TryGetValue(obj, out value))
                {
                    PreProcess();
                    LCAProcessing<T> lcaProcessing = new LCAProcessing<T>(_indexLookup, _nodes, _values);
                    _preprocessing.Add(obj, lcaProcessing);
                }

                value = _preprocessing[obj];
                _rootNode = rootNode;
                _indexLookup = value.IndexLookup as Dictionary<ITreeNode<T>, NodeIndex>;
                _nodes = value.Nodes as List<ITreeNode<T>>;
                _values = value.Values;
            }

            /// <summary>
            /// Finds the common parent between two nodes.
            /// </summary>
            /// <param name="x">The x.</param>
            /// <param name="y">The y.</param>
            /// <returns></returns>
            public ITreeNode<T> FindCommonParent(ITreeNode<T> x, ITreeNode<T> y)
            {
                if (x == null) throw new ArgumentNullException(nameof(x));
                if (y == null) throw new ArgumentNullException(nameof(y));

                // Find the first time the nodes were visited during preprocessing.
                NodeIndex nodeIndex;
                if (!_indexLookup.TryGetValue(x, out nodeIndex))
                {
                    throw new ArgumentException("The x node was not found in the graph.");
                }
                var indexX = nodeIndex.FirstVisit;
                if (!_indexLookup.TryGetValue(y, out nodeIndex))
                {
                    throw new ArgumentException("The y node was not found in the graph.");
                }
                var indexY = nodeIndex.FirstVisit;

                // Adjust so X is less than Y
                int temp;
                if (indexY < indexX)
                {
                    temp = indexX;
                    indexX = indexY;
                    indexY = temp;
                }

                // Find the lowest value.
                temp = int.MaxValue;
                for (int i = indexX; i < indexY; i++)
                {
                    if (_values[i] < temp)
                    {
                        temp = _values[i];
                    }
                }
                return _nodes[temp];
            }

            private void PreProcess()
            {
                // Eulerian path visit of graph 
                Stack<ProcessingState> lastNodeStack = new Stack<ProcessingState>();
                ProcessingState current = new ProcessingState(_rootNode);
                lastNodeStack.Push(current);

                while (lastNodeStack.Count != 0)
                {
                    current = lastNodeStack.Pop();
                    NodeIndex nodeIndex;
                    int valueIndex;
                    if (!_indexLookup.TryGetValue(current.Value, out nodeIndex))
                    {
                        valueIndex = _nodes.Count;
                        _nodes.Add(current.Value);
                        _indexLookup[current.Value] = new NodeIndex(_values.Count, valueIndex);
                    }
                    else
                    {
                        valueIndex = nodeIndex.LookupIndex;
                    }
                    _values.Add(valueIndex);

                    // If there is a next then push the current value on to the stack along with 
                    // the current value.
                    ITreeNode<T> next;
                    if (current.Next(out next))
                    {
                        lastNodeStack.Push(current);
                        lastNodeStack.Push(new ProcessingState(next));
                    }
                }
                _nodes.TrimExcess();
                _values.TrimExcess();
            }

            private class ProcessingState
            {
                private readonly IEnumerator<ITreeNode<T>> _enumerator;

                /// <summary>
                /// Initializes a new instance of the <see cref="LeastCommonAncestorFinder&lt;T&gt;.ProcessingState"/> class.
                /// </summary>
                /// <param name="value">The value.</param>
                public ProcessingState(ITreeNode<T> value)
                {
                    Value = value;
                    _enumerator = value.Children.GetEnumerator();
                }

                /// <summary>
                /// Gets the node.
                /// </summary>
                /// <value>The value.</value>
                public ITreeNode<T> Value { get; }

                /// <summary>
                /// Gets the next child.
                /// </summary>
                /// <param name="value">The value.</param>
                /// <returns></returns>
                public bool Next(out ITreeNode<T> value)
                {
                    if (_enumerator.MoveNext())
                    {
                        value = _enumerator.Current;
                        return true;
                    }
                    value = null;
                    return false;
                }
            }

            public struct NodeIndex
            {
                public readonly int FirstVisit;
                public readonly int LookupIndex;

                public NodeIndex(int firstVisit, int lookupIndex)
                {
                    FirstVisit = firstVisit;
                    LookupIndex = lookupIndex;
                }
            }
        }

        /// <summary>
        /// A tree node.
        /// </summary>
        /// <typeparam name="T">The type of the value associated with this node.</typeparam>
        public interface ITreeNode<T>
        {
            /// <summary>
            /// Gets or sets the value.
            /// </summary>
            /// <value>The value.</value>
            T Value { get; set; }

            /// <summary>
            /// Gets the children.
            /// </summary>
            /// <value>The children.</value>
            IEnumerable<ITreeNode<T>> Children { get; }
        }
    }
}