using System.Collections.Generic;
using TreeEdit.Spg.Script;

namespace TreeEdit.Spg.ConnectedComponents
{
    internal abstract class ConnectionComparer<T>
    {
        public List<EditOperation<T>> Script { get; set; }
        public abstract bool IsConnected(int indexI, int indexJ);

        public ConnectionComparer(List<EditOperation<T>> script)
        {
            Script = script;
        }
    }
}
