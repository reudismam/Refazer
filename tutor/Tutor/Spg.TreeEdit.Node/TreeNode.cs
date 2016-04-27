using System.Collections.Generic;
using System.Linq;
using Tutor.Spg.TreeEdit.Node;

namespace Spg.TreeEdit.Node
{
    /// <summary>
    /// TreeNode class
    /// </summary>
    /// <typeparam name="T">Node type</typeparam>
    public class TreeNode<T> : ITreeNode<T>
    {
        /// <summary>
        /// Children nodes
        /// </summary>
        private List<ITreeNode<T>> _children;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="children">The children.</param>
        public TreeNode(T value, TLabel label, List<ITreeNode<T>> children)
        {
            Value = value;
            Label = label;
            _children = children;
            foreach (var child in _children)
            {
                child.Parent = this;
            }
        }

        /// <summary>
        /// Initializes a new instance of the class
        /// </summary>
        /// <param name="value">value</param>
        public TreeNode(T value, TLabel label)
        {
            Value = value;
            Label = label;
            _children = new List<ITreeNode<T>>();
        }

        public ITreeNode<T> Parent { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public T Value { get; set; }

        public TLabel Label { get; set; }


        /// <summary>
        /// Children get and set method
        /// </summary>
        List<ITreeNode<T>> ITreeNode<T>.Children
        {
            get
            {
                return _children;
            }

            set
            {
                _children = value;
            }
        }

        /// <summary>
        /// Get descendants nodes
        /// </summary>
        /// <returns></returns>
        public List<ITreeNode<T>> DescendantNodes()
        {
            var list = new List<ITreeNode<T>>();

            if (!_children.Any())
            {
                return list;
            }

            foreach (var item in _children)
            {
                list.Add(item);
                list.AddRange(item.DescendantNodes());
            }

            return list;
        }

        /// <summary>
        /// Add a child at k position
        /// </summary>
        /// <param name="child">Child</param>
        /// <param name="k">Position</param>
        public void AddChild(ITreeNode<T> child, int k)
        {
            _children.Insert(k, child);
        }

        /// <summary>
        /// Remove node from k position
        /// </summary>
        /// <param name="k">position</param>
        public void RemoveNode(int k)
        {
            _children.RemoveAt(k);
        }

        public bool IsLabel(TLabel label)
        {
            return label.Equals(Label);
        }

        public List<ITreeNode<T>> DescendantNodesAndSelf()
        {
            var list = DescendantNodes();
            list.Insert(0, this);
            return list;
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
            TreeNode<T> compare = (TreeNode<T>)obj;
            return Value.Equals(compare.Value);
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
}