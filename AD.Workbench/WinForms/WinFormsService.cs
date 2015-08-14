﻿using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using ICSharpCode.Core;
using ICSharpCode.Core.WinForms;
using AD.Workbench.Serivces;
using AD.Workbench.Workbench;

namespace AD.Workbench.WinForms
{
    sealed class WinFormsService : IWinFormsService
    {
        public void Print(IPrintable printable)
        {
            using (PrintDocument pdoc = printable.PrintDocument)
            {
                if (pdoc != null)
                {
                    using (PrintDialog ppd = new PrintDialog())
                    {
                        ppd.Document = pdoc;
                        ppd.AllowSomePages = true;
                        if (ppd.ShowDialog(ADService.WinForms.MainWin32Window) == DialogResult.OK)
                        { // fixed by Roger Rubin
                            pdoc.Print();
                        }
                    }
                }
                else
                {
                    MessageService.ShowError("${res:ICSharpCode.SharpDevelop.Commands.Print.CreatePrintDocumentError}");
                }
            }
        }

        public void PrintPreview(IPrintable printable)
        {
            using (PrintDocument pdoc = printable.PrintDocument)
            {
                if (pdoc != null)
                {
                    PrintPreviewDialog ppd = new PrintPreviewDialog();
                    ppd.TopMost = true;
                    ppd.Document = pdoc;
                    ppd.Show(ADService.WinForms.MainWin32Window);
                }
                else
                {
                    MessageService.ShowError("${res:ICSharpCode.SharpDevelop.Commands.Print.CreatePrintDocumentError}");
                }
            }
        }

        public IWinFormsToolbarService ToolbarService
        {
            get
            {
                return ADService.GetRequiredService<IWinFormsToolbarService>();
            }
        }

        public IWinFormsMenuService MenuService
        {
            get
            {
                return ADService.GetRequiredService<IWinFormsMenuService>();
            }
        }

        public Font DefaultMonospacedFont
        {
            get
            {
                return WinFormsResourceService.DefaultMonospacedFont;
            }
        }

        public IWin32Window MainWin32Window
        {
            get
            {
                return (ADWorkbench)ADService.Workbench;
            }
        }

        public Font LoadDefaultMonospacedFont(FontStyle style)
        {
            return WinFormsResourceService.LoadDefaultMonospacedFont(style);
        }

        public Font LoadFont(Font baseFont, FontStyle newStyle)
        {
            return WinFormsResourceService.LoadFont(baseFont, newStyle);
        }

        public Font LoadFont(string fontName, int size, FontStyle style)
        {
            return WinFormsResourceService.LoadFont(fontName, size, style);
        }

        public Bitmap GetResourceServiceBitmap(string resourceName)
        {
            return WinFormsResourceService.GetBitmap(resourceName);
        }

        public Icon GetResourceServiceIcon(string resourceName)
        {
            return WinFormsResourceService.GetIcon(resourceName);
        }

        public Icon BitmapToIcon(Bitmap bitmap)
        {
            return WinFormsResourceService.BitmapToIcon(bitmap);
        }

        public void ApplyRightToLeftConverter(Control control, bool recurse)
        {
            if (recurse)
                RightToLeftConverter.ConvertRecursive(control);
            else
                RightToLeftConverter.Convert(control);
        }

        public void SetContent(System.Windows.Controls.ContentControl contentControl, object content, IServiceProvider serviceProvider)
        {
            if (contentControl == null)
                throw new ArgumentNullException("contentControl");
            // serviceObject = object implementing the old clipboard/undo interfaces
            // to allow WinForms AddIns to handle WPF commands

            var host = contentControl.Content as ADWindowsFormsHost;
            if (host != null)
            {
                if (host.Child == content)
                {
                    host.ServiceProvider = serviceProvider;
                    return;
                }
                host.Dispose();
            }
            if (content is System.Windows.Forms.Control)
            {
                contentControl.Content = new ADWindowsFormsHost
                {
                    Child = (System.Windows.Forms.Control)content,
                    ServiceProvider = serviceProvider,
                    DisposeChild = false
                };
            }
            else if (content is string)
            {
                contentControl.Content = new System.Windows.Controls.TextBlock
                {
                    Text = content.ToString(),
                    TextWrapping = System.Windows.TextWrapping.Wrap
                };
            }
            else
            {
                contentControl.Content = content;
            }
        }

        public void SetContent(System.Windows.Controls.ContentPresenter contentControl, object content, IServiceProvider serviceProvider)
        {
            if (contentControl == null)
                throw new ArgumentNullException("contentControl");
            // serviceObject = object implementing the old clipboard/undo interfaces
            // to allow WinForms AddIns to handle WPF commands

            var host = contentControl.Content as ADWindowsFormsHost;
            if (host != null)
            {
                if (host.Child == content)
                {
                    host.ServiceProvider = serviceProvider;
                    return;
                }
                host.Dispose();
            }
            if (content is System.Windows.Forms.Control)
            {
                contentControl.Content = new ADWindowsFormsHost
                {
                    Child = (System.Windows.Forms.Control)content,
                    ServiceProvider = serviceProvider,
                    DisposeChild = false
                };
            }
            else if (content is string)
            {
                contentControl.Content = new System.Windows.Controls.TextBlock
                {
                    Text = content.ToString(),
                    TextWrapping = System.Windows.TextWrapping.Wrap
                };
            }
            else
            {
                contentControl.Content = content;
            }
        }

        public CustomWindowsFormsHost CreateWindowsFormsHost(IServiceProvider serviceProvider = null, bool processShortcutsInWPF = false)
        {
            return new ADWindowsFormsHost(processShortcutsInWPF)
            {
                ServiceProvider = serviceProvider,
                DisposeChild = false
            };
        }
    }
}
