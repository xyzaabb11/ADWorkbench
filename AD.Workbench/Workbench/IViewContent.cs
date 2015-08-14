using System;

namespace AD.Workbench.Workbench
{
    public interface IViewContent : IDisposable, ICanBeDirty, IServiceProvider
    {
        object Control
        {
            get;
        }

        /// <summary>
        /// Gets the control which has focus initially.
        /// </summary>
        object InitiallyFocusedControl
        {
            get;
        }
        IWorkbenchWindow WorkbenchWindow
        {
            get;
            set;
        }

        string TitleName
        {
            get;
        }
        /// <summary>
        /// The text on the tab page when more than one view content
        /// is attached to a single window.
        /// </summary>
        string TabPageText
        {
            get;
        }
        /// <summary>
        /// Is called each time the name for the content has changed.
        /// </summary>
        event EventHandler TitleNameChanged;

        /// <summary>
        /// The tooltip that will be shown when you hover the mouse over the title
        /// </summary>
        string InfoTip
        {
            get;
        }

        /// <summary>
        /// Is called each time the info tip for the content has changed.
        /// </summary>
        event EventHandler InfoTipChanged;

        /// <summary>
        /// Is raised when the value of the TabPageText property changes.
        /// </summary>
        event EventHandler TabPageTextChanged;

        bool IsReadOnly { get; }

        bool IsDisposed { get; }

        event EventHandler Disposed;
    }
}
