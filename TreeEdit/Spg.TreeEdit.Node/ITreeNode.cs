using System.Collections.Generic;

namespace Spg.TreeEdit.Node
{
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
        List<ITreeNode<T>> Children { get; set; }

        /// <summary>
        /// Gets the children.
        /// </summary>
        /// <value>The children.</value>
        List<ITreeNode<T>> GetDescendantsNodes();

        void AddChild(ITreeNode<T> child, int k);

        void RemoveNode(int k);

    }
}