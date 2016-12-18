using ProseFunctions.Substrings;
using TreeElement.Spg.Node;
using TreeElement.Spg.Walker;

namespace TreeEdit.Spg.TreeEdit.Update
{
    public class TreeManager<T>
    {
        public static TreeNode<T> GetNodeAtHeight(TreeNode<T> tree, int height)
        {
            var dist = BFSWalker<T>.Dist(tree);
            var targetNodeHeight = ConverterHelper.TreeAtHeight(tree, dist, height);
            return targetNodeHeight;
        }
    }
}
