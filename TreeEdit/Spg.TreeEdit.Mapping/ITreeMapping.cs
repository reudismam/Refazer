using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeEdit.Spg.TreeEdit.Mapping
{
    public interface ITreeMapping
    {
        Dictionary<SyntaxNodeOrToken, SyntaxNodeOrToken> Mapping(SyntaxNodeOrToken t1, SyntaxNodeOrToken t2);
    }
}
