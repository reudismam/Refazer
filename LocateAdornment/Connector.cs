using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO.Packaging;
using System.Runtime.InteropServices;
using System.Windows.Media;
using LocationCodeRefactoring.Spg.LocationCodeRefactoring.Controller;
using LocationCodeRefactoring.Spg.LocationRefactor.Transformation;
using Microsoft.Build.BuildEngine;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;
using Spg.LocationRefactor.TextRegion;


namespace LocateAdornment
{
    [Export(typeof(IWpfTextViewCreationListener))]
    [ContentType("text")]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    public sealed class Connector : IWpfTextViewCreationListener
    {
        [Export(typeof(AdornmentLayerDefinition))]
        [Name("CommentAdornmentLayer")]
        [Order(After = PredefinedAdornmentLayers.Selection, Before = PredefinedAdornmentLayers.Text)]
        public AdornmentLayerDefinition commentLayerDefinition;

        public void TextViewCreated(IWpfTextView textView)
        {
            LocateAdornmentManager.Create(textView);
        }

        static public void Execute(IWpfTextViewHost host)
        {
            IWpfTextView view = host.TextView;
            //Add a comment on the selected text. 
            if (!view.Selection.IsEmpty)
            {
                //Get the provider for the comment adornments in the property bag of the view.
                LocateAdornmentProvider provider = view.Properties.GetProperty<LocateAdornmentProvider>(typeof(LocateAdornmentProvider));

                //Add some arbitrary author and comment text. 
                string author = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                string comment = "Four score....";

                var snaphot = view.Selection.SelectedSpans[0];
                ITextSnapshot document = snaphot.Snapshot;
                string source = document.GetText();
                string span = source.Substring(snaphot.Start, snaphot.Length);

                EditorController controller = EditorController.GetInstance();
                controller.Edit(snaphot.Start, snaphot.Length, span, source);

                //Add the comment adornment using the provider.
                provider.Add(view.Selection.SelectedSpans[0], author, comment);
            }
        }

        static public void Select(IWpfTextViewHost host, TRegion region)
        {
            IWpfTextView view = host.TextView;
            LocateAdornmentProvider provider = view.Properties.GetProperty<LocateAdornmentProvider>(typeof(LocateAdornmentProvider));

            //Add some arbitrary author and comment text. 
            string author = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            string comment = "Four score....";
       
            provider.AddRegion(region, author, comment);
        }

        static public string GetText(IWpfTextViewHost host)
        {
                IWpfTextView view = host.TextView;

                ITextSnapshot document = view.TextSnapshot;
                return document.GetText();
        }

        static public void Update(IWpfTextViewHost host, List<Transformation> transformations)
        {
            IWpfTextView view = host.TextView;
            LocateAdornmentProvider provider = view.Properties.GetProperty<LocateAdornmentProvider>(typeof(LocateAdornmentProvider));
            ITextSnapshot current = view.TextBuffer.CurrentSnapshot;

            EditorController controller = EditorController.GetInstance();
            string newText = "";
            foreach (Transformation transformation in transformations)
            {
                if (controller.CurrentViewCodeBefore.Equals(transformation.transformation.Item1))
                {
                    newText = transformation.transformation.Item2;
                    break;
                }
            }

            Span span = new Span(0, view.TextSnapshot.GetText().Length);
            view.TextBuffer.Replace(span, newText);

            Transform(transformations);
            // view.TextBuffer.Delete(span);
            //view.TextBuffer.Insert(0, newText);
        }

        private static void Transform(List<Transformation> transformations)
        {
            foreach (var transformation in transformations)
            {
                System.IO.StreamWriter file = new System.IO.StreamWriter(transformation.SourcePath);
                file.WriteLine(transformation.transformation.Item2);

                file.Close();
            }

        }
    }
}
