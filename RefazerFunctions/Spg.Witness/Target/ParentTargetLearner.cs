using Microsoft.CodeAnalysis;
using TreeEdit.Spg.Script;
using TreeElement.Spg.Node;

namespace RefazerFunctions.Spg.Witness.Target
{
    public class ParentTargetLearner : LearnTargetTemplate
    {
        protected override TreeNode<SyntaxNodeOrToken> Target(Edit<SyntaxNodeOrToken> edit)
        {
            return edit.EditOperation.Parent;
        }

        protected override bool ProcessEditOperation(Edit<SyntaxNodeOrToken> edit) => true;

    }
}
