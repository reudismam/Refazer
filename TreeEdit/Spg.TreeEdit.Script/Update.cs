using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace TreeEdit.Spg.TreeEdit.Script
{
    public class Update : EditOperation
    {
        //public SyntaxNodeOrToken From { get; internal set; }
        public SyntaxNodeOrToken To { get; internal set; }

        public override string ToString()
        {
            return "Update(" + T1Node.Kind() + "- (" + T1Node + ")" + " to " + To.Kind() + "- (" + To + "))";
        }

        public Update(SyntaxNodeOrToken from, SyntaxNodeOrToken to, SyntaxNodeOrToken parent, int k) : base(from, parent, k)
        {
            To = to;
        }
    }
}
