using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using TreeEdit.Spg.Script;
using TreeElement.Spg.Node;

namespace TreeEdit.Spg.ConnectedComponents
{
    public class ConnectedComponentMannager<T>
    {
        /// <summary>
        /// Visited edit operations
        /// </summary>
        private static Dictionary<Tuple<T, T, int>, int> _visited;

        /// <summary>
        /// Edit operations graph
        /// </summary>
        private static Dictionary<Tuple<T, T, int>, List<EditOperation<T>>> Graph { get; set; }

        private static ConnectionComparer<T> ConnectionComparer { get; set; }

        /// <summary>
        /// Computed connected components
        /// </summary>
        /// <param name="script">edit script</param>
        /// <returns>Connected components</returns>
        private static List<List<EditOperation<T>>> ConnectedComponentsBase(List<EditOperation<T>> script)
        {
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

        public static List<List<EditOperation<T>>> ConnectedComponents(List<EditOperation<T>> script)
        {
            ConnectionComparer = new FullConnected(script);
            BuildGraph(script);        
            var ccs =  ConnectedComponentsBase(script);
            return ccs;
        }

        public static List<List<EditOperation<T>>> EditConnectedComponents(List<EditOperation<T>> script)
        {
            ConnectionComparer = new EditConnected(script);
            BuildGraph(script);
            return ConnectedComponentsBase(script);
        }

        public static List<List<EditOperation<T>>> DescendantsConnectedComponents(List<EditOperation<T>> script)
        {
            ConnectionComparer = new DescendantsConnected(script);
            BuildGraph(script);
            return ConnectedComponentsBase(script);
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

            foreach (var edit in script)
            {
                var t = Tuple.Create(edit.T1Node.Value, edit.Parent.Value, edit.K);
                Graph[t] = new List<EditOperation<T>>();
            }

            for (int i = 0; i < script.Count; i++)
            {
                var editI = script[i];
                var ti = Tuple.Create(editI.T1Node.Value, editI.Parent.Value, editI.K);

                for (int j = i + 1; j < script.Count; j++)
                {
                    var editJ = script[j];
                    var tj = Tuple.Create(editJ.T1Node.Value, editJ.Parent.Value, editJ.K);

                    if (IsConnected(i, j))
                    {
                        Graph[ti].Add(editJ);
                        Graph[tj].Add(editI);
                    }
                }
            }
        }

        private static bool IsConnected(int editI, int editJ)
        {
            return ConnectionComparer.IsConnected(editI, editJ) || ConnectionComparer.IsConnected(editJ, editI);
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

                //Two nodes have the same parent
                if (editI.Parent.Equals(editJ.Parent) && IsValidBlock(editI.Parent) && !editI.Parent.IsLabel(new TLabel(SyntaxKind.ClassDeclaration)) /*&& !editI.Parent.IsLabel(new TLabel(SyntaxKind.SwitchStatement))*/) return true;        

                //T1Node from an operation is the parent in another edit operation 
                if (editI.T1Node.DescendantNodesAndSelf().Contains(editJ.Parent)) return true;

                if (editI.T1Node.DescendantNodesAndSelf().Contains(editJ.T1Node)) return true;

                //var parentNodes = GetNodes(editI.Parent.Value);
                //if (parentNodes.DescendantNodesAndSelf().Contains(editJ.T1Node)) return true;
                //if (parentNodes.DescendantNodesAndSelf().Contains(editJ.Parent)) return true;

                if (editI.Parent.Parent != null && editI.Parent.Parent.Equals(editJ.T1Node)) return true;

                if (editI is Move<T>)
                {
                    var move = editI as Move<T>;
                    //Specific case for move nodes
                    if (move.PreviousParent.Equals(editJ.Parent)) return false;
                }

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
                SyntaxNodeOrToken newT2 = (SyntaxNodeOrToken)(object) value;
                var newnode = ConverterHelper.ConvertCSharpToTreeNode(newT2);
                ITreeNode<T> newT1 = (ITreeNode<T>)(object) newnode;
                return newT1;
            }
        }

        public static bool IsValidBlock(ITreeNode<T> parent)
        {
            if (!parent.IsLabel(new TLabel(SyntaxKind.Block))) return true;

            //T newT1 = (T)(object) parent.Value;
            SyntaxNodeOrToken newT2 = (SyntaxNodeOrToken)(object)parent.Value;
            if (newT2.Parent.IsKind(SyntaxKind.MethodDeclaration))
            {
                return false;
            }
            return true;
        }

        private class EditConnected : ConnectionComparer<T>
        {
            public EditConnected(List<EditOperation<T>> script) : base(script)
            {
            }
            public override bool IsConnected(int indexI, int indexJ)
            {
                var editI = Script[indexI];
                var editJ = Script[indexJ];

                var typeI = editI.GetType();
                var typeJ = editJ.GetType();

                if (!(editJ is Update<T>) && typeI != typeJ) return false;

                for (int i = Math.Min(indexI, indexJ) + 1; i < Math.Max(indexI, indexJ); i++)
                {
                    var type = Script[i].GetType();
                    if (!(Script[i] is Update<T>) && type != typeI) return false;
                }

                return new DescendantsConnected(Script).IsConnected(indexI, indexJ);
            }
        }

        private class DescendantsConnected : ConnectionComparer<T>
        {
            public DescendantsConnected(List<EditOperation<T>> script) : base(script)
            {
            }
            public override bool IsConnected(int indexI, int indexJ)
            {
                var editI = Script[indexI];
                var editJ = Script[indexJ];

                //Two nodes have the same parent
                if (editI.Parent.DescendantNodesAndSelf().Contains(editJ.Parent) && IsValidBlock(editI.Parent)) return true;

                //T1Node from an operation is the parent in another edit operation 
                if (editI.T1Node.DescendantNodesAndSelf().Contains(editJ.Parent)) return true;

                if (editI.Parent.Parent != null && editI.Parent.Parent.DescendantNodesAndSelf().Contains(editJ.T1Node)) return true;

                if (editI is Move<T>)
                {
                    var move = editI as Move<T>;
                    //Specific case for move nodes
                    if (move.PreviousParent.DescendantNodesAndSelf().Contains(editJ.Parent)) return false;
                }
                return false;
            }          
        }
    }
}
