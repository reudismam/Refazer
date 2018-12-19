﻿using System.Collections.Generic;
using RefazerFunctions.Substrings;

namespace TreeElement.Spg.Node
{
    /// <summary>
    /// TreeNode class
    /// </summary>
    /// <typeparam name="T">Node type</typeparam>
    public class TreeNode<T>
    {
        /// <summary>
        /// Gets the children.
        /// </summary>
        /// <value>The children.</value>
        public List<TreeNode<T>> Children { get; set; }

        /// <summary>
        /// Parent node
        /// </summary>
        public TreeNode<T> Parent { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public T Value { get; set; }

        /// <summary>
        /// Define the label of this TreeNode
        /// </summary>
        public TLabel Label { get; set; }

        /// <summary>
        /// Define the Type of this TreeNode
        /// </summary>
        public TType NodeType { get; set; }


        /// <summary>
        /// Define the syntax tree that this TreeNode is associated
        /// </summary>
        public TreeNode<T> SyntaxTree { get; set; }

        /// <summary>
        /// Define the start position of this TreeNode
        /// </summary>
        public int Start { get; set; }

        /// <summary>
        /// Define the status of this TreeNode
        /// </summary>
        public NodeStatus Status { get; set; }


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="label">Label</param>
        /// <param name="children">The children.</param>
        public TreeNode(T value, TLabel label, List<TreeNode<T>> children)
        {
            Value = value;
            Label = label;
            Children = children;
            foreach (var child in Children)
            {
                child.Parent = this;
            }
            Status = NodeStatus.None;
        }

        /// <summary>
        /// Initializes a new instance of the class
        /// </summary>
        /// <param name="value">value</param>
        /// <param name="label">Label</param>
        public TreeNode(T value, TLabel label)
        {
            Value = value;
            Label = label;
            Children = new List<TreeNode<T>>();
            Status = NodeStatus.None;
        }

        /// <summary>
        /// Initializes a new instance of the class
        /// </summary>
        /// <param name="value">value</param>
        /// <param name="label">Label</param>
        public TreeNode(T value, TLabel label, TType type)
        {
            Value = value;
            Label = label;
            NodeType = type;
            Children = new List<TreeNode<T>>();
            Status = NodeStatus.None;
        }

        /// <summary>
        /// Update type of node
        /// </summary>
        public void UpdateNodeType(TType nodeType)
        {
            NodeType = new TType(nodeType);
        }

        /// <summary>
        /// Get descendants nodes
        /// </summary>
        /// <returns></returns>
        public List<TreeNode<T>> DescendantNodes()
        {
            var traversal = new TreeTraversal<T>();
            var list = traversal.PostOrderTraversal(this);
            return list;
        }

        /// <summary>
        /// Add a child at k position
        /// </summary>
        /// <param name="child">Child</param>
        /// <param name="k">Position</param>
        public void AddChild(TreeNode<T> child, int k)
        {
            child.Parent = this;
            Children.Insert(k, child);
        }

        /// <summary>
        /// Remove node from k position
        /// </summary>
        /// <param name="k">position</param>
        public void RemoveNode(int k)
        {
            Children.RemoveAt(k);
        }

        public bool IsLabel(TLabel label)
        {
            return label.Equals(Label);
        }

        public List<TreeNode<T>> DescendantNodesAndSelf()
        {
            var list = DescendantNodes();
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
            var compare = (TreeNode<T>)obj;
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

        /// <summary>
        /// Determines if the two TreeNodes are equal.
        /// </summary>
        /// <param name="t1">First TreeNode</param>
        /// <param name="compare">Second TreeNode</param>
        /// <returns></returns>
        public static bool IsEqual(TreeNode<T> t1, TreeNode<T> compare)
        {
            if (!t1.IsLabel(compare.Label)) return false;
            if (!t1.Value.Equals(compare.Value)) return false;

            var t1Children = t1.Children;
            var compChildren = compare.Children;

            if (t1Children.Count != compChildren.Count) return false;

            for (int i = 0; i < t1Children.Count; i++)
            {
                var t1Child = t1Children[i];
                var compChild = compChildren[i];
                var issame = IsEqual(t1Child, compChild);
                if (!issame) return false;
            }
            return true;
        }
    }

    /// <summary>
    /// Define the node status
    /// </summary>
    public enum NodeStatus
    {
        Inserted,
        Deleted,
        Updade,
        None
    }
}