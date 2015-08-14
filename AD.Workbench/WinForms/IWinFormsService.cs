﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ICSharpCode.Core;

namespace AD.Workbench.WinForms
{
    [SDService("SD.WinForms")]
    public interface IWinFormsService
    {
        /// <summary>
        /// Gets the Windows Forms ToolBar service.
        /// </summary>
        IWinFormsToolbarService ToolbarService { get; }

        /// <summary>
        /// Gets the Windows Forms menu service.
        /// </summary>
        IWinFormsMenuService MenuService { get; }

        /// <summary>
        /// Gets the default monospaced font (Consolas or Courier New).
        /// </summary>
        Font DefaultMonospacedFont { get; }

        /// <summary>
        /// Loads the default monospaced font (Consolas or Courier New).
        /// </summary>
        Font LoadDefaultMonospacedFont(FontStyle style);

        /// <summary>
        /// The LoadFont routines provide a safe way to load fonts.
        /// </summary>
        /// <param name="baseFont">The existing font from which to create the new font.</param>
        /// <param name="newStyle">The new style of the font.</param>
        /// <returns>
        /// The font to load or the baseFont (if the requested font couldn't be loaded).
        /// </returns>
        Font LoadFont(Font baseFont, FontStyle newStyle);

        /// <summary>
        /// The LoadFont routines provide a safe way to load fonts.
        /// </summary>
        /// <param name="fontName">The name of the font to load.</param>
        /// <param name="size">The size of the font to load.</param>
        /// <param name="style">The <see cref="System.Drawing.FontStyle"/> of the font</param>
        /// <returns>
        /// The font to load or the menu font, if the requested font couldn't be loaded.
        /// </returns>
        Font LoadFont(string fontName, int size, FontStyle style = FontStyle.Regular);

        /// <summary>
        /// The main window as IWin32Window.
        /// </summary>
        IWin32Window MainWin32Window { get; }

        /// <summary>
        /// Shows the print dialog and prints the specified printable.
        /// </summary>
        void Print(IPrintable printable);

        /// <summary>
        /// Opens the print preview dialog.
        /// </summary>
        void PrintPreview(IPrintable printable);

        /// <summary>
        /// Infrastructure method; please use the <c>IResourceService.GetBitmap()</c> extension method instead.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        Bitmap GetResourceServiceBitmap(string resourceName);

        /// <summary>
        /// Infrastructure method; please use the <c>IResourceService.GetIcon()</c> extension method instead.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        Icon GetResourceServiceIcon(string resourceName);

        /// <summary>
        /// Creates a new icon from the given bitmap.
        /// </summary>
        Icon BitmapToIcon(Bitmap bitmap);

        /// <summary>
        /// If the UI is in right-to-left mode, this method will mirror the specified control.
        /// In left-to-right mode, nothing happens.
        /// </summary>
        void ApplyRightToLeftConverter(Control control, bool recurse = true);

        /// <summary>
        /// Sets the Content property of the specified ControlControl to the specified content.
        /// If the content is a Windows-Forms control, it is wrapped in a WindowsFormsHost.
        /// If the content control already contains a WindowsFormsHost with that content,
        /// the old WindowsFormsHost is kept.
        /// When a WindowsFormsHost is replaced with another content, the host is disposed (but the control
        /// inside the host isn't)
        /// </summary>
        void SetContent(System.Windows.Controls.ContentControl contentControl, object content, IServiceProvider serviceProvider = null);

        /// <summary>
        /// Sets the Content property of the specified ContentPresenter to the specified content.
        /// If the content is a Windows-Forms control, it is wrapped in a WindowsFormsHost.
        /// If the content control already contains a WindowsFormsHost with that content,
        /// the old WindowsFormsHost is kept.
        /// When a WindowsFormsHost is replaced with another content, the host is disposed (but the control
        /// inside the host isn't)
        /// </summary>
        void SetContent(System.Windows.Controls.ContentPresenter contentControl, object content, IServiceProvider serviceProvider = null);

        /// <summary>
        /// Creates a new SDWindowsFormsHost instance.
        /// </summary>
        /// <param name="serviceProvider">The service provider that provides the IClipboardHandler, IUndoHandler etc. implementations.</param>
        /// <param name="processShortcutsInWPF">
        /// Determines whether the shortcuts for the default actions (Cut,Copy,Paste,Undo, etc.)
        /// are processed by the WPF command system.
        /// The default value is false. Pass true only if WinForms does not handle those shortcuts by itself.
        /// See SD-1671 and SD-1737.
        /// </param>
        /// <returns>SDWindowsFormsHost instance</returns>
        CustomWindowsFormsHost CreateWindowsFormsHost(IServiceProvider serviceProvider = null, bool processShortcutsInWPF = false);
    }
}
