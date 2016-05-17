using System;
using System.Collections.Generic;
using TreeEdit.Spg.Script;

namespace TreeEdit.Spg.ConnectedComponents
{
    public class TreeConnectedComponents<T>
    {
        private static Dictionary<Tuple<T, T, int>, List<EditOperation<T>>> _graph;

        private static Dictionary<Tuple<T, T, int>, int> _visited;


        private static Dictionary<Tuple<T, T, int>, List<EditOperation<T>>> Graph
        {
            get { return _graph; }
            set { _graph = value; }
        }

        public static List<List<EditOperation<T>>> ConnectedComponents(List<EditOperation<T>> script)
        {
            BuildGraph(script);

            _visited = new Dictionary<Tuple<T, T, int>, int>();

            int i = 0;
            var dic = new Dictionary<int, List<EditOperation<T>>>();
            foreach (var edit in script)
            {
                var t = Tuple.Create(edit.T1Node.Value, edit.Parent.Value, edit.K);
                if (!_visited.ContainsKey(t))
                {
                    dic.Add(i, new List<EditOperation<T>>());
                    DepthFirstSearch(edit, i++);
                }
            }
            
            foreach (var edit in script)
            {
                var t = Tuple.Create(edit.T1Node.Value, edit.Parent.Value, edit.K);
                int cc = _visited[t];

                dic[cc].Add(edit);
            }

            var ccs = new List<List<EditOperation<T>>>(dic.Values);
            
            return ccs;
        }

        private static void DepthFirstSearch(EditOperation<T> editOperation, int i)
        {
            var t = Tuple.Create(editOperation.T1Node.Value, editOperation.Parent.Value, editOperation.K);
            _visited.Add(t, i);

            foreach (var edit in Graph[t])
            {
                var te = Tuple.Create(edit.T1Node.Value, edit.Parent.Value, edit.K);
                if (!_visited.ContainsKey(te))
                {
                    DepthFirstSearch(edit, i);
                }
            }
        }

        private static void BuildGraph(List<EditOperation<T>> script)
        {
            
            Graph = new Dictionary<Tuple<T, T, int>, List<EditOperation<T>>>();

            for (int i = 0; i < script.Count; i++)
            {
                var editI = script[i];
                var ti = Tuple.Create(editI.T1Node.Value, editI.Parent.Value, editI.K);

                if (!Graph.ContainsKey(ti))
                {
                    Graph.Add(ti, new List<EditOperation<T>>());
                }

                for (int j = i + 1; j < script.Count; j++)
                {
                    var editJ = script[j];

                    //Two nodes have the same parent
                    if (editI.Parent.Equals(editJ.Parent))
                    {
                        Graph[ti].Add(editJ);
                    }

                    //T1Node from an operation is the parent in another edit operation 
                    if (editI.T1Node.Equals(editJ.Parent))
                    {
                        Graph[ti].Add(editJ);
                    }

                    if (editI is Move<T>)
                    {
                        var move = editI as Move<T>;
                        //Specific case for deleted nodes
                        if (move.PreviousParent.Equals(editJ.Parent))
                        {
                            Graph[ti].Add(editJ);
                        }
                    }
                }
            }
        }
    }
}
