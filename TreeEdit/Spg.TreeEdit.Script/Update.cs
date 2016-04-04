using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace TreeEdit.Spg.TreeEdit.Script
{
    public class Update : EditOperation
    {
        public SyntaxNodeOrToken To { get; internal set; }

        /// <summary>
        /// Construct a update object
        /// </summary>
        /// <param name="from">Node that will be moved</param>
        /// <param name="to">Update node</param>
        /// <param name="parent">Where the node will go</param>
        public Update(SyntaxNodeOrToken from, SyntaxNodeOrToken to, SyntaxNodeOrToken parent) : base(from, parent, -1)
        {
            To = to;
        }

        /// <summary>
        /// String representation of this object
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Update(" + T1Node.Kind() + "- (" + T1Node + ")" + " to " + To.Kind() + "- (" + To + "))";
        }
    }
}
