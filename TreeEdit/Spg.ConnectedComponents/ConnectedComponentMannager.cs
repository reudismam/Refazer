﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using TreeEdit.Spg.Script;
using TreeElement.Spg.Node;

namespace TreeEdit.Spg.ConnectedComponents
{
    /// <summary>
    /// Compute connected components.
    /// </summary>
    public class ConnectedComponentMannager<T>
    {
        /// <summary>
        /// Visited edit operations. Required to compute connected components (DFS implementation).
        /// </summary>
        private static Dictionary<Tuple<T, T, int>, int> _visited;

        /// <summary>
        /// Edit operations graph
        /// </summary>
        private static Dictionary<Tuple<T, T, int>, List<EditOperation<T>>> Graph { get; set; }

        /// <summary>
        /// Connected component strategy
        /// </summary>
        private static ConnectionComparer<T> ConnectionComparer { get; set; }

        /// <summary>
        /// Compute connected components.
        /// </summary>
        /// <param name="primaryEditions"></param>
        /// <param name="editOperations">edit script</param>
        private static List<List<EditOperation<T>>> ComputeConnectedComponents(List<EditOperation<T>> primaryEditions, List<EditOperation<T>> editOperations)
        {
            _visited = new Dictionary<Tuple<T, T, int>, int>();
            int i = 0;
            var dic = new Dictionary<int, List<EditOperation<T>>>();
            foreach (var edit in editOperations)
            {
                var t = Tuple.Create(edit.T1Node.Value, edit.Parent.Value, edit.K);
                if (!_visited.ContainsKey(t) && primaryEditions.Contains(edit))
                {
                    dic.Add(i, new List<EditOperation<T>>());
                    DepthFirstSearch(edit, i++);
                }
            }
            foreach (var edit in editOperations)
            {
                var t = Tuple.Create(edit.T1Node.Value, edit.Parent.Value, edit.K);
                int cc = _visited[t];
                dic[cc].Add(edit);
            }
            var ccs = new List<List<EditOperation<T>>>(dic.Values);
            return ccs;
        }

        /// <summary>
        /// Depth first search
        /// </summary>
        /// <param name="editOperation"></param>
        /// <param name="i"></param>
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

        public static List<List<EditOperation<T>>> ConnectedComponents(List<EditOperation<T>> primaryEditions, List<EditOperation<T>> editOperations)
        {
            ConnectionComparer = new FullConnected(editOperations);
            BuildDigraph(editOperations);
            var ccs = ComputeConnectedComponents(primaryEditions, editOperations);
            return ccs;
        }

        /// <summary>
        /// Build a digraph of the transformation. An edition i is connected to edition j if edit j depends
        /// that edit i insert a node in the tree.
        /// </summary>
        /// <param name="script">List of operations</param>
        private static void BuildDigraph(List<EditOperation<T>> script)
        {
            Graph = new Dictionary<Tuple<T, T, int>, List<EditOperation<T>>>();
            foreach (var edit in script)
            {
                var t = Tuple.Create(edit.T1Node.Value, edit.Parent.Value, edit.K);
                Graph[t] = new List<EditOperation<T>>();
            }

            for (int i = 0; i < script.Count; i++)
            {
                var editI = script[i];
                var ti = Tuple.Create(editI.T1Node.Value, editI.Parent.Value, editI.K);
                
                for (int j = 0; j < script.Count; j++)
                {
                    if (i == j) continue;
                    var editJ = script[j];
                    var tj = Tuple.Create(editJ.T1Node.Value, editJ.Parent.Value, editJ.K);

                    if (ConnectionComparer.IsConnected(i, j))
                    {
                        Graph[ti].Add(editJ);
                        Graph[tj].Add(editI);
                    }
                }
            }
        }

        private class FullConnected : ConnectionComparer<T>
        {
            public FullConnected(List<EditOperation<T>> script) : base(script)
            {
            }

            public override bool IsConnected(int indexI, int indexJ)
            {
                var editI = Script[indexI];
                var editJ = Script[indexJ];
      
                if (editI.T1Node.DescendantNodesAndSelf().Contains(editJ.Parent)) return true;
                if (editI.T1Node.DescendantNodesAndSelf().Contains(editJ.T1Node)) return true;

                var nodes = GetNodes(editI.T1Node.Value);
                if (nodes.DescendantNodesAndSelf().Contains(editJ.T1Node)) return true;
                if (editI.Parent.Parent != null && editI.Parent.Parent.Equals(editJ.T1Node)) return true;

                if (editI is Update<T>)
                {
                    var update = editI as Update<T>;
                    var toNodeParent = GetNodes(update.ToParent.Value);
                    if (toNodeParent.DescendantNodesAndSelf().Contains(editJ.T1Node)) return true;
                }

                if (editJ is Update<T>)
                {
                    var update = editJ as Update<T>;
                    var parentNodes = GetNodes(editI.Parent.Value);
                    if (parentNodes.DescendantNodesAndSelf().Contains(editJ.T1Node)) return true;
                    if (parentNodes.DescendantNodesAndSelf().Contains(update.To)) return true;
                }
                return false;
            }

            private ITreeNode<T> GetNodes(T value)
            {
                SyntaxNodeOrToken newT2 = (SyntaxNodeOrToken)(object)value;
                var newnode = ConverterHelper.ConvertCSharpToTreeNode(newT2);
                ITreeNode<T> newT1 = (ITreeNode<T>)(object)newnode;
                return newT1;
            }
        }

        public static List<EditOperation<T>> ComputePrimaryEditions(List<EditOperation<T>> script)
        {
            ConnectionComparer = new FullConnected(script);
            var primariesFlag = script.Select(o => true).ToList();
            for (int i = 0; i < script.Count; i++)
            {
                var editI = script[i];
                for (int j = 0; j < script.Count; j++)
                {
                    if (i == j) continue;
                    var editJ = script[j];
                    if (!editI.Parent.Equals(editJ.Parent) && ConnectionComparer.IsConnected(i, j))
                    {
                        primariesFlag[j] = false; //j is not a primary operation
                    }
                }
            }
            var primaries = script.Where((t, i) => primariesFlag[i]).ToList();
            return primaries;
        }
    }
}
