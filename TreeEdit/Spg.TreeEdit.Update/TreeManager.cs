using TreeElement.Spg.Node;
using TreeElement.Spg.Walker;

namespace TreeEdit.Spg.TreeEdit.Update
{
    public class TreeManager<T>
    {
        public static ITreeNode<T> GetNodeAtHeight(ITreeNode<T> tree)
        {
            var dist = BFSWalker<T>.Dist(tree);
            var targetNodeHeight = ConverterHelper.TreeAtHeight(tree, dist, 2);
            return targetNodeHeight;
        }
    }
}
