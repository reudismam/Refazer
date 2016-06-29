using System.Collections.Generic;

namespace TreeElement.Spg.Node
{
    /// <summary>
    /// A tree node.
    /// </summary>
    /// <typeparam name="T">The type of the value associated with this node.</typeparam>
    public interface ITreeNode<T>
    {
        ITreeNode<T> Parent { get; set; }
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        T Value { get; set; }

        TLabel Label { get; set; }

        ITreeNode<T> SyntaxTree { get; set; }

        int Start { get; set; }

        /// <summary>
        /// Gets the children.
        /// </summary>
        /// <value>The children.</value>
        List<ITreeNode<T>> Children { get; set; }

        /// <summary>
        /// Gets the children.
        /// </summary>
        /// <value>The children.</value>
        List<ITreeNode<T>> DescendantNodes();

        /// <summary>
        /// Add a child at k position
        /// </summary>
        /// <param name="child">Child</param>
        /// <param name="k">Position</param>
        void AddChild(ITreeNode<T> child, int k);

        /// <summary>
        /// Remove a node from k position
        /// </summary>
        /// <param name="k">position</param>
        void RemoveNode(int k);


        bool IsLabel(TLabel label);

        List<ITreeNode<T>> DescendantNodesAndSelf();
    }
}