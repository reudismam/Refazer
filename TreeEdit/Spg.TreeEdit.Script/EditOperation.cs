using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace TreeEdit.Spg.TreeEdit.Script
{
    public abstract class EditOperation
    {
        /// <summary>
        /// Parent node associated to this edit operation
        /// </summary>
        public SyntaxNodeOrToken Parent { get; set; }

        /// <summary>
        /// Node in the source tree associated to this edit operation
        /// </summary>
        public SyntaxNodeOrToken T1Node { get; set; }

        /// <summary>
        /// index associated to this edit operation
        /// </summary>
        public int K { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="t1Node">Node in source (t1) tree</param>
        /// <param name="parent">Parent of t1 node</param>
        /// <param name="k">Position associated to this edit operation</param>
        protected EditOperation(SyntaxNodeOrToken t1Node, SyntaxNodeOrToken parent, int k)
        {
            Parent = parent;
            T1Node = t1Node;
            K = k;
        }
    }
}
