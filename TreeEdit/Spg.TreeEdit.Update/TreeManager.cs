using ProseSample.Substrings;
using TreeElement.Spg.Walker;

namespace TreeEdit.Spg.TreeEdit.Update
{
    public class TreeManager<T>
    {
        public static ITreeNode<T> GetNodeAtHeight(ITreeNode<T> tree, int height)
        {
            var dist = BFSWalker<T>.Dist(tree);
            var targetNodeHeight = ConverterHelper.TreeAtHeight(tree, dist, height);
            return targetNodeHeight;
        }
    }
}
