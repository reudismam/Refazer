using System;
using System.Collections.Generic;
using System.Windows.Media;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;

namespace LocateAdornment
{
    internal class LocateAdornmentManager
    {
        private readonly IWpfTextView view;
        private readonly IAdornmentLayer layer;
        private readonly LocateAdornmentProvider provider;

        private LocateAdornmentManager(IWpfTextView view)
        {
            this.view = view;
            this.view.LayoutChanged += OnLayoutChanged;
            this.view.Closed += OnClosed;

            this.layer = view.GetAdornmentLayer("CommentAdornmentLayer");

            this.provider = LocateAdornmentProvider.Create(view);
            this.provider.LocationsChanged += OnCommentsChanged;
        }

        public static LocateAdornmentManager Create(IWpfTextView view)
        {
            return view.Properties.GetOrCreateSingletonProperty<LocateAdornmentManager>(delegate { return new LocateAdornmentManager(view); });
        }

        private void OnCommentsChanged(object sender, LocateChangedEventArgs e)
        {
            //Remove the comment (when the adornment was added, the comment adornment was used as the tag). 
            if (e.LocationRemoved != null)
                this.layer.RemoveAdornmentsByTag(e.LocationRemoved);

            //Draw the newly added comment (this will appear immediately: the view does not need to do a layout). 
            if (e.LocationAdded != null)
                this.DrawComment(e.LocationAdded);
        }

        private void OnClosed(object sender, EventArgs e)
        {
            this.provider.Detach();
            this.view.LayoutChanged -= OnLayoutChanged;
            this.view.Closed -= OnClosed;
        }

        private void OnLayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
        {
            //Get all of the comments that intersect any of the new or reformatted lines of text.
            List<LocateAdornment> newComments = new List<LocateAdornment>();

            //The event args contain a list of modified lines and a NormalizedSpanCollection of the spans of the modified lines.  
            //Use the latter to find the comments that intersect the new or reformatted lines of text. 
            foreach (Span span in e.NewOrReformattedSpans)
            {
                newComments.AddRange(this.provider.GetComments(new SnapshotSpan(this.view.TextSnapshot, span)));
            }

            //It is possible to get duplicates in this list if a comment spanned 3 lines, and the first and last lines were modified but the middle line was not. 
            //Sort the list and skip duplicates.
            newComments.Sort(delegate (LocateAdornment a, LocateAdornment b) { return a.GetHashCode().CompareTo(b.GetHashCode()); });

            LocateAdornment lastComment = null;
            foreach (LocateAdornment comment in newComments)
            {
                if (comment != lastComment)
                {
                    lastComment = comment;
                    this.DrawComment(comment);
                }
            }
        }

        private void DrawComment(LocateAdornment comment)
        {
            SnapshotSpan span = comment.Span.GetSpan(this.view.TextSnapshot);
            Geometry g = this.view.TextViewLines.GetMarkerGeometry(span);

            if (g != null)
            {
                //Find the rightmost coordinate of all the lines that intersect the adornment. 
                double maxRight = 0.0;
                foreach (ITextViewLine line in this.view.TextViewLines.GetTextViewLinesIntersectingSpan(span))
                    maxRight = Math.Max(maxRight, line.Right);

                //Create the visualization.
                LocateBlock block = new LocateBlock(maxRight, this.view.ViewportRight, g, comment.Author, comment.Text);

                //Add it to the layer. 
                this.layer.AddAdornment(span, comment, block);
            }
        }


    }
}
