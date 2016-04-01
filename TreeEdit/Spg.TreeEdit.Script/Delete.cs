
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace TreeEdit.Spg.TreeEdit.Script
{
    public class Delete: EditOperation
    {
        public Delete(SyntaxNodeOrToken t1Node, SyntaxNodeOrToken parent, int k) : base(t1Node, parent, k)
        {
        }
    }
}
