using Spg.ExampleRefactoring.Expression;

namespace Spg.ExampleRefactoring.Intersect
{
    internal class FakeConstIntersectStrategy : ConstIntersectStrategyBase
    {
        public override ExpressionKind GetExpressionKind()
        {
            return ExpressionKind.FakeConstrStr;
        }
    }
}
