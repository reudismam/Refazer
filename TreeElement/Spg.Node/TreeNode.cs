using System.Collections.Generic;
using TreeElement.Spg.Walker;

namespace TreeElement.Spg.Node
{
    /// <summary>
    /// TreeNode class
    /// </summary>
    /// <typeparam name="T">Node type</typeparam>
    public class ITreeNode<T>
    { 
        /// <summary>
        /// Gets the children.
        /// </summary>
        /// <value>The children.</value>
        public List<ITreeNode<T>> Children { get; set; }

        public ITreeNode<T> Parent { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public T Value { get; set; }

        public TLabel Label { get; set; }

        public ITreeNode<T> SyntaxTree { get; set; }

        public int Start { get; set; }


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="label">Label</param>
        /// <param name="children">The children.</param>
        public ITreeNode(T value, TLabel label, List<ITreeNode<T>> children)
        {
            Value = value;
            Label = label;
            Children = children;
            foreach (var child in Children)
            {
                child.Parent = this;
            }
        }

        /// <summary>
        /// Initializes a new instance of the class
        /// </summary>
        /// <param name="value">value</param>
        /// <param name="label">Label</param>
        public ITreeNode(T value, TLabel label)
        {
            Value = value;
            Label = label;
            Children = new List<ITreeNode<T>>();
        }

        ///// <summary>
        ///// Children get and set method
        ///// </summary>
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

        ///// <summary>
        ///// Get descendants nodes
        ///// </summary>
        ///// <returns></returns>
        //public List<ITreeNode<T>> DescendantNodes()
        //{
        //    var list = BFSWalker<T>.BreadFirstSearch(this);
        //    return list;
        //}

        /// <summary>
        /// Get descendants nodes
        /// </summary>
        /// <returns></returns>
        public List<ITreeNode<T>> DescendantNodes()
        {
            //var list = new List<ITreeNode<T>>();

            //if (!_children.Any())
            //{
            //    return list;
            //}

            //foreach (var item in _children)
            //{
            //    list.Add(item);
            //    list.AddRange(item.DescendantNodes());
            //}

            //return list;
            //return BFSWalker<T>.BreadFirstSearch(this);
            var traversal = new TreeTraversal<T>();
            var list = traversal.PostOrderTraversal(this);
            //list.RemoveAt(list.Count - 1);
            return list;
        }

        /// <summary>
        /// Add a child at k position
        /// </summary>
        /// <param name="child">Child</param>
        /// <param name="k">Position</param>
        public void AddChild(ITreeNode<T> child, int k)
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

        public List<ITreeNode<T>> DescendantNodesAndSelf()
        {
            var list = DescendantNodes();
            //list.Insert(0, this);
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
            if (!(obj is ITreeNode<T>))
            {
                return false;
            }
            ITreeNode<T> compare = (ITreeNode<T>)obj;
            return /*IsEqual(this, compare);*/ Value.Equals(compare.Value);
        }

        /// <summary>
        /// Hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public static bool IsEqual(ITreeNode<T> t1, ITreeNode<T> compare)
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
}