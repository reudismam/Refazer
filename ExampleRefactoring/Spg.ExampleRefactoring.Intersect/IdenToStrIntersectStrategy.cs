using Spg.ExampleRefactoring.Expression;
using Spg.ExampleRefactoring.Position;

namespace Spg.ExampleRefactoring.Intersect
{
    internal class IdenToStrIntersectStrategy: SubStrIntersectStrategyBase
    {
        /// <summary>
        /// Return ExpressionKind.IdentoStr
        /// </summary>
        /// <returns>Return ExpressionKind.IdentoStr</returns>
        public override ExpressionKind GetExpressionKind()
        {
            return ExpressionKind.Identostr;
        }

        /// <summary>
        /// Create a new IdenToStr
        /// </summary>
        /// <param name="p1">First position</param>
        /// <param name="p2">Second position</param>
        /// <returns>A new IdenToStr</returns>
        public override IExpression GetExpression(IPosition p1, IPosition p2)
        {
            return new IdenToStr(p1, p2);
        }
    }
}





