using System;
using System.Collections.Generic;
using System.Linq;
using LCA.Spg.Manager;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using TreeEdit.Spg.Script;
using ProseFunctions.Substrings;
using TreeEdit.Spg.TreeEdit.Update;
using TreeElement.Spg.Node;

namespace TreeEdit.Spg.ConnectedComponents
{
    /// <summary>
    /// Compute connected components.
    /// </summary>
    public class ConnectedComponentManager<T>
    {
        /// <summary>
        /// Visited edit operations. Required to compute connected components (DFS implementation).
        /// </summary>
        private static Dictionary<Tuple<T, T, int>, int> _visited;

        private static TreeNode<SyntaxNodeOrToken> _inputTree;

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
            foreach (var edit in primaryEditions)
            {
                var t = Tuple.Create(edit.T1Node.Value, edit.Parent.Value, edit.K);
                if (!_visited.ContainsKey(t))
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
            dic = JoinPrimaryOperationsByParent(primaryEditions, i, dic);
            var ccs = new List<List<EditOperation<T>>>(dic.Values);
            return ccs;
        }

        private static Dictionary<int, List<EditOperation<T>>> JoinPrimaryOperationsByParent(List<EditOperation<T>> primaryEditions, int i, Dictionary<int, List<EditOperation<T>>> connectedComponentsDictionary)
        {
            //Parent dictionary, which contains the parent and the connected components to this parent.
            var parentDictionary = new Dictionary<TreeNode<T>, HashSet<int>>();
            foreach (var v in primaryEditions.Where(v => !parentDictionary.ContainsKey(v.Parent)))
            {
                parentDictionary.Add(v.Parent, new HashSet<int>());
            }

            foreach (var vi in primaryEditions)
            {
                foreach (var vj in primaryEditions)
                {
                    if (IsConnected(vi, vj))
                    {
                        var t = Tuple.Create(vj.T1Node.Value, vj.Parent.Value, vj.K);
                        var index = _visited[t];
                        parentDictionary[vi.Parent].Add(index);
                    }
                }
            }

            foreach (var keypair in parentDictionary)
            {
                if (keypair.Value.Count > 1)
                {
                    i++;
                    foreach (var index in keypair.Value)
                    {
                        if (connectedComponentsDictionary.ContainsKey(index))
                        {
                            if (!connectedComponentsDictionary.ContainsKey(i))
                            {
                                connectedComponentsDictionary.Add(i, new List<EditOperation<T>>());
                            }
                            connectedComponentsDictionary[i].AddRange(connectedComponentsDictionary[index]);
                            connectedComponentsDictionary.Remove(index);
                        }
                    }
                }
            }

            //connectedComponentsDictionary = JoinUpdateOperations(connectedComponentsDictionary);
            return connectedComponentsDictionary;
        }

        private static Dictionary<int, List<EditOperation<T>>> JoinUpdateOperations(Dictionary<int, List<EditOperation<T>>> connectedComponentsDictionary)
        {
            //If the dictionary contains only an operation there is nothing to be joined.
            if (connectedComponentsDictionary.Count == 1) return connectedComponentsDictionary;
            var joinList = new List<Tuple<EditOperation<T>, EditOperation<T>>>();
            foreach (var keypair in connectedComponentsDictionary)
            {
                //If list of editions does not contains update do not process
                if (!keypair.Value.All(o => o is Update<T>)) continue;
                //First operation in the connected components, which represents this component.
                var operation = keypair.Value.First();
                EditOperation<T> closest = null;
                TreeNode<T> validNode = null;
                int closestDistance = -1;
                foreach (var keypaircomp in connectedComponentsDictionary)
                {
                    if (keypair.Key == keypaircomp.Key) continue;
                    //If list of editions only contains update do not process
                    if (keypair.Value.All(o => o is Update<T>) && keypaircomp.Value.All(o => o is Update<T>)) continue;

                    var toComputeLcaList = new List<SyntaxNodeOrToken>();
                    var t1NodeValueOperation = (SyntaxNodeOrToken) (object) operation.T1Node.Value;
                    toComputeLcaList.Add(t1NodeValueOperation);
                    foreach (var otherOperation in keypaircomp.Value)
                    {
                        TreeNode<T> tocompare = otherOperation is Update<T> || otherOperation is Delete<SyntaxNodeOrToken> ? otherOperation.T1Node : otherOperation.Parent;
                        var otherOperationValue = (SyntaxNodeOrToken) (object) tocompare.Value;
                        if (TreeUpdate.FindNode(_inputTree, otherOperationValue) != null)
                        {
                            validNode = tocompare;
                            toComputeLcaList.Add(otherOperationValue);
                        }
                    }
                    var lca = LCAManager.GetInstance().LeastCommonAncestor(toComputeLcaList, _inputTree.Value);
                    var distToOp = DistanceToRoot(operation.T1Node, lca);
                    var distToPop = DistanceToRoot(validNode, lca);
                    if (Math.Abs(distToOp - distToPop) > closestDistance)
                    {
                        closest = keypaircomp.Value.Last();
                        closestDistance = Math.Abs(distToOp - distToPop);
                    }
                }
                if (closest != null)
                {
                    joinList.Add(Tuple.Create(operation, closest));
                }
            }

            foreach (var v in joinList)
            {
                var vi = v.Item1;
                var vj = v.Item2;

                var ti = Tuple.Create(vi.T1Node.Value, vi.Parent.Value, vi.K);
                var indexi = _visited[ti];

                var tj = Tuple.Create(vj.T1Node.Value, vj.Parent.Value, vj.K);
                var indexj = _visited[tj];

                connectedComponentsDictionary[indexj].AddRange(connectedComponentsDictionary[indexi]);
                connectedComponentsDictionary.Remove(indexi);
            }
            return connectedComponentsDictionary;
        }

        public static int DistanceToRoot(TreeNode<T> target, SyntaxNodeOrToken parent)
        {
            int dist = 0;
            for (TreeNode<T> node = target; node != null && !node.Value.Equals(parent); node = node.Parent)
            {
                dist++;
            }
            return dist;
        }

        private static bool IsConnected(EditOperation<T> vi, EditOperation<T> vj)
        {
            var parentDescendantNodesAndSelf = GetNode(vi.Parent.Value).DescendantNodesAndSelf();
            return parentDescendantNodesAndSelf.Contains(vj.Parent) && !vi.Parent.IsLabel(new TLabel(SyntaxKind.ClassDeclaration));
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

        public static List<List<EditOperation<T>>> ConnectedComponents(List<EditOperation<T>> primaryEditions, List<EditOperation<T>> editOperations, TreeNode<SyntaxNodeOrToken> inpTree)
        {
            ConnectionComparer = new FullConnected(editOperations);
            _inputTree = inpTree;
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

                if (!(editI.GetType() == editJ.GetType() || editI is Update<SyntaxNode> || editJ is Update<SyntaxNodeOrToken>))
                {
                    return false;
                }

                if (editI.T1Node.DescendantNodesAndSelf().Contains(editJ.Parent)) return true;
                if (editI.T1Node.DescendantNodesAndSelf().Contains(editJ.T1Node)) return true;

                var nodes = GetNode(editI.T1Node.Value);
                if (nodes.DescendantNodesAndSelf().Contains(editJ.T1Node)) return true;
                if (editI.Parent.Parent != null && editI.Parent.Parent.Equals(editJ.T1Node)) return true;

                if (editI is Update<T>)
                {
                    var update = editI as Update<T>;
                    var toNodeParent = GetNode(update.ToParent.Value);
                    if (toNodeParent.DescendantNodesAndSelf().Contains(editJ.T1Node)) return true;
                }

                if (editJ is Update<T>)
                {
                    var update = editJ as Update<T>;
                    var parentNodes = GetNode(editI.Parent.Value);
                    if (parentNodes.DescendantNodesAndSelf().Contains(editJ.T1Node)) return true;
                    if (parentNodes.DescendantNodesAndSelf().Contains(update.To)) return true;
                }
                return false;
            }
        }

        private static TreeNode<T> GetNode(T value)
        {
            SyntaxNodeOrToken newT2 = (SyntaxNodeOrToken)(object)value;
            var newnode = ConverterHelper.ConvertCSharpToTreeNode(newT2);
            TreeNode<T> newT1 = (TreeNode<T>)(object)newnode;
            return newT1;
        }

        private static SyntaxNodeOrToken GetSyntaxNode(T value)
        {
            SyntaxNodeOrToken newT2 = (SyntaxNodeOrToken)(object)value;
            return newT2;
        }

        ////public static List<Edit<SyntaxNodeOrToken>> PrimaryEditions(Script script)
        ////{
        ////    var edits = script.Edits.Select(o => o.EditOperation).ToList();
        ////    var primaryEditions = ConnectedComponentManager<SyntaxNodeOrToken>.PrimaryEditions(edits);
        ////    var children = primaryEditions.Select(o => new Edit<SyntaxNodeOrToken>(o)).ToList();
        ////}

        /// <summary>
        /// Compute the primary editions. The primary editions are editions
        /// that do not depend that another edit to be created to exist.
        /// </summary>
        /// <param name="script">Script to create the primary editions</param>
        public static List<EditOperation<T>> PrimaryEditions(List<EditOperation<T>> script)
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
                    //The two edition are connected, but they are not siblings.
                    if (ConnectionComparer.IsConnected(i, j) && !editI.Parent.Equals(editJ.Parent))
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
