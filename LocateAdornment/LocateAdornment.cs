using Microsoft.VisualStudio.Text;

namespace LocateAdornment
{

    internal class LocateAdornment
    {
        public readonly ITrackingSpan Span;
        public readonly string Author;
        public readonly string Text;
        public LocateAdornment(SnapshotSpan span, string author, string text)
        {
            this.Span = span.Snapshot.CreateTrackingSpan(span, SpanTrackingMode.EdgeExclusive);
            this.Author = author;
            this.Text = text;
        }
    }


}
