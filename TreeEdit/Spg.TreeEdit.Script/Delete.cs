using Microsoft.CodeAnalysis;

namespace TreeEdit.Spg.TreeEdit.Script
{
    public class Delete: EditOperation
    {
        public Delete(SyntaxNodeOrToken deletedNode) : base(deletedNode, null, -1)
        {
        }

        public override string ToString()
        {
            return "Delete(" + T1Node + ")";
        }
    }
}
