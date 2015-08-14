using System;
using System.Collections.Generic;
using System.Windows;

namespace AD.Workbench.Workbench
{
    public interface IWorkbench
    {
        Window MainWindow { get; }

        ICollection<IViewContent> ViewContentCollection
        {
            get;
        }

        /// <summary>
        /// A collection in which all active workspace windows are saved.
        /// </summary>
        IList<PadDescriptor> PadContentCollection
        {
            get;
        }

        IList<IWorkbenchWindow> WorkbenchWindowCollection
        {
            get;
        }
        /// <summary>
        /// The active view content inside the active workbench window.
        /// </summary>
        IViewContent ActiveViewContent
        {
            get;
        }
        event EventHandler ActiveViewContentChanged;
        void ShowView(IViewContent content);

        void ShowView(IViewContent content, bool switchToOpenedView);

        /// <summary>
        /// Activates the specified pad.
        /// </summary>
        void ActivatePad(PadDescriptor content);

        /// <summary>
        /// Returns a pad from a specific type.
        /// </summary>
        PadDescriptor GetPad(Type type);
    }
}
