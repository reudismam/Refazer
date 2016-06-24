using Microsoft.CodeAnalysis;
using TreeElement.Spg.Node;

namespace ProseSample.Substrings
{
    public class Node
    {
        public ITreeNode<SyntaxNodeOrToken> Value;
        public Node(ITreeNode<SyntaxNodeOrToken> value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        //public override bool Equals(object obj)
        //{
        //    return Value.Equals(obj);
        //}

        //public override int GetHashCode()
        //{
        //    return ToString().GetHashCode();
        //}
    }
}
