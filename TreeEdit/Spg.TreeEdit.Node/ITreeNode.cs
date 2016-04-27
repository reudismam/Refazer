//using System.Collections.Generic;

//namespace Spg.TreeEdit.Node
//{
//    /// <summary>
//    /// A tree node.
//    /// </summary>
//    /// <typeparam name="T">The type of the value associated with this node.</typeparam>
//    public interface ITreeNode<T>
//    {
//        /// <summary>
//        /// Gets or sets the value.
//        /// </summary>
//        /// <value>The value.</value>
//        T Value { get; set; }

//        object Label { get; set; }

//        /// <summary>
//        /// Gets the children.
//        /// </summary>
//        /// <value>The children.</value>
//        List<ITreeNode<T>> Children { get; set; }

//        /// <summary>
//        /// Gets the children.
//        /// </summary>
//        /// <value>The children.</value>
//        List<ITreeNode<T>> DescendantNodes();

//        /// <summary>
//        /// Add a child at k position
//        /// </summary>
//        /// <param name="child">Child</param>
//        /// <param name="k">Position</param>
//        void AddChild(ITreeNode<T> child, int k);

//        /// <summary>
//        /// Remove a node from k position
//        /// </summary>
//        /// <param name="k">position</param>
//        void RemoveNode(int k);


//        bool IsLabel(object label);

//        List<ITreeNode<T>> DescendantNodesAndSelf();
//    }
//}