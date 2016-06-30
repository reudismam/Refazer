using Microsoft.CodeAnalysis;
using TreeEdit.Spg.Script;
using TreeElement.Spg.Node;

namespace ProseSample.Substrings.Spg.Witness.Target
{
    public class T1TargetLearner: LearnTargetTemplate
    {
        protected override ITreeNode<SyntaxNodeOrToken> Target(Edit<SyntaxNodeOrToken> edit)
        {
            return edit.EditOperation.T1Node;
        }
    }
}
