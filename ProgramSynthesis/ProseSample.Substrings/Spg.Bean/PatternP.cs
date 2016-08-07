using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeElement.Spg.Node;

namespace ProseSample.Substrings
{
    public class PatternP: Pattern
    {
        public int K { get; set; }
        public PatternP(ITreeNode<Token> tree, int k) : base(tree)
        {
            K = k;
        }
    }
}
