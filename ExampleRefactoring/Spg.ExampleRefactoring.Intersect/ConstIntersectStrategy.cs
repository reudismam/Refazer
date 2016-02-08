using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spg.ExampleRefactoring.Expression;
using Spg.ExampleRefactoring.Position;

namespace Spg.ExampleRefactoring.Intersect
{
    internal class ConstIntersectStrategy : ConstIntersectStrategyBase
    {
        public override ExpressionKind GetExpressionKind()
        {
            return ExpressionKind.Consttrustr;
        }
    }
}
