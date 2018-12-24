using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.ProgramSynthesis.AST;

namespace Clustering
{
    public class TransformationCluster
    {
        public List<Tuple<SyntaxNodeOrToken, SyntaxNodeOrToken>> Examples { get; set; }

        public ProgramNode Program { get; set; }

        public TransformationCluster()
        {
            Examples = new List<Tuple<SyntaxNodeOrToken, SyntaxNodeOrToken>>();
        }
    }
}
