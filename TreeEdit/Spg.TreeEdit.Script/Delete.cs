using Microsoft.CodeAnalysis;

namespace TreeEdit.Spg.TreeEdit.Script
{
    public class Delete: EditOperation
    {
        public Delete(SyntaxNodeOrToken deletedNode) : base(deletedNode, null, -1)
        {
        }
    }
}
