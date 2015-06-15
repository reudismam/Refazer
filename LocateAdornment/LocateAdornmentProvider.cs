using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Spg.LocationRefactor.TextRegion;

namespace LocateAdornment
{
    internal class LocateAdornmentProvider
    {
        private ITextBuffer buffer;
        private IList<LocateAdornment> comments = new List<LocateAdornment>();
        public event EventHandler<LocateChangedEventArgs> LocationsChanged;

        private LocateAdornmentProvider(ITextBuffer buffer)
        {
            this.buffer = buffer;
            //listen to the Changed event so we can react to deletions. 
            this.buffer.Changed += OnBufferChanged;
        }
        public static LocateAdornmentProvider Create(IWpfTextView view)
        {
            return view.Properties.GetOrCreateSingletonProperty<LocateAdornmentProvider>(delegate { return new LocateAdornmentProvider(view.TextBuffer); });
        }

        public void Detach()
        {
            if (this.buffer != null)
            {
                //remove the Changed listener 
                this.buffer.Changed -= OnBufferChanged;
                this.buffer = null;
            }
        }

        private void OnBufferChanged(object sender, TextContentChangedEventArgs e)
        {
            //Make a list of all comments that have a span of at least one character after applying the change. There is no need to raise a changed event for the deleted adornments. The adornments are deleted only if a text change would cause the view to reformat the line and discard the adornments.
            IList<LocateAdornment> keptComments = new List<LocateAdornment>(this.comments.Count);

            foreach (LocateAdornment comment in this.comments)
            {
                Span span = comment.Span.GetSpan(e.After);
                //if a comment does not span at least one character, its text was deleted. 
                if (span.Length != 0)
                {
                    keptComments.Add(comment);
                }
            }

            this.comments = keptComments;
        }

        public void Add(SnapshotSpan span, string author, string text)
        {
            if (span.Length == 0)
                throw new ArgumentOutOfRangeException("span");
            if (author == null)
                throw new ArgumentNullException("author");
            if (text == null)
                throw new ArgumentNullException("text");

            //Create a comment adornment given the span, author and text.
            LocateAdornment comment = new LocateAdornment(span, author, text);

            //Add it to the list of comments. 
            this.comments.Add(comment);

            //Raise the changed event.
            EventHandler<LocateChangedEventArgs> commentsChanged = this.LocationsChanged;
            if (commentsChanged != null)
                commentsChanged(this, new LocateChangedEventArgs(comment, null));
        }

        public void AddRegion(TRegion region, string author, string text)
        {
            ITextSnapshot current = buffer.CurrentSnapshot;
            SnapshotSpan span = new SnapshotSpan(current, region.Start, region.Length);

            try
            {
                //RemoveComments(span);
            }
            catch (ArgumentOutOfRangeException)
            {
                
            }

            if (span.Length == 0)
                throw new ArgumentOutOfRangeException("span");
            if (author == null)
                throw new ArgumentNullException("author");
            if (text == null)
                throw new ArgumentNullException("text");

            //Create a comment adornment given the span, author and text.
            LocateAdornment comment = new LocateAdornment(span, author, text);

            //Add it to the list of comments. 
            this.comments.Add(comment);

            //Raise the changed event.
            EventHandler<LocateChangedEventArgs> commentsChanged = this.LocationsChanged;
            if (commentsChanged != null)
                commentsChanged(this, new LocateChangedEventArgs(comment, null));
        }
        public void RemoveComments(SnapshotSpan span)
        {
            EventHandler<LocateChangedEventArgs> commentsChanged = this.LocationsChanged;

            //Get a list of all the comments that are being kept 
            IList<LocateAdornment> keptComments = new List<LocateAdornment>(this.comments.Count);

            foreach (LocateAdornment comment in this.comments)
            {
                //find out if the given span overlaps with the comment text span. If two spans are adjacent, they do not overlap. To consider adjacent spans, use IntersectsWith. 
                if (comment.Span.GetSpan(span.Snapshot).OverlapsWith(span))
                {
                    //Raise the change event to delete this comment. 
                    if (commentsChanged != null)
                        commentsChanged(this, new LocateChangedEventArgs(null, comment));
                }
                else
                    keptComments.Add(comment);
            }

            this.comments = keptComments;
        }

        public Collection<LocateAdornment> GetComments(SnapshotSpan span)
        {
            IList<LocateAdornment> overlappingComments = new List<LocateAdornment>();
            foreach (LocateAdornment comment in this.comments)
            {
                if (comment.Span.GetSpan(span.Snapshot).OverlapsWith(span))
                    overlappingComments.Add(comment);
            }

            return new Collection<LocateAdornment>(overlappingComments);
        }

    }
}
