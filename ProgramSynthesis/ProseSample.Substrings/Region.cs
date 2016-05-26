using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Tutor.Spg.Node;

namespace ProseSample.Substrings
{
    public class Region
    {
        public ITreeNode<SyntaxNodeOrToken> Tree { get; set; }

        public Region(ITreeNode<SyntaxNodeOrToken> tree)
        {
            Tree = tree;
        }

        public Region() { }

        public override bool Equals(object obj)
        {
            return Tree.Equals(obj);
        }

        public override string ToString()
        {
            return Tree.ToString();
        }
    }
}
