using Microsoft.CodeAnalysis;

namespace RefazerObject.Transformation
{
    public class TransformationInfo
    {
        public SyntaxNodeOrToken Before { get; set; }

        public SyntaxNodeOrToken After { get; set; }

        public TStatus Status { get; set; }

        public TransformationInfo(SyntaxNodeOrToken before, SyntaxNodeOrToken after, TStatus status = TStatus.Ok)
        {
            Before = before;
            After = after;
            Status = status;
        }
    }

    // ReSharper disable once InconsistentNaming
    public enum TStatus
    {
        Ok,
        Error
    };
}
