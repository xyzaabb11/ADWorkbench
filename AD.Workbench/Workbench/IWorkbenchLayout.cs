using System;
using System.Collections.Generic;

namespace AD.Workbench.Workbench
{
    public interface IWorkbenchLayout
    {

        /// <summary>
        /// The active workbench window.
        /// </summary>
        IWorkbenchWindow ActiveWorkbenchWindow
        {
            get;
        }
        void Attach(IWorkbench workbench);
        void Detach();

        IList<IWorkbenchWindow> WorkbenchWindows
        {
            get;
        }

        /// <summary>
        /// The active content. This can be either a IViewContent or a IPadContent, depending on
        /// where the focus currently is.
        /// </summary>
        IServiceProvider ActiveContent
        {
            get;
        }

        event EventHandler ActiveWorkbenchWindowChanged;
        event EventHandler ActiveContentChanged;
        /// <summary>
        /// Shows a new <see cref="IPadContent"/>.
        /// </summary>
        void ShowPad(PadDescriptor padDescriptor);

        /// <summary>
        /// Activates a pad (Show only makes it visible but Activate does
        /// bring it to foreground)
        /// </summary>
        void ActivatePad(PadDescriptor padDescriptor);

        /// <summary>
        /// Hides a <see cref="IPadContent"/>.
        /// </summary>
        void HidePad(PadDescriptor padDescriptor);

        /// <summary>
        /// Closes and disposes a <see cref="IPadContent"/>.
        /// </summary>
        void UnloadPad(PadDescriptor padDescriptor);

        /// <summary>
        /// Returns true, if the pad header is visible (the pad content doesn't need to be visible).
        /// </summary>
        bool IsVisible(PadDescriptor padDescriptor);
		

        IWorkbenchWindow ShowView(IViewContent content, bool switchToOpenedView);
    }
}
