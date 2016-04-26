using System.Collections.Generic;
using System.Linq;

namespace Spg.TreeEdit.Node
{
    /// <summary>
    /// TreeNode class
    /// </summary>
    /// <typeparam name="T">Node type</typeparam>
    public class TreeNode<T> : ITreeNode<T>
    {
        private List<ITreeNode<T>> _children;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="children">The children.</param>
        public TreeNode(T value, List<ITreeNode<T>> children)
        {
            Value = value;
            _children = children;
        }

        public TreeNode(T value)
        {
            Value = value;
            _children = new List<ITreeNode<T>>();
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public T Value { get; set; }


        List<ITreeNode<T>> ITreeNode<T>.Children
        {
            get { return _children; }

            set { _children = value; }
        }

        public List<ITreeNode<T>> GetDescendantsNodes()
        {
            var list = new List<ITreeNode<T>>();

            if (!_children.Any())
            {
                return list;
            }

            foreach (var item in _children)
            {
                list.Add(item);
                list.AddRange(item.GetDescendantsNodes());
            }

            return list;
        }

        public void AddChild(ITreeNode<T> child, int k)
        {
            _children.Insert(k, child);
        }


        public void RemoveNode(int k)
        {
            _children.RemoveAt(k);
        }

        ///// <summary>
        ///// Gets the children.
        ///// </summary>
        ///// <value>The children.</value>
        //List<ITreeNode<T>> ITreeNode<T>.Children
        //{
        //    get
        //    {
        //        return _children;
        //    }

        //    set
        //    {
        //        _children = value;
        //    }
        //}

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
}