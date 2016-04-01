using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace TreeEdit.Spg.TreeEdit.Script
{
    public class Move : Insert
    {
        /*internal int k;

        public SyntaxNodeOrToken Node { get; internal set; }*/

        public override string ToString()
        {
            return "Move(" + T1Node.Kind() + " to " + Parent.Kind() + ", " + K + ")";
        }

        public Move(SyntaxNodeOrToken t1Node, SyntaxNodeOrToken parent, int k) : base(t1Node, parent, k)
        {
        }
    }

}
