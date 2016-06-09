using Microsoft.CodeAnalysis;
using TreeElement.Spg.Node;

namespace ProseSample.Substrings.Spg.Witness
{
    public class Parent: Context
    {
        public override SyntaxNodeOrToken Target(ITreeNode<SyntaxNodeOrToken> sot)
        {
            return sot.Parent.Value;
        }
    }
}
