using System.Collections.Generic;

namespace Spg.TreeEdit.Node
{
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
}