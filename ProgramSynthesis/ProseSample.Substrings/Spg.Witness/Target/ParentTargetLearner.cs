using Microsoft.CodeAnalysis;
using TreeEdit.Spg.Script;
using ProseSample.Substrings;

namespace ProseSample.Substrings.Spg.Witness.Target
{
    public class ParentTargetLearner : LearnTargetTemplate
    {
        protected override ITreeNode<SyntaxNodeOrToken> Target(Edit<SyntaxNodeOrToken> edit)
        {
            return edit.EditOperation.Parent;
        }

        protected override bool ProcessEditOperation(Edit<SyntaxNodeOrToken> edit) => true;

    }
}
