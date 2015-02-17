using System;
using System.Collections.Generic;
using ExampleRefactoring.Spg.ExampleRefactoring.AST;
using ExampleRefactoring.Spg.ExampleRefactoring.LCS;
using Microsoft.CodeAnalysis;

namespace LeastCommonAncestor
{
    /// <summary>
    /// Least common ancestor
    /// </summary>
    /// <typeparam name="T">Type of data</typeparam>
    public class LCA<T>
    {
        /// <summary>
        /// The least common ancestor
        /// </summary>
        /// <param name="root">Root node</param>
        /// <param name="n1">First syntax node</param>
        /// <param name="n2">Second syntax node</param>
        /// <returns>The least common ancestor of node n1 and n2.</returns>
        public T LeastCommonAncestor(string id, ITreeNode<T> rootNode, ITreeNode<T> x, ITreeNode<T> y)
        {
            //TreeNode<SyntaxNodeOrToken> rootNode = ConvertToTreeNode(root);

            //ITreeNode<SyntaxNodeOrToken> x = new TreeNode<SyntaxNodeOrToken>(n1);
            //ITreeNode<SyntaxNodeOrToken> y = new TreeNode<SyntaxNodeOrToken>(n2);
            //LCAManager manager = LCAManager.GetInstance();
            //TreeNode<SyntaxNodeOrToken> rootNode = manager.ConvertToTreeNode(root.AsNode()) as TreeNode<SyntaxNodeOrToken>;
            //ITreeNode<SyntaxNodeOrToken> x = manager.Find(root, n1) as ITreeNode<SyntaxNodeOrToken>;
            //ITreeNode<SyntaxNodeOrToken> y = manager.Find(root, n2) as ITreeNode<SyntaxNodeOrToken>;

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
            public IList<ITreeNode<T>> Children
            {
                get
                {
                    return _children;
                }
            }
            /// <summary>
            /// Gets the children.
            /// </summary>
            /// <value>The children.</value>
            IEnumerable<ITreeNode<T>> ITreeNode<T>.Children
            {
                get
                {
                    return _children;
                }
            }

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

            Dictionary<object, LCAProcessing<T>>  preprocessing = new Dictionary<object, LCAProcessing<T>>();
            private static LeastCommonAncestorFinder<T> instance;
 

            /// <summary>
            /// Initializes a new instance of the <see cref="LeastCommonAncestorFinder&lt;T&gt;"/> class.
            /// </summary>
            /// <param name="rootNode">The root node.</param>
            private LeastCommonAncestorFinder()
            {
                //if (rootNode == null)
                //{
                //    throw new NotImplementedException("rootNode");
                //}
                //_rootNode = rootNode;
                //LCAProcessing<T> value;
                //if (!preprocessing.TryGetValue(obj, out value))
                //{
                //    PreProcess();
                //    LCAProcessing<T> lcaProcessing = new LCAProcessing<T>(_indexLookup, _nodes, _values);
                //    preprocessing.Add(obj, lcaProcessing);
                //}

                //value = preprocessing[obj];
                //_rootNode = rootNode;
                //_indexLookup = value._indexLookup as Dictionary<ITreeNode<T>, NodeIndex>;
                //_nodes = value._nodes as List<ITreeNode<T>>;
                //_values = value._values;
            }

            public static LeastCommonAncestorFinder<T> GetInstance(object obj, ITreeNode<T> rootNode)
            {
                if (instance == null)
                {
                    instance = new LeastCommonAncestorFinder<T>();
                }
                instance.Init(obj, rootNode);

                return instance;
            }
            private void Init(object obj, ITreeNode<T> rootNode)
            {
                if (rootNode == null)
                {
                    throw new NotImplementedException("rootNode");
                }
                _rootNode = rootNode;
                LCAProcessing<T> value;
                if (!preprocessing.TryGetValue(obj, out value))
                {
                    PreProcess();
                    LCAProcessing<T> lcaProcessing = new LCAProcessing<T>(_indexLookup, _nodes, _values);
                    preprocessing.Add(obj, lcaProcessing);
                }

                value = preprocessing[obj];
                _rootNode = rootNode;
                _indexLookup = value._indexLookup as Dictionary<ITreeNode<T>, NodeIndex>;
                _nodes = value._nodes as List<ITreeNode<T>>;
                _values = value._values;
            }


            /// <summary>
            /// Finds the common parent between two nodes.
            /// </summary>
            /// <param name="x">The x.</param>
            /// <param name="y">The y.</param>
            /// <returns></returns>
            public ITreeNode<T> FindCommonParent(ITreeNode<T> x, ITreeNode<T> y)
            {
                // Find the first time the nodes were visited during preprocessing.
                NodeIndex nodeIndex;
                int indexX, indexY;
                if (!_indexLookup.TryGetValue(x, out nodeIndex))
                {
                    throw new ArgumentException("The x node was not found in the graph.");
                }
                indexX = nodeIndex.FirstVisit;
                if (!_indexLookup.TryGetValue(y, out nodeIndex))
                {
                    throw new ArgumentException("The y node was not found in the graph.");
                }
                indexY = nodeIndex.FirstVisit;

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
                ITreeNode<T> next;
                lastNodeStack.Push(current);

                NodeIndex nodeIndex;
                int valueIndex;
                while (lastNodeStack.Count != 0)
                {
                    current = lastNodeStack.Pop();
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

                private IEnumerator<ITreeNode<T>> _enumerator;

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
                public ITreeNode<T> Value { get; private set; }

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

/*private static TreeNode<DymToken> ConvertToTreeNode(SyntaxNodeOrToken st)
        {
            if (st.ChildNodesAndTokens().Count == 0)
            {
                return new TreeNode<DymToken>(new DymToken(st));
            }

            List<TreeNode<DymToken>> childrens = new List<TreeNode<DymToken>>();
            foreach (SyntaxNodeOrToken sot in st.ChildNodesAndTokens())
            {
                //childrens.Add(sot);
                TreeNode<DymToken> nodes = ConvertToTreeNode(sot);
                childrens.Add(nodes);
            }

            TreeNode<DymToken> tree = new TreeNode<DymToken>(new DymToken(st), childrens.ToArray());
            return tree;
        }*/

//public void Init(SyntaxNodeOrToken root, SyntaxNodeOrToken n1, SyntaxNodeOrToken n2)
//{
//    /*TreeNode<int> rootNode = new TreeNode<int>(
//        0,
//        new TreeNode<int>(1,
//            new TreeNode<int>(2),
//            new TreeNode<int>(3),
//            new TreeNode<int>(4,
//                new TreeNode<int>(5),
//                new TreeNode<int>(6))
//            ),
//        new TreeNode<int>(7,
//            new TreeNode<int>(8),
//            new TreeNode<int>(9)
//            )
//    );*/

//    //TreeNode<DymToken> rootNode = ConvertToTreeNode(root);
//    TreeNode<SyntaxNodeOrToken> rootNode = ConvertToTreeNode(root);

//    /*ITreeNode<SyntaxNodeOrToken> x = ((rootNode.Children[0] as TreeNode<SyntaxNodeOrToken>).Children[2] as TreeNode<SyntaxNodeOrToken>).Children[1];
//    ITreeNode<SyntaxNodeOrToken> y = ((rootNode.Children[0] as TreeNode<SyntaxNodeOrToken>).Children[1]);*/
//    //ITreeNode<SyntaxNodeOrToken> x = new TreeNode<SyntaxNodeOrToken>(n1);
//    //ITreeNode<DymToken> y = new TreeNode<DymToken>(new DymToken(n2));
//    //LeastCommonAncestorFinder<DymToken> finder = new LeastCommonAncestorFinder<DymToken>(rootNode);
//    //ITreeNode<DymToken> result = finder.FindCommonParent(x, y);
//    ITreeNode<SyntaxNodeOrToken> x = new TreeNode<SyntaxNodeOrToken>(n1);
//    ITreeNode<SyntaxNodeOrToken> y = new TreeNode<SyntaxNodeOrToken>(n2);



//    LeastCommonAncestorFinder<SyntaxNodeOrToken> finder = new LeastCommonAncestorFinder<SyntaxNodeOrToken>(rootNode);
//    ITreeNode<SyntaxNodeOrToken> result = finder.FindCommonParent(x, y);

//    Console.WriteLine(result.Value);
//    Console.ReadLine();
//}