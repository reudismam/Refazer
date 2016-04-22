using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TreeEdit.Spg.TreeEdit.Isomorphic;
using TreeEdit.Spg.TreeEdit.Mapping;
using TreeEdit.Spg.TreeEdit.Script;

namespace TreeEdit.Spg.TreeEdit.Update
{
    public class TreeUpdate
    {
        /// <summary>
        /// Map to annotation to each edit operations
        /// </summary>
        public Dictionary<EditOperation, SyntaxAnnotation> Ann { get; set; }

        /// <summary>
        /// Newest updated tree
        /// </summary>
        public SyntaxNodeOrToken CurrentTree { get; set; }

        /// <summary>
        /// Map element from T1 (before tree) to T2 (after tree) nodes
        /// </summary>
        // ReSharper disable once InconsistentNaming
        private Dictionary<SyntaxNodeOrToken, SyntaxNodeOrToken> _M;

        /// <summary>
        /// Map each annoted node
        /// </summary>
        private Dictionary<SyntaxNode, List<SyntaxAnnotation>> _annts;

        /// <summary>
        /// Indicate the edit operations that was processed.
        /// </summary>
        public Dictionary<EditOperation, bool> Processed;

        private List<EditOperation> _script;


        /// <summary>
        /// Update the tree following the edit script
        /// </summary>
        /// <param name="script">Edit script</param>
        /// <param name="tree">Tree to be updated</param>
        /// <param name="M">Mapping from each element for the before and after tree.</param>
        // ReSharper disable once InconsistentNaming
        public void UpdateTree(List<EditOperation> script, SyntaxNodeOrToken tree, Dictionary<SyntaxNodeOrToken, SyntaxNodeOrToken> M)
        {
            _script = script;
            PreProcessTree(script, tree, M);

            foreach (var item in script)
            {
                if (!Processed.ContainsKey(item))
                {
                    ProcessEditOperation(item);
                }
            }
        }

        public void PreProcessTree(List<EditOperation> script, SyntaxNodeOrToken tree, Dictionary<SyntaxNodeOrToken, SyntaxNodeOrToken> M)
        {
            _script = script;
            InitializeAtributes(tree, M);
            CreateEditionDictionary(script);
            Annotate(script); //We also need to update the dict.
        }

        /// <summary>
        /// Annotate the tree
        /// </summary>
        /// <param name="script">Edition script</param>
        private void Annotate(List<EditOperation> script)
        {
            var traversalIndex = new Dictionary<SyntaxNodeOrToken, int>();

            TreeTraversal traversal = new TreeTraversal();
            var order = traversal.PostOrderTraversal(CurrentTree);

            for (int index = 0; index < order.Count; index++)
            {
                var node = order[index];
                if (_annts.ContainsKey(node.AsNode()))
                {
                    traversalIndex.Add(node, index);
                }
            }

            foreach (var item in _annts)
            {
                traversal = new TreeTraversal();
                order = traversal.PostOrderTraversal(CurrentTree);

                if (traversalIndex.ContainsKey(item.Key))
                {
                    int keyIndex = traversalIndex[item.Key];
                    SyntaxNode key = order.ElementAt(keyIndex).AsNode();
                    var annVisitor = new AddAnnotationRewriter(key, item.Value);
                    CurrentTree = annVisitor.Visit(CurrentTree.AsNode());
                }
            }
        }

        /// <summary>
        /// Intialize global attributes
        /// </summary>
        /// <param name="tree">Source tree</param>
        /// <param name="M">Mapping</param>
        // ReSharper disable once InconsistentNaming
        private void InitializeAtributes(SyntaxNodeOrToken tree, Dictionary<SyntaxNodeOrToken, SyntaxNodeOrToken> M)
        {
            CurrentTree = tree;
            _M = M;
            Ann = new Dictionary<EditOperation, SyntaxAnnotation>();
            _annts = new Dictionary<SyntaxNode, List<SyntaxAnnotation>>();
            Processed = new Dictionary<EditOperation, bool>();
        }

        /// <summary>
        /// Create edition dictionary. The key contains the updated node 
        /// and the value, the list of operations in this node.
        /// </summary>
        /// <param name="script"></param>
        private void CreateEditionDictionary(List<EditOperation> script)
        {
            int id = 0;
            foreach (var s in script)
            {
                AnnotateMoveOperation(s, id);
                AnnotateInsertOperation(s, id);
                AnnotateUpdateOperation(s, id);
                AnnotateDeleteOperation(s);

                id++;
            }
        }

        private void AnnotateDeleteOperation(EditOperation editOperation)
        {
            if (editOperation is Delete)
            {
                SyntaxAnnotation sn = new SyntaxAnnotation("DEL");
                Ann.Add(editOperation, sn);

                if (!_annts.ContainsKey(editOperation.T1Node.AsNode()))
                {
                    _annts[editOperation.T1Node.AsNode()] = new List<SyntaxAnnotation>();
                }
                _annts[editOperation.T1Node.AsNode()].Add(sn);
            }
        }

        /// <summary>
        /// Annotate move operations
        /// </summary>
        /// <param name="eop">Edition operation</param>
        /// <param name="id">Unique Id</param>
        private void AnnotateMoveOperation(EditOperation eop, int id)
        {
            if (eop is Move)
            {
                if (!_annts.ContainsKey(eop.Parent.AsNode()))
                {
                    _annts[eop.Parent.AsNode()] = new List<SyntaxAnnotation>();
                    SyntaxAnnotation sn = new SyntaxAnnotation("ANC" + id);
                    _annts[eop.Parent.AsNode()].Add(sn);
                }
                Ann.Add(eop, _annts[eop.Parent.AsNode()].First());
            }
        }

        /// <summary>
        /// Annotate update operations
        /// </summary>
        /// <param name="s">Edit operation</param>
        /// <param name="id">Unique id</param>
        private void AnnotateUpdateOperation(EditOperation s, int id)
        {
            if (s is Script.Update)
            {
                if (!_annts.ContainsKey(s.Parent.AsNode()))
                {
                    _annts[s.Parent.AsNode()] = new List<SyntaxAnnotation>();
                    SyntaxAnnotation sn = new SyntaxAnnotation("ANC" + id);
                    _annts[s.Parent.AsNode()].Add(sn);
                }

                Ann.Add(s, _annts[s.Parent.AsNode()].First());
            }
        }

        //TODO refactor this method (AnnotateInsertOperation)
        /// <summary>
        /// Annotate insert operation
        /// </summary>
        /// <param name="eop">edit operation</param>
        /// <param name="id">Unique id</param>
        private void AnnotateInsertOperation(EditOperation eop, int id)
        {
            if (eop is Insert)
            {
                var y = eop.T1Node.Parent;
                var z = _M.ToList().Find(o => o.Value.Equals(y)).Key;

                if (!_annts.ContainsKey(z.AsNode()))
                {
                    _annts.Add(z.AsNode(), new List<SyntaxAnnotation>());
                    SyntaxAnnotation upAnn = new SyntaxAnnotation("ANC" + id);
                    _annts[z.AsNode()].Add(upAnn);
                }
                Ann[eop] = _annts[z.AsNode()].First();
            }
        }

        public SyntaxNodeOrToken ProcessEditOperation(EditOperation operation)
        {
            foreach (var editOperation in _script)
            {
                ProcessEditOperation(editOperation, null, null);
            }
            return null;
        }


        /// <summary>
        /// Process edit operation
        /// </summary>
        /// <param name="eop">Edit operation</param>
        /// <param name="currentNode">Current node</param>
        /// <param name="uptNode">Updated node</param>
        /// <returns>Node after running edit operation</returns>
        private SyntaxNode ProcessEditOperation(EditOperation eop, SyntaxNode currentNode, SyntaxNode uptNode)
        {
            if (eop is Insert)
            {
                currentNode = ProcessInsertOperation(eop);
            }

            if (eop is Move)
            {
                currentNode = ProcessMoveOperation(eop);
            }

            if (eop is Script.Update)
            {
                currentNode = ProcessUpdateOperation(eop);
            }
            return currentNode;
        }

        /// <summary>
        /// Process insert operation
        /// </summary>
        /// <param name="eop">Insert operation</param>
        /// <returns>Updated version of current node</returns>
        private SyntaxNode ProcessInsertOperation(EditOperation eop)
        {
            var oldNode = OldAnchor(eop);
            var replacement = oldNode;

            int k = eop.K - 1;

            //TODO refactor this code: rush
            var oldChild = eop.Parent.AsNode().ChildNodes().ElementAt(k);
            bool b = ScriptContains(oldChild) || IsomorphicManager.IsIsomorphic(oldChild, eop.T1Node);

            var parentAnnotation = new SyntaxAnnotation(Ann[eop].Kind + "Parent");

            AddAnnotationRewriter addAnn = new AddAnnotationRewriter(oldNode, new List<SyntaxAnnotation> { parentAnnotation, Ann[eop] });

            replacement = addAnn.Visit(replacement);

            var childAnnotation = new SyntaxAnnotation(Ann[eop].Kind + "Child");
            var childAnnotation1 = GetChildAnnotation(eop);

            AddAnnotationRewriter childAnn;
            if (childAnnotation1 != null)
            {
                childAnn = new AddAnnotationRewriter(eop.T1Node.AsNode(), new List<SyntaxAnnotation> { childAnnotation, childAnnotation1 });
            }
            else
            {
                childAnn = new AddAnnotationRewriter(eop.T1Node.AsNode(), new List<SyntaxAnnotation> { childAnnotation });
            }

            var newNode = childAnn.Visit(eop.T1Node.AsNode());

            var children = replacement.ChildNodes();

            var child = children.ElementAt(k);

            if (b)
            {
                replacement = replacement.ReplaceNode(replacement.FindNode(child.Span), newNode);
            }
            else
            {
                replacement = replacement.InsertNodesBefore(replacement.FindNode(child.Span), new List<SyntaxNode> { newNode });
            }

            UpdateTreeRewriter reTree = new UpdateTreeRewriter(oldNode, replacement);
            CurrentTree = reTree.Visit(CurrentTree.AsNode());

            var replacementChild = CurrentTree.AsNode().GetAnnotatedNodes(childAnnotation).First();
            replacement = CurrentTree.AsNode().GetAnnotatedNodes(parentAnnotation).First();

            var oldList = new List<SyntaxNodeOrToken> { eop.T1Node, eop.Parent };
            var replacementList = new List<SyntaxNodeOrToken> { replacementChild, replacement };

            for (int i = 0; i < replacementList.Count; i++)
            {
                var replacementNode = replacementList[i];
                var oldNodeEop = oldList[i];
                foreach (var editOperation in _script)
                {
                    if (editOperation.T1Node != null && editOperation.T1Node.Equals(oldNodeEop))
                    {
                        editOperation.T1Node = replacementNode;
                    }

                    if (editOperation.Parent != null && editOperation.Parent.Equals(oldNodeEop))
                    {
                        editOperation.Parent = replacementNode;
                    }
                }
            }
            return replacement;
        }

        private SyntaxAnnotation GetChildAnnotation(EditOperation eop)
        {
            var sot = eop.T1Node;

            foreach (var edit in _script)
            {
                if (sot.Span.Contains(edit.Parent.Span) && edit.Parent.Span.Contains(sot.Span))
                {
                    return Ann[edit];
                }
            }

            return null;
        }


        /// <summary>
        /// Process move operation
        /// </summary>
        /// <param name="eop">Move operation</param>
        /// <returns>Updated version of current node</returns>
        private SyntaxNode ProcessMoveOperation(EditOperation eop)
        {
            var oldNode = OldAnchor(eop);

            var replacement = oldNode;

            int k = eop.K - 1;

            //TODO refactor this code: rush
            //var oldChild = oldNode.ChildNodes().ElementAt(k);

            bool b = ScriptContains(eop.T1Node);

            var parentAnnotation = new SyntaxAnnotation(Ann[eop].Kind + "Parent");

            AddAnnotationRewriter addAnn = new AddAnnotationRewriter(oldNode, new List<SyntaxAnnotation> { parentAnnotation, Ann[eop] });

            replacement = addAnn.Visit(replacement);

            var childAnnotation = new SyntaxAnnotation(Ann[eop].Kind + "Child");
            var childAnnotation1 = GetChildAnnotation(eop);

            AddAnnotationRewriter childAnn;
            if (childAnnotation1 != null)
            {
                childAnn = new AddAnnotationRewriter(eop.T1Node.AsNode(), new List<SyntaxAnnotation> { childAnnotation, childAnnotation1 });
            }
            else
            {
                childAnn = new AddAnnotationRewriter(eop.T1Node.AsNode(), new List<SyntaxAnnotation> { childAnnotation });
            }

            var newNode = childAnn.Visit(eop.T1Node.AsNode());

            var children = replacement.ChildNodes();

            var child = children.ElementAt(k);

            if (b)
            {
                replacement = replacement.ReplaceNode(replacement.FindNode(child.Span), newNode);
            }
            else
            {
                replacement = replacement.InsertNodesBefore(replacement.FindNode(child.Span), new List<SyntaxNode> { newNode });
            }

            UpdateTreeRewriter reTree = new UpdateTreeRewriter(oldNode, replacement);
            CurrentTree = reTree.Visit(CurrentTree.AsNode());

            var replacementChild = CurrentTree.AsNode().GetAnnotatedNodes(childAnnotation).First();
            replacement = CurrentTree.AsNode().GetAnnotatedNodes(parentAnnotation).First();

            var oldList = new List<SyntaxNodeOrToken> { eop.T1Node, eop.Parent };
            var replacementList = new List<SyntaxNodeOrToken> { replacementChild, replacement };

            for (int i = 0; i < replacementList.Count; i++)
            {
                var replacementNode = replacementList[i];
                var oldNodeEop = oldList[i];
                foreach (var editOperation in _script)
                {
                    if (editOperation.T1Node != null && editOperation.T1Node.Equals(oldNodeEop))
                    {
                        editOperation.T1Node = replacementNode;
                    }

                    if (editOperation.Parent != null && editOperation.Parent.Equals(oldNodeEop))
                    {
                        editOperation.Parent = replacementNode;
                    }
                }
            }
            return replacement;
            //var parentAnnotation = new SyntaxAnnotation(Ann[eop].Kind + "Parent");
            //AddAnnotationRewriter addAnn = new AddAnnotationRewriter(oldNode, new List<SyntaxAnnotation> { parentAnnotation, Ann[eop] });
            //replacement = addAnn.Visit(replacement);

            //var childAnnotation = new SyntaxAnnotation(Ann[eop] + "Child");
            //AddAnnotationRewriter childAnn = new AddAnnotationRewriter(eop.T1Node.AsNode(), new List<SyntaxAnnotation> { childAnnotation, Ann[eop] });
            //var newNode = childAnn.Visit(eop.T1Node.AsNode());

            //var children = replacement.ChildNodes();

            //var child = children.ElementAt(k);

            //if (b)
            //{
            //    replacement = replacement.ReplaceNode(replacement.FindNode(child.Span), newNode);
            //}
            //else
            //{
            //    replacement = replacement.InsertNodesBefore(replacement.FindNode(child.Span), new List<SyntaxNode> { newNode });
            //}

            //UpdateTreeRewriter reTree = new UpdateTreeRewriter(oldNode, replacement);
            //CurrentTree = reTree.Visit(CurrentTree.AsNode());

            //var replacementChild = CurrentTree.AsNode().GetAnnotatedNodes(childAnnotation).First();
            //replacement = CurrentTree.AsNode().GetAnnotatedNodes(parentAnnotation).First();

            //var oldList = new List<SyntaxNodeOrToken> { eop.T1Node, eop.Parent };
            //var replacementList = new List<SyntaxNodeOrToken> { replacementChild, replacement };

            //for (int i = 0; i < replacementList.Count; i++)
            //{
            //    var replacementNode = replacementList[i];
            //    var oldNodeEop = oldList[i];
            //    foreach (var editOperation in _script)
            //    {
            //        if (editOperation.T1Node != null && editOperation.T1Node.Equals(oldNodeEop))
            //        {
            //            editOperation.T1Node = replacementNode;
            //        }

            //        if (editOperation.Parent != null && editOperation.Parent.Equals(oldNodeEop))
            //        {
            //            editOperation.Parent = replacementNode;
            //        }
            //    }
            //}

            //return replacement;
        }

        private SyntaxNode OldAnchor(EditOperation eop)
        {
            var annotation = Ann[eop];
            //TODO refactor annotation
            var moveL = CurrentTree.AsNode().GetAnnotatedNodes(annotation.Kind).ToList();
            var oldNode = moveL.First();

            foreach (var annoted in moveL)
            {
                if (annoted.Span.Start == eop.Parent.Span.Start && annoted.Span.Length == eop.Parent.Span.Length)
                {
                    oldNode = annoted;
                }
            }
            return oldNode;
        }

        private bool ScriptContains(SyntaxNodeOrToken elementAt)
        {

            foreach (var editOperation in _script)
            {
                if (editOperation is Delete)
                {
                    foreach (var node in editOperation.T1Node.AsNode().DescendantNodesAndSelf())
                    {
                        if (node.Span.Contains(elementAt.Span) &&
                    elementAt.Span.Contains(node.Span))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Process update operation
        /// </summary>
        /// <param name="eop">Update operation</param>
        /// <returns>Updated version of the current node</returns>
        private SyntaxNode ProcessUpdateOperation(EditOperation eop)
        {
            //TODO correct the update anchor
            var oldNode = OldAnchor(eop);

            var replacement = oldNode;
            //TODO refactor this code: rush

            var parentAnnotation = new SyntaxAnnotation(Ann[eop].Kind + "Parent");
            AddAnnotationRewriter addAnn = new AddAnnotationRewriter(oldNode, new List<SyntaxAnnotation> { parentAnnotation, Ann[eop] });
            replacement = addAnn.Visit(replacement);

            var toNode = ((Script.Update)eop).To.AsNode();
            var childAnnotation = new SyntaxAnnotation(Ann[eop] + "Child");
            AddAnnotationRewriter childAnn = new AddAnnotationRewriter(toNode, new List<SyntaxAnnotation> { childAnnotation, Ann[eop] });
            var newNode = childAnn.Visit(toNode);

            var children = replacement.ChildNodes();

            var child = children.First(); // refactor this. Create a None node.
            foreach (var childItem in children)
            {
                if (IsomorphicManager.IsIsomorphic(childItem, eop.T1Node))
                {
                    child = childItem;
                }
            }

            replacement = replacement.ReplaceNode(replacement.FindNode(child.Span), newNode);

            UpdateTreeRewriter reTree = new UpdateTreeRewriter(oldNode, replacement);
            CurrentTree = reTree.Visit(CurrentTree.AsNode());

            var replacementChild = CurrentTree.AsNode().GetAnnotatedNodes(childAnnotation).First();
            replacement = CurrentTree.AsNode().GetAnnotatedNodes(parentAnnotation).First();

            var oldList = new List<SyntaxNodeOrToken> { eop.T1Node, eop.Parent };
            var replacementList = new List<SyntaxNodeOrToken> { replacementChild, replacement };

            for (int i = 0; i < replacementList.Count; i++)
            {
                var replacementNode = replacementList[i];
                var oldNodeEop = oldList[i];
                foreach (var editOperation in _script)
                {
                    if (editOperation.T1Node != null && editOperation.T1Node.Equals(oldNodeEop))
                    {
                        editOperation.T1Node = replacementNode;
                    }

                    if (editOperation.Parent != null && editOperation.Parent.Equals(oldNodeEop))
                    {
                        editOperation.Parent = replacementNode;
                    }
                }
            }

            return replacement;
        }
    }
}