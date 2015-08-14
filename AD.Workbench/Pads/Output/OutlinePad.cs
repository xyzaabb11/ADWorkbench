using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using AD.Workbench.Serivces;
using AD.Workbench.Workbench;

namespace AD.Workbench.Pads.Output
{
    /// <summary>
    /// Implement this interface to make a view content display tools in the outline pad.
    /// </summary>
//     [ViewContentService]
/*
    public interface IOutlineContentHost
    {
        /// <summary>
        /// Gets the control to display in the outline pad.
        /// </summary>
        object OutlineContent { get; }
    }*/

    /// <summary>
    /// A pad that shows a single child control determined by the document that currently has the focus.
    /// </summary>

    public class OutlinePad : AbstractPadContent, IOutputPad
    {
//         ContentPresenter contentControl = new ContentPresenter();

        private TextBox panelBox;

        private static OutlinePad instance;
        private StringBuilder textBuilder;
        public static OutlinePad Instance
        {
            get
            {
                if (instance == null)
                    ADService.MainThread.InvokeIfRequired(InitializeInstance);
                return instance;
            }
        }

        static void InitializeInstance()
        {
            ADService.Workbench.GetPad(typeof(OutlinePad)).CreatePad();
        }

        public override object Control
        {
            get
            {
                return panelBox;
            }
        }


        public OutlinePad()
        {
            instance = this;
            textBuilder = new StringBuilder();
            panelBox = new TextBox();
            panelBox.IsReadOnly = true;
            panelBox.TextWrapping = TextWrapping.Wrap;
            panelBox.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
        }

        public void AppendText(string text)
        {
            const int maxTextSize = 50 * 1000 * 1000; // 50m chars = 100 MB
            const string truncatedText = "<Text was truncated because it was too long>\r\n";

            lock (textBuilder)
            {
                if (textBuilder.Length + text.Length > maxTextSize)
                {
                    int amountToCopy = maxTextSize / 2 - text.Length;
                    if (amountToCopy <= 0)
                    {
                        SetText(truncatedText + text.Substring(text.Length - maxTextSize / 2, maxTextSize / 2));
                    }
                    else
                    {
                        SetText(truncatedText + textBuilder.ToString(textBuilder.Length - amountToCopy, amountToCopy) + text);
                    }
                }
                else
                {
                    textBuilder.Append(text);
                    panelBox.Text = textBuilder.ToString();
                    panelBox.ScrollToEnd();
                }
            }
            
        }

        public void AppendLine(string text)
        {
            AppendText(text + Environment.NewLine);
        }

        public void SetText(string text)
        {
            lock (textBuilder)
            {
                // clear text:
                textBuilder.Length = 0;
                // reset capacity: we must shrink the textBuilder at some point to reclaim memory
                textBuilder.Capacity = text.Length + 16;
                textBuilder.Append(text);
                panelBox.Text = textBuilder.ToString();
                panelBox.ScrollToEnd();
            }
        }

        public void ClearText()
        {
            SetText(string.Empty);
        }
    }
}
