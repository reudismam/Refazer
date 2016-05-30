using System.Collections.Generic;
using DbscanImplementation;
using Microsoft.CodeAnalysis;
using TreeEdit.Spg.Script;

namespace TreeEdit.Spg.Clustering
{
    public class EditOperationDatasetItem: DatasetItemBase
    {
        public List<EditOperation<SyntaxNodeOrToken>> Operations;
        public EditOperationDatasetItem(List<EditOperation<SyntaxNodeOrToken>> operations)
        {
            Operations = operations;
        }
    }
}