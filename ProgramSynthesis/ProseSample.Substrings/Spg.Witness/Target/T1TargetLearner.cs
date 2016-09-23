﻿using Microsoft.CodeAnalysis;
using TreeEdit.Spg.Script;
using ProseSample.Substrings;
using TreeElement.Spg.Node;

namespace ProseSample.Substrings.Spg.Witness.Target
{
    public class T1TargetLearner: LearnTargetTemplate
    {
        protected override TreeNode<SyntaxNodeOrToken> Target(Edit<SyntaxNodeOrToken> edit)
        {
            return edit.EditOperation.T1Node;
        }

        protected override bool ProcessEditOperation(Edit<SyntaxNodeOrToken> edit) => edit.EditOperation is Delete<SyntaxNodeOrToken> || edit.EditOperation is Update<SyntaxNodeOrToken>? true : false;

    }
}
