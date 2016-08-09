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

        public override string ToString()
        {
            return $"PatternP({Tree}, {K})";
        }
    }
}
