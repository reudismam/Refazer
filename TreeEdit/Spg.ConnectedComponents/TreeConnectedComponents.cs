using System.Collections.Generic;
using C5;
using Spg.TreeEdit.Node;
using TreeEdit.Spg.TreeEdit.Script;

namespace TreeEdit.Spg.ConnectedComponents
{
    public class TreeConnectedComponents<T>
    {
        private static Dictionary<EditOperation<T>, List<EditOperation<T>>> _graph;

        private static Dictionary<EditOperation<T>, int> visited;


        private static Dictionary<EditOperation<T>, List<EditOperation<T>>> Graph
        {
            get
            {
                return _graph;
            }
            set
            {
                _graph = value;
            }
        }

        public static List<List<EditOperation<T>>> ConnectedComponents(List<EditOperation<T>> script)
        {
            BuildGraph(script);

            visited = new Dictionary<EditOperation<T>, int>();

            int i = 0;
            var dic = new Dictionary<int, List<EditOperation<T>>>();
            foreach (var edit in script)
            {
                if (!visited.ContainsKey(edit))
                {
                    dic.Add(i, new List<EditOperation<T>>());
                    DFS(edit, i++);
                }
            }
            
            foreach (var edit in script)
            {
                int cc = visited[edit];

                dic[cc].Add(edit);
            }

            var ccs = new List<List<EditOperation<T>>>(dic.Values);
            
            return ccs;
        }

        private static void DFS(EditOperation<T> editOperation, int i)
        {
            visited.Add(editOperation, i);

            foreach (var edit in Graph[editOperation])
            {
                if (!visited.ContainsKey(edit))
                {
                    DFS(edit, i);
                }
            }
        }

        private static void BuildGraph(List<EditOperation<T>> script)
        {
            Graph = new Dictionary<EditOperation<T>, List<EditOperation<T>>>();

            for (int i = 0; i < script.Count; i++)
            {
                var edit_i = script[i];

                if (!Graph.ContainsKey(edit_i))
                {
                    Graph.Add(edit_i, new List<EditOperation<T>>());
                }

                for (int j = i + 1; j < script.Count; j++)
                {
                    var edit_j = script[j];

                    //Two nodes have the same parent
                    if (edit_i.Parent.Equals(edit_j.Parent))
                    {
                        Graph[edit_i].Add(edit_j);
                    }

                    //T1Node from an operation is the parent in another edit operation 
                    if (edit_i.T1Node.Equals(edit_j.Parent))
                    {
                        Graph[edit_i].Add(edit_j);
                    }

                    //Specific case for deleted nodes
                    if (edit_i.T1Node.Parent.Equals(edit_j.Parent))
                    {
                        Graph[edit_i].Add(edit_j);
                    }
                }
            }
        }
    }
}
