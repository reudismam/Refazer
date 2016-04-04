using Microsoft.CodeAnalysis;

namespace TreeEdit.Spg.TreeEdit.Script
{
    public class Delete: EditOperation
    {
        public Delete(SyntaxNodeOrToken movedNode, SyntaxNodeOrToken parent, int k) : base(movedNode, parent, k)
        {
        }
    }
}
