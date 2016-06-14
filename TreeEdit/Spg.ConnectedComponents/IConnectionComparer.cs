using TreeEdit.Spg.Script;

namespace TreeEdit.Spg.ConnectedComponents
{
    interface IConnectionComparer<T>
    {
        bool IsConnected(EditOperation<T> editI, EditOperation<T> editJ);
    }
}
