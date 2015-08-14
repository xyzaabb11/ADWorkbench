using System;

namespace AD.Workbench.Workbench
{
    public interface IPadContent : IDisposable, IServiceProvider
    {
        /// <summary>
        /// This is the UI element for the view.
        /// You can use both Windows.Forms and WPF controls.
        /// </summary>
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
    }
}
