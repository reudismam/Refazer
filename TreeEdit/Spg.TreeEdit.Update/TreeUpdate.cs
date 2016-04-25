using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
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
            PreProcessTree(script, tree, M);

            foreach (var item in script)
            {
                if (!Processed.ContainsKey(item))
                {
                    ProcessScript(item);
                }
            }
        }

        public void PreProcessTree(List<EditOperation> script, SyntaxNodeOrToken tree, Dictionary<SyntaxNodeOrToken, SyntaxNodeOrToken> M)
        {
            _script = script;
            InitializeAtributes(tree, M);
            CreateEditionDictionary(script);
            Annotate();
        }

        /// <summary>
        /// Annotate the tree
        /// </summary>
        private void Annotate()
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
                Annotate(s, id);                //Anotate common cases
                AnnotateDeleteOperation(s);     //Anotate delete

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
        private void Annotate(EditOperation eop, int id)
        {
            if (eop is Delete) return;

            var parent = eop.Parent.AsNode();

            if (!_annts.ContainsKey(parent))
            {
                _annts[parent] = new List<SyntaxAnnotation>();
                SyntaxAnnotation sn = new SyntaxAnnotation("ANC" + id);
                _annts[parent].Add(sn);
            }
            Ann.Add(eop, _annts[parent].First());

        }

        private SyntaxNode GetT1Parent(EditOperation eop)
        {
            if (eop is Insert)
            {
                var y = eop.T1Node.Parent;
                var z = _M.ToList().Find(o => o.Value.Equals(y)).Key;
                return z.AsNode();
            }

            return eop.Parent.AsNode();

        }

        public void ProcessScript(EditOperation operation)
        {
            foreach (var editOperation in _script)
            {
                ProcessEditOperation(editOperation);
            }
        }


        /// <summary>
        /// Process edit operation
        /// </summary>
        /// <param name="eop">Edit operation</param>
        /// <returns>Node after running edit operation</returns>
        private void ProcessEditOperation(EditOperation eop)
        {
            if (eop is Delete)
            {
                //TODO decide what to do whant operation is a delete operation
            }
            else
            {
                ProcessOperation(eop);
            }

        }

        /// <summary>
        /// Process insert operation
        /// </summary>
        /// <param name="editOperation">Edit operation</param>
        /// <returns>Updated version of current node</returns>
        private void ProcessOperation(EditOperation editOperation)
        {
            //Get the parent of the child that will be replaced
            SyntaxNode anchorParentNode;
            var anchorNodeKChild = ToBeReplacedNode(editOperation, out anchorParentNode);
            SyntaxNode replacement = anchorParentNode;

            //Get the child that will be replaced
            var kChildNode = GetKChildNode(editOperation);

            //Annotate the node that will repalce to we have access to this latter
            SyntaxAnnotation parentAnnotation;
            replacement = AnnotateParentNode(out parentAnnotation, editOperation, anchorParentNode, replacement);

            //Annotate the child that will replace to we have access to this latter
            SyntaxAnnotation childAnnotation;
            var newNode = AnnotateNewKChildNode(out childAnnotation, editOperation, kChildNode);

            //Get the replace child
            var child = GetTargetChild(editOperation, replacement);

            //Get the replace parent
            replacement = PerformEditOperationAnchorNode(anchorNodeKChild, replacement, child, newNode, editOperation);

            //Update the current tree
            UpdateCurrentTree(anchorParentNode, replacement);

            //Update script nodes
            UpdateScript(editOperation, childAnnotation, parentAnnotation);
        }

        private static SyntaxNode GetKChildNode(EditOperation editOperation)
        {
            if (editOperation is Script.Update)
            {
                return ((Script.Update)editOperation).To.AsNode();
            }

            var kChildNode = editOperation.T1Node.AsNode();
            return kChildNode;
        }

        private static SyntaxNode GetTargetChild(EditOperation editOperation, SyntaxNode replacement)
        {
            if (editOperation is Script.Update)
            {
                var children = replacement.ChildNodes();

                var child = children.First(); // refactor this. Create a None node.
                foreach (var childItem in children)
                {
                    if (IsomorphicManager.IsIsomorphic(childItem, editOperation.T1Node))
                    {
                        child = childItem;
                    }
                }
                return child;
            }
            else
            {
                var children = replacement.ChildNodes();
                var child = children.ElementAt(editOperation.K - 1);
                return child;
            }
        }

        private SyntaxNode AnnotateParentNode(out SyntaxAnnotation parentAnnotation, EditOperation eop, SyntaxNode anchorParentNode, SyntaxNode replacement)
        {
            parentAnnotation = new SyntaxAnnotation(Ann[eop].Kind + "Parent");

            AddAnnotationRewriter addAnn = new AddAnnotationRewriter(anchorParentNode, new List<SyntaxAnnotation> { parentAnnotation, Ann[eop] });

            replacement = addAnn.Visit(replacement);
            return replacement;
        }

        private SyntaxNodeOrToken ToBeReplacedNode(EditOperation eop, out SyntaxNode oldNode)
        {
            oldNode = OldAnchor(eop);
            SyntaxNodeOrToken oldChild;
            if (eop is Insert)
            {
                oldChild = eop.Parent.AsNode().ChildNodes().ElementAt(eop.K - 1);
                return oldChild;
            }
            oldChild = eop.T1Node; // eop is a Move
            return oldChild;
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

        private void UpdateScript(EditOperation eop, SyntaxAnnotation childAnnotation, SyntaxAnnotation parentAnnotation)
        {
            SyntaxNode replacement;
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
        }

        private void UpdateCurrentTree(SyntaxNode anchorParentNode, SyntaxNode replacement)
        {
            UpdateTreeRewriter reTree = new UpdateTreeRewriter(anchorParentNode, replacement);
            CurrentTree = reTree.Visit(CurrentTree.AsNode());
        }

        private SyntaxNode PerformEditOperationAnchorNode(SyntaxNodeOrToken anchorNodeKChild, SyntaxNode replacement, SyntaxNode child, SyntaxNode newNode, EditOperation eop)
        {
            if (eop is Script.Update)
            {
                replacement = replacement.ReplaceNode(replacement.FindNode(child.Span), newNode);
                return replacement;
            }

            bool b = ScriptContains(anchorNodeKChild) || IsomorphicManager.IsIsomorphic(anchorNodeKChild, eop.T1Node); ;
            if (b)
            {
                replacement = replacement.ReplaceNode(replacement.FindNode(child.Span), newNode);
            }
            else
            {
                replacement = replacement.InsertNodesBefore(replacement.FindNode(child.Span), new List<SyntaxNode> { newNode });
            }
            return replacement;
        }

        private SyntaxNode AnnotateNewKChildNode(out SyntaxAnnotation childAnnotation, EditOperation eop, SyntaxNode kchildNode)
        {
            childAnnotation = new SyntaxAnnotation(Ann[eop].Kind + "Child");
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
            return newNode;
        }

        private SyntaxNode OldAnchor(EditOperation eop)
        {
            //TODO correct the update anchor
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
    }
}