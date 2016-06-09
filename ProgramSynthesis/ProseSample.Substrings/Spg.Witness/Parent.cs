using Microsoft.CodeAnalysis;
using TreeElement.Spg.Node;

namespace ProseSample.Substrings.Spg.Witness
{
    public class Parent: Context
    {
        public override SyntaxNodeOrToken Target(ITreeNode<SyntaxNodeOrToken> sot)
        {
            if (sot.Parent == null)
            {
                return null;
            }
            return sot.Parent.Value;
        }
    }
}
