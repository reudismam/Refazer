using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spg.ExampleRefactoring.Expression;
using Spg.ExampleRefactoring.Position;

namespace Spg.ExampleRefactoring.Intersect
{
    internal class SubStrIntersectStrategy: SubStrIntersectStrategyBase
    {
        public override ExpressionKind GetExpressionKind()
        {
            return ExpressionKind.SubStr;
        }

        public override IExpression GetExpression(IPosition p1, IPosition p2)
        {
            return new SubStr(p1, p2);
        }
    }
}
