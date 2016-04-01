using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace TreeEdit.Spg.TreeEdit.Script
{
    public class Insert : EditOperation
    {
        public override string ToString()
        {
            return "Insert(" + T1Node.Kind() + ", " + Parent.Kind() + ", " + K + ")";
        }

        public Insert(SyntaxNodeOrToken t1Node, SyntaxNodeOrToken parent, int k) : base(t1Node, parent, k)
        {
        }
    }
}
